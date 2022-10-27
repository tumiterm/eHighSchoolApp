using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Parent : Base
    {
        public Guid ParentId { get; set; }

        [Display(Name = "Learner")]
        public Guid? LearnerId { get; set; }

        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public eRelationship Relationship {get; set;}

        [Display(Name = "RSA ID Copy")]
        public string? RSAID { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        public string Cellphone { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        public string Telephone { get; set; }
        public string? WorkNumber { get; set; }
        public string? Email { get; set; }


    }
}
