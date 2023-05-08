using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public interface IServiceService
    {
        Task<Service> GetService(int? id);
        Task<ServiceViewModel> GetServiceViewModel(int? id);
        Task<List<ServiceViewModel>> GetServices();
        Task<List<ServiceViewModel>> GetServicesViewModelByBusinessId(int? businessId);
        Task<List<Service>> GetServicesByBusinessId(int? businessId);
        Task<List<ServiceViewModel>> GetServicesViewModelByWorkerId(int? workerId);
        Task<List<Service>> GetServicesByWorkerId(int? workerId);
        Task<List<WorkerServices>> GetWorkerServicesByWorkerId(int? workerId);
        Task<List<ServiceViewModel>> GetServicesByViewModelAdvertisementId(int? advertisementId);
        Task<List<Service>> GetServicesByAdvertisementId(int? advertisementId);
        Task<Service> CreateService(ServiceViewModel model);
        Task<Service> UpdateService(ServiceViewModel model);
        Task<bool> DeleteService(int? id);
        Task<ServiceViewModel> ToViewModel(Service service);
        ServiceViewModel ToViewFormModel(Service service);
        Task<List<ServiceViewModel>> ToViewModels(List<Service> services);
    }
}
