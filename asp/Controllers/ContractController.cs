using asp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IMongoCollection<Contract> _contractCollection;

        public ContractController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BdsDB");
            _contractCollection = database.GetCollection<Contract>("Contracts");
        }

        // 1. Lấy danh sách tất cả hợp đồng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetAll()
        {
            var contracts = await _contractCollection.Find(_ => true).ToListAsync();
            return Ok(contracts);
        }

        // 2. Tìm hợp đồng theo mã giao dịch
        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<Contract>> GetByTransaction(string transactionId)
        {
            var contract = await _contractCollection.Find(c => c.TransactionId == transactionId).FirstOrDefaultAsync();
            if (contract == null) return NotFound(new { message = "Giao dịch này chưa có hợp đồng" });
            return Ok(contract);
        }

        // 3. Tạo mới hợp đồng
        [HttpPost]
        public async Task<ActionResult> Create(Contract contract)
        {
            await _contractCollection.InsertOneAsync(contract);
            return Ok(new { message = "Lưu hợp đồng thành công!", data = contract });
        }

        // 4. Xóa hợp đồng
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _contractCollection.DeleteOneAsync(c => c.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return Ok(new { message = "Đã xóa hợp đồng" });
        }
    }
}