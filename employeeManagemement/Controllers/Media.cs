using employeeManagemement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;


namespace employeeManagemement.Controllers
{
    public class Media : Controller
    {
      
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Media(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            List<ObjFile> ObjFiles = new List<ObjFile>();
            foreach (string strfile in Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Files")))
            {
                FileInfo fi = new FileInfo(strfile);
                ObjFile obj = new ObjFile();
                obj.File = fi.Name;
                obj.Size = fi.Length;
                obj.Type = GetFileTypeByExtension(fi.Extension);
                ObjFiles.Add(obj);
            }

            return View(ObjFiles);
        }

        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetFileTypeByExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                default:
                    return "Unknown";
            }
        }

        [HttpPost]
        public ActionResult Index(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            TempData["Message"] = "Files uploaded successfully";
            return RedirectToAction("Index");
        }
    }
}
