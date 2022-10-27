using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class LearnerTeacherViewModel
    {
        public List<Assessment> Assessments { get; set; }
        public List<ParentMessage> ParentMessages { get; set; }
        public List<Learner> Learners { get; set; }
        public List<Meeting> Meetings { get; set; }

    }
}
