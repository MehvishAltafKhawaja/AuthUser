using System;
using System.Collections.Generic;

namespace UserAuth.Data;

public partial class Student
{
    public int StudId { get; set; }

    public string StudName { get; set; } = null!;

    public string? StudGen { get; set; }

    public DateOnly? StudDob { get; set; }

    public long? StudPhn { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }
}
