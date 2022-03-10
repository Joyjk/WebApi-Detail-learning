using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Services;

namespace WebApi_test.Helpers
{
    public class GenreHATEOASAttribute:HATEOASAttribute
    {
        private readonly LinksGenerator linksGenerator;

        public GenreHATEOASAttribute(LinksGenerator linksGenerator)
        {
            this.linksGenerator = linksGenerator;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var includeHATEOAS = shouldIncludeHateoas(context);
            if(!includeHATEOAS)
            {
                await next();
                return;
            }

            await linksGenerator.Generate<GenreDTO>(context, next);

        }
    }
}
