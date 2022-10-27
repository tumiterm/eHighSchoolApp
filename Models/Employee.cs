using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Employee : Base
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Display(Name = "RSA ID Number")]
        public string? RSAIdNumber { get; set; }

        [Display(Name = "Passport Number")]
        public string? PassportNum { get; set; }
        public eTitle Title { get; set; }
        public eGender Gender { get; set; }
        public eDisability Disability { get; set; }
        public eRace Race { get; set; }

        [Display(Name = "Home Language")]
        public eLanguage HomeLanguage { get; set; }

        [Display(Name = "Upload ID / Passport")]
        public string? IDPassportFile { get; set; }
        public eNationality Nationality { get; set; }

        [Display(Name = "Employee Type")]
        public eEmployeeType EmployeeType { get; set; }

        [Display(Name = "Employee Status")]
        public eEmployeeStatus EmployeeStatus { get; set; }

        [Display(Name = "Employment Type")]
        public eEmploymentType EmploymentType { get; set; }


        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        public string Cellphone { get; set; }
        
        [Display(Name="Alternative Cellphone")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        public string? AlternativeCell { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Grade To Teach")]
        public eGrade? Grade { get; set; }

        [NotMapped]
        public IFormFile? IDPassportFormFile { get; set; }

    }
}
