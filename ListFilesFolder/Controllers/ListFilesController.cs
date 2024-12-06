using ListFilesFolder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

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

        public IActionResult CreateContract()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(ContractViewModel contract, IFormFile[] files)
        {
            // 1. Sözleşme adını al ve dosyaların kaydedileceği yolu oluştur
            var contractFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Contracts", contract.ContractName);

            // 2. Eğer sözleşme klasörü yoksa, oluştur
            if (!Directory.Exists(contractFolderPath))
            {
                Directory.CreateDirectory(contractFolderPath);
            }

            // 3. Dosyaları yükle
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    var filePath = Path.Combine(contractFolderPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }


            return RedirectToAction("Index"); 
        }
    }
}
