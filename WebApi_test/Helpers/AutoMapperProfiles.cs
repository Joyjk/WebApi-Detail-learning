using AutoMapper;
using WebApi_test.DTO;
using WebApi_test.Entities;

namespace WebApi_test.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap< GenreCreatingDTO, Genre>();
        }
    }
}
