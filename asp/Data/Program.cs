using System;
using MongoDB.Driver;
using asp.Data; // Đảm bảo đúng namespace của bạn

try
{
    Console.WriteLine(">>> [1] BAT DAU KHOI DONG SERVER...");

    var builder = WebApplication.CreateBuilder(args);

    var mongoConnectionString = builder.Configuration.GetSection("MongoDB:ConnectionString").Value;
    var mongoDatabaseName = builder.Configuration.GetSection("MongoDB:DatabaseName").Value;

    // Kiểm tra xem biến có bị Null không
    Console.WriteLine(">>> [2] Kiem tra ConnectionString: " + (string.IsNullOrEmpty(mongoConnectionString) ? "BI THIEU (NULL)!" : "DA TIM THAY"));

    var mongoClient = new MongoClient(mongoConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

    builder.Services.AddSingleton(mongoDatabase);
    builder.Services.AddSingleton<IMongoClient>(mongoClient);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Đổi tên Swagger thành MSSV của bạn
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "NguyenHuuNghia_2123110281",
            Version = "v1",
            Description = "Hệ thống API Bất Động Sản"
        });
    });

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseCors("AllowAll");
    app.UseAuthorization();
    app.MapControllers();
    app.UseStaticFiles();

    Console.WriteLine(">>> [3] KHOI DONG THANH CONG, CHUAN BI RUN...");
    app.Run();
}
catch (Exception ex)
{
    // BẪY LỖI: In to ra màn hình Render
    Console.WriteLine("==================================================");
    Console.WriteLine("🚨 LOI NGHIEM TRONG TRUOC KHI CHAY: " + ex.Message);
    Console.WriteLine(ex.ToString());
    Console.WriteLine("==================================================");
    throw;
}