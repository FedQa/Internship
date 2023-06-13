namespace Internship.Models
{
    public class StudentViewModel
    {
        public int IdStudent { get; set; }
        public int IdCompany { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Gpa { get; set; }
        public int Priority { get; set; }
        public int GroupId { get; set; }
        public string CompanyName { get; set; }
        public List<ApplicationViewModel> Applications { get; set; }
        public List<DistributionViewModel> Companies { get; set; }
    }
}
