using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMongoCollection<News> _newsCollection;

        public NewsController(IMongoDatabase database)
        {
            _newsCollection = database.GetCollection<News>("News");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return Ok(await _newsCollection.Find(_ => true).SortByDescending(n => n.CreatedAt).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] News news)
        {
            if (string.IsNullOrEmpty(news.Title))
                return BadRequest(new { message = "Tiêu đề không được để trống!" });

            await _newsCollection.InsertOneAsync(news);
            return Ok(new { message = "Đăng tin tức thành công!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var result = await _newsCollection.DeleteOneAsync(n => n.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return Ok(new { message = "Đã xóa tin tức!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(string id, [FromBody] News news)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest(new { message = "ID không hợp lệ!" });
            var existingNews = await _newsCollection.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (existingNews == null) return NotFound(new { message = "Không tìm thấy tin tức để cập nhật!" });
            if (string.IsNullOrWhiteSpace(news.Title))
                return BadRequest(new { message = "Tiêu đề không được để trống!" });

            existingNews.Title = news.Title;
            existingNews.Description = news.Description;
            existingNews.ImageUrl = news.ImageUrl;
            existingNews.Author = news.Author;
            existingNews.UpdatedAt = DateTime.Now;

            var result = await _newsCollection.ReplaceOneAsync(n => n.Id == id, existingNews);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return Ok(new { message = "Cập nhật tin tức thành công!" });
            }
            return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lưu dữ liệu!" });
        }
    }
}