using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class QuizStart : Base
    {

        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Quiz Questions")]
        public Guid AssociativeId { get; set; }
        public eResponseType? ResponseType { get; set; }
        public string? Answer { get; set; }
    }
}
