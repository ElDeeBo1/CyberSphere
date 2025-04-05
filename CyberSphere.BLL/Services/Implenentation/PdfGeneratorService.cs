using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Migrations;
using CyberSphere.DAL.Repo.Interface;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implementation
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        private readonly IStudentRepo studentRepo;
        private readonly ICourseRepo courseRepo;

        public PdfGeneratorService(IStudentRepo studentRepo,ICourseRepo courseRepo)
        {
            this.studentRepo = studentRepo;
            this.courseRepo = courseRepo;
        }
        public async Task<string> GenerateCertificate(Student student, Course course)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Student cannot be null");
            }

            var studented = await studentRepo.GetStudentById(student.Id);
            if (studented == null)
            {
                throw new Exception($"Student with Id {student.Id} not found");
            }

            var courseed =  courseRepo.GetCourse(course.Id);
            if (courseed == null)
            {
                throw new Exception($"Course with Id {course.Id} not found");
            }
            // التأكد من أن مجلد الشهادات موجود
            string certificatesDirectory = System.IO.Path.Combine("wwwroot", "certificates");
            Directory.CreateDirectory(certificatesDirectory);

            // تحديد مسار حفظ ملف الشهادة
            string certificatePath = System.IO.Path.Combine(certificatesDirectory, $"{student.Id}_{course.Id}.pdf");

            using (var writer = new PdfWriter(certificatePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf, new PageSize(PageSize.A4).Rotate());

                    // إضافة خلفية ملونة
                    PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
                    canvas.SaveState();
                    canvas.SetFillColor(new DeviceRgb(173, 216, 230)); // لون أزرق فاتح
                    canvas.Rectangle(0, 0, PageSize.A4.Rotate().GetWidth(), PageSize.A4.Rotate().GetHeight());
                    canvas.Fill();
                    canvas.RestoreState();

                    // إضافة شعار الموقع إذا كان موجودًا
                    string logoPath = System.IO.Path.Combine("wwwroot", "Logo", "logo.png");
                    if (File.Exists(logoPath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .SetAutoScale(true)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        document.Add(logo);
                    }

                    // إضافة عنوان الشهادة
                    document.Add(new Paragraph("CERTIFICATE OF APPRECIATION")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(24)
                        .SetBold());

                    // اسم الطالب
                    document.Add(new Paragraph("This certificate is proudly presented to")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18));

                    document.Add(new Paragraph($"{student.FirstName} {student.LastName}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(22)
                        .SetBold());

                    // تفاصيل الكورس
                    document.Add(new Paragraph($"For successfully completing the course: {course.Title}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18));

                    // تاريخ إصدار الشهادة
                    document.Add(new Paragraph($"Issued on: {DateTime.UtcNow.ToShortDateString()}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16));

                    // رابط الموقع
                    document.Add(new Paragraph("www.yourwebsite.com")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(14));
                }
            }

            // إرجاع المسار النسبي للشهادة ليتمكن المستخدم من تحميلها
            string relativePath = certificatePath.Replace("wwwroot", "").Replace("\\", "/");
            return relativePath;
        }
    }
}
