using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi_test.DTO
{
    public class GenreDTO : IGenerateHATEOASLinks
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Link> Links { get; set; }= new List<Link>();

        public void GenerateLinks(IUrlHelper urlHelper)
        {

           Links.Add(new Link(urlHelper.Link
                ("getGenre", new { Id = Id }), "get-Genre", "GET"));
            Links.Add(new Link(urlHelper.Link
                ("putGenre", new { Id = Id }), "put-Genre", "PUT"));
            Links.Add(new Link(urlHelper.Link
                ("deleteGenre", new { Id = Id }), "delete-Genre", "DELETE"));
        }

        public ResourceCollection<T> GenerateLinksCollection<T>(List<T> dtos, IUrlHelper urlHelper)
        {
            throw new System.NotImplementedException();
        }
    }
}
