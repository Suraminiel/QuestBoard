using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuestBoard.Controllers
{
    [Authorize(Roles = "User")]
    [Route("files")]
    public class FileController : Controller
    {
        private readonly string _imagePath;// = @"UploadedFiles\ProfilPictures";

        public FileController()
        {
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "ProfilPictures");
        }

        [HttpGet("images/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            string filePath = Path.Combine(_imagePath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            string contentType = "image/jpeg";


            return File(imageBytes, contentType);
        }

        [HttpGet("profilPic/{userId}")]
        public IActionResult ProfilPic(string userId)
        {
            string filePath = Path.Combine(_imagePath, userId, "profilPicture.png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            string contentType = "image/jpeg";


            return File(imageBytes, contentType);
        }
    }
}
