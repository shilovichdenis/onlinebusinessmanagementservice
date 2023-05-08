using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class WorkerViewModel
    {
        public int WorkerId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int? BusinessId { get; set; }
        public Business? Business { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string ImagesPath { get; set; }


        public List<IFormFile>? Images { get; set; }
        public List<string>? ImagesForView { get; set; }
        public List<WorkerServices>? Services { get; set; } = null;
        public List<WorkerReviewViewModel>? Reviews { get; set; } = null;
        public List<RecordViewModel>? Records { get; set; } = null;
        public List<Schedule>? Schedule { get; set; } = null;

        public Worker ToWorker()
        {
            return new Worker()
            {
                UserId = UserId,
                BusinessId = BusinessId,
                Description = Description,
                Rating = Rating,
                ImagesPath = ImagesPath,
            };
        }

        public static void UpdateEntity(WorkerViewModel model, ref Worker worker)
        {
            worker.UserId = model.UserId;
            worker.BusinessId = model.BusinessId;
            worker.Description = model.Description;
            worker.Rating = model.Rating;
            worker.ImagesPath = model.ImagesPath;
        }
    }
}
