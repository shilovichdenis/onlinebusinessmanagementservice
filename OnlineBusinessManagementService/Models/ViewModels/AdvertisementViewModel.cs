using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class AdvertisementViewModel
    {
        public int AdvertisementId { get; set; }
        public int BusinessId { get; set; }
        public Business? Business { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public List<AdvertisementServices>? Services { get; set; }

        public Advertisement ToAdvertisement()
        {
            return new Advertisement
            {
                BusinessId = this.BusinessId,
                Name = this.Name,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                ImagePath = this.ImagePath,
                Discount = this.Discount
            };
        }

        public static void UpdateEntity(AdvertisementViewModel model, ref Advertisement advertisement)
        {
            advertisement.BusinessId = model.BusinessId;
            advertisement.Name = model.Name;
            advertisement.StartDate = model.StartDate;
            advertisement.EndDate = model.EndDate;
            advertisement.Description = model.Description;
            advertisement.Discount = model.Discount;
            advertisement.ImagePath = model.ImagePath;
        }
    }
}
