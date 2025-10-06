using System;

namespace WorkTime
{
    public class Task
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now; // Otomatik olarak oluşturulma tarihi
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public double? TotalWorkHours { get; set; } // Çalışma süresi, null olabileceği için nullable (?)



        // Navigational property
        public ApplicationUser User { get; set; }


    }
}

