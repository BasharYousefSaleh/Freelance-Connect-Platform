namespace Hoor.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Hoor.Web.ViewModels.Jobs;
    using Hoor.Web.ViewModels.Users.Freelancers;

    public class HomeViewModel
    {
        public int JobsCount { get; set; }

        public int OffersCount { get; set; }

        public int FreelancersCount { get; set; }

        public IEnumerable<FreelancerViewModel> Freelancers { get; set; }

        public IEnumerable<AllJobsListViewModel> Jobs { get; set; }
    }
}
