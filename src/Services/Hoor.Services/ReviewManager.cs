namespace Hoor.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hoor.Data.Common.Repositories;
    using Hoor.Data.Models;
    using Hoor.Services.Interfaces;
    using Hoor.Services.Mapping;
    using Hoor.Web.ViewModels.Reviews;
    using Microsoft.EntityFrameworkCore;

    public class ReviewManager : IReviewManager
    {
        private readonly IDeletableEntityRepository<Review> repository;

        public ReviewManager(IDeletableEntityRepository<Review> repository)
        {
            this.repository = repository;
        }

        public async Task CreateAsync(ReviewInputModel input)
        {
            var review = new Review
            {
                RecipientId = input.RecipientId,
                SenderId = input.SenderId,
                Rating = input.Rating,
                Text = input.Text,
            };

            await this.repository.AddAsync(review);
            await this.repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllUserReviewsAsync<T>(string userId)
            => await this.repository
                .All()
                .Where(x => x.RecipientId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

        public int GetReviewsCount(string userId)
            => this.repository
                .All()
                .Count(x => x.RecipientId == userId);
    }
}
