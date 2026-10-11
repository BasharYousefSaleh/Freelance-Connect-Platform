
namespace Hoor.Web.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Jobs;
    using Hoor.Web.ViewModels.Notifications;
    using Hoor.Web.ViewModels.Offers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class OffersController : BaseController
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly UserManager<ApplicationUser> userManager;

        public OffersController (
            IFreelancePlatform freelancePlatform,
            UserManager<ApplicationUser> userManager )
        {
            this.freelancePlatform = freelancePlatform;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> All ( string id )
        {
            var offers = await this.freelancePlatform.OfferManager
                .GetJobOffersAsync<JobOfferViewModel>(id);

            return this.View(offers);
        }

        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> MyOffers ( )
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var offers = await this.freelancePlatform.OfferManager
                .GetUserJobOffersAsync<UserOffersViewModel>(user.Id);

            var offersCount = this.freelancePlatform.OfferManager.GetActiveOffersCount(user.Id);

            this.ViewData["OffersCount"] = offersCount;

            return this.View(offers);
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> AddOffer ( OfferInputModel input )
        {
            if ( !this.ModelState.IsValid )
            {
                return this.RedirectToAction("Index", "Jobs", new { id = input.JobId });
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var job = await this.freelancePlatform.JobManager
                .GetJobByIdAsync<SingleJobViewModel>(input.JobId);

            await this.freelancePlatform.OfferManager.CreateAsync(input);

            var notification = new Notification
            {
                Icon = GlobalConstants.OfferIcon,
                Text = $"{user.FirstName} {user.LastName} . قام بتقديم عرض للوضيفة:  {job.Title}.",
                RedirectAction = "All",
                RedirectController = "Offers",
                RedirectId = job.Id,
            };

            await this.freelancePlatform.NotificationManager.CreateAsync(notification, job.EmployerId);

            return this.RedirectToAction("MyOffers", "Offers");
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AcceptOffer ( string offerId, string jobId )
        {
            await this.freelancePlatform.OfferManager.AcceptOfferAsync(offerId);
            await this.freelancePlatform.JobManager.SetJobStatusAsync(JobStatus.InContract, jobId);
            var contractId = await this.freelancePlatform.ContractManager.CreateAsync(offerId);

            var contract = await this.freelancePlatform
                .ContractManager
                .GetContractByIdAsync<ContractNotificationViewModel>(contractId);

            await this.freelancePlatform.ContractManager.SetContractStatusAsync(ContractStatus.Paying, contractId);

            var notification = new Notification
            {
                Icon = GlobalConstants.ContractIcon,
                Text = $"{contract.EmployerFirstName} {contract.EmployerLastName} " +
                $"تمت الموافقة على العرض خاصتك في الوضيفة '{contract.JobTitle}' والان العقد في المرحله الاولى",
                RedirectAction = "Index",
                RedirectController = "Contracts",
                RedirectId = contract.Id,
            };



            await this.freelancePlatform.NotificationManager
                .CreateAsync(notification, contract.FreelancerId);

            return this.RedirectToAction("Index", "Contracts", new { id = contractId });
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> DeleteOffer ( string offerId )
        {
            await this.freelancePlatform.OfferManager.DeleteOfferAsync(offerId);

            if ( this.User.IsInRole("Freelancer") )
            {
                return this.RedirectToAction("MyOffers", "Offers");
            }

            if ( this.User.IsInRole("Employer") )
            {
                return this.RedirectToAction("MyJobs", "Jobs");
            }

            return this.View("Error");
        }
    }
}
