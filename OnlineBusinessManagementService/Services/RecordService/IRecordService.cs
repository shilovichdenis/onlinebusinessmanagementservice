namespace OnlineBusinessManagementService.Services
{
    public interface IRecordService
    {
        Task<RecordViewModel> GetRecord(int? recordId);
        Task<List<RecordViewModel>> GetRecords();
        Task<List<RecordViewModel>> GetRecordsByUserId(string? userId);
        Task<List<RecordViewModel>> GetRecordsByWorkerId(int? workerId);
        Task<List<RecordViewModel>> GetRecordsByDate(DateTime dateTime);
        Task<List<RecordViewModel>> GetRecordsByBusinessId(int? businessId);
        Task<Record> CreateRecord(RecordViewModel model);
        Task<Record> UpdateRecord(RecordViewModel model);
        Task<bool> DeleteRecord(int? recordId);
        Task<RecordViewModel> ToViewModel(Record record);
        Task<List<RecordViewModel>> ToViewModels(List<Record> records);
        Task<bool> AddServiceToRecord(int? recordId, int? wServiceId);
        Task<bool> RemoveServiceFromRecord(int? recordId, int? wServiceId);
        Task<List<RecordServices>> GetServices(int? recordId);

    }
}
