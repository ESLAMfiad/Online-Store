using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.APIs.helpers;
using Store.Core;
using Store.Core.Repository;
using Store.Core.Services;
using Store.Repository;
using Store.Service;

namespace Store.APIs.Extentions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IPaymentService,PaymentService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService,OrderService>();
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository)); //by inject obj
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //by inject obj
            //builder.Services.AddAutoMapper(M=>M.AddProfiles(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles)); //treqa ashl ll profiles

            #region Errorhandling 
            Services.Configure<ApiBehaviorOptions>(options =>
               {
                   options.InvalidModelStateResponseFactory = (ActionContext) =>
                   {
                       //modelstate= dic[keyvalue]
                       //key=name of parameter
                       //value= error itself
                       var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                       .SelectMany(p => p.Value.Errors)
                       .Select(e => e.ErrorMessage).ToArray();
                       var ValidationErrorResponse = new ApiValidationErrors()
                       {
                           Errors = errors
                       };
                       return new BadRequestObjectResult(ValidationErrorResponse);
                   };
               }); 
            #endregion
            return Services; //dft felcontainer b3dha rg3to tany ll program
        }
    }
}
