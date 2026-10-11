namespace Hoor.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hoor.Common;

    public interface IContractManager
    {
        Task<string> CreateAsync(string offerId);
        Task SetContractInformed(string message, string contractId, UserType userType);
        Task SetContractStatusAsync(ContractStatus status, string contractId);

        Task CansleContractStatusAsync(string freelanceid, string contractId);

        Task<T> GetContractByIdAsync<T>(string id);

        Task<IEnumerable<T>> GetAllUserContractsAsync<T>(string userId);

        int GetFinishedContractsCount(string userId);

        int GetOngoingContractsCount(string userId);
    }
}
