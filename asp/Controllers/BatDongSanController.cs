using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using asp.Data; // Đảm bảo namespace này trỏ đúng vào file AppDbContext của bạn

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatDongSanController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor để Inject DbContext vào Controller
        public BatDongSanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BatDongSan
        // Lấy danh sách toàn bộ bất động sản từ SQL Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<asp.Data.BatDongSan>>> GetBatDongSans()
        {
            // Kiểm tra nếu DbSet bị null
            if (_context.BatDongSans == null)
            {
                return NotFound();
            }
            return await _context.BatDongSans.ToListAsync();
        }

        // GET: api/BatDongSan/5
        // Lấy chi tiết một bất động sản theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<asp.Data.BatDongSan>> GetBatDongSan(int id)
        {
            if (_context.BatDongSans == null)
            {
                return NotFound();
            }

            var batDongSan = await _context.BatDongSans.FindAsync(id);

            if (batDongSan == null)
            {
                return NotFound();
            }

            return batDongSan;
        }

        // POST: api/BatDongSan
        // Thêm mới một bất động sản vào SQL Server
        [HttpPost]
        public async Task<ActionResult<asp.Data.BatDongSan>> PostBatDongSan(asp.Data.BatDongSan batDongSan)
        {
            _context.BatDongSans.Add(batDongSan);
            await _context.Set<asp.Data.BatDongSan>().AddAsync(batDongSan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBatDongSan", new { id = batDongSan.Id }, batDongSan);
        }

        // DELETE: api/BatDongSan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatDongSan(int id)
        {
            var batDongSan = await _context.BatDongSans.FindAsync(id);
            if (batDongSan == null)
            {
                return NotFound();
            }

            _context.BatDongSans.Remove(batDongSan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}