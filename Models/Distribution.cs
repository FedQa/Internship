namespace Internship.Models
{
    public class Distribution
    {
        public List<Student> Students { get; set; }
        public List<Company> Companies { get; set; }
    }
    public class DistributionViewModel
    {
        public List<StudentViewModel> Students { get; set; }
        public Student Student { get; set; }
        public List<Student> ListStudents { get; set; }
        public List<Company> Companies { get; set; }
        public List<StudentCompanyPriority> StudentCompanyPriorities { get; set; }
    }

    public class StudentCompanyPriority
    {
        public int StudentId { get; set; }
        public int CompanyId { get; set; }
        public int Priority { get; set; }
    }
}

