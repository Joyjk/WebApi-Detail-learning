using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_test.Entities;

namespace WebApi_test.Services
{
    public interface IRepository
    {
        Task<List<Genre>> getAllGenre();
        public Genre getGenreByID(int id);
        public void AddGenre(Genre genre);
    }
}
