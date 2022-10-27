using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class GradeQuiz : Base
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name="Learner")]
        public Guid LearnerId { get; set; }

        [Display(Name = "Quiz")]
        public Guid QuizId { get; set; }

        [Display(Name = "Mark Obtained")]
        public int Mark { get; set; }
    }
}
