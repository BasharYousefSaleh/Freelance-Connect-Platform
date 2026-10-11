using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Hoor.Web.Areas.Identity.Pages.Account
{
    public class RegesterConfirmationPageModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {


            ViewData["Title"] = "Confirm Email";
            return this.Page();

        }
        
    }
}
