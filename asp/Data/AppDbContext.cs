namespace asp.Data;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Đây chính là bảng BatDongSan sẽ xuất hiện trong SQL Server
    public DbSet<BatDongSan> BatDongSans { get; set; }
    public object BatDongSan { get; internal set; }
}

// Model đại diện cho dữ liệu
public class BatDongSan
{
    public int Id { get; set; }
    public string TieuDe { get; set; }
    public string DiaChi { get; set; }
    public decimal Gia { get; set; }
    public string LoaiHinh { get; set; }
}