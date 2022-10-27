using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class ParentMessage : Base
    {
        [Key]
        public Guid MessageId { get; set; }
        public string Message { get; set; }

        [Display(Name = "Message Type")]
        public eMessageType MessageType { get; set; }

        [Display(Name = "Attachment Type")]
        public eParentAttachment? AttachmentType { get; set; }

        [Display(Name = "Teacher")]
        public Guid? TeacherId { get; set; }

        [Display(Name = "Learner Name & Surname")]
        public string Learner { get; set; }
        public string? Attachment { get; set; }

        [NotMapped]
        [Display(Name = "Attachment File")]
        public IFormFile AttachmentFile { get; set; }

    }
}
