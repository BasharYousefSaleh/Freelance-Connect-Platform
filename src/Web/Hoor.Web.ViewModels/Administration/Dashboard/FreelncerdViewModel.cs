using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoor.Web.ViewModels.Administration.Dashboard
{
    public class FreelncerdViewModel
    {
        public FreelncerdViewModel(int SettingsCount, string Name, int Rating)
        {
            this.Name = Name;
            this.Rating = Rating;
            this.SettingsCount = SettingsCount;
        }
        public int SettingsCount { get; set; }
        //public IEnumerable<FreelancerViewModel> Freelancers { get; set; }
        public string Name { get; set; }

        public int Rating { get; set; }
    }
}
