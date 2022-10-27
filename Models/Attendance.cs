using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Attendance : Base
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LearnerId { get; set; }

        [Display(Name = "Attendance Cycle")]
        public eAttandanceCycle Cycle { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime Till { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

        [Display(Name = "Days Absent")]
        public int Absent { get; set; }
    }
}
