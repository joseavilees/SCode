using Microsoft.Extensions.DependencyInjection;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Filters;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline
{
    public static class Injector
    {
        public static IServiceCollection AddFileChangePipeline(this IServiceCollection services)
        {
            services.AddSingleton<IFileChangePipeline, FileChangePipeline>();

            services.AddTransient<IFileChangePipelineLogStep, FileChangePipelinePipelineLogStep>();
            services.AddTransient<IFileChangePipelineBatchChangesStep, FileChangePipelineBatchChangesStep>();
            services.AddTransient<IFileChangePipelineMergeChangesStep, FileChangePipelineMergeChangesStep>();
            services.AddTransient<IFileChangePipelinePublishChangesStep, FileChangePipelinePublishChangesStep>();
            services.AddTransient<IFileChangePipelineTransformChangeToLogicStep, FileChangePipelineTransformChangeToLogicStep>();

            services.AddSingleton<IFileChangePipelinePathFilter, FileChangePipelinePathFilter>();
            
            return services;
        }
    }
}