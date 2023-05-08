using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class BusinessReviewService : IBusinessReviewService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;

        public BusinessReviewService(ApplicationDbContext context,
            IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<BusinessReview> CreateBusinessReview(BusinessReviewViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var review = model.ToReview();

            await _context.BusinessReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            var business = await _context.Businesses.FindAsync(model.BusinessId);
            var reviews = await _context.BusinessReviews.Where(b => b.BusinessId == model.BusinessId).ToListAsync();
            business.Rating = reviews.Sum(r => r.Rating) / reviews.Count;
            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<BusinessReview> UpdateBusinessReview(BusinessReviewViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.BusinessReviews.FindAsync(model.BusinessReviewId);

            if (review == null)
            {
                throw new ArgumentException();
            }

            BusinessReviewViewModel.UpdateEntity(model, ref review);

            _context.BusinessReviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteBusinessReview(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.BusinessReviews.FindAsync(id);

            if (review == null)
            {
                throw new ArgumentException();
            }

            _context.BusinessReviews.Remove(review);
            await _context.SaveChangesAsync();
            var business = await _context.Businesses.FindAsync(review.BusinessId);
            var reviews = await _context.BusinessReviews.Where(b => b.BusinessId == review.BusinessId).ToListAsync();
            business.Rating = reviews.Count != 0 ? reviews.Sum(r => r.Rating) / reviews.Count : 0;
            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessReviewViewModel> GetBusinessReview(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var review = await _context.BusinessReviews.Include(b => b.User).Include(b => b.Business).FirstOrDefaultAsync(b => b.Id == id);

            if (review == null)
            {
                throw new ArgumentNullException();
            }

            return ToViewModel(review);
        }

        public async Task<List<BusinessReviewViewModel>> GetBusinessReviews()
        {
            var reviews = await _context.BusinessReviews.Include(b => b.User).Include(b => b.Business).ToListAsync();
            return ToViewModels(reviews);
        }

        public async Task<List<BusinessReviewViewModel>> GetBusinessReviewsByBusinessId(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var reviews = await _context.BusinessReviews.Include(b => b.User).Include(b => b.Business).Where(r => r.BusinessId == businessId).ToListAsync();
            return ToViewModels(reviews);
        }

        public async Task<List<BusinessReviewViewModel>> GetBusinessReviewsByUserId(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var reviews = await _context.BusinessReviews.Include(b => b.User).Include(b => b.Business).Where(r => r.UserId == userId).ToListAsync();
            return ToViewModels(reviews);
        }

        public BusinessReviewViewModel ToViewModel(BusinessReview businessReview)
        {
            return new BusinessReviewViewModel()
            {
                BusinessReviewId = businessReview.Id,
                UserId = businessReview.UserId,
                User = businessReview.User,
                Business = businessReview.Business,
                BusinessId = businessReview.BusinessId,
                Rating = businessReview.Rating,
                Description = businessReview.Description,
            };
        }

        public List<BusinessReviewViewModel> ToViewModels(List<BusinessReview> businessReviews)
        {
            var models = new List<BusinessReviewViewModel>();
            foreach (var review in businessReviews)
            {
                models.Add(ToViewModel(review));
            }
            return models;
        }
    }
}
