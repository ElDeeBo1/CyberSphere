using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Helper
{
    public class FileHelper
    {
        public static string UploadFile(string folderName, IFormFile? file)
        {
            if (file == null) return null;

            try
            {
                // 1) تحديد مسار المجلد
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folderName);

                // التأكد من أن المجلد موجود
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 2) إنشاء اسم فريد للملف
                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

                // 3) دمج المسار مع اسم الملف
                string finalPath = Path.Combine(folderPath, fileName);

                // 4) حفظ الملف في المجلد
                using (var stream = new FileStream(finalPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // إرجاع اسم الملف لحفظه في قاعدة البيانات
                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

            public static string DeleteFile(string folderName, string? fileName)
        {
            if (fileName is not null) return "No file match file name";
            try
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imges", folderName, fileName);

                if (File.Exists(directory))
                {
                    File.Delete(directory);
                    return "File Deleted";
                }

                return "File Not Deleted";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

    }

}
