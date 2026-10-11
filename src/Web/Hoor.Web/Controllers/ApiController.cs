using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[action]")]
public class ApiController : ControllerBase
{
    private static string VerificationCode; // لتخزين الرمز مؤقتًا

    [HttpPost]
    public async Task<IActionResult> SendVerificationCode([FromBody] EmailRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { success = false, message = "البريد الإلكتروني مطلوب." });

        try
        {
            // إنشاء رمز تحقق عشوائي
            VerificationCode = new Random().Next(100000, 999999).ToString();

            // إعداد رسالة البريد الإلكتروني
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hoor", "amranomr122@gmail.com"));
            message.To.Add(new MailboxAddress("", request.Email));
            message.Subject = "رمز التحقق";
            message.Body = new TextPart("plain")
            {
                Text = $"رمز التحقق الخاص بك هو: {VerificationCode}"
            };

            // إرسال البريد الإلكتروني باستخدام MailKit
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("amranomr122@gmail.com", "dwuv nsma dcjj ayoy"); // استبدل بكلمة مرور التطبيق
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult VerifyCode([FromBody] CodeRequest request)
    {
        if (request.Code == VerificationCode)
        {
            return Ok(new { success = true });
        }

        return BadRequest(new { success = false, message = "رمز التحقق غير صحيح." });
    }


}

public class EmailRequest
{
    public string Email { get; set; }
}

public class CodeRequest
{
    public string Code { get; set; }
}
