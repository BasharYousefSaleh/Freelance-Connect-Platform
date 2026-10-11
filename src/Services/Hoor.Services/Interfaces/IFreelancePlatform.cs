namespace Hoor.Services.Interfaces
{
    public interface IFreelancePlatform
    {
        IHoorPaymentManeger HoorPaymentManeger { get; }
        IJobManager JobManager { get; }

        ICategoryManager CategoryManager { get; }

        IOfferManager OfferManager { get; }

        IContractManager ContractManager { get; }

        IUserManager UserManager { get; }

        IStripeManager StripeManager { get; }

        IMessageManager MessageManager { get; }

        IFileManager FileManager { get; }

        IReviewManager ReviewManager { get; }

        INotificationManager NotificationManager { get; }
    }
}
