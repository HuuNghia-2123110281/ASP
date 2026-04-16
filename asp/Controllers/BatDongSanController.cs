using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using Microsoft.AspNetCore.Hosting; // THÊM THƯ VIỆN NÀY ĐỂ XỬ LÝ ĐƯỜNG DẪN
using System.IO; // THÊM THƯ VIỆN XỬ LÝ FILE
using System;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatDongSanController : ControllerBase
    {
        private readonly IMongoCollection<BatDongSan> _bdsCollection;
        private readonly IWebHostEnvironment _env; // Thêm biến môi trường để tìm thư mục lưu ảnh

        // Cập nhật Constructor để tiêm IWebHostEnvironment vào
        public BatDongSanController(IMongoDatabase database, IWebHostEnvironment env)
        {
            _bdsCollection = database.GetCollection<BatDongSan>("BatDongSans");
            _env = env;
        }

        // 1. Lấy danh sách + Phân trang (Giữ nguyên của Nghĩa)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BatDongSan>>> GetBatDongSans(int page = 1, int pageSize = 10)
        {
            var list = await _bdsCollection.Find(_ => true)
                .SortByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return Ok(list);
        }

        // 2. Xem chi tiết theo ID (Giữ nguyên của Nghĩa)
        [HttpGet("{id}")]
        public async Task<ActionResult<BatDongSan>> GetBatDongSan(string id)
        {
            var batDongSan = await _bdsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (batDongSan == null) return NotFound(new { message = "Không tìm thấy!" });
            return Ok(batDongSan);
        }

        // 3. Thêm mới BĐS - ĐÃ NÂNG CẤP ĐỂ HỨNG FILE ẢNH
        [HttpPost]
        public async Task<ActionResult<BatDongSan>> PostBatDongSan([FromForm] BatDongSanRequest request)
        {
            if (string.IsNullOrEmpty(request.TieuDe))
                return BadRequest(new { message = "Tên BĐS không được để trống" });

            string? hinhAnhUrl = null;

            try
            {
                // Xử lý lưu File ảnh vào server nếu người dùng có up hình
                if (request.HinhAnhFile != null && request.HinhAnhFile.Length > 0)
                {
                    // Lấy đường dẫn thư mục wwwroot/images
                    string uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Đổi tên file ngẫu nhiên chống trùng (VD: 1234_hinhnha.jpg)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.HinhAnhFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Copy file lưu vào ổ cứng
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.HinhAnhFile.CopyToAsync(fileStream);
                    }

                    // Đường dẫn này sẽ được lưu vào MongoDB
                    hinhAnhUrl = "/images/" + uniqueFileName;
                }

                // Gom dữ liệu để lưu vào Database
                var batDongSan = new BatDongSan
                {
                    TieuDe = request.TieuDe,
                    LoaiHinh = request.LoaiHinh,
                    Gia = request.Gia,
                    DiaChi = request.DiaChi,
                    MoTa = request.MoTa,
                    HinhAnhUrl = hinhAnhUrl ?? "https://loremflickr.com/400/300/house"
                };

                await _bdsCollection.InsertOneAsync(batDongSan);
                return CreatedAtAction(nameof(GetBatDongSan), new { id = batDongSan.Id }, batDongSan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lưu BĐS: " + ex.Message });
            }
        }

        // 4. Sửa BĐS (Giữ nguyên của Nghĩa)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatDongSan(string id, BatDongSan updatedBds)
        {
            var result = await _bdsCollection.ReplaceOneAsync(x => x.Id == id, updatedBds);
            if (result.MatchedCount == 0) return NotFound();
            return Ok(updatedBds);
        }

        // 5. Xóa BĐS (Giữ nguyên của Nghĩa)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatDongSan(string id)
        {
            var result = await _bdsCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return NoContent();
        }
    }
}