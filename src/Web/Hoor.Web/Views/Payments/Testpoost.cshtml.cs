using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Hoor.Web.Views.Payments
{
    public class TestpoostModel : PageModel
    {

            [Required]
            [Display(Name = "ContractId")]
            public string ContractId { get; set; }
      
        
        public void OnGet()
        {

        }

        public void OnPost() { 
        
        }
    }
}
