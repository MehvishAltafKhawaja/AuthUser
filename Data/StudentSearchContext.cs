using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserAuth.Data;

public partial class StudentSearchContext : DbContext
{
    public StudentSearchContext()
    {
    }

    public StudentSearchContext(DbContextOptions<StudentSearchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-OKV41EO\\SQLEXPRESS;Database=StudentSearch;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__37E309C3A62D66E2");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("Course_id");
            entity.Property(e => e.CourseDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Coursefee).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudId).HasName("PK__Student__F5C0A7FF5AAF5235");

            entity.ToTable("Student");

            entity.Property(e => e.StudId).ValueGeneratedNever();
            entity.Property(e => e.CourseId).HasColumnName("Course_Id");
            entity.Property(e => e.StudGen)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StudName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Students)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Student_Course");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
