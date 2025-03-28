using CyberSphere.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Progress> Progresss { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Level>().HasKey(l => l.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Lesson>().HasKey(l => l.Id);
            modelBuilder.Entity<Progress>().HasKey(p => p.Id);


            modelBuilder.Entity<Level>()
                .HasMany(l => l.Courses)
                .WithOne(c => c.Level)
                .HasForeignKey(c => c.LevelId)
                .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Level>()
                .HasMany(l => l.SubLevels)
                .WithOne(l => l.ParentLevel)
                .HasForeignKey(l => l.ParentLevelId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Student>()
         .HasOne(s => s.User)
         .WithMany() // إذا لم يكن لديك قائمة طلاب داخل `ApplicationUser`
         .HasForeignKey(s => s.UserId)
         .OnDelete(DeleteBehavior.Cascade);

            // ✅ علاقة Progress مع Student
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Progresss)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ علاقة Progress مع Course
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Progresss)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.NoAction); // ✅ الحل لمنع الخطأ

            // ✅ علاقة Progress مع Lesson
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Lesson)
                .WithMany(l => l.Progresss)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.NoAction); // ✅ الحل لمنع الخطأ
        }
    }
}
