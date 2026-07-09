using System;
using System.Collections.Generic;

namespace UserAuth.Data;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public string? CourseDuration { get; set; }

    public decimal? Coursefee { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
