using System;
using MediatR;

namespace SCode.Client.Teacher.ConsoleApp.Domain.Commands
{
    public class SendRequestedFileCommand : IRequest
    {
        public Guid Id { get; }

        public int FileId { get; }

        public string TargetMethod { get; }

        public SendRequestedFileCommand(Guid id, int fileId, string targetMethod)
        {
            Id = id;
            FileId = fileId;
            TargetMethod = targetMethod;
        }
    }
}