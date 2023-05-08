using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }
        public int BusinessId { get; set; }
        public Business? Business { get; set; }
        public int CategoryId { get; set; }
        public CategoryOfService? Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Time { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public List<WorkerViewModel>? Workers { get; set; }
        public AdvertisementViewModel? Advertisement { get; set; }

        public Service ToService()
        {
            return new Service()
            {
                Name = this.Name,
                ImagePath = this.ImagePath,
                CategoryId = this.CategoryId,
                BusinessId = this.BusinessId,
                Price = this.Price,
                Time = this.Time,
            };
        }

        public static void UpdateEntity(ServiceViewModel model, ref Service service)
        {
            service.BusinessId = model.BusinessId;
            service.CategoryId = model.CategoryId;
            service.Name = model.Name;
            service.Price = model.Price;
            service.Time = model.Time;
            service.ImagePath = model.ImagePath;
        }
    }
}
