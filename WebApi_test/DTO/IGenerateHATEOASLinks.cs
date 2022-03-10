using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi_test.DTO
{
    public interface IGenerateHATEOASLinks
    {
        void GenerateLinks(IUrlHelper urlHelper);
        ResourceCollection<T> GenerateLinksCollection<T>(List<T> dtos, IUrlHelper urlHelper);
    }
}
