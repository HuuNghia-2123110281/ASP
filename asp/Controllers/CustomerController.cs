using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMongoCollection<Customer> _customerCollection;

        public CustomerController(IMongoDatabase database)
        {
            _customerCollection = database.GetCollection<Customer>("Customers");
        }

        // 1. LẤY TẤT CẢ KHÁCH HÀNG (GET ALL)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var list = await _customerCollection.Find(_ => true).SortByDescending(c => c.CreatedAt).ToListAsync();
            return Ok(list);
        }

        // 2. LẤY CHI TIẾT 1 KHÁCH HÀNG (GET ONE)
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
        {
            var customer = await _customerCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // 3. THÊM MỚI (POST)
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName) || string.IsNullOrWhiteSpace(customer.Phone))
                return BadRequest(new { message = "Họ tên và Số điện thoại không được để trống!" });

            customer.CreatedAt = DateTime.Now;
            await _customerCollection.InsertOneAsync(customer);
            return Ok(new { message = "Thêm khách hàng thành công!", data = customer });
        }

        // 4. CẬP NHẬT (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] Customer customerIn)
        {
            try
            {
                var existing = await _customerCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (existing == null) return NotFound(new { message = "Không tìm thấy khách hàng!" });

                existing.FullName = customerIn.FullName;
                existing.Phone = customerIn.Phone;
                existing.Email = customerIn.Email;
                existing.Address = customerIn.Address;

                await _customerCollection.ReplaceOneAsync(c => c.Id == id, existing);
                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        // 5. XÓA (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                var result = await _customerCollection.DeleteOneAsync(c => c.Id == id);
                if (result.DeletedCount > 0)
                    return Ok(new { message = "Xóa khách hàng thành công!" });

                return NotFound(new { message = "Không tìm thấy khách hàng!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }
    }
}