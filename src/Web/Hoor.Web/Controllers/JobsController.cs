namespace Hoor.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Jobs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class JobsController : BaseController
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly UserManager<ApplicationUser> userManager;

        public JobsController(
            IFreelancePlatform freelancePlatform,
            UserManager<ApplicationUser> userManager)
        {
            this.freelancePlatform = freelancePlatform;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.View("Error"); // Handle invalid input
            }
            var job = await this.freelancePlatform.JobManager
                .GetJobByIdAsync<SingleJobViewModel>(id);
            if (!string.IsNullOrEmpty(job.AttachmentUrl)) { 
            var filename = Path.GetFileName(job.AttachmentUrl);
            var fileext = Path.GetExtension(job.AttachmentUrl);
            job.AttachmentName = filename.Split('_').First().ToUpper();
            job.AttachmentExt = fileext;
            }

            if (job is null)
            {
                return this.View("Error");
            }

            return this.View(job);
        }

        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> All([FromQuery] AllJobsQueryModel query)
        {
            var jobs = await this.freelancePlatform.JobManager
                .GetAllJobPostsAsync<AllJobsListViewModel>(
                query.Category,
                query.JobTitle,
                query.Sorting,
                query.CurrentPage);

            var categories = await this.freelancePlatform.CategoryManager
                .GetAllJobCategoriesAsync<CategoriesListViewModel>();

            query.Jobs = jobs;
            query.Categories = categories;

            return this.View(query);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> MyJobs()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var jobs = await this.freelancePlatform.JobManager
                .GetAllUserJobPostsAsync<UserJobsListViewModel>(user.Id);

            return this.View(jobs);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddJob()
        {
            var jobCategories = await this.freelancePlatform.CategoryManager
                .GetAllJobCategoriesAsync<CategoriesListViewModel>();

            return this.View(new JobInputModel { Categories = jobCategories });
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddJob(JobInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var jobCategories = await this.freelancePlatform.CategoryManager
                    .GetAllJobCategoriesAsync<CategoriesListViewModel>();

                return this.View(new JobInputModel { Categories = jobCategories });
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (input.Attachment != null) { 
                string attachmenturl= await this.freelancePlatform.FileManager.UploadAttachmentAsync(input.Attachment);
                //input.Budget += 10;
            input.AttachmentUrl = attachmenturl;
            }
            await this.freelancePlatform.JobManager.CreateAsync(input, user.Id);

            return this.RedirectToAction("MyJobs", "Jobs");
        }
    }
}
