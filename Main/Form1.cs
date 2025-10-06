using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace WorkTime
{
    public partial class Form1 : Form
    {
        private List<ApplicationUser> users;
        private List<Task> tasks = new List<Task>();


        public Form1()
        {
            InitializeComponent();
            LoadUsersAndTasks();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateUsersGridViewWithAverageWorkTime(); // Ortalama Tablosunu yükleyen metot
            descriptionLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            startDateLabel.Font = new Font("Arial", 10, FontStyle.Regular);
            LoadUsersAndTasks(); // Görevleri ve kullanýcýlarý yükleyen metot
        }


        // Kullanýcýlarý ve görevlerini yükler -QUEST TABLE-
        private void LoadUsersAndTasks()
        {
            using (var db = new TaskContext())
            {
                // Kullanýcýlarý ve görevlerini yükle
                var usersWithTasks = db.Users.Include(u => u.Tasks).ToList();

                // DataGridView'ý temizleyelim
                dataGridViewTasks.Rows.Clear();

                // Tüm görevleri DataGridView'a ekleyelim
                foreach (var user in usersWithTasks)
                {
                    if (user.Tasks != null)
                    {
                        foreach (var task in user.Tasks)
                        {
                            // Her görev için bir satýr ekliyoruz  -QUEST TABLE-
                            dataGridViewTasks.Rows.Add(
                                user.Name,
                                task.TaskId,
                                task.Description,
                                task.StartDate,
                                task.EndTime,
                                task.TotalWorkHours
                            );
                        }
                    }
                }
            }
        }


        private void dataGridViewTasks_CellClick(object sender, DataGridViewCellEventArgs e) // DataGridView'deki bir hücreye týklandýðýnda çalýþýr
        {
            // Eðer týklanan satýr geçerli bir satýr ve sütun "FinishTask" sütunu ise iþleme baþla
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewTasks.Columns["FinishTask"].Index)
            {
                // Týklanan satýr seçiliyor
                var selectedRow = dataGridViewTasks.Rows[e.RowIndex];

                // Seçili satýrdaki "TaskID" hücresinin deðeri alýnýyor
                var taskIdValue = selectedRow.Cells["TaskID"].Value;

                // Eðer "TaskID" null ya da geçersiz bir deðer ise iþlem iptal ediliyor
                if (taskIdValue == null || taskIdValue == DBNull.Value)
                {
                    MessageBox.Show("Geçersiz görev ID'si.");
                    return;
                }

                // "TaskID" deðiþkeni int olarak parse ediliyor
                int taskId;
                if (!int.TryParse(taskIdValue.ToString(), out taskId))
                {
                    MessageBox.Show("Geçersiz görev ID'si.");
                    return;
                }

                // Veritabaný baðlantýsý açýlýyor
                using (var db = new TaskContext())
                {
                    // Veritabanýndan týklanan "TaskID" ile eþleþen görev alýnýyor
                    var task = db.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                    // Eðer görev bulunamazsa uyarý veriliyor
                    if (task == null)
                    {
                        MessageBox.Show("Görev bulunamadý.");
                        return;
                    }

                    // Þu anki tarih ve saat alýnýyor
                    DateTime currentDate = DateTime.Now;

                    // Görevin "EndTime" (bitiþ zamaný) güncelleniyor
                    task.EndTime = currentDate;

                    // DataGridView'deki ilgili hücreye bitiþ tarihi yazýlýyor
                    selectedRow.Cells["EndDate"].Value = currentDate;

                    // Görevin baþlangýç tarihi alýnýyor
                    DateTime startDate = Convert.ToDateTime(selectedRow.Cells["StartDate"].Value);

                    // Mola süresi tanýmlanýyor (1 saat)
                    TimeSpan breakDuration = new TimeSpan(1, 0, 0);

                    // Çalýþma süresi hesaplanýyor
                    double totalWorkTime = CalculateWorkTime(startDate, currentDate, breakDuration);

                    // Çalýþma süresinin saat ve dakika olarak ayrýlmasý
                    int totalHours = (int)totalWorkTime; // Tam saat kýsmý
                    int totalMinutes = (int)((totalWorkTime - totalHours) * 60); // Dakika kýsmý

                    // Çalýþma süresi DataGridView'deki "TotalDate" hücresine yazýlýyor
                    selectedRow.Cells["TotalDate"].Value = $"{totalHours} saat {totalMinutes} dakika";

                    // WorkLogs tablosuna yeni bir giriþ ekleniyor
                    WorkLog workLog = new WorkLog
                    {
                        UserId = task.UserId,         // Görevi tamamlayan kullanýcýnýn ID'si
                        StartDate = startDate,        // Görevin baþlangýç tarihi
                        EndDate = currentDate,        // Görevin bitiþ tarihi
                        TotalWorkHours = totalWorkTime // Toplam çalýþma süresi (saat olarak)
                    };

                    // Veritabanýna yeni log ekleniyor ve kaydediliyor
                    try
                    {
                        db.WorkLogs.Add(workLog);
                        db.SaveChanges(); // Veritabanýna deðiþiklikler kaydediliyor
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Veritabaný hatasý: {ex.Message}");
                        return;
                    }
                }

                // Kullanýcýlarýn ortalama çalýþma süresini güncelleyen bir fonksiyon çaðýrýlýyor
                UpdateUsersGridViewWithAverageWorkTime();
            }
        }


        private void finishButton_Click(object sender, EventArgs e) // "Finish" butonuna týklandýðýnda çalýþýr
        {
            // DataGridView'de seçili satýr alýnýyor
            var selectedRow = dataGridViewTasks.SelectedRows[0];

            // Seçili satýrýn "EndTime" hücresindeki deðer alýnýyor
            var endTimeCellValue = selectedRow.Cells["EndTime"].Value;

            // Þu anki tarih ve saat alýnýyor
            DateTime currentDate = DateTime.Now;

            // Eðer "EndTime" hücresinin deðeri null ya da DBNull.Value ise geçerli tarih atanýyor
            if (endTimeCellValue == null || endTimeCellValue == DBNull.Value)
            {
                selectedRow.Cells["EndTime"].Value = currentDate; // "EndTime" hücresine geçerli tarih atanýyor
            }

            // Görev ID'si alýnýyor
            int taskId = (int)selectedRow.Cells["TaskID"].Value;

            // Veritabaný baðlantýsý oluþturuluyor
            using (var db = new TaskContext())
            {
                // Görev ID'sine göre ilgili görev veritabanýndan alýnýyor
                var task = db.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                // Eðer görev bulunursa, "EndTime" güncelleniyor ve kaydediliyor
                if (task != null)
                {
                    task.EndTime = currentDate; // Görevin bitiþ tarihi güncelleniyor
                    db.SaveChanges(); // Veritabanýna deðiþiklikler kaydediliyor
                }
            }

            // Çalýþma süresini ve diðer bilgileri içeren bir WorkLog nesnesi oluþturuluyor
            WorkLog workLog = new WorkLog
            {
                StartDate = (DateTime)selectedRow.Cells["StartDate"].Value, // Görevin baþlangýç tarihi
                EndDate = currentDate, // Görevin bitiþ tarihi
                TotalWorkHours = CalculateWorkTime((DateTime)selectedRow.Cells["StartDate"].Value, currentDate, TimeSpan.Zero), // Toplam çalýþma süresi
                UserId = (int)selectedRow.Cells["UserID"].Value // Görevi tamamlayan kullanýcýnýn ID'si
            };

            // Oluþturulan WorkLog veritabanýna kaydediliyor
            try
            {
                using (var context = new TaskContext())
                {
                    context.WorkLogs.Add(workLog); // Yeni WorkLog kaydý ekleniyor
                    context.SaveChanges(); // Deðiþiklikler veritabanýna kaydediliyor
                }
            }
            catch (Exception ex) // Eðer bir hata oluþursa kullanýcýya mesaj gösteriliyor
            {
                MessageBox.Show("Veritabaný hatasý: " + ex.Message);
                return;
            }

            // Kullanýcýya görev tamamlandýðýna dair bilgi mesajý gösteriliyor
            MessageBox.Show($"Görev tamamlandý. Bitiþ tarihi: {currentDate:dd/MM/yyyy HH:mm}");

            // Görevin tamamlanmasýnýn ardýndan DataGridView yenileniyor
            LoadUsersAndTasks(); // DataGridView'ý güncelleyen bir fonksiyon çaðýrýlýyor
        }


        // Kullanýcýya ait ortalama çalýþma süresi hesaplama Baþlangýcý
        private void UpdateUsersGridViewWithAverageWorkTime() // Kullanýcýlarýn ortalama çalýþma sürelerini güncelleyerek DataGridView'e yansýtýr
        {
            // Veritabaný baðlantýsýný baþlat
            using (var context = new TaskContext())
            {
                // Kullanýcýlarýn bilgilerini gösteren GridView'in tüm sütun ve satýrlarýný temizle
                usersGridView.Columns.Clear();
                usersGridView.Rows.Clear();

                // DataGridView'e sütunlarý ekle
                usersGridView.Columns.Add("UserID", "User ID"); // Kullanýcý ID sütunu
                usersGridView.Columns.Add("UserName", "Kullanýcý Adý"); // Kullanýcý Adý sütunu
                usersGridView.Columns.Add("AverageWorkTime", "Ortalama Çalýþma Süresi"); // Ortalama Çalýþma Süresi sütunu

                // Tüm kullanýcýlarý veritabanýndan al
                var users = context.Users.ToList();

                // Her kullanýcý için ortalama çalýþma süresini hesapla ve tabloya ekle
                foreach (var user in users)
                {
                    // Kullanýcýnýn ortalama çalýþma süresini hesapla
                    double averageWorkTime = CalculateUserAverageWorkTime(user);

                    // Çalýþma süresini saat ve dakika formatýna dönüþtür
                    int totalHours = (int)averageWorkTime; // Tam saat kýsmýný al
                    int totalMinutes = (int)((averageWorkTime - totalHours) * 60); // Kalan kýsmý dakikaya çevir

                    // Kullanýcýnýn ID, adý ve ortalama çalýþma süresini DataGridView'e ekle
                    usersGridView.Rows.Add(user.Id, user.Name, $"{totalHours} saat {totalMinutes} dakika");
                }
            }
        }


        private double CalculateUserAverageWorkTime(ApplicationUser user)
        {
            using (var context = new TaskContext())
            {
                // Kullanýcýya ait tüm WorkLogs verilerini al
                var userWorkLogs = context.WorkLogs
                                          .Where(w => w.UserId == user.Id) // Kullanýcý ID'sine göre filtrele
                                          .ToList();

                // Eðer kullanýcýya ait iþ kaydý yoksa, ortalama olarak 0 döndür
                if (userWorkLogs.Count == 0)
                {
                    return 0; // Eðer iþ kaydý yoksa, ortalama 0 olur
                }

                // WorkLogs içerisindeki TotalWorkHours deðerlerinden ortalama hesapla
                double averageWorkTime = userWorkLogs.Average(w => w.TotalWorkHours);

                return averageWorkTime; // Ortalamayý geri döndür
            }
        }


        //ÇALIÞMA SAATÝ KODU BAÞLANGIÇ
        private double CalculateWorkTime(DateTime startTime, DateTime endTime, TimeSpan breakDuration)
        {
            // Tatil günleri tanýmlanýyor
            var holidays = new List<DateTime>
    {
        new DateTime(startTime.Year, 1, 1),   // 1 Ocak - Yeni Yýl
        new DateTime(startTime.Year, 4, 23),  // 23 Nisan - Ulusal Egemenlik ve Çocuk Bayramý
        new DateTime(startTime.Year, 5, 19),  // 19 Mayýs - Atatürk'ü Anma, Gençlik ve Spor Bayramý
        new DateTime(startTime.Year, 8, 30),  // 30 Aðustos - Zafer Bayramý
        new DateTime(startTime.Year, 10, 29)  // 29 Ekim - Cumhuriyet Bayramý
    };

            double totalWorkHours = 0; // Toplam çalýþma saatlerini tutacak deðiþken
            DateTime currentDate = startTime.Date;

            // **Özel Durum: 28 Ekim**
            DateTime october28 = new DateTime(startTime.Year, 10, 28); // 28 Ekim tarihi
            if (startTime.Date <= october28 && endTime.Date >= october28)
            {
                if (startTime.Date == october28)
                {
                    // 28 Ekim'deki çalýþma süresi (4.5 saat)
                    DateTime october28WorkStart = new DateTime(october28.Year, october28.Month, october28.Day, 8, 0, 0);
                    DateTime october28WorkEnd = new DateTime(october28.Year, october28.Month, october28.Day, 12, 30, 0); // 12:30'da bitiþ

                    // Baþlangýç ve bitiþ saatlerini sýnýrlandýr
                    DateTime effectiveStart = startTime > october28WorkStart ? startTime : october28WorkStart;
                    DateTime effectiveEnd = endTime < october28WorkEnd ? endTime : october28WorkEnd;

                    // Çalýþma süresini hesapla
                    TimeSpan october28WorkDuration = effectiveEnd - effectiveStart;
                    if (october28WorkDuration.TotalHours > 0)
                    {
                        totalWorkHours += october28WorkDuration.TotalHours;
                    }
                }
                else if (startTime.Date < october28 && endTime.Date > october28)
                {
                    // Eðer baþlangýç ve bitiþ 28 Ekim'i kapsýyorsa, 4.5 saatlik çalýþma ekle
                    totalWorkHours += 4.5;
                }
            }            // **Özel Durum: 28 Ekim**


           
            // Çalýþma saatlerini hesaplamak için günün tarihine bakýyoruz
            while (currentDate <= endTime.Date)
            {
                //29 EKÝMÝ MANUEL OLARAK ATLATTIM AÇILIÞ
                // 29 Ekim kontrolü
                if (currentDate.Day == 29 && currentDate.Month == 10)
                {
                    currentDate = currentDate.AddDays(1);
                    continue; // 29 Ekim'i atla
                }
                //29 EKÝMÝ MANUEL OLARAK ATLATTIM KAPANIÞ

                // Eðer 1 Ocak'a denk gelirse, sadece 1 Ocak'ý dýþarýda býrakacaðýz
                if (currentDate.Day == 1 && currentDate.Month == 1)
                {
                    // 1 Ocak'ta çalýþma yapýlmýyor, sadece bu günün üzerine çalýþma eklemiyoruz
                    currentDate = currentDate.AddDays(1);
                    continue; // Bu günü atla ve bir sonraki güne geç
                }

                // Hafta sonu veya tatil kontrolü
                if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                    currentDate.DayOfWeek != DayOfWeek.Sunday &&
                    !holidays.Contains(currentDate)) // Eðer tatil günü deðilse
                {
                    // Günün çalýþma saatleri
                    DateTime dayStart = currentDate.AddHours(8); // 08:00
                    DateTime dayEnd = currentDate.AddHours(18);  // 18:00
                    DateTime lunchStart = currentDate.AddHours(12).AddMinutes(30); // 12:30
                    DateTime lunchEnd = currentDate.AddHours(13).AddMinutes(30);   // 13:30

                    // Baþlangýç ve bitiþ saatleri arasýnda kalan çalýþma süresini hesapla
                    DateTime effectiveStart = startTime > dayStart ? startTime : dayStart;
                    DateTime effectiveEnd = endTime < dayEnd ? endTime : dayEnd;

                    if (effectiveStart < effectiveEnd)
                    {
                        TimeSpan workDuration = effectiveEnd - effectiveStart;

                        // Eðer çalýþma saatleri öðle molasýný içeriyorsa, mola süresini düþ
                        if (effectiveStart < lunchEnd && effectiveEnd > lunchStart)
                        {
                            TimeSpan overlap = lunchEnd - lunchStart;
                            if (effectiveStart >= lunchStart && effectiveEnd <= lunchEnd)
                            {
                                workDuration -= effectiveEnd - effectiveStart; // Tamamen moladaysa çalýþma yok
                            }
                            else if (effectiveStart < lunchStart && effectiveEnd > lunchEnd)
                            {
                                workDuration -= overlap; // Mola süresini çýkar
                            }
                            else if (effectiveStart >= lunchStart)
                            {
                                workDuration -= lunchEnd - effectiveStart; // Molanýn baþlangýç kýsmýný çýkar
                            }
                            else if (effectiveEnd <= lunchEnd)
                            {
                                workDuration -= effectiveEnd - lunchStart; // Molanýn bitiþ kýsmýný çýkar
                            }
                        }

                        totalWorkHours += workDuration.TotalHours;
                    }
                }
                else if (holidays.Contains(currentDate))  // Tatil günlerinde (1 Ocak gibi) çalýþma yok
                {
                    // Tatil günlerinde çalýþma saati 0 olmalý, bu yüzden bu günlerde hiç saat eklenmez
                    totalWorkHours += 0;
                }

                currentDate = currentDate.AddDays(1); // Bir sonraki güne geç
            }

            // Çalýþma saatini 2 ondalýk basamaða yuvarla
            return Math.Round(totalWorkHours, 2);
        }
        //ÇALIÞMA SAATÝ KODU BÝTÝÞ
    }
}
