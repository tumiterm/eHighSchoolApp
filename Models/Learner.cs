using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Learner : Base
    {
        [Key]
        public Guid LearnerId { get; set; }

        [Display(Name = "Reference")]
        public string? LearnerReference { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "ID Number")]
        [StringLength(13)]
        [MaxLength(13)]
        public string? RSAIDNumber { get; set; } 

        [Display(Name = "Passport Number")]
        public string? PassportNumber { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public eGrade Grade { get; set; }

        [Display(Name = "Intake Cycle")]
        public eIntake IntakeCycle { get; set; }
        public eGender Gender { get; set; }
        public eNationality Nationality { get; set; }

        [Display(Name = "Has Disability")]
        public eSelection HasDisability { get; set; }
        public eDisability? Disability {get; set;}

        [Display(Name = "Is SA Citizen?")]
        public eCitizen IsSaCitizen { get; set; }

        [Display(Name = "Address Type")]
        public eAddressType AddressType { get; set; }

        [Display(Name="Street/Complex Number")]
        public string AddressLine1 { get; set; }

        [Display(Name = "City")]
        public string? AddressLine2 { get; set; }
        public eProvince? Province { get; set; }

        [Display(Name = "Postal Code")]
        public string Postal { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [StringLength(10)]
        public string? Cellphone { get; set; }
        public string? IDPassport { get; set; }
        public string? ReportCard { get; set; }

        [NotMapped]
        public IFormFile? IDPassportFile { get; set; }
        [NotMapped]
        public IFormFile? ReportCardFile { get; set; }





    }
}
