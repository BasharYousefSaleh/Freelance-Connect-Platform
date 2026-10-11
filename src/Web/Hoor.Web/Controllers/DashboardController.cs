namespace Hoor.Web.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Dashboard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DashboardController : BaseController
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController (
            IFreelancePlatform freelancePlatform,
            UserManager<ApplicationUser> userManager )
        {
            this.userManager = userManager;
            this.freelancePlatform = freelancePlatform;
        }

        [Authorize(Roles = "Employer, Freelancer")]
        public async Task<IActionResult> Index ( )
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if ( this.User.IsInRole("Employer") )
            {
                var employer = new EmployerDashboardViewModel
                {
                    Name = user.FirstName,
                    TotalMoneyEarned = this.freelancePlatform.StripeManager.GetFreelancerBalanceAmount(user.Id),

                    FinishedContractsCount = this.freelancePlatform.ContractManager.GetFinishedContractsCount(user.Id),
                    OngoingContractsCount = this.freelancePlatform.ContractManager.GetOngoingContractsCount(user.Id),
                    JobPostedCount = this.freelancePlatform.JobManager.GetPostedJobsCount(user.Id),
                    ReviewsCount = this.freelancePlatform.ReviewManager.GetReviewsCount(user.Id),
                };

                return this.View("Employer", employer);
            }
            else if ( this.User.IsInRole("Freelancer") )
            {
                List<Withdrawal> withdrawals = await this.freelancePlatform.HoorPaymentManeger.GetWithdrawalsByUserIdAsync(user.Id);
                decimal totalwithdrawabl = 0;
                int withdrawalconfirmedcount = 0;
                int withdrawalNotconfirmedcount = 0;
                foreach ( var item in withdrawals )
                {
                    totalwithdrawabl += item.Amount;
                    if ( ( bool ) item.IsConfirmed )
                    {
                        withdrawalconfirmedcount++;
                    }
                    else
                    {
                        withdrawalNotconfirmedcount++;
                    }
                }
                decimal? withdrawableAmount = 0;
                //withdrawableAmount -= totalwithdrawabl;
                decimal? pendingAmount = 0;
                List<Payment> payments = await this.freelancePlatform.HoorPaymentManeger.GetPaymentsByUserIDAsync(user.Id);
                foreach ( var item in payments )
                {
                    if ( item.Status == PaymentState.succeeded.ToString() )
                    {
                        if ( item.FreeStatus == "OK" )
                        {
                            withdrawableAmount += item.AmountFreelacer;
                        }
                        else if ( item.FreeStatus == "NO" )
                        {
                            pendingAmount += item.AmountFreelacer;
                        }
                    }
                }
                decimal? total = 0;
                total = withdrawableAmount > totalwithdrawabl ? ( withdrawableAmount - totalwithdrawabl ) : ( totalwithdrawabl - withdrawableAmount );
                var freelancer = new FreelancerDashboardViewModel
                {
                    Name = user.FirstName,
                    //TotalMoneyEarned = this.freelancePlatform.StripeManager.GetFreelancerBalanceAmount(user.Id),
                    TotalMoneyEarned = total.ToString(),
                    TotalMoneyOngoing = pendingAmount.ToString(),
                    //TotalMoneyEarned ="0",
                    OngoingContractsCount = this.freelancePlatform.ContractManager.GetOngoingContractsCount(user.Id),
                    FinishedContractsCount = this.freelancePlatform.ContractManager.GetFinishedContractsCount(user.Id),
                    ReviewsCount = this.freelancePlatform.ReviewManager.GetReviewsCount(user.Id),
                    SentOffersCount = this.freelancePlatform.OfferManager.GetSentOffersCount(user.Id),
                    withdrawalconfirmedcount = withdrawalconfirmedcount,
                    withdrawalNotconfirmedcount = withdrawalNotconfirmedcount
                };

                return this.View("Freelancer", freelancer);
            }

            return this.View("Error");
        }
    }
}
