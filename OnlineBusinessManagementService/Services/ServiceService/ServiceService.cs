using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class ServiceService : IServiceService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;
        protected readonly IWorkerService _workerService;
        protected readonly IAdvertisementService _advertisementService;
        public ServiceService(
            ApplicationDbContext context,
            IImageService imageService,
            IWorkerService workerService,
            IAdvertisementService advertisementService)
        {
            _context = context;
            _imageService = imageService;
            _workerService = workerService;
            _advertisementService = advertisementService;
        }

        public async Task<Service> CreateService(ServiceViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (model.Image != null)
            {
                var business = await _context.Businesses.FindAsync(model.BusinessId);
                var dir = business.Name.ToLower().Replace('"', ' ').Replace(" ", "").Replace("_", "").Replace("-", "").Replace("«", "").Replace("»", "").Trim();
                var imagePath = _imageService.AddImage(Path.Combine("businesses", dir, "services"), model.Image);
                if (imagePath == null)
                {
                    throw new ArgumentNullException();
                }

                model.ImagePath = imagePath;
            }
            else
            {
                model.ImagePath = @"images\default\defaultservice.png";
            }

            var service = model.ToService();
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task<Service> UpdateService(ServiceViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var service = await _context.Services.FindAsync(model.ServiceId);

            if (service == null)
            {
                throw new ArgumentNullException();
            }

            ServiceViewModel.UpdateEntity(model, ref service);

            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> DeleteService(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Service Id is null");
            }

            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                throw new ArgumentNullException($"Service with ID {id} not found");
            }

            if (_imageService.DeleteImage(service.ImagePath))
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Service> GetService(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            var service = await _context.Services.Include(s => s.Category).Include(s => s.Business).FirstOrDefaultAsync(f => f.Id == id);

            if (service == null)
            {
                throw new ArgumentNullException();
            }

            return service;
        }

        public async Task<ServiceViewModel> GetServiceViewModel(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            var service = await _context.Services.Include(s => s.Category).Include(s => s.Business).FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                throw new ArgumentNullException();
            }

            return await ToViewModel(service);
        }

        public async Task<List<ServiceViewModel>> GetServices()
        {
            var services = await _context.Services.Include(s => s.Category).Include(s => s.Business).ToListAsync();
            return await ToViewModels(services);
        }

        public async Task<List<Service>> GetServicesByBusinessId(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var services = await _context.Services.Include(s => s.Category).Include(s => s.Business).Where(s => s.BusinessId == businessId).ToListAsync();
            return services;
        }

        public async Task<List<WorkerServices>> GetWorkerServicesByWorkerId(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var workerService = await _context.WorkerServices.Where(w => w.WorkerId == workerId).Include(w => w.Worker).Include(w => w.Service).ToListAsync();

            return workerService;
        }

        public async Task<List<Service>> GetServicesByWorkerId(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var workerService = await _context.WorkerServices.Include(w => w.Worker).Include(w => w.Service).ThenInclude(w => w.Category).Where(w => w.WorkerId == workerId).ToListAsync();

            var services = new List<Service>();
            foreach (var wService in workerService)
            {
                services.Add(await _context.Services.Include(s => s.Business).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == wService.ServiceId));
            }

            return services;
        }

        public ServiceViewModel ToViewFormModel(Service service)
        {
            return new ServiceViewModel()
            {
                ServiceId = service.Id,
                BusinessId = service.BusinessId,
                Business = service.Business,
                Name = service.Name,
                ImagePath = service.ImagePath,
                Category = service.Category,
                CategoryId = service.CategoryId,
                Price = service.Price,
                Time = service.Time,
            };
        }

        public async Task<ServiceViewModel> ToViewModel(Service service)
        {
            var advService = await _context.AdvertisementServices.FirstOrDefaultAsync(a => a.ServiceId == service.Id);
            return new ServiceViewModel()
            {
                ServiceId = service.Id,
                BusinessId = service.BusinessId,
                Business = service.Business,
                Name = service.Name,
                ImagePath = service.ImagePath,
                Category = service.Category,
                CategoryId = service.CategoryId,
                Price = service.Price,
                Time = service.Time,
                Workers = await _workerService.GetWorkersByServiceId(service.Id),
                Advertisement = advService != null ? await _advertisementService.GetAdvertisement(advService.AdvertisementId) : null
            };
        }

        public async Task<List<ServiceViewModel>> ToViewModels(List<Service> services)
        {
            var models = new List<ServiceViewModel>();

            foreach (var service in services)
            {
                models.Add(await ToViewModel(service));
            }
            return models;
        }

        public async Task<List<Service>> GetServicesByAdvertisementId(int? advertisementId)
        {
            var aServices = await _context.AdvertisementServices.Where(a => a.AdvertisementId == advertisementId).ToListAsync();
            var services = new List<Service>();
            foreach (var aService in aServices)
            {
                var service = await _context.Services.FindAsync(aService.ServiceId);
                services.Add(service);
            }
            return services;
        }

        public async Task<List<ServiceViewModel>> GetServicesViewModelByBusinessId(int? businessId)
        {
            var result = await GetServicesByBusinessId(businessId);
            return await ToViewModels(result);
        }

        public async Task<List<ServiceViewModel>> GetServicesViewModelByWorkerId(int? workerId)
        {
            var result = await GetServicesByWorkerId(workerId);
            return await ToViewModels(result);
        }


        public async Task<List<ServiceViewModel>> GetServicesByViewModelAdvertisementId(int? advertisementId)
        {
            var result = await GetServicesByAdvertisementId(advertisementId);
            return await ToViewModels(result);
        }

    }
}
