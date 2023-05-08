using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class BusinessViewModel
    {
        public int BusinessId { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkingHours { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public CategoryOfBusiness Category { get; set; }
        public double Rating { get; set; }
        public string? ImagesPath { get; set; }
        public string? LogoPath { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<string>? ImagesForView { get; set; }
        [NotMapped]
        public IFormFile? Logo { get; set; }
        public List<Worker>? Workers { get; set; } = new List<Worker>();
        public List<ServiceViewModel>? Services { get; set; } = null;
        public List<BusinessReviewViewModel>? Reviews { get; set; } = new List<BusinessReviewViewModel>();
        public List<AdvertisementViewModel>? Advertisements { get; set; } = new List<AdvertisementViewModel>();
        public List<RecordViewModel>? Records { get; set; } = new List<RecordViewModel>();

        public Business ToBusiness()
        {
            return new Business()
            {
                UserId = this.UserId,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber,
                WorkingHours = this.WorkingHours,
                Address = this.Address,
                Description = this.Description,
                Category = this.Category,
                Rating = this.Rating,
                ImagesPath = this.ImagesPath,
                LogoPath = this.LogoPath
            };
        }

        public static void UpdateEntity(BusinessViewModel model, ref Business business)
        {
            business.UserId = model.UserId;
            business.Name = model.Name;
            business.PhoneNumber = model.PhoneNumber;
            business.WorkingHours = model.WorkingHours;
            business.Address = model.Address;
            business.Description = model.Description;
            business.Category = model.Category;
            business.Rating = model.Rating;
            business.ImagesPath = model.ImagesPath;
            business.LogoPath = model.LogoPath;
        }
    }
}
