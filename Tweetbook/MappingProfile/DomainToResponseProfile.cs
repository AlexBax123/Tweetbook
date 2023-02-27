using AutoMapper;
using System.Linq;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;

namespace Tweetbook.MappingProfile
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest=>dest.Tags, opt=>opt.MapFrom(src=>src.Tags.Select(t=>new TagResponse { Id= t.Id, Name=t.Name})))
                ;
            CreateMap<Tag, TagResponse>();

        }
    }
}
