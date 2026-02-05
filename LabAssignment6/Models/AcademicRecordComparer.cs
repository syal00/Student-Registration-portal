using LabAssignment6.DataAccess;

namespace LabAssignment6.Models
{
    public class AcademicRecordComparer : IComparer<AcademicRecord>
    {
        private readonly string _sortField;
        private readonly bool _sortAscending;

        public AcademicRecordComparer(string sortField, bool sortAscending = true)
        {
            _sortField = sortField;
            _sortAscending = sortAscending;
        }

        public int Compare(AcademicRecord? x, AcademicRecord? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            int result = 0;

            // Always put null grades first
            if (x.Grade == null && y.Grade != null) return -1;
            if (x.Grade != null && y.Grade == null) return 1;
            if (x.Grade == null && y.Grade == null)
            {
                // Both are null, sort by the specified field
                result = CompareByField(x, y);
            }
            else
            {
                // Both have grades, sort by the specified field
                result = CompareByField(x, y);
            }

            return _sortAscending ? result : -result;
        }

        private int CompareByField(AcademicRecord x, AcademicRecord y)
        {
            return _sortField.ToLower() switch
            {
                "course" => string.Compare(x.CourseCodeNavigation?.Title ?? "", y.CourseCodeNavigation?.Title ?? "", StringComparison.OrdinalIgnoreCase),
                "student" => string.Compare(x.Student?.Name ?? "", y.Student?.Name ?? "", StringComparison.OrdinalIgnoreCase),
                _ => 0
            };
        }
    }
}

