namespace Hoor.Web.Tests.Controllers
{
    using System.Collections.Generic;

    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Web.Controllers;
    using Microsoft.AspNetCore.Identity;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class NotificationControllerTests
    {
        [Fact]
        public void MarkNotificationAsReadShouldReturnJson()
            => MyController<NotificationsController>
                .Instance(
                    controller => controller
                        .WithDependencies(
                            From.Services<IFreelancePlatform>(),
                            From.Services<UserManager<ApplicationUser>>())
                        .WithUser(
                            user => user
                                .WithIdentifier("test123")
                                .WithUsername("tonsan1")
                                .InRole("Employer")))
                .Calling(x => x.MarkNotificationAsRead("testId"))
                .ShouldHave()
                .ActionAttributes(
                    attributes => attributes
                        .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Json();
    }
}
