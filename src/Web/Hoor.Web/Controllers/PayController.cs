using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Hoor.Web.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PayController : ControllerBase
    {
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] PaymentUploadDto dto)
        {
            if (dto.PaymentImage == null || dto.PaymentImage.Length == 0)
            {
                return BadRequest("يرجى رفع صورة صالحة.");
            }

            // حفظ الملف
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, $"payment_{dto.ContractId}_{Guid.NewGuid()}.png");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.PaymentImage.CopyToAsync(stream);
            }

            // معالجة بيانات إضافية
            Console.WriteLine($"تم استقبال الدفع لعقد رقم: {dto.ContractId}, نوع الدفع: {dto.PaymentType}");

            return Ok(new { Message = "تم رفع الملف بنجاح." });
        }
    }

    // تعريف الـ DTO لتحميل الملفات
    public class PaymentUploadDto
    {
        public IFormFile PaymentImage { get; set; }
        public string ContractId { get; set; }
        public string PaymentType { get; set; }
    }
}


