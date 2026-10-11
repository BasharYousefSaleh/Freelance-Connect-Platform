using Hoor.Data.Common.Repositories;
using Hoor.Data.Models;
using Hoor.Services.Interfaces;
using Hoor.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hoor.Services
{
    public class HoorPaymentManeger : IHoorPaymentManeger
    {
        private readonly IRepository<Payment> paymentRepository;
        private readonly IRepository<Withdrawal> withdrawalRepository;
        private readonly IRepository<PaymentMethod> PaymentMethodRepository;

        public HoorPaymentManeger (
            IRepository<Payment> repository,
            IRepository<PaymentMethod> paymentMethodRepository,
            IRepository<Withdrawal> withdrawalRepository )
        {
            paymentRepository = repository;
            PaymentMethodRepository = paymentMethodRepository;
            this.withdrawalRepository = withdrawalRepository;
        }

        public async Task<bool> ChangePaymentFreeState ( string state, string ContractId )
        {
            Payment payment = await paymentRepository.All()
              .Where(x => x.ContractID == ContractId)
              .FirstOrDefaultAsync();

            payment.FreeStatus = state.ToString();
            if ( state == "NO" )
            {
                payment.ReleaseDate = DateTime.Now;
            }

            this.paymentRepository.Update(payment);

            await paymentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Payment> CreatePaymentAsync ( Payment payment )
        {


            await paymentRepository.AddAsync(payment);
            await paymentRepository.SaveChangesAsync();
            return payment;
            // Save payment to database
        }

        public async Task<Payment> GetPaymentAsync ( string PaymentID )
        {


            return await paymentRepository.All()
                .Where(x => x.Id == PaymentID)
                .To<Payment>()
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> GetPaymentByContractIDAsync ( string ContractID )
        {
            return await paymentRepository.All()
                .Where(x => x.ContractID == ContractID)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Payment>> GetPaymentsByUserIDAsync ( string UserID )
        {
            decimal withdrawableAmount = 0;
            decimal pendingAmount = 0;
            List<Payment> payments = await this.paymentRepository.All()
                .Where(x => x.Contract.FreelancerId == UserID)
                .ToListAsync();

            foreach ( var payment in payments )
            {
                if ( payment.ReleaseDate != null )
                {
                    if ( payment.ReleaseDate?.AddDays(10) <= DateTime.Now && payment.FreeStatus != "OK" )
                    {
                        // بعد مرور 10 أيام، نعتبر المبلغ قابلاً للسحب
                        withdrawableAmount += payment.Amount;
                        payment.FreeStatus = "OK"; // تغيير الحالة إلى Released
                        paymentRepository.Update(payment);
                    }
                }
            }

            await this.paymentRepository.SaveChangesAsync();

            return payments;
        }

        public void ThanksPage ( string Title, string HeadTitle, string id )
        {


        }

        public async Task<bool> UpdatePaymentAsync ( Payment payment )
        {
            paymentRepository.Update(payment);
            await paymentRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Payment> UpdatePaymentStatusAsync ( string ContractID, string Status )
        {
            Payment payment = await paymentRepository.All()
               .Where(x => x.ContractID == ContractID)
               .FirstOrDefaultAsync();
            payment.Status = Status;
            paymentRepository.Update(payment);
            await paymentRepository.SaveChangesAsync();

            return payment;

        }
        public async Task<PaymentMethod> SeedPaymentMethod ( )
        {

            PaymentMethod[] paymentMethods = {
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay12.png",Name = "محفظة جيب" , Status = "True",PaymentType="Locle"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay10.png",Name = "جوالي" , Status = "True",PaymentType="Locle"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay1.png",Name = "ام فلوس" , Status = "True",PaymentType="Locle"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay2.png",Name = "محفظتي" , Status = "True",PaymentType="Locle"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay9.png",Name = "ون كاش" , Status = "True",PaymentType="Locle"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "774026721",Icon ="/pyments/pay01.png",Name = "البطائق الألكترونية" , Status = "True",PaymentType="Card"},
            new PaymentMethod(){AccountName ="ممدوح احمد عبدالحبيب مالك",AccountNumber = "488344924",Icon ="/pyments/pay02.png",Name = "Binance" , Status = "True",PaymentType="Binance"},
            };
            foreach ( var item in paymentMethods )
            {
                await this.PaymentMethodRepository.AddAsync(item);
            }
            await this.PaymentMethodRepository.SaveChangesAsync();

            return paymentMethods[0];

        }
        public async Task<List<PaymentMethod>> GetAllMethodAsync ( )
        {

            List<PaymentMethod> paymentMethods;
            //جلب جميع طرق الدفع وتخزينها بالمصفوفة
            paymentMethods = await this.PaymentMethodRepository.All().ToListAsync();

            return paymentMethods;

        }
        public async Task<PaymentMethod> GetMethodAsync ( string id )
        {
            return await this.PaymentMethodRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Withdrawal> CreateWithdrawalAsync ( Withdrawal withdrawal )
        {
            await withdrawalRepository.AddAsync(withdrawal);
            await withdrawalRepository.SaveChangesAsync();
            return withdrawal;
        }

        public async Task<Withdrawal> GetWithdrawalByIdAsync ( string Id )
        {
            return await withdrawalRepository.All().Where(e => e.Id == Id).FirstAsync();
        }

        public async Task<List<Withdrawal>> GetWithdrawalsByUserIdAsync ( string UserId )
        {
            return await
            withdrawalRepository.All().Where(e => e.UserId == UserId).ToListAsync();
        }
    }
}
