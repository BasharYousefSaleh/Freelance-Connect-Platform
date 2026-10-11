using Hoor.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;

namespace Hoor.Web.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl = null)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = System.Text.Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(code));

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                //  ÕœÌÀ «·⁄„Êœ ConfirmEmail ≈·Ï true
                user.EmailConfirmed = true;
                var updateResult = await this.userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    this.StatusMessage = "Error updating the confirmation status.";
                }
                else
                {
                    this.StatusMessage = "‘ﬂ—« ·ﬂ ⁄·Ï «· Õﬁﬁ „‰ «·»—Ìœ «·«·ﬂ —Ê‰Ì Œ«’ ﬂ." + user.EmailConfirmed;
                }
            }
            else
            {
                this.StatusMessage = "Error confirming your email.";
            }

            returnUrl ??= Url.Content("~/");
            //return this.Redirect(returnUrl);
            return Page();
            
        }
    }
}
