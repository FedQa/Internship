using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Application
    {
        public int IdApplication { get; set; }
        public int IdStudent { get; set; }
        public int IdCompany { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? Status { get; set; }
        public int? Priority { get; set; }

        public virtual Company IdCompanyNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
