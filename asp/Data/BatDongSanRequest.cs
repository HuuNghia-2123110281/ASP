using Microsoft.AspNetCore.Http;

namespace asp.Data
{
    public class BatDongSanRequest
    {
        public string TieuDe { get; set; } = null!;
        public string LoaiHinh { get; set; } = null!;
        public double Gia { get; set; }
        public string DiaChi { get; set; } = null!;
        public string? MoTa { get; set; }
        public IFormFile? HinhAnhFile { get; set; }
    }
}