using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Qualification : Base
    {
        [Key]
        public Guid QualificationId { get; set; }
        public Guid EmployeeId { get; set; }

        [Display(Name = "Qualification Type")]
        public eQualificationType QualificationType { get; set;}

        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime Till { get; set; }
        public string Institution { get; set; }

        [Display(Name = "Qualification Status")]
        public eQualificationStatus Status { get; set; }

        [NotMapped]
        [Display(Name = "Add More ?")]
        public bool AddMore { get; set; }

    }
}
