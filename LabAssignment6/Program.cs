using LabAssignment6.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddDbContext<StudentrecordContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentRecordConnection")));

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StudentrecordContext>();
    
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Seed data if database is empty
    if (!context.Courses.Any())
    {
        // Add Courses
        var courses = new[]
        {
            new Course { Code = "CST8256", Title = "Web Programming Languages I", Description = "Introduction to web programming", HoursPerWeek = 4, FeeBase = 500.00m },
            new Course { Code = "CST8257", Title = "Web Programming Languages II", Description = "Advanced web programming", HoursPerWeek = 4, FeeBase = 550.00m },
            new Course { Code = "CST8284", Title = "Object-Oriented Programming", Description = "OOP concepts and design patterns", HoursPerWeek = 4, FeeBase = 600.00m },
            new Course { Code = "CST8285", Title = "Database Systems", Description = "Database design and SQL", HoursPerWeek = 3, FeeBase = 450.00m }
        };
        context.Courses.AddRange(courses);
        
        // Add Students
        var students = new[]
        {
            new Student { Id = "S001", Name = "John Smith" },
            new Student { Id = "S002", Name = "Jane Doe" },
            new Student { Id = "S003", Name = "Bob Johnson" },
            new Student { Id = "S004", Name = "Alice Williams" },
            new Student { Id = "S005", Name = "Charlie Brown" }
        };
        context.Students.AddRange(students);
        
        // Add Academic Records
        var academicRecords = new[]
        {
            new AcademicRecord { StudentId = "S001", CourseCode = "CST8256", Grade = 85 },
            new AcademicRecord { StudentId = "S001", CourseCode = "CST8257", Grade = 90 },
            new AcademicRecord { StudentId = "S002", CourseCode = "CST8256", Grade = 92 },
            new AcademicRecord { StudentId = "S002", CourseCode = "CST8284", Grade = 88 },
            new AcademicRecord { StudentId = "S003", CourseCode = "CST8256", Grade = 75 },
            new AcademicRecord { StudentId = "S003", CourseCode = "CST8285", Grade = 80 },
            new AcademicRecord { StudentId = "S004", CourseCode = "CST8257", Grade = 95 },
            new AcademicRecord { StudentId = "S004", CourseCode = "CST8284", Grade = 87 },
            new AcademicRecord { StudentId = "S005", CourseCode = "CST8256", Grade = 78 },
            new AcademicRecord { StudentId = "S005", CourseCode = "CST8285", Grade = 82 }
        };
        context.AcademicRecords.AddRange(academicRecords);
        
        // Add Roles
        var roles = new[]
        {
            new Role { Id = 1, Role1 = "Administrator" },
            new Role { Id = 2, Role1 = "Instructor" },
            new Role { Id = 3, Role1 = "Staff" }
        };
        context.Roles.AddRange(roles);
        
        // Add Employees
        var employees = new[]
        {
            new Employee { Id = 1, Name = "Admin User", UserName = "admin", Password = "admin123" },
            new Employee { Id = 2, Name = "John Instructor", UserName = "instructor", Password = "instructor123" },
            new Employee { Id = 3, Name = "Staff Member", UserName = "staff", Password = "staff123" }
        };
        context.Employees.AddRange(employees);
        
        // Assign roles to employees
        employees[0].Roles.Add(roles[0]); // Admin has Administrator role
        employees[1].Roles.Add(roles[1]); // Instructor has Instructor role
        employees[2].Roles.Add(roles[2]); // Staff has Staff role
        
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
