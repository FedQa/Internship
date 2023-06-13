using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class PracticeManager
    {
        public PracticeManager()
        {
            Practices = new HashSet<Practice>();
        }

        public int IdManager { get; set; }
        public int? IdCompany { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }

        public virtual Company? IdCompanyNavigation { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}
