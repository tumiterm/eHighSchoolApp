using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class LearnerParentViewModel
    {
        public List<Learner> learners { get; set; }
        public List<Parent> Parents { get; set; }
        public List<Meeting> Meetings { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<Attendance> Attendances { get; set; }



    }
}
