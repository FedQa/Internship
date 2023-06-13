using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Student
    {
        public Student()
        {
            Applications = new HashSet<Application>();
        }

        public int IdStudent { get; set; }
        public int? IdPractice { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? GroupId { get; set; }
        public decimal? Gpa { get; set; }
        public string? ResumeLink { get; set; }

        public virtual Report? Report { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}
