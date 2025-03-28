using CyberSphere.DAL.Entities;

public class Progress
{
    public int Id { get; set; }

    // Required relationship only with Student
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    // Optional relationships
    public int? CourseId { get; set; }
    public virtual Course? Course { get; set; }

    public int? LessonId { get; set; }
    public virtual Lesson? Lesson { get; set; }

    // Progress tracking fields
    public int LessonsCompleted { get; set; }
    public int TotalLessons { get; set; }
    public double CompletionPercentage { get; set; }
    public bool IsCompleted => CompletionPercentage >= 100;
}