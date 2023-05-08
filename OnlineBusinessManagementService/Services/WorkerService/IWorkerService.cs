using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public interface IWorkerService
    {
        Task<WorkerViewModel> GetWorker(int? workerId);
        Task<WorkerViewModel> GetWorkerByUserId(string? userId);
        Task<List<Worker>> GetWorkersByBusinessId(int? businessId);
        Task<List<WorkerViewModel>> GetWorkersByServiceId(int? serviceId);
        Task<List<WorkerViewModel>> GetAllWorkers();
        Task<Worker> CreateWorker(WorkerViewModel model);
        Task<Worker> UpdateWorker(WorkerViewModel model);
        Task<bool> DeleteWorker(int? workerId);
        Task<WorkerViewModel> ToViewModel(Worker worker);
        Task<List<WorkerViewModel>> ToViewModels(List<Worker> workers);
        Task<WorkerServices> AddService(int? workerId, int? serviceId);
        Task<bool> RemoveService(int? workerId, int? serviceId);
    }
}
