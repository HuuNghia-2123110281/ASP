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
    }
}