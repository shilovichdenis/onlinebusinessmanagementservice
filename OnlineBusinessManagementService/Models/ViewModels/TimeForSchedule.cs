using Microsoft.EntityFrameworkCore;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class TimeForSchedule
    {
        public int Id { get; set; }
        public string TimeString { get; set; }
        public int TimeInt { get; set; }
    }
}
