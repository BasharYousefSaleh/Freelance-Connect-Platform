namespace Hoor.Web.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Withdrawal;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class WithdrawalController : Controller
    {
        private readonly IFreelancePlatform freelancePlatform;
        private readonly UserManager<ApplicationUser> userManager;
        public WithdrawalController ( IFreelancePlatform freelancePlatform, UserManager<ApplicationUser> userManager )
        {
            this.userManager = userManager;
            this.freelancePlatform = freelancePlatform;

        }
        public async Task<IActionResult> Index ( )
        {
            decimal? withdrawableAmount = 0;
            decimal? pendingAmount = 0;
            var user = await this.userManager.GetUserAsync(this.User);
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
            var modle = new WithdrawalViewModle
            {
                UserId = user.Id,
                Balance = withdrawableAmount ?? 0,
                PaymentMethods = await this.freelancePlatform.HoorPaymentManeger.GetAllMethodAsync(),

            };


            return View(modle);
        }
        [HttpPost]
        public async Task<IActionResult> WithdrawalRequest ( string PaymentMethod, string Account, string AccountName, string Amount )
        {

            // التحقق من ان البيانات مدخلة بشكل صحيح

            if ( string.IsNullOrEmpty(Amount) || string.IsNullOrEmpty(PaymentMethod) || string.IsNullOrEmpty(AccountName) )
            {
                return RedirectToAction("Index");
            }

            // التحقق من ان الرصيد كافي
            if ( decimal.Parse(Amount) <= 0 )
            {
                return RedirectToAction("Index");
            }
            var user = await this.userManager.GetUserAsync(this.User);

            var wither = new Withdrawal
            {
                AccountId = Account,
                AccountName = AccountName,
                Amount = decimal.Parse(Amount),
                PaymentMethod = PaymentMethod,
                UserId = user.Id,
                IsConfirmed = false,
            };
            await this.freelancePlatform.HoorPaymentManeger.CreateWithdrawalAsync(wither);

            var formData1 = new
            {

                HeadTitle = "تم ارسال الطلب",
                Title = "ستتم مراجعة العملية وتحوبل المبلغ مباشرة . مالم قم بمراسلة ادارة الموقع.",
            };


            return RedirectToAction("Thanks", "Payments", formData1);

            // معالجة طلب السحب هنا...

            //return RedirectToAction("Success");
        }

    }
    public class WithdrawalModel
    {
        public string Amount;
        public string PaymentMethod;
        public string Account;
        public string AccountName;
    }
}
