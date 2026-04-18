using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMongoCollection<Project> _projectCollection;

        public ProjectController(IMongoDatabase database)
        {
            _projectCollection = database.GetCollection<Project>("Projects");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll()
        {
            return Ok(await _projectCollection.Find(_ => true).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            await _projectCollection.InsertOneAsync(project);
            return Ok(new { message = "Thêm dự án thành công!", data = project });
        }
    }
}