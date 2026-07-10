using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CourseLedger.DataAccess;

public partial class StudentrecordContext : DbContext
{
    public StudentrecordContext()
    {
    }

    public StudentrecordContext(DbContextOptions<StudentrecordContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicRecord> AcademicRecords { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicRecord>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseCode }).HasName("PK__Academic__3D052599C63BA3FB");

            entity.ToTable("AcademicRecord");

            entity.Property(e => e.StudentId).HasMaxLength(16);
            entity.Property(e => e.CourseCode).HasMaxLength(16);

            entity.HasOne(d => d.CourseCodeNavigation).WithMany(p => p.AcademicRecords)
                .HasForeignKey(d => d.CourseCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AcademicRecord_Course");

            entity.HasOne(d => d.Student).WithMany(p => p.AcademicRecords)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AcademicRecord_Student");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Course__A25C5AA694384533");

            entity.ToTable("Course");

            entity.Property(e => e.Code).HasMaxLength(16);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC070CC93D07");

            entity.ToTable("Employee");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasIndex(e => e.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Employee_UserName");

            entity.HasMany(d => d.Roles).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ToRole"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ToEmployee"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "RoleId");
                        j.ToTable("Employee_Role");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("Employee_Id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("Role_Id");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Role1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC0749778D6A");

            entity.ToTable("Student");

            entity.Property(e => e.Id).HasMaxLength(16);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
