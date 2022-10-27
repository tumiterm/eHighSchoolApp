using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.ViewModels
{
    public class QuizResponseViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string QuizName = string.Empty;
        public eSubject Subject { get; set; }
        public eGrade Grade { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; }
    }
}
