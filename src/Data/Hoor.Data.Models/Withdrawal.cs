using Hoor.Data.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hoor.Data.Models
{
    public class Withdrawal : BaseDeletableModel<string>
    {
        public Withdrawal ( )
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
        //حقول عملية المسحوبات
        public string Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        [Required]
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public bool? IsConfirmed { get; set; }
    }

}
