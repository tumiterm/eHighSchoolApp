using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.eNums;

namespace SchoolApp.Models
{
    public class TimeTable : Base
    {
        [Key]
        public Guid Id { get; set; }
        public eSubject Subject { get; set; }
        public eGrade Grade { get; set; }
        public string Title { get; set; }
        public string? Attachment { get; set; }
        [NotMapped]
        [Display(Name = "Attach File")]
        public IFormFile AttachFile { get; set; }

    }
}
