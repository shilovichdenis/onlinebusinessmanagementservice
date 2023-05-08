using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class BusinessService : IBusinessService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;
        protected readonly IUserService _userService;
        protected readonly IWorkerService _workerService;
        protected readonly IServiceService _serviceService;
        protected readonly IBusinessReviewService _businessReviewService;
        protected readonly IAdvertisementService _advertisementService;
        protected readonly IRecordService _recordService;
        public BusinessService(
            ApplicationDbContext context,
            IImageService imageService,
            IWorkerService workerService,
            IServiceService serviceService,
            IBusinessReviewService businessReviewService,
            IAdvertisementService advertisementService,
            IRecordService recordService)
        {
            _context = context;
            _imageService = imageService;
            _workerService = workerService;
            _serviceService = serviceService;
            _businessReviewService = businessReviewService;
            _advertisementService = advertisementService;
            _recordService = recordService;
        }

        public async Task<Business> CreateBusiness(BusinessViewModel model)
        {
            if (model == null) throw new ArgumentNullException();

            if (model.Images == null)
            {
                throw new ArgumentNullException();
            }
            var dir = model.Name.ToLower().Replace(" ", "").Replace("_", "").Replace("-", "").Replace("«", "").Replace("»", "").Replace('"', ' ').Trim();

            var imagePath = _imageService.AddImages(Path.Combine("businesses", dir, "images"), model.Images);
            var logoPath = _imageService.AddLogo(Path.Combine("businesses", dir, "logo"), model.Logo);
            if (imagePath == null || logoPath == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                model.ImagesPath = imagePath;
                model.LogoPath = logoPath;
            }

            var business = model.ToBusiness();

            await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateBusiness(BusinessViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var business = await _context.Businesses.FindAsync(model.BusinessId);

            if (business == null)
            {
                throw new ArgumentException();
            }

            BusinessViewModel.UpdateEntity(model, ref business);

            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<bool> DeleteBusiness(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var business = await _context.Businesses.FindAsync(id);

            if (business == null)
            {
                throw new ArgumentException();
            }

            var dir = business.Name.ToLower().Replace(" ", "").Replace("_", "").Replace("-", "").Replace("«", "").Replace("»", "").Replace('"', ' ').Trim();
            if (_imageService.DeleteDirectory(Path.Combine("images", "businesses", dir)))
            {
                _context.Businesses.Remove(business);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<BusinessViewModel>> GetBusinesses()
        {
            var businesses = await _context.Businesses.Include(b => b.User).OrderBy(b => b.Name).ToListAsync();
            var models = new List<BusinessViewModel>();
            foreach (var business in businesses)
            {
                models.Add(await ToViewModel(business));
            }
            return models;
        }

        public async Task<BusinessViewModel> GetBusinessByUserId(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var business = await _context.Businesses.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (business == null)
            {
                throw new ArgumentNullException();
            }
            var model = await ToViewModel(business);

            return model;
        }
        public async Task<List<BusinessViewModel>> GetBusinessesByCategoryId(int? categoryId)
        {
            var businesses = await _context.Businesses.Include(b => b.User).ToListAsync();
            var models = new List<BusinessViewModel>();

            foreach (var business in businesses)
            {
                if (await _context.Services.Where(s => s.BusinessId == business.Id).AnyAsync(s => s.CategoryId == categoryId))
                {
                    models.Add(await ToViewModel(business));
                }
            }

            return models;
        }

        public async Task<BusinessViewModel> GetBusiness(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var business = await _context.Businesses.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == businessId);

            if (business == null)
            {
                throw new ArgumentNullException();
            }
            var model = await ToViewModel(business);

            return model;
        }

        public async Task<BusinessViewModel> ToViewModel(Business business)
        {
            return new BusinessViewModel()
            {
                BusinessId = business.Id,
                Name = business.Name,
                User = business.User,
                PhoneNumber = business.PhoneNumber,
                WorkingHours = business.WorkingHours,
                Address = business.Address,
                Description = business.Description,
                Category = business.Category,
                Rating = business.Rating,
                ImagesPath = business.ImagesPath,
                LogoPath = business.LogoPath,
                ImagesForView = _imageService.GetImagesByPath(business.ImagesPath),
                Reviews = await _businessReviewService.GetBusinessReviewsByBusinessId(business.Id),
                Workers = await _workerService.GetWorkersByBusinessId(business.Id),
                Services = await _serviceService.GetServicesViewModelByBusinessId(business.Id),
                Advertisements = await _advertisementService.GetAdvertisementsByBusinessId(business.Id),
                Records = await _recordService.GetRecordsByBusinessId(business.Id)
            };
        }

        public async Task<bool> AddWorker(User user, int? businessId)
        {
            if (user == null || businessId == null)
            {
                throw new ArgumentNullException("User or Business ID is NULL");
            }

            if (await _context.Workers.Include(w => w.User).AnyAsync(w => w.User.Email == user.Email))
            {
                var worker = await _context.Workers.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Email == user.Email);
                if (worker.BusinessId == null)
                {
                    worker.BusinessId = businessId;
                    _context.Workers.Update(worker);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new ArgumentException("Worker is already taken");
                }
            }
            else
            {
                await _workerService.CreateWorker(await _workerService.ToViewModel(user.ToWorker(businessId)));
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemoveWorker(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var worker = await _context.Workers.FindAsync(workerId);

            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            worker.BusinessId = null;

            var services = await _context.WorkerServices.Where(s => s.WorkerId == worker.Id).ToListAsync();
            _context.WorkerServices.RemoveRange(services);
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
