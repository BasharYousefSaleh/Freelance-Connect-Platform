namespace Hoor.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hoor.Data.Common.Repositories;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryManager : ICategoryManager
    {
        private readonly IRepository<Category> repository;

        public CategoryManager(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllJobCategoriesAsync<T>()
            => await this.repository
                .All()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToListAsync();
    }
}
