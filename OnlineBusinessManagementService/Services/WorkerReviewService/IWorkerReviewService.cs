namespace OnlineBusinessManagementService.Services
{
    public interface IWorkerReviewService
    {
        Task<WorkerReviewViewModel> GetWorkerReview(int? id);
        Task<List<WorkerReviewViewModel>> GetWorkerReviewsByWorkerId(int? workerId);
        Task<List<WorkerReviewViewModel>> GetWorkerReviewsByUserId(string? userId);
        Task<List<WorkerReviewViewModel>> GetWorkerReviews();
        Task<WorkerReview> CreateWorkerReview(WorkerReviewViewModel model);
        Task<WorkerReview> UpdateWorkerReview(WorkerReviewViewModel model);
        Task<bool> DeleteWorkerReview(int? id);
        Task<WorkerReviewViewModel> ToViewModel(WorkerReview workerReview);
        Task<List<WorkerReviewViewModel>> ToViewModels(List<WorkerReview> workerReviews);
    }
}
