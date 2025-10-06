namespace WorkTime
{
    public class WorkLog
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalWorkHours { get; set; }  // Bu doğru alan olmalı

        // İlişkilendirilecek kullanıcı (isteğe bağlı)
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Eğer 'Duration' için ek bir alan kullanmak istemiyorsanız, bunu kaldırabilirsiniz.
        // Eğer 'Duration' hala kullanılacaksa, doğru ismi verin.
        // public int Duration { get; set; } 
    }
}
