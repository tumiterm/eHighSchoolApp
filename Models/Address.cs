using SchoolApp.eNums;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Address : Base
    {
        [Key]
        public Guid AddressId { get; set; }
        public Guid EmployeeId { get; set; }

        [Display(Name = "Unit / House / Complex Number")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Surburb / City")]
        public string? AddressLine2 { get; set; }
        public eProvince Province { get; set; }

        [Display(Name = "Postal Code")]
        [MaxLength(4)]
        [StringLength(4)]
        public string PostalCode2 { get; set; }

        [Display(Name = "Is The Same ?")]
        public bool IsSame { get; set; }

        [Display(Name = "Unit / House / Complex Number")]
        public string? PostalAddressLine { get; set; }

        [Display(Name = "Surburb / City")]
        public string? PostalAddressLine2 { get; set; }

        [Display(Name = "Province")]
        public eProvince? PostalProvince { get; set; }

        [Display(Name = "Postal Code")]
        [MaxLength(4)]
        [StringLength(4)]
        public string PostalCode { get; set; }

    }
}
