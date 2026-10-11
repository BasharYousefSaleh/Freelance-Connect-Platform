namespace Hoor.Web.ViewModels.Withdrawal
{
    using Hoor.Data.Models;
    using System.Collections.Generic;

    // كلاس عرض صفحة المسحوبات          
    public class WithdrawalViewModle
    {
        //معرف المستخدم , الرصيد المتح,طرق الدفع,
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

        public WithdrawaInputlModel? WithdrawaInputl { get; set; }

    }
    public class WithdrawaInputlModel
    {
        public string Amount;
        public string PaymentMethod;
        public string Account;
        public string AccountName;
    }
}
