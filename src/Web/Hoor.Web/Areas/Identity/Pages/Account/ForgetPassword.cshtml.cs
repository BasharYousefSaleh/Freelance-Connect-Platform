using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hoor.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;

namespace Hoor.Web.Areas.Identity.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ForgetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool flag = false;

        public class InputModel
        {
            [Required(ErrorMessage = "Ì—ÃÏ ≈œŒ«· «·»—Ìœ «·≈·ﬂ —Ê‰Ì.")]
            [EmailAddress(ErrorMessage = "’Ì€… «·»—Ìœ «·≈·ﬂ —Ê‰Ì €Ì— ’ÕÌÕ….")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
               flag = true;
                await Task.Delay(5000); //  √ŒÌ— »„ﬁœ«— 5 ÀÊ«‰Ú


                string returnUrl = this.Url.Content("~/");

                var user = await this.userManager.FindByEmailAsync(Input.Email);
                var codeToken = await this.userManager.GeneratePasswordResetTokenAsync(user);
                codeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeToken));
            var callbackUrl = this.Url.Page(
                       "/Account/ResetPassword",
                       pageHandler: null,
                       values: new { area = "Identity", userId = user.Id, code = codeToken, returnUrl = returnUrl },
                       protocol: this.Request.Scheme);

            if (!ModelState.IsValid)
            {
                return Page();
            }
                string token = await userManager.GeneratePasswordResetTokenAsync(user);
                string confirmLink = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { userId = user.Id, code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)) },
                    protocol: Request.Scheme);

                bool isEmailSent = await SendEmailAsync(Input.Email, "≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—", confirmLink);

                //            SendEmailAsync(this.Input.Email, "«⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—",
                //#pragma warning disable SA1117 // Parameters should be on same line or separate lines
                //                       $"· €ÌÌ— ﬂ·„… «·„—Ê— ﬁ„ »«·÷€ÿ ⁄·Ï «·—«»ÿ :  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                //#pragma warning restore SA1117 // Parameters should be on same line or separate lines

                // √÷› „‰ÿﬁ ≈—”«· «·»—Ìœ «·≈·ﬂ —Ê‰Ì Â‰«.

            }
            catch (Exception)
            {
                flag = false;
                ModelState.AddModelError("", "«·»—Ìœ «·„œŒ· €Ì— „ÊÃÊœ");



            }
            return Page();
        }
        private async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
        {
            try
            {
                string emailTemplate = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                        direction: rtl;
                    width: 100%;
                    max-width: 600px;
                    margin: 20px auto;
                    background: #fff;
                    border-radius: 8px;
                    overflow: hidden;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #c1181e;
                    color: #fff;
                    padding: 20px;
                    text-align: center;
                    font-size: 24px;
                }}
                .content {{
                    padding: 20px;
                    line-height: 1.6;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .button {{
                    display: block;
                    width: fit-content;
                    background-color: #007bff;
                    color: #fff !important;
                    text-decoration: none;
                    padding: 10px 20px;
                    margin: 20px auto;
                    border-radius: 5px;
                    text-align: center;
                }}
                .footer {{
                    background-color: #f1f1f1;
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    «” ⁄«œ… ﬂ·„… «·„—Ê—
                </div>
                <div class='content'>
                    <p>„—Õ»«°</p>
                    <p>·ﬁœ ÿ·»  «” ⁄«œ… ﬂ·„… «·„—Ê— «·Œ«’… »Õ”«»ﬂ. ≈–« ﬂ‰  √‰  „‰ ﬁ«„ »Â–« «·ÿ·»° Ì—ÃÏ «·÷€ÿ ⁄·Ï «·—«»ÿ √œ‰«Â ·≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—:</p>
                    <a href='{HtmlEncoder.Default.Encode(confirmLink)}' class='button'>≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—</a>
                    <p>≈–« ·„  ﬂ‰ √‰  „‰ ÿ·» Â–« «·≈Ã—«¡° Ì„ﬂ‰ﬂ  Ã«Â· Â–Â «·—”«·… »√„«‰.</p>
                    <p>‘ﬂ—«°</p>
                    <p>›—Ìﬁ «·œ⁄„ - „‰’… Õ—</p>
                </div>
                <div class='footer'>
                    &copy; 2024 Ã„Ì⁄ «·ÕﬁÊﬁ „Õ›ÊŸ….
                </div>
            </div>
        </body>
        </html>";

                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                message.From = new MailAddress("amranomr122@gmail.com");
                message.To.Add(email);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = emailTemplate;

                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("amranomr122@gmail.com", "gjmj mloq epuz xmax");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
