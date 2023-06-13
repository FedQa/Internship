using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Practice
    {
        public int IdPractice { get; set; }
        public int? IdManager { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual PracticeManager? IdManagerNavigation { get; set; }
    }
}
