namespace Hoor.Web.ViewModels.Notifications
{
    using Hoor.Data.Models;
    using Hoor.Services.Mapping;

    public class ContractNotificationViewModel : IMapFrom<Contract>
    {
        public string Id { get; set; }

        public string FreelancerId { get; set; }

        public string FreelancerFirstName { get; set; }

        public string FreelancerLastName { get; set; }

        public string EmployerId { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string JobId { get; set; }

        public string JobTitle { get; set; }
    }
}
