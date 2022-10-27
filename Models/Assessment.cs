using SchoolApp.eNums;
using SchoolApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{ 
    public class Assessment : Base
    {
        [Key]
        public Guid AssessmentId { get; set; }
        public eSubject Subject { get; set; }
        public eClass Classroom { get; set; }
        public eGrade Grade { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Assessment Date")]
        public DateTime AssessmentDate { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Assessment Time")]
        public DateTime AssessmentTime { get; set; }

        [Display(Name = "Assessment Status")]
        public eStatus AssessmentStatus { get; set; }

        [Display(Name = "Assessment Type")]
        public eAssesType AssessmentType { get; set; }

        [Display(Name = "Send Message ?")]
        public bool SendMessage { get; set; }
    }
}
