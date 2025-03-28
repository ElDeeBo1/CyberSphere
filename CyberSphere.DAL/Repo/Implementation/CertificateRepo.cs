using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Implementation
{
    public class CertificateRepo : ICertificateRepo
    {
        private readonly ApplicationDbContext dbContext;

        public CertificateRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Certificate> CreateCertificate(Certificate certificate)
        {
            try
            {
                var existingCertificate = await dbContext.Certificates
                                                 .FirstOrDefaultAsync(c => c.StudentId == certificate.StudentId && c.CourseId == certificate.CourseId);

                if (existingCertificate != null)
                {
                    return existingCertificate; // If already exists, return it.
                }

                await dbContext.Certificates.AddAsync(certificate);
                await dbContext.SaveChangesAsync();
                return certificate;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Certificate>> GetCertificatesByStudentId(int Studentid)
        {
            try
            {
                return await dbContext.Certificates
                    .Where(c => c.StudentId == Studentid)
                    .Include(c => c.Course)
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> CertificateExists(int studentId, int courseId)
        {
            return await dbContext.Certificates
                .AnyAsync(c => c.StudentId == studentId && c.CourseId == courseId);
        }
    }
}
