namespace Hoor.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hoor.Web.ViewModels.Reviews;

    public interface IReviewManager
    {
        Task CreateAsync(ReviewInputModel input);

        Task<IEnumerable<T>> GetAllUserReviewsAsync<T>(string userId);

        int GetReviewsCount(string userId);
    }
}
