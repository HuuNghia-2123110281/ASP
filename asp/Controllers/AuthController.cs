using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using asp.Data;
using System.Net;
using System.Net.Mail;

namespace asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<User> _userCollection;

        public AuthController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _userCollection.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Tài khoản này đã có người sử dụng!",
                    developer = "Nguyen Huu Nghia"
                });
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "KhachHang";
            }

            await _userCollection.InsertOneAsync(user);
            return Ok(new
            {
                message = "Đăng ký thành công!",
                developer = "Nguyen Huu Nghia"
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginInfo)
        {
            var user = await _userCollection.Find(u =>
                u.Username == loginInfo.Username &&
                u.Password == loginInfo.Password).FirstOrDefaultAsync();

            if (user == null) return Unauthorized(new
            {
                message = "Sai tài khoản hoặc mật khẩu!",
                developer = "Nguyen Huu Nghia"
            });

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                fullName = user.FullName ?? user.Username,
                username = user.Username,
                role = user.Role,
                developer = "Nguyen Huu Nghia"
            });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userCollection.Find(u => u.Email == request.Username).FirstOrDefaultAsync();

            if (user == null)
                return BadRequest(new { message = "Email này chưa được đăng ký!", developer = "Nguyen Huu Nghia" });


            string otp = new Random().Next(100000, 999999).ToString();


            user.ResetOtp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

            try
            {
                var fromAddress = new MailAddress("nguyenhuunghia03052004@gmail.com", "BĐS Hỗ Trợ");
                var toAddress = new MailAddress(user.Email!);
                const string fromPassword = "fgiidufvbljcasrj";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Mã xác thực OTP - BĐS",
                    Body = $@"
                        <h3>Chào {user.FullName ?? "Khách hàng"},</h3>
                        <p>Bạn vừa yêu cầu khôi phục mật khẩu. Mã xác thực OTP của bạn là:</p>
                        <h2 style='color:blue; letter-spacing: 5px; padding: 10px; background: #f4f4f4; display: inline-block;'>{otp}</h2>
                        <p style='color:red;'>Mã này sẽ hết hạn trong vòng 5 phút.</p>
                        <p>Tuyệt đối không chia sẻ mã này cho bất kỳ ai.</p>",
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }

                return Ok(new { message = "Mã OTP đã được gửi đến Email của bạn!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống gửi mail!" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

            if (user == null)
                return BadRequest(new { message = "Tài khoản không tồn tại!" });

            if (user.ResetOtp != request.Otp)
                return BadRequest(new { message = "Mã OTP không chính xác!" });

            if (user.OtpExpiry < DateTime.UtcNow)
                return BadRequest(new { message = "Mã OTP đã hết hạn. Vui lòng yêu cầu gửi lại!" });

            user.Password = request.NewPassword;

            user.ResetOtp = null;
            user.OtpExpiry = null;

            await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

            return Ok(new { message = "Đổi mật khẩu thành công!" });
        }
    }

    public class ForgotPasswordRequest { public string? Username { get; set; } }
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? Otp { get; set; }
        public string? NewPassword { get; set; }
    }
}