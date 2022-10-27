using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Subject
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Subject")]
        public eSubject Subj { get; set; }
        public Guid LearnerId { get; set; }

        [Display(Name = "Date of Enrollment")]
        public string? Date { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Is Done?")]
        public bool IsDone { get; set; }
    }
}
