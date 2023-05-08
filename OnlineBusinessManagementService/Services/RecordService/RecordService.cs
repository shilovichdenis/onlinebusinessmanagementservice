using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Services
{
    public class RecordService : IRecordService
    {
        protected readonly ApplicationDbContext _context;
        public RecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Record> CreateRecord(RecordViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var record = model.ToRecord();

            var result = await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Record> UpdateRecord(RecordViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var record = await _context.Records.FindAsync(model.RecordId);

            if (record == null)
            {
                throw new ArgumentNullException();
            }

            RecordViewModel.UpdateEntity(model, ref record);

            _context.Records.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteRecord(int? recordId)
        {
            if (recordId == null)
            {
                throw new ArgumentNullException();
            }

            var record = await _context.Records.FindAsync(recordId);

            if (record == null)
            {
                throw new ArgumentNullException();
            }

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RecordViewModel> GetRecord(int? recordId)
        {
            if (recordId == null)
            {
                throw new ArgumentNullException();
            }

            var record = await _context.Records.Include(r => r.User).Include(r => r.Worker)
                .ThenInclude(w => w.User).Include(r => r.TimeSchedule).ThenInclude(s => s.Worker).FirstOrDefaultAsync(r => r.Id == recordId);

            if (record == null)
            {
                throw new ArgumentNullException();
            }

            return await ToViewModel(record);
        }
        public async Task<List<RecordViewModel>> GetRecordsByBusinessId(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var records = await _context.Records.Include(r => r.User)
                .Include(r => r.TimeSchedule)
                .Include(r => r.Worker)
                .ThenInclude(w => w.Business)
                .Where(r => r.Worker.BusinessId == businessId).ToListAsync();

            return await ToViewModels(records);
        }

        public async Task<List<RecordViewModel>> GetRecordsByDate(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                throw new ArgumentNullException();
            }

            var records = await _context.Records.Include(r => r.User)
                .Include(r => r.Worker)
                .Include(r => r.TimeSchedule)
                .Where(r => r.TimeSchedule.DateTime.Date == dateTime.Date)
                .OrderBy(r => r.TimeSchedule)
                .ThenBy(r => r.isConfirmed)
                .ThenBy(r => r.isSuccessful).ToListAsync();

            return await ToViewModels(records);
        }

        public async Task<List<RecordViewModel>> GetRecordsByUserId(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var records = await _context.Records.Include(r => r.User)
                .Include(r => r.Worker).ThenInclude(w => w.Business)
                .Include(r => r.Worker).ThenInclude(w => w.User)
                .Include(r => r.TimeSchedule)
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.TimeSchedule)
                .ThenBy(r => r.isConfirmed)
                .ThenBy(r => r.isSuccessful).ToListAsync();

            return await ToViewModels(records);
        }

        public async Task<List<RecordViewModel>> GetRecordsByWorkerId(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var records = await _context.Records.Include(r => r.User)
                .Include(r => r.Worker)
                .Include(r => r.TimeSchedule)
                .Where(r => r.WorkerId == workerId)
                .OrderBy(r => r.TimeSchedule)
                .ThenBy(r => r.isConfirmed)
                .ThenBy(r => r.isSuccessful).ToListAsync();

            return await ToViewModels(records);
        }

        public async Task<RecordViewModel> ToViewModel(Record record)
        {
            return new RecordViewModel()
            {
                RecordId = record.Id,
                UserId = record.UserId,
                User = record.User,
                Worker = record.Worker,
                WorkerId = record.WorkerId,
                TimeScheduleId = record.TimeScheduleId,
                TimeSchedule = record.TimeSchedule,
                TotalPrice = record.TotalPrice,
                isConfirmed = record.isConfirmed,
                isSuccessful = record.isSuccessful,
                Services = await _context.RecordServices.Where(r => r.RecordId == record.Id).ToListAsync()
            };
        }

        public async Task<List<RecordViewModel>> ToViewModels(List<Record> records)
        {
            var models = new List<RecordViewModel>();

            foreach (var record in records)
            {
                models.Add(await ToViewModel(record));
            }

            return models;
        }

        public async Task<List<RecordViewModel>> GetRecords()
        {
            var records = await _context.Records.Include(r => r.User)
                .Include(r => r.Worker)
                .Include(r => r.TimeSchedule)
                .OrderBy(r => r.TimeSchedule)
                .ThenBy(r => r.isConfirmed)
                .ThenBy(r => r.isSuccessful).ToListAsync();

            return await ToViewModels(records);
        }

        public async Task<bool> AddServiceToRecord(int? recordId, int? wServiceId)
        {
            if (recordId == null || wServiceId == null)
            {
                throw new ArgumentNullException();
            }
            var record = await _context.Records.FindAsync(recordId);
            var wService = await _context.WorkerServices.Include(w => w.Service).FirstOrDefaultAsync(w => w.Id == wServiceId);
            record.TotalPrice += wService.Service.Price;
            _context.Records.Update(record);
            await _context.RecordServices.AddAsync(new RecordServices() { RecordId = (int)recordId, ServiceId = wServiceId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveServiceFromRecord(int? recordId, int? wServiceId)
        {
            if (recordId == null || wServiceId == null)
            {
                throw new ArgumentNullException();
            }
            var record = await _context.Records.FindAsync(recordId);
            var wService = await _context.WorkerServices.Include(w => w.Service).FirstOrDefaultAsync(w => w.Id == wServiceId);
            record.TotalPrice -= wService.Service.Price;
            _context.Records.Update(record);
            var rService = await _context.RecordServices.FirstOrDefaultAsync(r => r.ServiceId == wServiceId && r.RecordId == recordId);
            _context.RecordServices.Remove(rService);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecordServices>> GetServices(int? recordId)
        {
            if (recordId == null)
            {
                throw new ArgumentNullException();
            }
            var rService = await _context.RecordServices.Where(r => r.RecordId == recordId).Include(r => r.Service).ThenInclude(wS => wS.Service).ThenInclude(s => s.Category).ToListAsync();
            return rService;
        }

    }
}
