using Hoor.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoor.Data.Models
{
    public class Payment : BaseDeletableModel<string>
    {
        public Payment() {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }

        public string Id { get; set; }

        public decimal Amount { get; set; }

        public decimal? AmountAfterStripeFee { get; set; }

        public decimal? AmountAfterPlatformFee { get; set; }

        public decimal? AmountFreelacer { get; set; }

        public decimal? PlatformBaqshes { get; set; }

        public string Status { get; set; }

        public string? FreeStatus { get; set; }

        public string PaymentType { get; set; }

        public string? PaymentCurrncy { get; set; }

        public string? PaymentImage { get; set; }

        public decimal? PaymentCurrncyAmount { get; set; }

        [Required]
        public string ContractID { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal? TransferAmount { get; set; }

        public virtual Contract Contract { get; set; }
    }

}
