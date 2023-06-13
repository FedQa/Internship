using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Company
    {
        public Company()
        {
            Applications = new HashSet<Application>();
            Contracts = new HashSet<Contract>();
            PracticeManagers = new HashSet<PracticeManager>();
        }

        public int IdCompany { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public int? AvailableSeats { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<PracticeManager> PracticeManagers { get; set; }
    }
}
