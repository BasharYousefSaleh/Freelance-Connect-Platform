namespace Hoor.Web.ViewModels.Contracts
{
    using Hoor.Data.Models;
    using Hoor.Services.Mapping;

    public class AttachmentListViewModel : IMapFrom<Attachment>
    {
        public string Name { get; set; }

        public string ShortenedName => this.Name.Length > 15 ? $"{this.Name.Substring(0, 15)}..." : this.Name;

        public string Extension { get; set; }

        public string Url { get; set; }
    }
}
