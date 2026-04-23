using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllUsers()
        {
            var users = await _userCollection.Find(_ => true)
                .Project(u => new { u.Id, u.Username, u.FullName, u.Email, u.Role })
                .ToListAsync();
            return Ok(users);
        }


        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateProfile(string username, [FromBody] User updatedInfo)
        {
            var user = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin người dùng!" });
            }

            if (!string.IsNullOrEmpty(updatedInfo.FullName)) user.FullName = updatedInfo.FullName;
            if (!string.IsNullOrEmpty(updatedInfo.Email)) user.Email = updatedInfo.Email;
            if (!string.IsNullOrEmpty(updatedInfo.Phone)) user.Phone = updatedInfo.Phone;

            await _userCollection.ReplaceOneAsync(u => u.Username == username, user);

            return Ok(new
            {
                message = "Cập nhật hồ sơ thành công!",
                fullName = user.FullName
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userCollection.DeleteOneAsync(u => u.Id == id);
            if (result.DeletedCount == 0) return NotFound(new { message = "Không tìm thấy nhân viên!" });
            return Ok(new { message = "Đã xóa nhân viên thành công!" });
        }
    }
}