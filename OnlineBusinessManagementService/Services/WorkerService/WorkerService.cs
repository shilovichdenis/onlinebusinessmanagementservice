using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OnlineBusinessManagementService.Data;

namespace OnlineBusinessManagementService.Services
{
    public class WorkerService : IWorkerService
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IImageService _imageService;
        protected readonly IWorkerReviewService _reviewService;
        protected readonly IUserService _userService;
        protected readonly IRecordService _recordService;
        protected readonly IScheduleService _scheduleService;
        public WorkerService(ApplicationDbContext context,
            IImageService imageService,
            IWorkerReviewService reviewService,
            IUserService userService,
            IRecordService recordService,
            IScheduleService scheduleService)
        {
            _context = context;
            _imageService = imageService;
            _reviewService = reviewService;
            _userService = userService;
            _recordService = recordService;
            _scheduleService = scheduleService;
        }

        public async Task<Worker> CreateWorker(WorkerViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Worker Model is NULL");
            }
            var imagePath = _imageService.CreateDirectory(Path.Combine("workers", model.User.Email.ToLower()));

            if (imagePath == null)
            {
                throw new ArgumentNullException("Image Path is NULL");
            }
            else
            {
                model.ImagesPath = imagePath;
            }

            var worker = model.ToWorker();
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task<Worker> UpdateWorker(WorkerViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var worker = await _context.Workers.FindAsync(model.WorkerId);

            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            WorkerViewModel.UpdateEntity(model, ref worker);

            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task<bool> DeleteWorker(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var worker = await _context.Workers.FindAsync(workerId);

            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            if (_imageService.DeleteDirectory(worker.ImagesPath))
            {
                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<WorkerViewModel>> GetAllWorkers()
        {
            var workers = await _context.Workers.Include(w => w.Business).Include(w => w.User).ToListAsync();
            return await ToViewModels(workers);
        }

        public async Task<WorkerViewModel> GetWorker(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var worker = await _context.Workers.Include(w => w.Business).Include(w => w.User).FirstOrDefaultAsync(w => w.Id == workerId);

            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            return await ToViewModel(worker);
        }

        public async Task<WorkerViewModel> GetWorkerByUserId(string? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }

            var worker = await _context.Workers.Include(w => w.Business).Include(w => w.User).FirstOrDefaultAsync(w => w.UserId == userId);

            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            return await ToViewModel(worker);
        }

        public async Task<List<WorkerViewModel>> GetWorkersByServiceId(int? serviceId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException();
            }
            var workers = new List<Worker>();
            var workerServices = await _context.WorkerServices.Where(w => w.ServiceId == serviceId).ToListAsync();
            foreach (var wService in workerServices)
            {
                workers.Add(await _context.Workers.Include(w => w.Business).Include(w => w.User).FirstOrDefaultAsync(f => f.Id == wService.WorkerId));
            }
            return await ToViewModels(workers);
        }

        public async Task<List<Worker>> GetWorkersByBusinessId(int? businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException();
            }

            var workers = await _context.Workers.Include(w => w.Business).Include(w => w.User).Where(w => w.BusinessId == businessId).ToListAsync();

            return workers;
        }

        public async Task<WorkerViewModel> ToViewModel(Worker worker)
        {
            return new WorkerViewModel()
            {
                WorkerId = worker.Id,
                UserId = worker.UserId,
                User = worker.User,
                BusinessId = worker.BusinessId != null ? worker.BusinessId : 0,
                Business = worker.BusinessId != null ? worker.Business : null,
                Description = worker.Description,
                Rating = worker.Rating,
                ImagesPath = worker.ImagesPath,
                ImagesForView = _imageService.GetImagesByPath(worker.ImagesPath),
                Services = await GetServices(worker.Id),
                Reviews = await _reviewService.GetWorkerReviewsByWorkerId(worker.Id),
                Records = await _recordService.GetRecordsByWorkerId(worker.Id),
                Schedule = await _scheduleService.GetSchedules(worker.Id)
            };
        }
        public async Task<List<WorkerViewModel>> ToViewModels(List<Worker> workers)
        {
            var models = new List<WorkerViewModel>();
            foreach (var worker in workers)
            {
                models.Add(await ToViewModel(worker));
            }
            return models;
        }

        public async Task<List<WorkerServices>> GetServices(int? workerId)
        {
            if (workerId == null)
            {
                throw new ArgumentNullException();
            }

            var workerServices = await _context.WorkerServices.Include(w => w.Service).ThenInclude(s => s.Category).Include(w => w.Worker).Where(w => w.WorkerId == workerId).ToListAsync();
            return workerServices;
        }

        public async Task<WorkerServices> AddService(int? workerId, int? serviceId)
        {
            if (workerId == null || serviceId == null)
            {
                throw new ArgumentNullException();
            }

            var wService = new WorkerServices() { ServiceId = serviceId, WorkerId = (int)workerId };

            await _context.WorkerServices.AddAsync(wService);
            await _context.SaveChangesAsync();
            return wService;
        }

        public async Task<bool> RemoveService(int? workerId, int? serviceId)
        {
            if (workerId == null || serviceId == null)
            {
                throw new ArgumentNullException();
            }

            var wService = await _context.WorkerServices.FirstOrDefaultAsync(w => w.WorkerId == workerId && w.ServiceId == serviceId);

            if (wService == null)
            {
                return false;
            }
            else
            {
                _context.WorkerServices.Remove(wService);
                await _context.SaveChangesAsync();
                return true;
            }
        }


    }
}
