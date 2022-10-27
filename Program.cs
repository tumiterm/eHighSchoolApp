using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Repository;
using SchoolApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Auth/OnSignIn";
    option.Cookie.Name = "SchoolAppCookie";
});


builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopLeft;
    config.HasRippleEffect = true;

});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork<Learner>, LearnerRepository>();
builder.Services.AddScoped<IUnitOfWork<School>, SchoolRepository>();
builder.Services.AddScoped<IUnitOfWork<Parent>, ParentRepository>();
builder.Services.AddScoped<IUnitOfWork<Subject>, SubjectsRepository>();
builder.Services.AddScoped<IUnitOfWork<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IUnitOfWork<Address>, AddressRepository>();
builder.Services.AddScoped<IUnitOfWork<Qualification>, QualificationRepository>();
builder.Services.AddScoped<IUnitOfWork<Attachment>, AttachmentRepository>();
builder.Services.AddScoped<IUnitOfWork<User>, UserRepository>();
builder.Services.AddScoped<IUnitOfWork<Attendance>, AttendanceRepository>();
builder.Services.AddScoped<IUnitOfWork<Message>, MessageRepository>();
builder.Services.AddScoped<IUnitOfWork<Meeting>, MeetingRepository>();
builder.Services.AddScoped<IUnitOfWork<TimeTable>, TimetableRepository>();
builder.Services.AddScoped<IUnitOfWork<ParentMessage>, ParentMessageRepository>();
builder.Services.AddScoped<IUnitOfWork<RSVP>, RSVPRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=OnSignIn}/{id?}");

app.Run();
