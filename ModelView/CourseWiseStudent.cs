using UserAuth.Data;

namespace UserAuth.ModelView
{
    public class CourseWiseStudent
    {
        public Course crsDetails { get; set; }
        public List<Student> studDetails { get; set; }
    }
}
