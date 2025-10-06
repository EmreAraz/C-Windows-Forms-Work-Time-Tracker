using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime
{
    public class Holiday
    {
        public int Id { get; set; }  // Birincil anahtar
        public DateTime HolidayDate { get; set; }  // Tatil günü
        public string Description { get; set; }    // Tatilin açıklaması
        public TimeSpan? StartTime { get; set; }   // Başlangıç saati (nullable)
        public TimeSpan? EndTime { get; set; }     // Bitiş saati (nullable)
    }
}