namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public int WorkerId { get; set; }
        public DateTime Day { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public List<Schedule> ToSchedule()
        {
            if (EndTime < StartTime)
            {
                throw new ArgumentException();
            }

            if (Day == DateTime.MinValue)
            {
                throw new ArgumentException();
            }

            var schedule = new List<Schedule>();
            var time = StartTime;

            for (int i = 0; i < EndTime - StartTime; i++)
            {
                var dateTime = new DateTime(Day.Year, Day.Month, Day.Day, time, 0, 0);
                schedule.Add(new Schedule { WorkerId = WorkerId, DateTime = dateTime });
                time++;
            }

            return schedule;
        }
    }
}
