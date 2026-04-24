using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryController(IMongoDatabase database)
        {
            _categoryCollection = database.GetCollection<Category>("Categories");
        }
        // 1. LẤY DANH SÁCH
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _categoryCollection.Find(_ => true).ToListAsync());
        }
        // 2. LẤY CHI TIẾT
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var cat = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (cat == null) return NotFound();
            return Ok(cat);
        }
        // 3. THÊM MỚI
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category cat)
        {
            if (string.IsNullOrEmpty(cat.Name)) return BadRequest("Tên loại hình không được để trống");
            await _categoryCollection.InsertOneAsync(cat);
            return Ok(new { message = "Thêm loại hình thành công!", data = cat });
        }
        // 4. CHỈNH SỬA
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category cat)
        {
            var existing = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existing == null) return NotFound();

            existing.Name = cat.Name;

            await _categoryCollection.ReplaceOneAsync(x => x.Id == id, existing);
            return Ok(new { message = "Cập nhật thành công!" });
        }
        // 5. XÓA 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var result = await _categoryCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return Ok(new { message = "Đã xóa loại hình này!" });
        }
    }
}