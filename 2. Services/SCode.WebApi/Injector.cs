using System.Linq;
using ByteSizeLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCode.WebApi.Application.Abstraction.Identity.ApiKeyAuthenticationSupport;
using SCode.WebApi.Application.Services;
using SCode.WebApi.Hubs.ClassroomHub;

namespace SCode.WebApi
{
    public static class Injector
    {
        public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
        {
            var studentWebAppUrl = configuration
                .GetValue<string>("StudentWebAppUrl");
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .WithOrigins(studentWebAppUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
        
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddApiKeyAuthentication();
            
            services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
            
            return services;
        }
        
        public static IServiceCollection AddAppSignalR(this IServiceCollection services)
        {
            services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.MaximumReceiveMessageSize =
                        (long) ByteSize.FromMebiBytes(0.5).Bytes;
                });

            // SignalR
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes
                    .Concat(new[] {"application/octet-stream"});
            });
            
            return services;
        }
        
        public static IServiceCollection AddApp(this IServiceCollection services)
        {
            services.AddClassroomHub();

            services.AddHostedService<AppService>();

            return services;
        }
    }
}