using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IMongoCollection<Contract> _contractCollection;

        public ContractController(IMongoDatabase database)
        {
            _contractCollection = database.GetCollection<Contract>("Contracts");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetAllContracts()
        {
            var list = await _contractCollection.Find(_ => true).ToListAsync();
            return Ok(list);
        }

        // 1. THÊM MỚI: Upload file Hợp đồng
        [HttpPost("upload")]
        public async Task<IActionResult> UploadContract([FromForm] ContractRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest(new { message = "Không tìm thấy file tải lên!" });

                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.File.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new { message = "Hệ thống chỉ chấp nhận định dạng PDF, Word (.doc, .docx) hoặc Hình ảnh!" });

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "contracts");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                var contract = new Contract
                {
                    TransactionId = request.TransactionId,
                    ContractNumber = request.ContractNumber,
                    FileUrl = $"/uploads/contracts/{fileName}"
                };

                await _contractCollection.InsertOneAsync(contract);
                return Ok(new { message = "Lưu hợp đồng thành công!", data = contract });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        // 2. XEM CHI TIẾT
        [HttpGet("transaction/{transId}")]
        public async Task<IActionResult> GetByTransaction(string transId)
        {
            var contract = await _contractCollection.Find(c => c.TransactionId == transId).FirstOrDefaultAsync();
            if (contract == null) return NotFound();
            return Ok(contract);
        }

        // 3. SỬA HỢP ĐỒNG
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(string id, [FromForm] ContractRequest request)
        {
            try
            {
                var existingContract = await _contractCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (existingContract == null) return NotFound(new { message = "Không tìm thấy hợp đồng để cập nhật!" });

                existingContract.TransactionId = request.TransactionId;
                existingContract.ContractNumber = request.ContractNumber;

                if (request.File != null && request.File.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(request.File.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                        return BadRequest(new { message = "Định dạng file không hợp lệ!" });

                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "contracts");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    if (!string.IsNullOrEmpty(existingContract.FileUrl))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingContract.FileUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.File.CopyToAsync(stream);
                    }

                    existingContract.FileUrl = $"/uploads/contracts/{fileName}";
                }

                await _contractCollection.ReplaceOneAsync(c => c.Id == id, existingContract);
                return Ok(new { message = "Cập nhật hợp đồng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        // 4. XÓA HỢP ĐỒNG
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(string id)
        {
            try
            {
                var contract = await _contractCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (contract == null) return NotFound(new { message = "Không tìm thấy hợp đồng!" });

                if (!string.IsNullOrEmpty(contract.FileUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", contract.FileUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                var result = await _contractCollection.DeleteOneAsync(c => c.Id == id);
                if (result.DeletedCount > 0) return Ok(new { message = "Đã xóa hợp đồng thành công!" });

                return BadRequest(new { message = "Lỗi khi xóa hợp đồng!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }
    }
}