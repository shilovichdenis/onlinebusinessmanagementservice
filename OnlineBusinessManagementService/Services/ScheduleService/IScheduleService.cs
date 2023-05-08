namespace OnlineBusinessManagementService.Services
{
    public interface IScheduleService
    {
        Task<List<TimeForSchedule>> GetTimeForSchedule();
        Task<List<Schedule>> GetSchedules(int? workerId);
        Task<bool> AddSchedule(ScheduleViewModel model);
        Task<bool> RemoveSchedule(int? scheduleId);
    }
}
