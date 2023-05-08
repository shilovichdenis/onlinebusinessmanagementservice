using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class WorkerReviewService : IWorkerReviewService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;

        public WorkerReviewService(ApplicationDbContext context,
            IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<WorkerReview> CreateWorkerReview(WorkerReviewViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var review = model.ToReview();

            await _context.WorkerReviews.AddAsync(review);
            await _context.SaveChangesAsync();

            var worker = await _context.Workers.FindAsync(review.WorkerId);
            var reviews = await _context.WorkerReviews.Where(b => b.WorkerId == review.WorkerId).ToListAsync();
            worker.Rating = reviews.Sum(r => r.Rating) / reviews.Count;
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<WorkerReview> UpdateWorkerReview(WorkerReviewViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.WorkerReviews.FindAsync(model.WorkerReviewId);

            if (review == null)
            {
                throw new ArgumentException();
            }

            WorkerReviewViewModel.UpdateEntity(model, ref review);

            _context.WorkerReviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteWorkerReview(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.WorkerReviews.FindAsync(id);

            if (review == null)
            {
                throw new ArgumentException();
            }

            _context.WorkerReviews.Remove(review);
            await _context.SaveChangesAsync();

            var worker = await _context.Workers.FindAsync(review.WorkerId);
            var reviews = await _context.WorkerReviews.Where(b => b.WorkerId == review.WorkerId).ToListAsync();
            worker.Rating = reviews.Count != 0 ? reviews.Sum(r => r.Rating) / reviews.Count : 0;
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkerReviewViewModel> GetWorkerReview(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.WorkerReviews.Include(f => f.Worker).Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);

            if (review == null)
            {
                throw new ArgumentNullException();
            }

            return await ToViewModel(review);
        }

        public async Task<List<WorkerReviewViewModel>> GetWorkerReviews()
        {
            var reviews = await _context.WorkerReviews.Include(r => r.Worker).Include(r => r.User).ToListAsync();
            return await ToViewModels(reviews);
        }

        public async Task<List<WorkerReviewViewModel>> GetWorkerReviewsByWorkerId(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var reviews = await _context.WorkerReviews.Include(r => r.Worker).Include(r => r.User).Where(r => r.WorkerId == workerId).ToListAsync();
            return await ToViewModels(reviews);
        }

        public async Task<List<WorkerReviewViewModel>> GetWorkerReviewsByUserId(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var reviews = await _context.WorkerReviews.Include(r => r.Worker).Include(r => r.User).Where(r => r.UserId == userId).ToListAsync();
            return await ToViewModels(reviews);
        }

        public async Task<WorkerReviewViewModel> ToViewModel(WorkerReview workerReview)
        {
            return new WorkerReviewViewModel()
            {
                WorkerReviewId = workerReview.Id,
                UserId = workerReview.UserId,
                User = workerReview.User,
                WorkerId = workerReview.Id,
                Worker = workerReview.Worker,
                Rating = workerReview.Rating,
                Description = workerReview.Description,
            };
        }

        public async Task<List<WorkerReviewViewModel>> ToViewModels(List<WorkerReview> workerReviews)
        {
            var models = new List<WorkerReviewViewModel>();
            foreach (var review in workerReviews)
            {
                models.Add(await ToViewModel(review));
            }
            return models;
        }

    }
}
