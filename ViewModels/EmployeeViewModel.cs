using SchoolApp.Models;

namespace SchoolApp.Services
{
    public class EmployeeViewModel
    {
        public Employee EmployeeModel { get; set; }
        public Address EmployeeAddress { get; set; }
        public List<Qualification> EmployeeQualifications { get; set; }

    }
}
