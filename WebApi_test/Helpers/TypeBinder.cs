using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi_test.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName  = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var deserializedValue = JsonConvert.DeserializeObject<T>(valueProviderResult.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedValue);
            }
            catch
            {

                bindingContext.ModelState.TryAddModelError(propertyName, "value is invalid for type List<int>");

            }
            return Task.CompletedTask;
        }
    }
}
