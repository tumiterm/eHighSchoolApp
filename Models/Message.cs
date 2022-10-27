using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Message : Base
    {
        public Guid MessageId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string MessageInfo { get; set; }
        public List<Learner>? Learners { get; set; }

        [Display(Name = "Learner")]
        public Guid? LearnerId { get; set; }
        public eSubject Subject { get; set; }
        public bool IsAllLearners { get; set; }

    }
}
