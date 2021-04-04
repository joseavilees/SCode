using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Clients;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Domain.Commands;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;
using SCode.Shared.Requests.FileContentRequests;

namespace SCode.Client.Teacher.ConsoleApp.Application.CommandHandlers
{
    public class SendRequestedFileCommandHandler : AsyncRequestHandler<SendRequestedFileCommand>
    {
        private readonly ILogger<SendRequestedFileCommandHandler> _logger;
        private readonly ILogicPathRepository _logicPathRepository;
        private readonly IClassroomHubClient _classroomHubClient;

        public SendRequestedFileCommandHandler(ILogger<SendRequestedFileCommandHandler> logger,
            IClassroomHubClient classroomHubClient, ILogicPathRepository logicPathRepository)
        {
            _logger = logger;
            _classroomHubClient = classroomHubClient;
            _logicPathRepository = logicPathRepository;
        }

        protected override async Task Handle(SendRequestedFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var path = _logicPathRepository
                    .Get(request.FileId);

                var content = FileHelper
                    .GetFileContent(path);

                var response = new FileContentResponse(request.Id, content);

                await _classroomHubClient
                    .SendAsync(methodName: request.TargetMethod, response);

                _logger.LogDebug("Enviado contenido archivo {Id}",
                    request.FileId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No fue posible responder a la solicitud de contenido " +
                                     "de archivo {@Request} ", request);
            }
        }
    }
}