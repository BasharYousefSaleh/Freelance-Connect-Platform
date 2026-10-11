namespace Hoor.Web.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContractsController : BaseController
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly UserManager<ApplicationUser> userManager;

        public ContractsController (
            IFreelancePlatform freelancePlatform,
            UserManager<ApplicationUser> userManager )
        {
            this.freelancePlatform = freelancePlatform;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Administrator, Freelancer, Employer")]
        public async Task<IActionResult> Index ( string id )
        {
            var userId = this.userManager.GetUserId(this.User);
            var contract = await this.freelancePlatform.ContractManager.GetContractByIdAsync<SingleContractViewModel>(id);

            if ( contract is null ||
                ( contract.FreelancerId != userId && contract.EmployerId != userId ) )
            {
                return this.View("Error");
            }

            return this.View(contract);
        }

        [Authorize(Roles = "Employer, Freelancer")]
        public async Task<IActionResult> MyContracts ( )
        {
            var userId = this.userManager.GetUserId(this.User);
            var contracts = await this.freelancePlatform.ContractManager
                .GetAllUserContractsAsync<UserContractsListViewModel>(userId);

            this.ViewData["ContractsCount"] = contracts.Count();

            return this.View(contracts);
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public IActionResult CompleteContract ( string contractId )
        {
            if ( contractId == null )
            {
                return this.View("Error");
            }

            return this.RedirectToAction("Checkout", "Payments", new { id = contractId });
        }
        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> CancelfreelancerContractfreelancer ( string contractId )
        {
            var contract = await this.freelancePlatform.ContractManager
                    .GetContractByIdAsync<SingleContractViewModel>(contractId);

            if ( contractId is null )
            {
                return this.View("Error");
            }

            //await this.freelancePlatform.ContractManager.CansleContractStatusAsync(contract.FreelancerId, contractId);
            await this.freelancePlatform.ContractManager.SetContractStatusAsync(ContractStatus.Canceled, contractId);
            await this.freelancePlatform.JobManager.SetJobStatusAsync(JobStatus.Open, contract.OfferJobId);

            var cancellationNotification = new Notification
            {
                Icon = GlobalConstants.ContractCancelationIcon,
                Text = $"{contract.FreelancerFirstName} {contract.FreelancerLastName}  قام بتأكيد الغاء العقد الخاص بالوضيفة:  {contract.OfferJobTitle}. الان الوضيفة متاحة يمكنك استقبال العروض.",
                RedirectAction = "MyContracts",
                RedirectController = "Contracts",

            };

            await this.freelancePlatform.NotificationManager.CreateAsync(cancellationNotification, contract.EmployerId);

            return this.RedirectToAction("MyContracts", "Contracts");

        }

        [HttpPost]
        [Authorize(Roles = "Employer, Freelancer")]
        public async Task<IActionResult> Thanks ( string contractId, string Title, string HeadTitle )
        {
            //جلب المودل واضافة الرسالة 

            Thanks tanks = new Thanks
            {
                Title = Title,
                HeadTitle = HeadTitle
            };

            var contract = await this.freelancePlatform.ContractManager
                    .GetContractByIdAsync<SingleContractViewModel>(contractId);


            await this.freelancePlatform.HoorPaymentManeger.ChangePaymentFreeState("NO", contractId);


            await this.freelancePlatform.ContractManager.SetContractStatusAsync(ContractStatus.Finished, contractId);

            await this.freelancePlatform.JobManager.SetJobStatusAsync(JobStatus.Closed, contract.OfferJobId);

            var notification = new Notification
            {
                Icon = GlobalConstants.ContractCompletetionIcon,
                Text = $"{contract.EmployerFirstName} {contract.EmployerLastName} قام باكمال العقد الخاص بالوضيفة :  '{contract.OfferJobTitle}'",
                RedirectAction = "Index",
                RedirectController = "Contracts",
                RedirectId = contract.Id,
            };

            await this.freelancePlatform.NotificationManager.CreateAsync(notification, contract.FreelancerId);


            return this.View(tanks);
        }


        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CancelContract ( string contractId )
        {
            var contract = await this.freelancePlatform.ContractManager
                .GetContractByIdAsync<SingleContractViewModel>(contractId);

            if ( contractId is null )
            {
                return this.View("Error");
            }

            if ( contract.Status == ContractStatus.Paying )
            {
                await this.freelancePlatform.ContractManager.SetContractStatusAsync(ContractStatus.Canceled, contractId);
                await this.freelancePlatform.JobManager.SetJobStatusAsync(JobStatus.Open, contract.OfferJobId);
                var cancellationNotification1 = new Notification
                {
                    Icon = GlobalConstants.ContractCancelationIcon,
                    Text = $"{contract.EmployerFirstName} {contract.EmployerLastName} قام بإلغاء العقد الخاص بالوضيفة  : {contract.OfferJobTitle}. ",
                    RedirectAction = "MyContracts",
                    RedirectController = "Contracts",
                };

                await this.freelancePlatform.NotificationManager.CreateAsync(cancellationNotification1, contract.FreelancerId);

                return this.RedirectToAction("MyContracts", "Contracts");
            }

            await this.freelancePlatform.ContractManager.CansleContractStatusAsync(contract.FreelancerId, contractId);
            //await this.freelancePlatform.ContractManager.SetContractStatusAsync(ContractStatus.Canceled, contractId);
            //await this.freelancePlatform.JobManager.SetJobStatusAsync(JobStatus.Open, contract.OfferJobId);

            var cancellationNotification = new Notification
            {
                Icon = GlobalConstants.ContractCancelationIcon,
                Text = $"{contract.EmployerFirstName} {contract.EmployerLastName} يريد الغاء عقد الوضيفة : {contract.OfferJobTitle}.",
                RedirectAction = "MyContracts",
                RedirectController = "Contracts",
            };

            await this.freelancePlatform.NotificationManager.CreateAsync(cancellationNotification, contract.FreelancerId);

            return this.RedirectToAction("MyContracts", "Contracts");
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> UploadWork ( [FromForm] IFormFile attachment, string contractId )
        {
            await this.freelancePlatform.FileManager.AddFileToContractAsync(attachment, contractId);
            var contract = await this.freelancePlatform.ContractManager
                .GetContractByIdAsync<SingleContractViewModel>(contractId);

            var notification = new Notification
            {
                Icon = GlobalConstants.ContractAttachmentIcon,
                Text = $"{contract.FreelancerFirstName} {contract.FreelancerLastName} قام بأضافة ملف في عقد الوضيفة:  {contract.OfferJobTitle}",
                RedirectAction = "Index",
                RedirectController = "Contracts",
                RedirectId = contract.Id,
            };

            await this.freelancePlatform.NotificationManager.CreateAsync(notification, contract.EmployerId);

            return this.RedirectToAction("Index", "Contracts", new { id = contractId });
        }
        [HttpPost]
        [Authorize(Roles = "Employer,Freelancer")]
        public async Task<IActionResult> InformRecuest ( [FromForm] string Informdescription, string contractId )
        {
            if ( contractId == null )
            {
                return this.View("Error");
            }

            var userId = this.userManager.GetUserId(this.User);
            var type = UserType.Freelancer;
            if ( User.IsInRole("Employer") )
                type = UserType.Employer;

            var contract = await this.freelancePlatform.ContractManager
                .GetContractByIdAsync<SingleContractViewModel>(contractId);

            await this.freelancePlatform.ContractManager.SetContractInformed(Informdescription, contractId, type);
            var cancellationNotification = new Notification
            {
                Icon = GlobalConstants.ContractCancelationIcon,
                Text = $"{contract.EmployerFirstName} {contract.EmployerLastName} قام بالإبلاغ عن عقد الوظيفة :  {contract.OfferJobTitle}.",
                RedirectAction = "MyContracts",
                RedirectController = "Contracts",
            };


            return this.RedirectToAction("Index", "Contracts", new { id = contractId });
        }

    }
}
