namespace Hoor.Web.ViewModels.Jobs
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    

    public class JobInputModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Title must be between {2} and {1} characters long.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        [Required]
        [Display(Name = "Budget")]
        [Range(10, double.MaxValue, ErrorMessage = "Budget is too low.")]
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between {2} and {1} characters long.")]
        public string Description { get; set; }
  
        public IFormFile Attachment { get; set; }

        public string AttachmentUrl { get; set; }

        public IEnumerable<CategoriesListViewModel> Categories { get; set; }
    }
}
