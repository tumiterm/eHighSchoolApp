using System.ComponentModel.DataAnnotations;
using SchoolApp.eNums;

namespace SchoolApp.Models
{
    public class RSVP : Base
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public eAvailability Availability { get; set; }
        
    }
}
