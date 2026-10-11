namespace Hoor.Web.ViewModels.Notifications
{
    using Hoor.Data.Models;
    using Hoor.Services.Mapping;

    public class JobNotificationViewModel : IMapFrom<Job>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string EmployerId { get; set; }
    }
}
