namespace Internship.Models
{
    public class MakePrioritiesViewModel
    {
        public Application Application { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
