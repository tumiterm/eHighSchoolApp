using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Meeting : Base
    {
        [Key]
        public Guid MeetingId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Meeting Messages")]
        public string Message { get; set; }

        [Display(Name = "For Grade?")]
        public eGrade Grade { get; set; }
        public List<Parent>? Parent { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Meeting Date")]
        public string Date { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Meeting Time")]
        public string Time { get; set; }

        [Display(Name = "Meeting Venue")]
        public eClass Venue { get; set; }
        public eUrgency Urgency { get; set; }

    }
}
