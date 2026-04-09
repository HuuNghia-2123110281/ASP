using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IMongoCollection<Owner> _ownerCollection;

        public OwnerController(IMongoClient mongoClient)
        {
            // Nhớ đảm bảo tên Database "BdsDB" giống với các Controller khác nhé
            var database = mongoClient.GetDatabase("BdsDB");
            _ownerCollection = database.GetCollection<Owner>("Owners");
        }

        // GET: api/Owner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAll()
        {
            var owners = await _ownerCollection.Find(_ => true).ToListAsync();
            return Ok(owners);
        }

        // POST: api/Owner
        [HttpPost]
        public async Task<ActionResult> Create(Owner owner)
        {
            await _ownerCollection.InsertOneAsync(owner);
            return Ok(new { message = "Thêm thông tin chủ nhà thành công!", data = owner });
        }

        // PUT: api/Owner/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Owner updatedOwner)
        {
            var result = await _ownerCollection.ReplaceOneAsync(o => o.Id == id, updatedOwner);

            if (result.MatchedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy thông tin chủ nhà" });
            }

            return Ok(new { message = "Cập nhật thành công!" });
        }

        // DELETE: api/Owner/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _ownerCollection.DeleteOneAsync(o => o.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy chủ nhà để xóa" });
            }

            return Ok(new { message = "Xóa thông tin chủ nhà thành công!" });
        }
    }
}