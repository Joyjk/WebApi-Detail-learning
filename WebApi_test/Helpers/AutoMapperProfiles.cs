using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<PersonCreationDTO, Person>().
                ForMember(x=>x.Picture, options=>options.Ignore());




            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>().
                ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x=>x.MoviesGenres, options=>options.MapFrom(MapMoviesGenres))
                .ForMember(x=>x.MoviesActors, options=>options.MapFrom(MapMoviesActors));
            CreateMap<Movie, MoviePatchDTO>();


            CreateMap<IdentityUser, UserDTO>().ForMember(x => x.EmailAddress,
                options => options.MapFrom(x => x.Email)).ForMember(x => x.UserId,
                options => options.MapFrom(x => x.Id));



        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            foreach (var id in movieCreationDTO.GenresIDs)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }
            return result;
        }
        private List<MoviesActor> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActor>();
            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActor() { PersonId = actor.PersonId, Character = actor.Character });
            }
            return result;
        }
    }
}
