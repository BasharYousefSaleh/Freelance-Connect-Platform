namespace Hoor.Web.Areas.Administration.Controllers
{
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Data;
    using Hoor.Services.Interfaces;
    using Hoor.Web.ViewModels.Administration.Dashboard;
    using Hoor.Web.ViewModels.Reviews;
    using Hoor.Web.ViewModels.Users.Freelancers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IFreelancePlatform freelancePlatform;

        public DashboardController(IFreelancePlatform freelancePlatform, ISettingsService settingsService)
        {
            
        
                this.freelancePlatform = freelancePlatform;
                this.settingsService = settingsService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Regester()
        {
            var freelancers = new List<Freelancer1>
    {
        new Freelancer1 { Name = "أحمد", Phone = "0123456789", Age = 25 },
        new Freelancer1 { Name = "سارة", Phone = "9876543210", Age = 30 },
        new Freelancer1 { Name = "محمد", Phone = "1234567890", Age = 28 }
    };

            var viewModel = new regestre
            {
                Freelancers1 = freelancers
            };

            return this.View(viewModel);
        }



        public IActionResult Employeed()
        {
            var viewModel = new Employeed { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        public  IActionResult Freelncerd()
        {
            FreelncerdViewModel viewModel =  new FreelncerdViewModel(10,"mamdouh",123);


            return  this.View(viewModel);
        }
    }
}
