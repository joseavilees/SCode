using System;
using System.Threading.Tasks;
using Flurl;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using SCode.Shared.Requests.FileContentRequests;

namespace SCode.Client.Teacher.ConsoleApp.Application.Factories
{
    public class ClassroomHubConnectionFactory : IClassroomHubConnectionFactory
    {
        private readonly AppSettings _appSettings;

        public ClassroomHubConnectionFactory(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public HubConnection GetInstance(Func<FileContentRequest, Task> requestFileHandler,
            Func<Exception, Task> connectionClosedHandler)
        {
            var instance = new HubConnectionBuilder()
                .WithUrl( GetHubUrl(), ConfigureAuthHttpHeaders)
                .Build();

            instance
                .On("RequestFile", requestFileHandler);

            instance.KeepAliveInterval =
                TimeSpan.FromSeconds(10);

            instance.Closed +=
                connectionClosedHandler;

            return instance;
        }

        private Url GetHubUrl()
        {
            var url = new Url(_appSettings.SCodeApiUrl)
                .AppendPathSegment("hubs")
                .AppendPathSegment("classroom");
            
            return url;
        }

        private void ConfigureAuthHttpHeaders(HttpConnectionOptions options)
        {
            options.Headers.Add("X-Api-Key", _appSettings.ApiKey);
        }
    }

    public interface IClassroomHubConnectionFactory
    {
        HubConnection GetInstance(Func<FileContentRequest, Task> requestFileHandler,
            Func<Exception, Task> connectionClosedHandler);
    }
}