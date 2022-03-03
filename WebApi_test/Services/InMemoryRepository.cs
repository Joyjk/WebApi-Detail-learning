using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_test.Entities;

namespace WebApi_test.Services
{
    public class InMemoryRepository:IRepository
    {
        private List<Genre> _genres;
        public InMemoryRepository()
        {
            _genres = new List<Genre>()
            {
                new Genre(){ Id=1, Name="Comedy"},
                new Genre(){ Id=2, Name="Action"},
                new Genre(){ Id=3, Name="Drama"},
            };   
        }

        public async Task<List<Genre>> getAllGenre()
        {
            await Task.Delay(1);
            return _genres; 
        }

        public Genre getGenreByID(int id)
        {
            return _genres.FirstOrDefault(x => x.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            genre.Id = _genres.Max(x => x.Id) + 1;
            _genres.Add(genre);
        }
    }
}
