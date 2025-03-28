﻿using CyberSphere.BLL.Services.Implementation;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Implementation;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class CertificateService : ICertificateService
    {
        private readonly IEmailService emailService;
        private readonly ICertificateRepo certificateRepo;
        private readonly ICourseRepo courseRepo;
        private readonly IStudentRepo studentRepo;
        private readonly IPdfGeneratorService pdfGeneratorService;
        private readonly IProgressRepo progressRepo;

        public CertificateService(IEmailService emailService, ICertificateRepo certificateRepo, IStudentRepo studentRepo, IPdfGeneratorService pdfGeneratorService, IProgressRepo progressRepo)
        {
            this.emailService = emailService;
            this.certificateRepo = certificateRepo;
            this.courseRepo = courseRepo;
            this.studentRepo = studentRepo;
            this.pdfGeneratorService = pdfGeneratorService;
            this.progressRepo = progressRepo;
        }

        //public async Task GenerateCertificateIfCompleted(int studentid, int courseid, string studentemail)
        //{
        //    var progress = await progressRepo.GetProgress(studentid, courseid);
        //    if (progress == null || !progress.IsCompleted)
        //    {
        //        throw new Exception("Course not yet complete");
        //    }
        //    var exist = await certificateRepo.CertificateExists(studentid, courseid);
        //    if (exist) return;

        //    string certificatePath = await pdfGeneratorService.GenerateCertificate(progress.Student, progress.Course);

        //    // ✅ Save the certificate in the database
        //    var certificate = new Certificate
        //    {
        //        StudentId = studentid,
        //        CourseId = courseid,
        //        IssuedAt = DateTime.UtcNow,
        //        CertificateURL = certificatePath
        //    };


        //    await certificateRepo.CreateCertificate(certificate);

        //    // ✅ Send the certificate via email
        //    string emailSubject = "🎉 Congratulations! Your Course Certificate is Ready!";
        //    string emailBody = $"Dear {progress.Student.FirstName},\n\n" +
        //        $"Congratulations on completing the {progress.Course.Title} course! 🎓\n\n" +
        //        $"You can download your certificate from the link below:\n" +
        //        $"[Download Certificate](https://yourwebsite.com{certificatePath})\n\n" +
        //        $"Best regards,\nCyberSphere Team";

        //    await emailService.SendEmailAsync(studentemail, emailSubject, emailBody);
        //}
        public async Task CheckAndGenerateCertificate(int studentId, int courseId)
        {
            var progress = await progressRepo.GetProgress(studentId, courseId);
            if (progress == null || !progress.IsCompleted) return; // ❌ لم يكتمل الكورس بعد

            var exist = await certificateRepo.CertificateExists(studentId, courseId);
            if (exist) return; // ❌ الشهادة موجودة بالفعل

            string certificatePath = await pdfGeneratorService.GenerateCertificate(progress.Student, progress.Course);

            // ✅ حفظ الشهادة في قاعدة البيانات
            var certificate = new Certificate
            {
                StudentId = studentId,
                CourseId = courseId,
                IssuedAt = DateTime.UtcNow,
                CertificateURL = certificatePath
            };

            await certificateRepo.CreateCertificate(certificate);

            // ✅ إرسال الشهادة عبر البريد الإلكتروني
            string emailSubject = "🎉 Congratulations! Your Course Certificate is Ready!";
            string emailBody = $"Dear {progress.Student.FirstName},\n\n" +
                $"Congratulations on completing the {progress.Course.Title} course! 🎓\n\n" +
                $"You can download your certificate from the link below:\n" +
                $"[Download Certificate](https://yourwebsite.com{certificatePath})\n\n" +
                $"Best regards,\nCyberSphere Team";

            await emailService.SendEmailAsync(progress.Student.User.Email, emailSubject, emailBody);
        }
        //public async Task<List<CertificateDto>> GetStudentCertificates(int studentId)
        //{
        //    var certificates = await dbContext.Certificates
        //        .Where(c => c.StudentId == studentId)
        //        .Select(c => new CertificateDto
        //        {
        //            CourseTitle = c.Course.Title,
        //            IssuedAt = c.IssuedAt,
        //            CertificateURL = c.CertificateURL
        //        })
        //        .ToListAsync();

        //    return certificates;
        //}


    }
}
