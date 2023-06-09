﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;

namespace MyBlog.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImagesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ActionResult<ApiResult>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("请上传图片");

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return ApiResultHelp.Success(' ',"图片上传成功");
        }
        [HttpGet]
		public IActionResult GetAllImages()
		{
			var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
			var imagePaths = Directory.GetFiles(uploadsFolder, "*.*", SearchOption.AllDirectories)
									.Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".gif"))
									.ToList();
			var images = new List<ImageInfo>();
			var baseUrl = "http://localhost:7037/Uploads/"; // 设置基本的URL路径
			foreach (var path in imagePaths)
			{
				var fileInfo = new FileInfo(path);
				var imageName = fileInfo.Name;
				var imageUrl = baseUrl + imageName; // 将基本URL路径与文件名组合在一起生成完整的URL
				var image = new ImageInfo
				{
					Path = imageUrl, // 将Path属性设置为生成的URL路径
					Name = imageName
				};
				images.Add(image);
			}
			return Ok(images);
		}

	}

	public class ImageInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
    }
}
