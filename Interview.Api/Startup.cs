using FluentValidation.AspNetCore;
using Interview.Api.Midleware;
using Interview.Application;
using Interview.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interview.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);

            services.AddApplication(Configuration);

            services.AddControllers(options => options.Filters.Add(new ApiResponseExceptionFilter()));
           
            services.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true)
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<ApplicationConstants>());

            services.AddAutoMapper(typeof(MappingsProfile));

            services
                .AddAuthentication(options => options.DefaultScheme = ApplicationConstants.Authentication.AuthenticationScheme)
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    ApplicationConstants.Authentication.AuthenticationScheme, op => { });

            services.AddHttpContextAccessor();

            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = ApiMidlewareDefinitions.InvalidModelStateResponseFactory);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
