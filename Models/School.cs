using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class School : Base
    {
        [Key]
        public Guid SchoolId { get; set; }

        [Display(Name="School Name")]
        public string Name { get; set; }

        [Display(Name = "School Type")]
        public eType Type { get; set; }

        [Display(Name = "School Logo")]
        public string? Logo { get; set; }

        [Display(Name = "Telephone Number")]
        public string? Tel { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "School Website")]
        [DataType(DataType.Url)]
        public string? Website { get; set; }
    }
}
