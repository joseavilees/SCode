using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCode.Client.Teacher.ConsoleApp.Application.Builders;
using SCode.Client.Teacher.ConsoleApp.Application.Clients;
using SCode.Client.Teacher.ConsoleApp.Application.Factories;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline;
using SCode.Client.Teacher.ConsoleApp.Application.Services;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;
using SCode.Client.Teacher.ConsoleApp.Infrastructure.Repositories;

namespace SCode.Client.Teacher.ConsoleApp
{
    public static class Injector
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton(GetAppSettings());
            
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IGitIgnoreService, GitIgnoreService>();
            services.AddSingleton<ILogicPathRepository, LogicPathRepository>();
            services.AddSingleton<ILogicAppFileEntryBuilder, LogicAppFileEntryBuilder>();
            services.AddSingleton<IDirectoryWatcherService, DirectoryWatcherService>();
            
            services.AddSingleton<IClassroomHubClient, ClassroomHubClient>();
            services.AddSingleton<IClassroomHubConnectionFactory, ClassroomHubConnectionFactory>();

            services.AddFileChangePipeline();
            
            services.AddMediatR(typeof(Injector));    

            return services;
        }
        
        private static AppSettings GetAppSettings()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration.Get<AppSettings>();
        }
    }
}