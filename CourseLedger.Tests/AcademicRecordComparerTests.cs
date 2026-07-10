using CourseLedger.DataAccess;
using CourseLedger.Models;

namespace CourseLedger.Tests;

public class AcademicRecordComparerTests
{
    private static AcademicRecord MakeRecord(string courseTitle, string studentName, int? grade = 80)
    {
        return new AcademicRecord
        {
            StudentId = studentName,
            CourseCode = courseTitle,
            Grade = grade,
            CourseCodeNavigation = new Course { Code = courseTitle, Title = courseTitle },
            Student = new Student { Id = studentName, Name = studentName }
        };
    }

    [Fact]
    public void Compare_SortsByCourseTitle_Ascending()
    {
        var comparer = new AcademicRecordComparer("course", sortAscending: true);
        var a = MakeRecord("Alpha Course", "Zoe");
        var b = MakeRecord("Beta Course", "Amy");

        Assert.True(comparer.Compare(a, b) < 0);
        Assert.True(comparer.Compare(b, a) > 0);
    }

    [Fact]
    public void Compare_SortsByCourseTitle_Descending()
    {
        var comparer = new AcademicRecordComparer("course", sortAscending: false);
        var a = MakeRecord("Alpha Course", "Zoe");
        var b = MakeRecord("Beta Course", "Amy");

        Assert.True(comparer.Compare(a, b) > 0);
    }

    [Fact]
    public void Compare_SortsByStudentName_Ascending()
    {
        var comparer = new AcademicRecordComparer("student", sortAscending: true);
        var a = MakeRecord("CST8256", "Amy");
        var b = MakeRecord("CST8256", "Zoe");

        Assert.True(comparer.Compare(a, b) < 0);
    }

    [Fact]
    public void Compare_NullGradesSortFirst()
    {
        var comparer = new AcademicRecordComparer("course", sortAscending: true);
        var withGrade = MakeRecord("Alpha", "Student", 90);
        var withoutGrade = MakeRecord("Beta", "Student", null);

        Assert.True(comparer.Compare(withoutGrade, withGrade) < 0);
    }
}
