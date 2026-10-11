using Hoor.Common;
using Hoor.Web.ViewModels.Users.Freelancers;
using System.Collections.Generic;

namespace Hoor.Web.ViewModels.Administration.Dashboard
{
    public class Freelancer1
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
    }

    public class regestre
    {
        public List<Freelancer1> Freelancers1 { get; set; } = new List<Freelancer1>();
    }
}
