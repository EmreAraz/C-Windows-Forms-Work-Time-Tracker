using System;
using System.Windows.Forms;

namespace WorkTime
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var context = new TaskContext())
            {
                // Seed metodunu çaðýrýyoruz ve loglama yapýyoruz
                DatabaseSeeder.Seed(context);

                // Tatil günlerini kontrol etme (Debug log)
                var holidays = context.Holidays.ToList();
                Console.WriteLine($"Toplam Tatil Gün Sayýsý: {holidays.Count}");
                foreach (var holiday in holidays)
                {
                    Console.WriteLine($"{holiday.HolidayDate.ToShortDateString()} - {holiday.Description}");
                }
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
