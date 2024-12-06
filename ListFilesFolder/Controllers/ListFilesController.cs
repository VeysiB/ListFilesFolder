using ListFilesFolder.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListFilesFolder.Controllers
{
    public class ListFilesController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;

        public ListFilesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            string[] filePaths  = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "Files/"));
            List<FileViewModel> lstFiles=new List<FileViewModel>();
            foreach (string file in filePaths)
            {
                lstFiles.Add(new FileViewModel { FileName=Path.GetFileName(file) });
            }
            return View(lstFiles);
        }
        public FileResult DownloadFile(string fileName)
        {
            string path=Path.Combine(_webHostEnvironment.WebRootPath,"Files/")+fileName;
            byte[] bytes=System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
