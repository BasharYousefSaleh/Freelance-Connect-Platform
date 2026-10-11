namespace Hoor.Web.ViewModels.Dashboard
{
    public class FreelancerDashboardViewModel
    {
        public string Name { get; set; }

        public int SentOffersCount { get; set; }
        public int withdrawalconfirmedcount { get; set; }
        public int withdrawalNotconfirmedcount { get; set; }

        public string TotalMoneyEarned { get; set; }

        public string? TotalMoneyOngoing { get; set; }

        public int OngoingContractsCount { get; set; }

        public int FinishedContractsCount { get; set; }

        public int ReviewsCount { get; set; }
    }
}
