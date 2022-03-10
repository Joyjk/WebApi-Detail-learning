using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;
using WebApi_test.DTO;

namespace WebApi_test.Services
{
    public class LinksGenerator
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        public LinksGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper GetUrlHelper()
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task Generate<T>(ResultExecutingContext context, ResultExecutionDelegate next) where T : class , IGenerateHATEOASLinks
        {
            var urlHelper = GetUrlHelper();
            var result = context.Result as ObjectResult;

            var model = result.Value as T;
            if (model == null)
            {

            }
            else
            {
                model.GenerateLinks(urlHelper);
            }

        }

    }
}
