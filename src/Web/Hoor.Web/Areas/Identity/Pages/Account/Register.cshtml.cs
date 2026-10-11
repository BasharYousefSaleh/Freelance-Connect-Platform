    namespace Hoor.Web.Areas.Identity.Pages.Account
    {
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Net.Mail;
        using System.Net;
        using System.Text;
        using System.Text.Encodings.Web;
        using System.Threading.Tasks;

        using Hoor.Common;
        using Hoor.Data.Models;
        using Hoor.Services.Interfaces;
        using Microsoft.AspNetCore.Authentication;
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Identity;
        using Microsoft.AspNetCore.Identity.UI.Services;
        using Microsoft.AspNetCore.Mvc;
        using Microsoft.AspNetCore.Mvc.RazorPages;
        using Microsoft.AspNetCore.WebUtilities;
        using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    [AllowAnonymous]
    #pragma warning disable SA1649 // File name should match first type name
        public class RegisterModel : PageModel
    #pragma warning restore SA1649 // File name should match first type name
        {
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly IFreelancePlatform freelancePlatform;
            private readonly UserManager<ApplicationUser> userManager;
            private readonly ILogger<RegisterModel> logger;
            private readonly IEmailSender emailSender;

            public RegisterModel(
                IFreelancePlatform freelancePlatform,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                ILogger<RegisterModel> logger,
                IEmailSender emailSender)
            {
                this.freelancePlatform = freelancePlatform;
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.logger = logger;
                this.emailSender = emailSender;
            }

            [BindProperty]
            public InputModel Input { get; set; }

            public string ReturnUrl { get; set; }

            public IList<AuthenticationScheme> ExternalLogins { get; set; }

            public class InputModel
            {
                [Required]
                [Range(0, 1, ErrorMessage = "An invalid role has been chosen.")]
                public UserType UserType { get; set; }

                [Required]
                [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
                [Display(Name = "First Name")]
                public string FirstName { get; set; }

                [Required]
                [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
                [Display(Name = "Last Name")]
                public string LastName { get; set; }

                [Required]
                [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
                [Display(Name = "Username")]
                public string Username { get; set; }

                [Required]
                [EmailAddress]
                [Display(Name = "Email")]
                public string Email { get; set; }

                [Required]
                [Range(1, 62, ErrorMessage = "Choose a location.")]
                [Display(Name = "Location")]
                public Country Location { get; set; }

                [Required]
                [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
                [DataType(DataType.Password)]
                [Display(Name = "Password")]
                public string Password { get; set; }

                [DataType(DataType.Password)]
                [Display(Name = "Confirm password")]
                [Compare("Password", ErrorMessage = "The Password and confirmation password do not match.")]
                public string ConfirmPassword { get; set; }

                [Required]
                [Display(Name = "NationalID")]
                public string NationalID { get; set; }

                [Required]
                [Display(Name = "Tags")]
                public string Tags { get; set; }

                [Required]
                [Display(Name = "Front ID Card")]
                public IFormFile FrontIDCard { get; set; }

                [Required]
                [Display(Name = "Back ID Card")]
                public IFormFile BackIDCard { get; set; }
            }

    #pragma warning disable SA1201 // Elements should appear in the correct order
            public async Task OnGetAsync(string code)
    #pragma warning restore SA1201 // Elements should appear in the correct order
            {
                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }

            public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
                returnUrl ??= this.Url.Content("/Login");
                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (this.ModelState.IsValid)
                {
                    ApplicationUser user;

                    if (string.IsNullOrWhiteSpace(this.Input.Tags) || this.Input.Tags.Split(',').Length < 3)
                    {
                        this.ModelState.AddModelError("Input.Tags", "يجب إضافاة 3 كلمات مفتاحية على الأقل.");
                        return this.Page();
                    }

                // التحقق من ان الايميل غير موجود مالم يضهر خطا يوضح ذلك
                var emailExists = await this.userManager.FindByEmailAsync(this.Input.Email);
                if (emailExists != null) {
                     this.ModelState.AddModelError("Input.Email", "البريد الالكتروني موجود مسبقا");
                    return this.Page();
                }

                // التحقق مما اذا كان الرقم القومي غير موجود مالم يضهر خطا يوضح ذلك

                var users = await this.userManager.Users.ToListAsync();
                foreach (var item in users)
                {
                    if (item.NationalID == Input.NationalID)
                    {
                        this.ModelState.AddModelError("Input.NationalID", "الرقم الوطني موجود مسبقا");
                        return this.Page();
                    }
                }

                //var nationalIDExists = await this.freelancePlatform.UserManager.GetUserByNationalIdAsync<ApplicationUser>(this.Input.NationalID);
                //if (nationalIDExists != null)
                //{
                    
                //}


                switch (this.Input.UserType)
                    {
                        case UserType.Freelancer:
                            user = new Freelancer();
                            break;
                        case UserType.Employer:
                            user = new Employer();
                            break;
                        default:
                            this.ModelState.AddModelError(null, "Choose your role.");
                            return this.Page();
                    }

                    if (this.Input?.FrontIDCard == null)
                    {
                        ModelState.AddModelError("", "يجب تحميل صورة البطاقة الأمامية.");
                        return Page();
                    }

                    if (this.freelancePlatform?.FileManager == null)
                    {
                        throw new InvalidOperationException("لم يتم تهيئة FileManager أو freelancePlatform بشكل صحيح.");
                    }

               
                    var account = this.freelancePlatform.StripeManager.CreateAccount(this.Input.FirstName, this.Input.Email);
                    string front = await this.freelancePlatform.FileManager.CarduploadAttachmentAsync(this.Input.FrontIDCard, account.Id + "front");
                    string back = await this.freelancePlatform.FileManager.CarduploadAttachmentAsync(this.Input.BackIDCard, account.Id + "back");

                user.Id = account.Id;
                    user.FirstName = this.Input.FirstName;
                    user.LastName = this.Input.LastName;
                    user.UserName = this.Input.Username;
                    user.Email = this.Input.Email;
                    user.Location = this.Input.Location;
                    user.ProfileImageUrl = "/imgprofile/user-avatar-placeholder.png";
                    user.FrontIDCard = front;
                    user.BackIDCard = back;
                    user.NationalID = this.Input.NationalID;
                    user.Tags = this.Input.Tags;
                    var result = await this.userManager.CreateAsync(user, this.Input.Password);

                    await this.userManager.AddToRoleAsync(user, this.Input.UserType.ToString());

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("User created a new account with password.");

                        var codeToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                        codeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeToken));
                        var callbackUrl = this.Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = codeToken, returnUrl = returnUrl },
                            protocol: this.Request.Scheme);

                        //await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                        SendEmailAsync(this.Input.Email, "Confirm your email",
    #pragma warning disable SA1117 // Parameters should be on same line or separate lines
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
#pragma warning restore SA1117 // Parameters should be on same line or separate lines

                        if (this.userManager.Options.SignIn.RequireConfirmedEmail) 
                        {
                        return this.RedirectToPage("RegesterConfirmationPage");

                        }

                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {

                        return this.RedirectToPage("RegesterConfirmationPage");
                        }
                        else
                        {
                            await this.signInManager.SignInAsync(user, isPersistent: false);
                            return this.LocalRedirect("/");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // If we got this far, something failed, redisplay form
                return this.Page();
            }

            private async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
            {
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    message.From = new MailAddress("amranomr122@gmail.com");
                    message.To.Add(email);
                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    message.Body = confirmLink;

                    smtpClient.Port = 587;
                    smtpClient.Host = "smtp.gmail.com";


                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("amranomr122@gmail.com", "qniz htke vsgf sbak");
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
