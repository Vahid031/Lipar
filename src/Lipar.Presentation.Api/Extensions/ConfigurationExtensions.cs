using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Presentation.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Lipar.Presentation.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddLiparConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, LiparOptions liparOptions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(liparOptions.Swagger.Url, liparOptions.Swagger.Name);
                //c.RoutePrefix = "";
            });

            app.UseApiExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



}

