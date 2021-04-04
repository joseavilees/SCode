using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SCode.WebApi.Hubs.ClassroomHub
{
    public static class Injector
    {
        public static void MapClassroomHub(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<ClassroomHubServerExtended>("/hubs/classroom");
        }
        
        public static IServiceCollection AddClassroomHub(this IServiceCollection services)
        {
            services.AddSingleton<IClassroomRepository, ClassroomRepository>();
            services.AddSingleton<IFileContentRequestClient, FileContentRequestClient>();

            return services;
        }
    }
}