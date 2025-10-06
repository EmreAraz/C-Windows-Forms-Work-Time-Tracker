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
            UpdateUsersGridViewWithAverageWorkTime(); // Ortalama Tablosunu y�kleyen metot
            descriptionLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            startDateLabel.Font = new Font("Arial", 10, FontStyle.Regular);
            LoadUsersAndTasks(); // G�revleri ve kullan�c�lar� y�kleyen metot
        }


        // Kullan�c�lar� ve g�revlerini y�kler -QUEST TABLE-
        private void LoadUsersAndTasks()
        {
            using (var db = new TaskContext())
            {
                // Kullan�c�lar� ve g�revlerini y�kle
                var usersWithTasks = db.Users.Include(u => u.Tasks).ToList();

                // DataGridView'� temizleyelim
                dataGridViewTasks.Rows.Clear();

                // T�m g�revleri DataGridView'a ekleyelim
                foreach (var user in usersWithTasks)
                {
                    if (user.Tasks != null)
                    {
                        foreach (var task in user.Tasks)
                        {
                            // Her g�rev i�in bir sat�r ekliyoruz  -QUEST TABLE-
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


        private void dataGridViewTasks_CellClick(object sender, DataGridViewCellEventArgs e) // DataGridView'deki bir h�creye t�kland���nda �al���r
        {
            // E�er t�klanan sat�r ge�erli bir sat�r ve s�tun "FinishTask" s�tunu ise i�leme ba�la
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewTasks.Columns["FinishTask"].Index)
            {
                // T�klanan sat�r se�iliyor
                var selectedRow = dataGridViewTasks.Rows[e.RowIndex];

                // Se�ili sat�rdaki "TaskID" h�cresinin de�eri al�n�yor
                var taskIdValue = selectedRow.Cells["TaskID"].Value;

                // E�er "TaskID" null ya da ge�ersiz bir de�er ise i�lem iptal ediliyor
                if (taskIdValue == null || taskIdValue == DBNull.Value)
                {
                    MessageBox.Show("Ge�ersiz g�rev ID'si.");
                    return;
                }

                // "TaskID" de�i�keni int olarak parse ediliyor
                int taskId;
                if (!int.TryParse(taskIdValue.ToString(), out taskId))
                {
                    MessageBox.Show("Ge�ersiz g�rev ID'si.");
                    return;
                }

                // Veritaban� ba�lant�s� a��l�yor
                using (var db = new TaskContext())
                {
                    // Veritaban�ndan t�klanan "TaskID" ile e�le�en g�rev al�n�yor
                    var task = db.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                    // E�er g�rev bulunamazsa uyar� veriliyor
                    if (task == null)
                    {
                        MessageBox.Show("G�rev bulunamad�.");
                        return;
                    }

                    // �u anki tarih ve saat al�n�yor
                    DateTime currentDate = DateTime.Now;

                    // G�revin "EndTime" (biti� zaman�) g�ncelleniyor
                    task.EndTime = currentDate;

                    // DataGridView'deki ilgili h�creye biti� tarihi yaz�l�yor
                    selectedRow.Cells["EndDate"].Value = currentDate;

                    // G�revin ba�lang�� tarihi al�n�yor
                    DateTime startDate = Convert.ToDateTime(selectedRow.Cells["StartDate"].Value);

                    // Mola s�resi tan�mlan�yor (1 saat)
                    TimeSpan breakDuration = new TimeSpan(1, 0, 0);

                    // �al��ma s�resi hesaplan�yor
                    double totalWorkTime = CalculateWorkTime(startDate, currentDate, breakDuration);

                    // �al��ma s�resinin saat ve dakika olarak ayr�lmas�
                    int totalHours = (int)totalWorkTime; // Tam saat k�sm�
                    int totalMinutes = (int)((totalWorkTime - totalHours) * 60); // Dakika k�sm�

                    // �al��ma s�resi DataGridView'deki "TotalDate" h�cresine yaz�l�yor
                    selectedRow.Cells["TotalDate"].Value = $"{totalHours} saat {totalMinutes} dakika";

                    // WorkLogs tablosuna yeni bir giri� ekleniyor
                    WorkLog workLog = new WorkLog
                    {
                        UserId = task.UserId,         // G�revi tamamlayan kullan�c�n�n ID'si
                        StartDate = startDate,        // G�revin ba�lang�� tarihi
                        EndDate = currentDate,        // G�revin biti� tarihi
                        TotalWorkHours = totalWorkTime // Toplam �al��ma s�resi (saat olarak)
                    };

                    // Veritaban�na yeni log ekleniyor ve kaydediliyor
                    try
                    {
                        db.WorkLogs.Add(workLog);
                        db.SaveChanges(); // Veritaban�na de�i�iklikler kaydediliyor
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Veritaban� hatas�: {ex.Message}");
                        return;
                    }
                }

                // Kullan�c�lar�n ortalama �al��ma s�resini g�ncelleyen bir fonksiyon �a��r�l�yor
                UpdateUsersGridViewWithAverageWorkTime();
            }
        }


        private void finishButton_Click(object sender, EventArgs e) // "Finish" butonuna t�kland���nda �al���r
        {
            // DataGridView'de se�ili sat�r al�n�yor
            var selectedRow = dataGridViewTasks.SelectedRows[0];

            // Se�ili sat�r�n "EndTime" h�cresindeki de�er al�n�yor
            var endTimeCellValue = selectedRow.Cells["EndTime"].Value;

            // �u anki tarih ve saat al�n�yor
            DateTime currentDate = DateTime.Now;

            // E�er "EndTime" h�cresinin de�eri null ya da DBNull.Value ise ge�erli tarih atan�yor
            if (endTimeCellValue == null || endTimeCellValue == DBNull.Value)
            {
                selectedRow.Cells["EndTime"].Value = currentDate; // "EndTime" h�cresine ge�erli tarih atan�yor
            }

            // G�rev ID'si al�n�yor
            int taskId = (int)selectedRow.Cells["TaskID"].Value;

            // Veritaban� ba�lant�s� olu�turuluyor
            using (var db = new TaskContext())
            {
                // G�rev ID'sine g�re ilgili g�rev veritaban�ndan al�n�yor
                var task = db.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                // E�er g�rev bulunursa, "EndTime" g�ncelleniyor ve kaydediliyor
                if (task != null)
                {
                    task.EndTime = currentDate; // G�revin biti� tarihi g�ncelleniyor
                    db.SaveChanges(); // Veritaban�na de�i�iklikler kaydediliyor
                }
            }

            // �al��ma s�resini ve di�er bilgileri i�eren bir WorkLog nesnesi olu�turuluyor
            WorkLog workLog = new WorkLog
            {
                StartDate = (DateTime)selectedRow.Cells["StartDate"].Value, // G�revin ba�lang�� tarihi
                EndDate = currentDate, // G�revin biti� tarihi
                TotalWorkHours = CalculateWorkTime((DateTime)selectedRow.Cells["StartDate"].Value, currentDate, TimeSpan.Zero), // Toplam �al��ma s�resi
                UserId = (int)selectedRow.Cells["UserID"].Value // G�revi tamamlayan kullan�c�n�n ID'si
            };

            // Olu�turulan WorkLog veritaban�na kaydediliyor
            try
            {
                using (var context = new TaskContext())
                {
                    context.WorkLogs.Add(workLog); // Yeni WorkLog kayd� ekleniyor
                    context.SaveChanges(); // De�i�iklikler veritaban�na kaydediliyor
                }
            }
            catch (Exception ex) // E�er bir hata olu�ursa kullan�c�ya mesaj g�steriliyor
            {
                MessageBox.Show("Veritaban� hatas�: " + ex.Message);
                return;
            }

            // Kullan�c�ya g�rev tamamland���na dair bilgi mesaj� g�steriliyor
            MessageBox.Show($"G�rev tamamland�. Biti� tarihi: {currentDate:dd/MM/yyyy HH:mm}");

            // G�revin tamamlanmas�n�n ard�ndan DataGridView yenileniyor
            LoadUsersAndTasks(); // DataGridView'� g�ncelleyen bir fonksiyon �a��r�l�yor
        }


        // Kullan�c�ya ait ortalama �al��ma s�resi hesaplama Ba�lang�c�
        private void UpdateUsersGridViewWithAverageWorkTime() // Kullan�c�lar�n ortalama �al��ma s�relerini g�ncelleyerek DataGridView'e yans�t�r
        {
            // Veritaban� ba�lant�s�n� ba�lat
            using (var context = new TaskContext())
            {
                // Kullan�c�lar�n bilgilerini g�steren GridView'in t�m s�tun ve sat�rlar�n� temizle
                usersGridView.Columns.Clear();
                usersGridView.Rows.Clear();

                // DataGridView'e s�tunlar� ekle
                usersGridView.Columns.Add("UserID", "User ID"); // Kullan�c� ID s�tunu
                usersGridView.Columns.Add("UserName", "Kullan�c� Ad�"); // Kullan�c� Ad� s�tunu
                usersGridView.Columns.Add("AverageWorkTime", "Ortalama �al��ma S�resi"); // Ortalama �al��ma S�resi s�tunu

                // T�m kullan�c�lar� veritaban�ndan al
                var users = context.Users.ToList();

                // Her kullan�c� i�in ortalama �al��ma s�resini hesapla ve tabloya ekle
                foreach (var user in users)
                {
                    // Kullan�c�n�n ortalama �al��ma s�resini hesapla
                    double averageWorkTime = CalculateUserAverageWorkTime(user);

                    // �al��ma s�resini saat ve dakika format�na d�n��t�r
                    int totalHours = (int)averageWorkTime; // Tam saat k�sm�n� al
                    int totalMinutes = (int)((averageWorkTime - totalHours) * 60); // Kalan k�sm� dakikaya �evir

                    // Kullan�c�n�n ID, ad� ve ortalama �al��ma s�resini DataGridView'e ekle
                    usersGridView.Rows.Add(user.Id, user.Name, $"{totalHours} saat {totalMinutes} dakika");
                }
            }
        }


        private double CalculateUserAverageWorkTime(ApplicationUser user)
        {
            using (var context = new TaskContext())
            {
                // Kullan�c�ya ait t�m WorkLogs verilerini al
                var userWorkLogs = context.WorkLogs
                                          .Where(w => w.UserId == user.Id) // Kullan�c� ID'sine g�re filtrele
                                          .ToList();

                // E�er kullan�c�ya ait i� kayd� yoksa, ortalama olarak 0 d�nd�r
                if (userWorkLogs.Count == 0)
                {
                    return 0; // E�er i� kayd� yoksa, ortalama 0 olur
                }

                // WorkLogs i�erisindeki TotalWorkHours de�erlerinden ortalama hesapla
                double averageWorkTime = userWorkLogs.Average(w => w.TotalWorkHours);

                return averageWorkTime; // Ortalamay� geri d�nd�r
            }
        }


        //�ALI�MA SAAT� KODU BA�LANGI�
        private double CalculateWorkTime(DateTime startTime, DateTime endTime, TimeSpan breakDuration)
        {
            // Tatil g�nleri tan�mlan�yor
            var holidays = new List<DateTime>
    {
        new DateTime(startTime.Year, 1, 1),   // 1 Ocak - Yeni Y�l
        new DateTime(startTime.Year, 4, 23),  // 23 Nisan - Ulusal Egemenlik ve �ocuk Bayram�
        new DateTime(startTime.Year, 5, 19),  // 19 May�s - Atat�rk'� Anma, Gen�lik ve Spor Bayram�
        new DateTime(startTime.Year, 8, 30),  // 30 A�ustos - Zafer Bayram�
        new DateTime(startTime.Year, 10, 29)  // 29 Ekim - Cumhuriyet Bayram�
    };

            double totalWorkHours = 0; // Toplam �al��ma saatlerini tutacak de�i�ken
            DateTime currentDate = startTime.Date;

            // **�zel Durum: 28 Ekim**
            DateTime october28 = new DateTime(startTime.Year, 10, 28); // 28 Ekim tarihi
            if (startTime.Date <= october28 && endTime.Date >= october28)
            {
                if (startTime.Date == october28)
                {
                    // 28 Ekim'deki �al��ma s�resi (4.5 saat)
                    DateTime october28WorkStart = new DateTime(october28.Year, october28.Month, october28.Day, 8, 0, 0);
                    DateTime october28WorkEnd = new DateTime(october28.Year, october28.Month, october28.Day, 12, 30, 0); // 12:30'da biti�

                    // Ba�lang�� ve biti� saatlerini s�n�rland�r
                    DateTime effectiveStart = startTime > october28WorkStart ? startTime : october28WorkStart;
                    DateTime effectiveEnd = endTime < october28WorkEnd ? endTime : october28WorkEnd;

                    // �al��ma s�resini hesapla
                    TimeSpan october28WorkDuration = effectiveEnd - effectiveStart;
                    if (october28WorkDuration.TotalHours > 0)
                    {
                        totalWorkHours += october28WorkDuration.TotalHours;
                    }
                }
                else if (startTime.Date < october28 && endTime.Date > october28)
                {
                    // E�er ba�lang�� ve biti� 28 Ekim'i kaps�yorsa, 4.5 saatlik �al��ma ekle
                    totalWorkHours += 4.5;
                }
            }            // **�zel Durum: 28 Ekim**


           
            // �al��ma saatlerini hesaplamak i�in g�n�n tarihine bak�yoruz
            while (currentDate <= endTime.Date)
            {
                //29 EK�M� MANUEL OLARAK ATLATTIM A�ILI�
                // 29 Ekim kontrol�
                if (currentDate.Day == 29 && currentDate.Month == 10)
                {
                    currentDate = currentDate.AddDays(1);
                    continue; // 29 Ekim'i atla
                }
                //29 EK�M� MANUEL OLARAK ATLATTIM KAPANI�

                // E�er 1 Ocak'a denk gelirse, sadece 1 Ocak'� d��ar�da b�rakaca��z
                if (currentDate.Day == 1 && currentDate.Month == 1)
                {
                    // 1 Ocak'ta �al��ma yap�lm�yor, sadece bu g�n�n �zerine �al��ma eklemiyoruz
                    currentDate = currentDate.AddDays(1);
                    continue; // Bu g�n� atla ve bir sonraki g�ne ge�
                }

                // Hafta sonu veya tatil kontrol�
                if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                    currentDate.DayOfWeek != DayOfWeek.Sunday &&
                    !holidays.Contains(currentDate)) // E�er tatil g�n� de�ilse
                {
                    // G�n�n �al��ma saatleri
                    DateTime dayStart = currentDate.AddHours(8); // 08:00
                    DateTime dayEnd = currentDate.AddHours(18);  // 18:00
                    DateTime lunchStart = currentDate.AddHours(12).AddMinutes(30); // 12:30
                    DateTime lunchEnd = currentDate.AddHours(13).AddMinutes(30);   // 13:30

                    // Ba�lang�� ve biti� saatleri aras�nda kalan �al��ma s�resini hesapla
                    DateTime effectiveStart = startTime > dayStart ? startTime : dayStart;
                    DateTime effectiveEnd = endTime < dayEnd ? endTime : dayEnd;

                    if (effectiveStart < effectiveEnd)
                    {
                        TimeSpan workDuration = effectiveEnd - effectiveStart;

                        // E�er �al��ma saatleri ��le molas�n� i�eriyorsa, mola s�resini d��
                        if (effectiveStart < lunchEnd && effectiveEnd > lunchStart)
                        {
                            TimeSpan overlap = lunchEnd - lunchStart;
                            if (effectiveStart >= lunchStart && effectiveEnd <= lunchEnd)
                            {
                                workDuration -= effectiveEnd - effectiveStart; // Tamamen moladaysa �al��ma yok
                            }
                            else if (effectiveStart < lunchStart && effectiveEnd > lunchEnd)
                            {
                                workDuration -= overlap; // Mola s�resini ��kar
                            }
                            else if (effectiveStart >= lunchStart)
                            {
                                workDuration -= lunchEnd - effectiveStart; // Molan�n ba�lang�� k�sm�n� ��kar
                            }
                            else if (effectiveEnd <= lunchEnd)
                            {
                                workDuration -= effectiveEnd - lunchStart; // Molan�n biti� k�sm�n� ��kar
                            }
                        }

                        totalWorkHours += workDuration.TotalHours;
                    }
                }
                else if (holidays.Contains(currentDate))  // Tatil g�nlerinde (1 Ocak gibi) �al��ma yok
                {
                    // Tatil g�nlerinde �al��ma saati 0 olmal�, bu y�zden bu g�nlerde hi� saat eklenmez
                    totalWorkHours += 0;
                }

                currentDate = currentDate.AddDays(1); // Bir sonraki g�ne ge�
            }

            // �al��ma saatini 2 ondal�k basama�a yuvarla
            return Math.Round(totalWorkHours, 2);
        }
        //�ALI�MA SAAT� KODU B�T��
    }
}
