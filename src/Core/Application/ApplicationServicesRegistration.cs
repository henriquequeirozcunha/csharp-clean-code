using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServicesRegistration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}