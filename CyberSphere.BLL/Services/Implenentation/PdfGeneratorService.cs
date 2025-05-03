using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
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

            string certificatesDirectory = System.IO.Path.Combine("wwwroot", "certificates");
            Directory.CreateDirectory(certificatesDirectory);

            string certificatePath = System.IO.Path.Combine(certificatesDirectory, $"{student.Id}_{course.Id}.pdf");

            using (var writer = new PdfWriter(certificatePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var pageSize = PageSize.A4.Rotate();
                    var document = new Document(pdf, pageSize);

                    // Add navy blue wave-like border
                    PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
                    canvas.SaveState();
                    canvas.SetFillColor(new DeviceRgb(0, 0, 128)); // Navy blue
                    canvas.MoveTo(0, 0);
                    canvas.CurveTo(50, 50, 100, 150, 150, 200); // Simplified wave effect
                    canvas.LineTo(pageSize.GetWidth() - 150, 200);
                    canvas.CurveTo(pageSize.GetWidth() - 100, 150, pageSize.GetWidth() - 50, 50, pageSize.GetWidth(), 0);
                    canvas.LineTo(pageSize.GetWidth(), pageSize.GetHeight());
                    canvas.LineTo(0, pageSize.GetHeight());
                    canvas.ClosePath();
                    canvas.Fill();

                    // Inner white background with gold border
                    float margin = 20;
                    canvas.SetFillColor(new DeviceRgb(255, 255, 255)); // White
                    canvas.SetStrokeColor(new DeviceRgb(218, 165, 32)); // Gold
                    canvas.SetLineWidth(2);
                    canvas.Rectangle(margin, margin, pageSize.GetWidth() - 2 * margin, pageSize.GetHeight() - 2 * margin);
                    canvas.FillStroke();

                    canvas.RestoreState();

                    // Load custom fonts
                    PdfFont titleFont = PdfFontFactory.CreateFont("Helvetica-Bold");
                    PdfFont bodyFont = PdfFontFactory.CreateFont("Helvetica");
                    PdfFont signatureFont = PdfFontFactory.CreateFont("Helvetica-Oblique"); // Cursive-like style

                    // Add logo at the top center
                    //string logoPath = System.IO.Path.Combine("wwwroot", "Logo", "logo.png");
                    //if (File.Exists(logoPath))
                    //{
                    //    ImageData imageData = ImageDataFactory.Create(logoPath);
                    //    Image logo = new Image(imageData)
                    //        .SetAutoScale(true)
                    //        .SetWidth(35)
                    //        .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    //        .SetMarginTop(15);
                    //    document.Add(logo);
                    //}

                    // Certificate title
                    document.Add(new Paragraph("CERTIFICATE OF ACHIEVEMENT")
                        .SetFont(titleFont)
                        .SetFontSize(36)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 0))
                        .SetMarginTop(30));

                    // Decorative line
                    document.Add(new Paragraph(new Text("\u007E\u007E").SetFontSize(24)) // ~~ as a simple decoration
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(10));

                    // "THIS CERTIFICATE IS PROUDLY PRESENTED TO"
                    document.Add(new Paragraph("THIS CERTIFICATE IS PROUDLY PRESENTED TO")
                        .SetFont(bodyFont)
                        .SetFontSize(14)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 128))
                        .SetMarginTop(20));

                    // Recipient name
                    document.Add(new Paragraph($"{student.FirstName} {student.LastName}")
                        .SetFont(signatureFont)
                        .SetFontSize(28)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 128))
                        .SetMarginTop(10));

                    // Certificate description
                    string description = $"successfully completed the \"{course.Title}\" course with dedication and excellence, demonstrating outstanding skills and commitment.";
                    document.Add(new Paragraph(description)
                        .SetFont(bodyFont)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 0, 0))
                        .SetMarginTop(20)
                        .SetMultipliedLeading(1.2f));

                    // Signatures and seal
                    Table signatureTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 40, 30 })).UseAllAvailableWidth();
                    signatureTable.SetMarginTop(50);

                    // Left signature
                    Cell leftCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);
                    string leftSigPath = System.IO.Path.Combine("wwwroot", "Logo", "signature1.png");
                    if (File.Exists(leftSigPath))
                    {
                        ImageData sigData = ImageDataFactory.Create(leftSigPath);
                        Image sigImage = new Image(sigData)
                            .SetAutoScale(true)
                            .SetWidth(100);
                        leftCell.Add(sigImage);
                    }
                    leftCell.Add(new Paragraph("Rufus Stewart")
                        .SetFont(bodyFont)
                        .SetFontSize(12));
                    leftCell.Add(new Paragraph("REPRESENTATIVES")
                        .SetFont(bodyFont)
                        .SetFontSize(10));
                    signatureTable.AddCell(leftCell);

                    // Center seal
                    Cell centerCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);
                    string sealPath = System.IO.Path.Combine("wwwroot", "Logo", "seal.png");
                    if (File.Exists(sealPath))
                    {
                        ImageData sealData = ImageDataFactory.Create(sealPath);
                        Image seal = new Image(sealData)
                            .SetAutoScale(true)
                            .SetWidth(80);
                        centerCell.Add(seal);
                    }
                    signatureTable.AddCell(centerCell);

                    // Right signature
                    Cell rightCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);
                    string rightSigPath = System.IO.Path.Combine("wwwroot", "Logo", "signature2.png");
                    if (File.Exists(rightSigPath))
                    {
                        ImageData sigData = ImageDataFactory.Create(rightSigPath);
                        Image sigImage = new Image(sigData)
                            .SetAutoScale(true)
                            .SetWidth(100);
                        rightCell.Add(sigImage);
                    }
                    rightCell.Add(new Paragraph("Olivia Wilson")
                        .SetFont(bodyFont)
                        .SetFontSize(12));
                    rightCell.Add(new Paragraph("REPRESENTATIVES")
                        .SetFont(bodyFont)
                        .SetFontSize(10));
                    signatureTable.AddCell(rightCell);

                    document.Add(signatureTable);

                    // Footer with Certificate ID and Issuing Date
                    Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                    footerTable.SetMarginTop(20);

                    footerTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .Add(new Paragraph($"Certificate ID: {student.Id}-{course.Id}")
                            .SetFont(bodyFont)
                            .SetFontSize(10)
                            .SetMarginLeft(20)));

                    footerTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph($"Issuing Date: {DateTime.Now:MMMM dd, yyyy}")
                            .SetFont(bodyFont)
                            .SetFontSize(10)
                            .SetMarginRight(20)));

                    document.Add(footerTable);

                    document.Close();
                }
            }

            string relativePath = certificatePath.Replace("wwwroot", "").Replace("\\", "/");
            return relativePath;
        }
    }
}