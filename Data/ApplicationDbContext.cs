using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;
using SchoolApp.Services;
using SchoolApp.Controllers;
using SchoolApp.ViewModels;

namespace SchoolApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Assessment> Assessment { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<TimeTable> EventTimeTable { get; set; }
        public DbSet<ParentMessage> ParentMessages { get; set; }
        public DbSet<RSVP> RSVP { get; set; }
        public DbSet<QuizResponse> Responses { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<QuizStart> QuizStart { get; set; }
        public DbSet<GradeQuiz> GradeQuiz { get; set; }

        



    }
}

