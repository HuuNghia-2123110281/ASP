using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IMongoCollection<Owner> _ownerCollection;

        public OwnerController(IMongoDatabase database)
        {
            _ownerCollection = database.GetCollection<Owner>("Owners");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAll()
        {
            return Ok(await _ownerCollection.Find(_ => true).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Owner owner)
        {
            await _ownerCollection.InsertOneAsync(owner);
            return Ok(new { message = "Thêm chủ nhà thành công!", data = owner });
        }
    }
}