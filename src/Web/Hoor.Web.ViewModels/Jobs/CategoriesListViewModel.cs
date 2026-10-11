namespace Hoor.Web.ViewModels.Jobs
{
    using Hoor.Data.Models;
    using Hoor.Services.Mapping;

    public class CategoriesListViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
