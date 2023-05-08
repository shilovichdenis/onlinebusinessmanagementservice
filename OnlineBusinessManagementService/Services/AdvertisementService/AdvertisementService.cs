using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;
using System.ComponentModel;

namespace OnlineBusinessManagementService.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;
        public AdvertisementService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<Advertisement> CreateAdvertisement(AdvertisementViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Advertisement Model is NULL");
            }

            if (model.Image == null)
            {
                throw new ArgumentNullException("Image is NULL");
            }

            var imagePath = _imageService.AddImage("advertisements", model.Image);

            if (imagePath == null)
            {
                throw new ArgumentNullException("Image Path is NULL");
            }
            else
            {
                model.ImagePath = imagePath;
            }

            var advertisement = model.ToAdvertisement();

            await _context.Advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();
            return advertisement;
        }

        public async Task<Advertisement> UpdateAdvertisement(AdvertisementViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var advertisement = await _context.Advertisements.FindAsync(model.AdvertisementId);

            if (advertisement == null)
            {
                throw new ArgumentException();
            }

            if (advertisement.Discount != model.Discount)
            {
                var aServices = await _context.AdvertisementServices.Where(a => a.AdvertisementId == model.AdvertisementId).ToListAsync();
                foreach (var aService in aServices)
                {
                    var service = await _context.Services.FindAsync(aService.ServiceId);
                    service.Price = (service.Price * 100) / (100 - advertisement.Discount);
                    service.Price = (service.Price * (100 - model.Discount)) / 100;
                    _context.Services.Update(service);
                    await _context.SaveChangesAsync();
                }
            }

            if (model.Image != null)
            {
                if (!_imageService.DeleteImage(model.ImagePath))
                {
                    throw new ArgumentException();
                }

                if (_imageService.AddImage("advertisements", model.Image) == null)
                {
                    throw new ArgumentNullException();
                }

                model.ImagePath = model.Image.FileName;
            }

            AdvertisementViewModel.UpdateEntity(model, ref advertisement);

            _context.Advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
            return advertisement;
        }

        public async Task<bool> DeleteAdvertisement(int? advertisementId)
        {
            if (advertisementId == null)
            {
                throw new ArgumentNullException();
            }

            var advertisement = await _context.Advertisements.FindAsync(advertisementId);

            if (advertisement == null)
            {
                throw new ArgumentException();
            }

            var aServices = await _context.AdvertisementServices.Where(a => a.AdvertisementId == advertisementId).ToListAsync();
            foreach (var aService in aServices)
            {
                var service = await _context.Services.FindAsync(aService.ServiceId);
                service.Price = (service.Price * 100) / (100 - advertisement.Discount);
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
            }


            if (_imageService.DeleteImage(advertisement.ImagePath))
            {
                _context.Advertisements.Remove(advertisement);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AdvertisementViewModel> GetAdvertisement(int? advertisementId)
        {
            if (advertisementId == null)
            {
                throw new ArgumentNullException();
            }

            var advertisement = await _context.Advertisements.Include(a => a.Business).FirstOrDefaultAsync(a => a.Id == advertisementId);

            if (advertisement == null)
            {
                throw new ArgumentException();
            }

            return await ToViewModel(advertisement);
        }

        public async Task<List<AdvertisementViewModel>> GetAdvertisements()
        {
            var advertisements = await _context.Advertisements.Include(a => a.Business).OrderBy(a => a.StartDate).ToListAsync();
            return await ToViewModels(advertisements);
        }

        public async Task<AdvertisementViewModel> GetAdvertisementByServiceId(int? serviceId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException();
            }

            var advertisementService = await _context.AdvertisementServices.FirstOrDefaultAsync(a => a.ServiceId == serviceId);

            var advertisement = await ToViewModel(await _context.Advertisements.Include(a => a.Business).FirstOrDefaultAsync(a => a.Id == advertisementService.AdvertisementId));

            return advertisement;
        }
        public async Task<List<AdvertisementViewModel>> GetAdvertisementsByBusinessId(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var advertisements = await _context.Advertisements.Where(a => a.BusinessId == businessId).ToListAsync();

            return await ToViewModels(advertisements);
        }

        public async Task<AdvertisementViewModel> ToViewModel(Advertisement advertisement)
        {
            return new AdvertisementViewModel()
            {
                AdvertisementId = advertisement.Id,
                BusinessId = advertisement.BusinessId,
                Business = advertisement.Business,
                Name = advertisement.Name,
                Description = advertisement.Description,
                StartDate = advertisement.StartDate,
                EndDate = advertisement.EndDate,
                Discount = advertisement.Discount,
                ImagePath = advertisement.ImagePath,
                Services = await _context.AdvertisementServices.Include(a => a.Service).ThenInclude(s => s.Category)
                .Include(a => a.Advertisement).Where(a => a.AdvertisementId == advertisement.Id).ToListAsync()
            };
        }

        public async Task<List<AdvertisementViewModel>> ToViewModels(List<Advertisement> advertisements)
        {
            var models = new List<AdvertisementViewModel>();
            foreach (var advertisement in advertisements)
            {
                models.Add(await ToViewModel(advertisement));
            }
            return models;
        }

        public async Task<bool> AddService(int? advertisementId, int? serviceId)
        {
            if (advertisementId == null || serviceId == null)
            {
                throw new ArgumentNullException();
            }

            var service = await _context.Services.FindAsync(serviceId);
            var advertisement = await _context.Advertisements.FindAsync(advertisementId);

            service.Price = (service.Price * (100 - advertisement.Discount)) / 100;

            _context.Services.Update(service);
            await _context.AdvertisementServices.AddAsync(new AdvertisementServices() { AdvertisementId = (int)advertisementId, ServiceId = (int)serviceId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveService(int? advertisementId, int? serviceId)
        {
            if (advertisementId == null || serviceId == null)
            {
                throw new ArgumentNullException();
            }
            var aService = await _context.AdvertisementServices.FirstOrDefaultAsync(a => a.AdvertisementId == advertisementId && a.ServiceId == serviceId);

            if (aService == null)
            {
                throw new ArgumentNullException();
            }

            var service = await _context.Services.FindAsync(serviceId);
            var advertisement = await _context.Advertisements.FindAsync(advertisementId);

            service.Price = (service.Price * 100) / (100 - advertisement.Discount);

            _context.Services.Update(service);
            _context.AdvertisementServices.Remove(aService);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
