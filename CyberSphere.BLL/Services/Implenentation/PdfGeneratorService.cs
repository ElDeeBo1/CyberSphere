using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Migrations;
using CyberSphere.DAL.Repo.Interface;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
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

        public PdfGeneratorService(IStudentRepo studentRepo, ICourseRepo courseRepo)
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

            var courseed = courseRepo.GetCourse(course.Id);
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
                    var pageSize = PageSize.A4.Rotate();
                    var document = new Document(pdf, pageSize);

                    // Add navy blue and gold border
                    PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
                    canvas.SaveState();

                    // Outer navy blue border
                    canvas.SetFillColor(new DeviceRgb(0, 0, 128)); // Navy blue
                    canvas.Rectangle(0, 0, pageSize.GetWidth(), pageSize.GetHeight());
                    canvas.Fill();

                    // Inner white background with gold border
                    float margin = 20;
                    canvas.SetFillColor(new DeviceRgb(255, 255, 255)); // White
                    canvas.SetStrokeColor(new DeviceRgb(218, 165, 32)); // Gold
                    canvas.SetLineWidth(2);
                    canvas.Rectangle(margin, margin, pageSize.GetWidth() - 2 * margin, pageSize.GetHeight() - 2 * margin);
                    canvas.FillStroke();

                    canvas.RestoreState();

                    // Load custom fonts (replace with actual font paths)
                    PdfFont titleFont = PdfFontFactory.CreateFont("Helvetica-Bold"); // Use a serif font like Times New Roman if available
                    PdfFont bodyFont = PdfFontFactory.CreateFont("Helvetica"); // Regular font
                    PdfFont signatureFont = PdfFontFactory.CreateFont("Helvetica"); // Use a cursive font like Zapfino or similar

                    // Add logo at the top center
                    string logoPath = System.IO.Path.Combine("wwwroot", "Logo", "logo.png");
                    if (File.Exists(logoPath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .SetAutoScale(true)
                            .SetWidth(20)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                            .SetMarginTop(20);
                        document.Add(logo);
                    }

                    // Add "Company name" placeholder
                    document.Add(new Paragraph("Cyber Sphere")
                        .SetFont(bodyFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(10));

                    // Certificate title
                    document.Add(new Paragraph("Certificate of participation")
                        .SetFont(titleFont)
                        .SetFontSize(28)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 128)) // Navy blue
                        .SetMarginTop(20));

                    // "THIS IS TO CERTIFY THAT"
                    document.Add(new Paragraph("THIS IS TO CERTIFY THAT")
                        .SetFont(bodyFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(218, 165, 32)) // Gold
                        .SetMarginTop(10));

                    // Recipient name
                    document.Add(new Paragraph($"{student.FirstName} {student.LastName}")
                        .SetFont(signatureFont)
                        .SetFontSize(36)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 128)) // Navy blue
                        .SetMarginTop(10));

                    // Certificate description
                    string description = $"successfully completed the \"{course.Title}\" course, demonstrating commitment, active participation, and a strong willingness to learn throughout the training period.";

                    document.Add(new Paragraph(description)
                        .SetFont(bodyFont)
                        .SetFontSize(14)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(218, 165, 32)) // Gold
                        .SetMarginTop(20)
                        .SetMultipliedLeading(1.2f));

                    // Signatures and seal
                    // Add seal image (replace with actual seal image path)
                    string sealPath = System.IO.Path.Combine("wwwroot", "Logo", "logo.PNG");
                    if (File.Exists(sealPath))
                    {
                        ImageData sealData = ImageDataFactory.Create(sealPath);
                        Image seal = new Image(sealData)
                            .SetAutoScale(true)
                            .SetWidth(80)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                            .SetMarginTop(20);
                        document.Add(seal);
                    }
                    else
                    {
                        // Placeholder for seal text
                        document.Add(new Paragraph("ATTENDED 2025")
                            .SetFont(bodyFont)
                            .SetFontSize(10)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(20));
                    }

                    // Signature lines
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                    table.SetMarginTop(10);

                    // Left signature (Manager)
                    Cell leftCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);
                    string managerSigPath = System.IO.Path.Combine("wwwroot", "Logo", "signature1.png");
                    if (File.Exists(managerSigPath))
                    {
                        ImageData sigData = ImageDataFactory.Create(managerSigPath);
                        Image sigImage = new Image(sigData)
                            .SetAutoScale(true)
                            .SetWidth(100);
                        leftCell.Add(sigImage);
                    }
                    leftCell.Add(new Paragraph("DR. AHMED RABEA")
                        .SetFont(bodyFont)
                        .SetFontSize(12));
                    leftCell.Add(new Paragraph("Manager")
                        .SetFont(bodyFont)
                        .SetFontSize(10));
                    table.AddCell(leftCell);

                    // Right signature (Director)
                    Cell rightCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);
                    string directorSigPath = System.IO.Path.Combine("wwwroot", "Logo", "signature2.png");
                    if (File.Exists(directorSigPath))
                    {
                        ImageData sigData = ImageDataFactory.Create(directorSigPath);
                        Image sigImage = new Image(sigData)
                            .SetAutoScale(true)
                            .SetWidth(100);
                        rightCell.Add(sigImage);
                    }
                    rightCell.Add(new Paragraph("WAEL ABD ELKADER")
                        .SetFont(bodyFont)
                        .SetFontSize(12));
                    rightCell.Add(new Paragraph("Director")
                        .SetFont(bodyFont)
                        .SetFontSize(10));
                    table.AddCell(rightCell);

                    document.Add(table);

                    // Certificate ID and issuing date
                    Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                    footerTable.SetMarginTop(20);

                    footerTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .Add(new Paragraph($"Certificate ID: 46820385043")
                            .SetFont(bodyFont)
                            .SetFontSize(10)
                            .SetMarginLeft(20)));

                    footerTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph($"Issuing Date:{DateTime.UtcNow}")
                            .SetFont(bodyFont)
                            .SetFontSize(10)
                            .SetMarginRight(20)));

                    document.Add(footerTable);

                    document.Close();
                }
            }
            // إرجاع المسار النسبي للشهادة ليتمكن المستخدم من تحميلها
            string relativePath = certificatePath.Replace("wwwroot", "").Replace("\\", "/");
            return relativePath;
        }
    }
}

