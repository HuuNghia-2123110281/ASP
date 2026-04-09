using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMongoCollection<Project> _projectCollection;

        public ProjectController(IMongoClient mongoClient)
        {
            // Kết nối đúng vào Database "BdsDB"
            var database = mongoClient.GetDatabase("BdsDB");
            _projectCollection = database.GetCollection<Project>("Projects");
        }

        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll()
        {
            var projects = await _projectCollection.Find(_ => true).ToListAsync();
            return Ok(projects);
        }

        // POST: api/Project
        [HttpPost]
        public async Task<ActionResult> Create(Project project)
        {
            await _projectCollection.InsertOneAsync(project);
            return Ok(new { message = "Thêm thông tin dự án thành công!", data = project });
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Project updatedProject)
        {
            var result = await _projectCollection.ReplaceOneAsync(p => p.Id == id, updatedProject);

            if (result.MatchedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy dự án" });
            }

            return Ok(new { message = "Cập nhật dự án thành công!" });
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _projectCollection.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "Không tìm thấy dự án để xóa" });
            }

            return Ok(new { message = "Xóa dự án thành công!" });
        }
    }
}