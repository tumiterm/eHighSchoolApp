using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models
{
    public class Attachment : Base
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public eAttachment AttachmentType { get; set; }

        [Display(Name= "Attachment")]
        public string UserAttachment { get; set; }

        [NotMapped]
        public IFormFile AttachmentFile { get; set; }


    }
}
