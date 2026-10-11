using Hoor.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Hoor.Web.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ì—ÃÏ ≈œŒ«· ﬂ·„… «·„—Ê—.")]
            [StringLength(100, ErrorMessage = "ÌÃ» √‰  Õ ÊÌ ﬂ·„… «·„—Ê— ⁄·Ï {2} Õ—Ê› ⁄·Ï «·√ﬁ·.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Ì—ÃÏ  √ﬂÌœ ﬂ·„… «·„—Ê—.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "ﬂ·„ « «·„—Ê— €Ì— „ ÿ«»ﬁ Ì‰.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // «·»ÕÀ ⁄‰ «·„” Œœ„ »‰«¡ ⁄·Ï UserId
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "«·„” Œœ„ €Ì— „ÊÃÊœ.");
                return Page();
            }

            // ›ﬂ  ‘›Ì— «·—„“ (code)
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));

            //  €ÌÌ— ﬂ·„… «·„—Ê—
            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, Input.Password);

            if (resetResult.Succeeded)
            {
                TempData["SuccessMessage"] = " „  €ÌÌ— ﬂ·„… «·„—Ê— »‰Ã«Õ!";
                return RedirectToPage("/Account/Login");
            }

            // ⁄—÷ «·√Œÿ«¡ ≈–« ›‘·  €ÌÌ— ﬂ·„… «·„—Ê—
            foreach (var error in resetResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
