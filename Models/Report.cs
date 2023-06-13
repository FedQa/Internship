using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Report
    {
        public int IdReport { get; set; }
        public int IdStudent { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Mark { get; set; }

        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
