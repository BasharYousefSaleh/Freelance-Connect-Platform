namespace Hoor.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Hoor.Data.Models;

    public static class Jobs
    {
        public static IEnumerable<Job> FiveJobs
            => Enumerable.Range(0, 5).Select(x => new Job());
    }
}
