using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class QuizResponse : Base
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AssociativeKey { get; set; }
        public string Question { get; set; } = string.Empty;
        public string? Answer { get; set; }
    }
}
