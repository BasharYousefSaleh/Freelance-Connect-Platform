using Hoor.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hoor.Services.Interfaces
{
    public interface IHoorPaymentManeger
    {
        //دالة اضافة عملية دفع جديدة 
        Task<Payment> CreatePaymentAsync ( Payment payment );

        Task<bool> UpdatePaymentAsync ( Payment payment );

        //دالة تعديل عملية دفع معينة بناء على رقم العقد
        Task<Payment> UpdatePaymentStatusAsync ( string ContractID, string Status );

        //دالة جلب عملية دفع حسب ID
        Task<Payment> GetPaymentAsync ( string PaymentID );

        //دالة جلب جميع عمليات الدفع التابعة لمستقل معين
        Task<List<Payment>> GetPaymentsByUserIDAsync ( string UserID );

        //دالة جلب عملية دفع حسب رقم العقد
        Task<Payment> GetPaymentByContractIDAsync ( string ContractID );

        void ThanksPage ( string Title, string HeadTitle, string id );

        Task<bool> ChangePaymentFreeState ( string state, string ContractId );
        Task<PaymentMethod> SeedPaymentMethod ( );

        Task<List<PaymentMethod>> GetAllMethodAsync ( );

        Task<PaymentMethod> GetMethodAsync ( string id );
        Task<Withdrawal> CreateWithdrawalAsync ( Withdrawal payment );
        //دالة جلب عملية سحب حسي id
        Task<Withdrawal> GetWithdrawalByIdAsync ( string Id );
        //دالة جلب عمليات اليحب حسب معرف المستخدم
        Task<List<Withdrawal>> GetWithdrawalsByUserIdAsync ( string UserId );


    }
}
