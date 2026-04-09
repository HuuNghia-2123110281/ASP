using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryController(IMongoClient mongoClient)
        {
            // Thay "BdsDB" bằng tên database thực tế của bạn
            var database = mongoClient.GetDatabase("BdsDB");
            _categoryCollection = database.GetCollection<Category>("Categories");
        }

        [cite_start]// GET: api/Category [cite: 250]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _categoryCollection.Find(_ => true).ToListAsync();
            return Ok(categories);
        }

        [cite_start]// POST: api/Category [cite: 251]
        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);
            return Ok(new { message = "Thêm danh mục thành công!", data = category });
        }

        [cite_start]// PUT: api/Category/{id} [cite: 252]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Category updatedCategory)
        {
            var result = await _categoryCollection.ReplaceOneAsync(c => c.Id == id, updatedCategory);

            if (result.MatchedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            return Ok(new { message = "Cập nhật thành công!" });
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _categoryCollection.DeleteOneAsync(c => c.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy danh mục để xóa" });
            }

            return Ok(new { message = "Xóa danh mục thành công!" });
        }
    }
}