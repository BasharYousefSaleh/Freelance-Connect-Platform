namespace Hoor.Web.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PaymentsController : Controller
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly HttpClient _httpClient;


        public PaymentsController ( IFreelancePlatform freelancePlatform, HttpClient httpClient )
        {
            this.freelancePlatform = freelancePlatform;
            this._httpClient = httpClient;
        }

        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> Checkout ( string contractId )
        {
            var contract = await this.freelancePlatform.ContractManager.GetContractByIdAsync<SingleContractViewModel>(contractId);

            if ( contract is null )
            {
                return this.View("Error");
            }

            Payment paymentval = await this.freelancePlatform.HoorPaymentManeger.GetPaymentByContractIDAsync(contractId);
            if ( paymentval is not null )
            {
                paymentval.PaymentType = "stripe";
                paymentval.Amount = contract.OfferFixedPrice;
                paymentval.AmountAfterStripeFee = ( decimal? ) ( ( int ) ( ( int ) contract.OfferFixedPrice - ( ( int ) contract.OfferFixedPrice * 0.029 ) ) - 0.30 );
                paymentval.AmountAfterPlatformFee = ( decimal? ) ( contract.OfferFixedPrice - ( ( float ) ( contract.OfferFixedPrice * GlobalConstants.PlatformFeePercentage ) / 100 ) );
                paymentval.AmountFreelacer = paymentval.AmountAfterPlatformFee;
                paymentval.PlatformBaqshes = ( decimal ) ( paymentval.AmountAfterStripeFee - paymentval.AmountAfterPlatformFee );
                paymentval.Status = PaymentState.Pending.ToString();
                paymentval.FreeStatus = "Pending";
                paymentval.ContractID = contract.Id;
                var recipientId1 = contract.FreelancerId;
                var paymentAmount1 = ( int ) contract.OfferFixedPrice * 100;
                var intent1 = this.freelancePlatform.StripeManager.CreatePaymentIntent(paymentAmount1, recipientId1, contract.Id);

                this.freelancePlatform.HoorPaymentManeger.UpdatePaymentAsync(paymentval);


                this.ViewData["ClientSecret"] = intent1.ClientSecret;
                this.ViewData["CurrentUser"] = recipientId1;

                return this.View(contract);

            }
            Payment payment = new Payment();
            payment.PaymentType = "stripe";
            payment.Amount = contract.OfferFixedPrice;
            payment.AmountAfterStripeFee = ( decimal? ) ( ( int ) ( ( int ) contract.OfferFixedPrice - ( ( int ) contract.OfferFixedPrice * 0.029 ) ) - 0.30 );
            payment.AmountAfterPlatformFee = ( decimal? ) ( contract.OfferFixedPrice - ( ( float ) ( contract.OfferFixedPrice * GlobalConstants.PlatformFeePercentage ) / 100 ) );
            payment.AmountFreelacer = payment.AmountAfterPlatformFee;
            payment.PlatformBaqshes = ( decimal ) ( payment.AmountAfterStripeFee - payment.AmountAfterPlatformFee );
            payment.Status = PaymentState.Pending.ToString();
            payment.FreeStatus = "Pending";
            payment.ContractID = contract.Id;

            var recipientId = contract.FreelancerId;
            var paymentAmount = ( int ) contract.OfferFixedPrice * 100;
            var intent = this.freelancePlatform.StripeManager.CreatePaymentIntent(paymentAmount, recipientId, contract.Id);


            _ = await freelancePlatform.HoorPaymentManeger.CreatePaymentAsync(payment);

            this.ViewData["ClientSecret"] = intent.ClientSecret;
            this.ViewData["CurrentUser"] = recipientId;

            return this.View(contract);
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> PaymentSucc ( [FromBody] dynamic data )
        {
            try
            {
                string id = data?.id?.ToString();

                if ( string.IsNullOrEmpty(id) )
                {
                    return BadRequest("Id is missing or invalid");
                }

                Console.WriteLine($"Received Id: {id}");
                return Ok(new { success = true, message = "Data received successfully" });
            }
            catch ( Exception ex )
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> PaymentMethods ( string contractId )
        {
            //await freelancePlatform.HoorPaymentManeger.SeedPaymentMethod();
            var contract = await this.freelancePlatform.ContractManager.GetContractByIdAsync<SingleContractViewModel>(contractId);



            return this.View(contract);
        }
        [HttpPost]
        [Authorize(Roles = "Freelancer, Employer")]
        public async Task<IActionResult> LocleCheckout ( string paymentid, string contractId )
        {
            var contract = await this.freelancePlatform.ContractManager.GetContractByIdAsync<SingleContractViewModel>(contractId);
            var paymentMethod = await freelancePlatform.HoorPaymentManeger.GetMethodAsync(paymentid);
            var payment = await freelancePlatform.HoorPaymentManeger.GetPaymentByContractIDAsync(contractId);
            if ( contract is null )
            {
                return this.View("Error");
            }

            if ( paymentMethod is null )
            {
                return this.View("Error");
            }
            LocleCheckoutViewModel viewModel;
            if ( payment is not null )
            {
                viewModel = new LocleCheckoutViewModel
                {
                    ContractId = contractId,
                    Id = paymentid,
                    AccountName = paymentMethod.AccountName,
                    AccountNumber = paymentMethod.AccountNumber,
                    Icon = paymentMethod.Icon,
                    Name = paymentMethod.Name,
                    PaymentType = paymentMethod.PaymentType,
                    Status = paymentMethod.Status,
                    OfferFixedPrice = contract.OfferFixedPrice,
                    ImageUrl = payment.PaymentImage,
                };
            }
            else
            {
                viewModel = new LocleCheckoutViewModel
                {
                    ContractId = contractId,
                    Id = paymentid,
                    AccountName = paymentMethod.AccountName,
                    AccountNumber = paymentMethod.AccountNumber,
                    Icon = paymentMethod.Icon,
                    Name = paymentMethod.Name,
                    PaymentType = paymentMethod.PaymentType,
                    Status = paymentMethod.Status,
                    OfferFixedPrice = contract.OfferFixedPrice,
                };
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> CompleteLoclePayment ( string contractId, string PaymentMethod, string CurrnceAmount, string Currnce, IFormFile PaymentImage )
        {
            var contract = await this.freelancePlatform.ContractManager.GetContractByIdAsync<SingleContractViewModel>(contractId);

            if ( contract is null )
            {
                return this.View("Error");
            }
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PaymentsInvoce");

            if ( !Directory.Exists(uploadFolderPath) )
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(PaymentImage.FileName)}_{Guid.NewGuid()}{Path.GetExtension(PaymentImage.FileName)}";

            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

            using ( var stream = new FileStream(filePath, FileMode.Create) )
            {
                await PaymentImage.CopyToAsync(stream);
            }

            var downloadableUrl = $"/PaymentsInvoce/{uniqueFileName}";

            Payment paymentval = await this.freelancePlatform.HoorPaymentManeger.GetPaymentByContractIDAsync(contractId);
            if ( paymentval is not null )
            {
                paymentval.PaymentType = PaymentMethod;
                paymentval.Amount = contract.OfferFixedPrice;
                paymentval.AmountAfterStripeFee = ( decimal? ) ( ( int ) ( ( int ) contract.OfferFixedPrice - ( ( int ) contract.OfferFixedPrice * 0.029 ) ) - 0.30 );
                paymentval.AmountAfterPlatformFee = ( decimal? ) ( contract.OfferFixedPrice - ( ( float ) ( contract.OfferFixedPrice * GlobalConstants.PlatformFeePercentage ) / 100 ) );
                paymentval.AmountFreelacer = paymentval.AmountAfterPlatformFee;
                paymentval.PlatformBaqshes = ( decimal ) ( paymentval.AmountAfterStripeFee - paymentval.AmountAfterPlatformFee );
                paymentval.Status = PaymentState.Pending.ToString();
                paymentval.FreeStatus = "Pending";
                paymentval.ContractID = contract.Id;
                paymentval.PaymentCurrncy = Currnce;
                paymentval.PaymentCurrncyAmount = decimal.Parse(CurrnceAmount);
                paymentval.PaymentImage = downloadableUrl;
                await this.freelancePlatform.HoorPaymentManeger.UpdatePaymentAsync(paymentval);
                var formData1 = new
                {

                    HeadTitle = "تمت عملية الدفع",
                    Title = "سيتم التحقق من بيانات عملية الدفع والرد لك مباشرتا. شكرا لك."
                };


                return RedirectToAction("Thanks", "Payments", formData1);

            }
            else
            {
                Payment payment = new Payment();
                payment.PaymentType = PaymentMethod;
                payment.Amount = contract.OfferFixedPrice;
                payment.AmountAfterStripeFee = ( decimal? ) ( ( int ) ( ( int ) contract.OfferFixedPrice - ( ( int ) contract.OfferFixedPrice * 0.029 ) ) - 0.30 );
                payment.AmountAfterPlatformFee = ( decimal? ) ( contract.OfferFixedPrice - ( ( float ) ( contract.OfferFixedPrice * GlobalConstants.PlatformFeePercentage ) / 100 ) );
                payment.AmountFreelacer = payment.AmountAfterPlatformFee;
                payment.PlatformBaqshes = ( decimal ) ( payment.AmountAfterStripeFee - payment.AmountAfterPlatformFee );
                payment.Status = PaymentState.Pending.ToString();
                payment.FreeStatus = "Pending";
                payment.ContractID = contract.Id;
                payment.PaymentCurrncy = Currnce;
                payment.PaymentCurrncyAmount = decimal.Parse(CurrnceAmount);
                payment.PaymentImage = downloadableUrl;
                await freelancePlatform.HoorPaymentManeger.CreatePaymentAsync(payment);
            }
            var formData = new
            {

                HeadTitle = "تمت عملية الدفع",
                Title = "سيتم التحقق من بيانات عملية الدفع والرد لك مباشرتا. شكرا لك."
            };


            return RedirectToAction("Thanks", "Payments", formData);


        }
        [HttpGet]
        public async Task<IActionResult> Thanks ( string Title, string HeadTitle )
        {
            Thanks tanks = new Thanks
            {
                Title = Title,
                HeadTitle = HeadTitle
            };

            return this.View(tanks);
        }
    }
}
