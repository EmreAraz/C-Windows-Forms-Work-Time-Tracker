using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTime
{
    internal class DatabaseSeeder
    {
        public static void Seed(TaskContext context)
        {
            if (!context.Holidays.Any())
            {
                var holidays = new List<Holiday>
            {
                new Holiday { HolidayDate = new DateTime(DateTime.Now.Year, 1, 1), Description = "Yeni Yıl" },
                new Holiday { HolidayDate = new DateTime(DateTime.Now.Year, 5, 19), Description = "19 Mayıs Atatürk'ü Anma, Gençlik ve Spor Bayramı" },
                new Holiday { HolidayDate = new DateTime(DateTime.Now.Year, 8, 30), Description = "Zafer Bayramı" },
                new Holiday { HolidayDate = new DateTime(DateTime.Now.Year, 10, 29), Description = "Cumhuriyet Bayramı" },
                new Holiday { HolidayDate = new DateTime(DateTime.Now.Year, 4, 23), Description = "23 Nisan Ulusal Egemenlik ve Çocuk Bayramı" },

                // 28 Ekim için yarım gün tatil ekliyoruz
                new Holiday
                {
                    HolidayDate = new DateTime(DateTime.Now.Year, 10, 28),
                    Description = "28 Ekim Cumhuriyet Bayramı - Yarım Gün",
                    StartTime = new TimeSpan(8, 0, 0),  // 08:00 AM
                    EndTime = new TimeSpan(12, 30, 0)   // 12:30 PM
                }
            };

                context.Holidays.AddRange(holidays);
                context.SaveChanges();
            }
        }
    }

}

