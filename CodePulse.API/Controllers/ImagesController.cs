using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call Image repository
            var images = await imageRepository.GetAll();

            //convert into DTO
            var response = new List<BlogImageDTO>();

            foreach (var image in images)
            {
                response.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    Title = image.Title,
                    Url = image.Url,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    DateCreated = DateTime.Now,
                });
            }

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, 
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if(ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

               blogImage =  await imageRepository.Upload(file, blogImage);

                //convert domain model to dto
                var response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size can not be more than 10MB");
            }
        }
    }
}
