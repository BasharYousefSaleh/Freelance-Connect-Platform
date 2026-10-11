using Hoor.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoor.Data.Models
{
    public class PaymentMethod : BaseDeletableModel<string>
    {
        public PaymentMethod()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string? Fee { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public string Icon{ get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
