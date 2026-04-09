using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly IMongoCollection<PropertyImage> _imageCollection;

        public PropertyImageController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BdsDB");
            _imageCollection = database.GetCollection<PropertyImage>("PropertyImages");
        }

        // GET: api/PropertyImage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyImage>>> GetAll()
        {
            var images = await _imageCollection.Find(_ => true).ToListAsync();
            return Ok(images);
        }

        // API ĐẶC BIỆT: Lấy tất cả hình ảnh của 1 BĐS cụ thể
        // GET: api/PropertyImage/property/{propertyId}
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<PropertyImage>>> GetImagesByPropertyId(string propertyId)
        {
            var images = await _imageCollection.Find(i => i.PropertyId == propertyId).ToListAsync();
            return Ok(images);
        }

        // POST: api/PropertyImage
        [HttpPost]
        public async Task<ActionResult> Create(PropertyImage image)
        {
            await _imageCollection.InsertOneAsync(image);
            return Ok(new { message = "Thêm hình ảnh thành công!", data = image });
        }

        // DELETE: api/PropertyImage/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _imageCollection.DeleteOneAsync(i => i.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy ảnh để xóa" });
            }

            return Ok(new { message = "Xóa ảnh thành công!" });
        }
    }
}