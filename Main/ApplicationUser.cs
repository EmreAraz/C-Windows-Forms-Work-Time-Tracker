using System.Collections.Generic;

namespace WorkTime
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

    }
}
