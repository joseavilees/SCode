using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Factories;
using SCode.Client.Teacher.ConsoleApp.Domain.Commands;
using SCode.Client.Teacher.ConsoleApp.Domain.Events;
using SCode.Shared.Requests.FileContentRequests;

namespace SCode.Client.Teacher.ConsoleApp.Application.Clients
{
    /// <summary>
    /// Interfaz de comunicación hacia el concentrador ClassroomHub
    /// </summary>
    public class ClassroomHubClient : IClassroomHubClient, IAsyncDisposable
    {
        private readonly ILogger<ClassroomHubClient> _logger;
        private readonly HubConnection _hubConnection;
        private readonly IMediator _bus;

        public ClassroomHubClient(ILogger<ClassroomHubClient> logger, IClassroomHubConnectionFactory factory, IMediator bus)
        {
            _logger = logger;
            _bus = bus;

            _hubConnection = factory.GetInstance(RequestFileHandler,
                HubConnectionClosedHandler);
        }

        public async Task<bool> Connect()
        {
            try
            {
                _logger.LogInformation("Conectando al servidor...");

                await _hubConnection
                    .StartAsync();

                await _bus
                    .Publish(new ClassroomHubClientConnectedEvent());

                _logger.LogInformation("Conectado al servidor");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No fue posible conectarse al servidor");
                
                return false;
            }
        }

        public Task SendAsync(string methodName, object arg1 = null)
        {
            if (_hubConnection.State == HubConnectionState.Connected) 
                return _hubConnection.SendAsync(methodName, arg1);
            
            _logger
                .LogInformation("Se ignora la llamada a {Method} al no tener conexión con el Hub", methodName);
                
            return Task.CompletedTask;
        }


        #region Handlers

        private Task RequestFileHandler(FileContentRequest request)
        {
            var command = new SendRequestedFileCommand(request.Id,
                request.FileId, request.TargetMethod);

            return _bus.Send(command);
        }

        private async Task HubConnectionClosedHandler(Exception arg)
        {
            _logger.LogInformation("Desconectado...");

            while (_hubConnection.State != HubConnectionState.Connected)
            {
                _logger.LogInformation("Reconectando...");

                await Connect();   
                
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        #endregion


        public ValueTask DisposeAsync()
        {
            return _hubConnection.DisposeAsync();
        }
    }

    public interface IClassroomHubClient
    {
        Task<bool> Connect();

        Task SendAsync(string methodName, object arg1);
    }
}