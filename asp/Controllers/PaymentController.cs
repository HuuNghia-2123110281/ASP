using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMongoCollection<Payment> _paymentCollection;

        public PaymentController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BdsDB");
            _paymentCollection = database.GetCollection<Payment>("Payments");
        }

        // 1. Lấy tất cả lịch sử thanh toán (Toàn hệ thống)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
        {
            var payments = await _paymentCollection.Find(_ => true).ToListAsync();
            return Ok(payments);
        }

        // 2. API QUAN TRỌNG: Lấy lịch sử đóng tiền của một Giao dịch cụ thể
        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByTransaction(string transactionId)
        {
            var payments = await _paymentCollection.Find(p => p.TransactionId == transactionId).ToListAsync();
            return Ok(payments);
        }

        // 3. Ghi nhận một đợt thanh toán mới
        [HttpPost]
        public async Task<ActionResult> Create(Payment payment)
        {
            await _paymentCollection.InsertOneAsync(payment);
            return Ok(new { message = "Ghi nhận thanh toán thành công!", data = payment });
        }

        // 4. Xóa một đợt thanh toán (Nếu nhập sai)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _paymentCollection.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return Ok(new { message = "Đã xóa bản ghi thanh toán" });
        }
    }
}