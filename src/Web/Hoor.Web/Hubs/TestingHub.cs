using Hoor.Common;
using Hoor.Data.Models;
using Hoor.Services.Interfaces;
using Hoor.Web.ViewModels.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Hoor.Web.Hubs
{

    public class TestingHub : Hub
    {
        public class PaymentData
        {
            public string PaymentImage { get; set; }
            public string ContractId { get; set; }
            public string PaymentType { get; set; }
        }

        private readonly IFreelancePlatform freelancePlatform;

        public TestingHub(
            IFreelancePlatform freelancePlatform,
            UserManager<ApplicationUser> userManager)
        {
            this.freelancePlatform = freelancePlatform;
        }

        public async Task<ActionResult<object>> Index()
        {
            return new { State = "" };
        }


        public async Task PaymentSucc(string contractId)
        {
            if (string.IsNullOrWhiteSpace(contractId))
            {
                //return;
            }

            try
            {
                var contract = await this.freelancePlatform.ContractManager
                        .GetContractByIdAsync<ContractNotificationViewModel>(contractId);

                await freelancePlatform.ContractManager.
                    SetContractStatusAsync(Common.ContractStatus.Ongoing, contractId);

                await this.freelancePlatform.HoorPaymentManeger.UpdatePaymentStatusAsync( contractId, PaymentState.succeeded.ToString());
                await freelancePlatform.HoorPaymentManeger.
                    ChangePaymentFreeState( PaymentState.succeeded.ToString(),contractId);

                var notification = new Notification
                {
                    Icon = GlobalConstants.ContractIcon,
                    Text = $"تم اكمال جميع اجراءات الوضيفة :'{contract.JobTitle}' يمكنك الان بدء التنفيذ",
                    RedirectAction = "Index",
                    RedirectController = "Contracts",
                    RedirectId = contract.Id,
                };


                await this.freelancePlatform.NotificationManager
                    .CreateAsync(notification, contract.FreelancerId);

                Console.WriteLine("Contract status updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating contract status: {ex.Message}");
            }
        }

        public async Task LoclePayment(string base64File)
        {
            
        }


    }
}
