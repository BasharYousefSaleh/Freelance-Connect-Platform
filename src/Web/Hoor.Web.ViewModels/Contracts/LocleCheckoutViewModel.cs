using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoor.Web.ViewModels.Contracts
{
    public class LocleCheckoutViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string ContractId { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public string Icon { get; set; }

        public int OfferFixedPrice { get; set; }

        public string? ImageUrl { get; set; }



    }
}
