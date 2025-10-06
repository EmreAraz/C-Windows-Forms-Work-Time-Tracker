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
                // Seed metodunu �a��r�yoruz ve loglama yap�yoruz
                DatabaseSeeder.Seed(context);

                // Tatil g�nlerini kontrol etme (Debug log)
                var holidays = context.Holidays.ToList();
                Console.WriteLine($"Toplam Tatil G�n Say�s�: {holidays.Count}");
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
