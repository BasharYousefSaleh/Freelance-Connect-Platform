namespace Hoor.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Hoor";
        public const int PlatformFeePercentage = 10;

        public const string AdministratorRoleName = "Administrator";
        public const string EmployerRoleName = "Employer";
        public const string FreelancerRoleName = "Freelancer";

        public const string LocalHostHomePage = "https://localhost:44319/";
        public const string LocalHostRegisterPage = "https://localhost:44319/Register";

        // Stripe Configuration (Use Environment Variables in Production)
        public const string StripeConfigurationKey = "YOUR_STRIPE_SECRET_KEY";
        public const string StripeTestOAuthLink = "https://connect.stripe.com/oauth/authorize?redirect_uri=https://connect.stripe.com/hosted/oauth&client_id=YOUR_CLIENT_ID&state=YOUR_STATE&response_type=code&scope=read_write&stripe_user[country]=US";
        public const string StripeWebHookEndpointSecret = "YOUR_STRIPE_WEBHOOK_SECRET";

        // notification icons
        public const string OfferIcon = "icon-material-outline-gavel";
        public const string ContractIcon = "icon-material-outline-library-books";
        public const string ContractAttachmentIcon = "icon-material-outline-attach-file";
        public const string ContractCompletetionIcon = "icon-feather-dollar-sign";
        public const string ContractCancelationIcon = "icon-line-awesome-close";
        public const string JobIcon = "icon-material-outline-business-center";

        // Cloudinary Configuration (Use Environment Variables in Production)
        public const string CloudinaryApiKey = "YOUR_CLOUDINARY_API_KEY";
        public const string CloudinaryApiSecretKey = "YOUR_CLOUDINARY_API_SECRET";
    }
}