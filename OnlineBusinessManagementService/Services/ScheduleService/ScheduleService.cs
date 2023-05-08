using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public class ScheduleService : IScheduleService
    {
        protected readonly ApplicationDbContext _context;
        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSchedule(ScheduleViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            var schedule = await _context.Schedules.Where(s => s.WorkerId == model.WorkerId).ToListAsync();
            var newSchedule = model.ToSchedule();
            newSchedule = newSchedule.ExceptBy(schedule.Select(s => s.DateTime), n => n.DateTime).ToList();
            await _context.Schedules.AddRangeAsync(newSchedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Schedule>> GetSchedules(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            return await _context.Schedules.Include(s => s.Worker)
                .Where(s => s.WorkerId == workerId)
                .OrderBy(s => s.DateTime).Where(s => s.DateTime >= DateTime.Now).ToListAsync();
        }

        public async Task<List<TimeForSchedule>> GetTimeForSchedule()
        {
            return await _context.TimeForSchedule.ToListAsync();
        }

        public async Task<bool> RemoveSchedule(int? scheduleId)
        {
            if (scheduleId == null)
            {
                throw new ArgumentNullException();
            }

            var schedule = await _context.Schedules.FindAsync(scheduleId);

            if (schedule == null)
            {
                throw new ArgumentException();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
