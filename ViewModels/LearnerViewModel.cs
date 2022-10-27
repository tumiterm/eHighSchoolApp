using SchoolApp.Models;

namespace SchoolApp.ViewModels
{
    public class LearnerViewModel
    {
        public Learner LearnerModel { get; set; }
        public Parent LearnerParent { get; set; }
        public List<School> LearnerSchool { get; set; }
        public List<Subject> LearnerSubjects { get; set; }

        public List<Attendance> Attendance { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<Message> Messages { get; set; }
        public List<TimeTable> TimeTables { get; set; }

    }
}
