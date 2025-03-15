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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // استدعاء الدالة الأساسية لتكوين جداول Identity
            base.OnModelCreating(modelBuilder);

            // تكوين المفاتيح الأساسية للكيانات الخاصة بك
            modelBuilder.Entity<Level>().HasKey(l => l.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Lesson>().HasKey(l => l.Id);

            // تكوين العلاقات بين الكيانات
            modelBuilder.Entity<Level>()
                .HasMany(l => l.Courses)
                .WithOne(c => c.Level)
                .HasForeignKey(c => c.LevelId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId);

            // اختياري: تكوين هيكل المستويات الهرمي
            modelBuilder.Entity<Level>()
                .HasMany(l => l.SubLevels)
                .WithOne(l => l.ParentLevel)
                .HasForeignKey(l => l.ParentLevelId)
                .OnDelete(DeleteBehavior.Restrict);  // منع الحذف التلقائي لتجنب المشاكل
        }
    }
}
