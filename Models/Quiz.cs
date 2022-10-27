using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Quiz : Base
    {
        [Key]
        public Guid QuizId { get; set; }
        public string QuizName { get; set; }
        public eSubject Subject { get; set; }
        public eGrade Grade { get; set; }
        public QuizResponse? Response { get; set; }

    }
}
