namespace Hoor.Web.ViewModels.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Hoor.Common;
    using Hoor.Data.Models;
    using Hoor.Services.Mapping;

    public class SingleJobViewModel : BaseJobViewModel, IHaveCustomMappings
    {
        public string Description { get; set; }

        public string ContractId { get; set; }

        public string EmployerId { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public double EmployerRating { get; set; }

        public string EmployerProfileImageUrl { get; set; }

        public Country EmployerLocation { get; set; }

        public List<string> OffersFreelancerIds { get; set; }

        public List<string> ContractsFreelancerIds { get; set; }
        public string fileurl { get; set; }
        public string EmployerLocationToString => this.EmployerLocation.GetAttribute<DisplayAttribute>().Name;
        public string? AttachmentUrl { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentExt { get; set; }

        public string DateFormatted => TimeCalculator.GetTimeAgo(this.CreatedOn);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Job, SingleJobViewModel>().ForMember(
                x => x.AttachmentUrl,
                options => options.MapFrom(j => j.AttachmentUrl))

                .ForMember(
                    x => x.OffersFreelancerIds,
                    options => options
                        .MapFrom(
                            j => j.Offers
                                .Where(o => !o.IsAccepted)
                                .Select(f => f.FreelancerId)))

                .ForMember(
                    x => x.ContractsFreelancerIds,
                    options => options
                        .MapFrom(
                            c => c.Contracts
                                .Where(x => x.Status == ContractStatus.Ongoing)
                                .Select(i => i.FreelancerId)))

                .ForMember(
                    x => x.ContractId,
                    options => options
                        .MapFrom(
                            c => c.Contracts
                                .FirstOrDefault(x => x.Status == ContractStatus.Ongoing)
                                .Id))

                .ForMember(
                    x => x.EmployerRating,
                    options => options
                        .MapFrom(
                            c => c.Employer.ReceivedReviews.Any() ?
                                Math.Round(c.Employer.ReceivedReviews.Average(x => x.Rating), 2) : 0.0));
        }
    }
}
