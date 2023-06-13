using System;
using System.Collections.Generic;

namespace Internship.Models
{
    public partial class Contract
    {
        public int IdContract { get; set; }
        public int? IdCompany { get; set; }
        public DateTime? ContractDate { get; set; }
        public int? ContractTerm { get; set; }
        public bool? LongTerm { get; set; }

        public virtual Company? IdCompanyNavigation { get; set; }
    }
}
