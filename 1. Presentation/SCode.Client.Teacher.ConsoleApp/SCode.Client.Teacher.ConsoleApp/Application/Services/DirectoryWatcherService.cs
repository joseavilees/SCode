using System;
using System.IO;
using MediatR;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Mappers;
using SCode.Client.Teacher.ConsoleApp.Domain.Events;

namespace SCode.Client.Teacher.ConsoleApp.Application.Services
{
    /// <summary>
    /// Monitoriza los cambios realizados sobre
    /// un directorio
    /// </summary>
    public sealed class DirectoryWatcherService : IDisposable, IDirectoryWatcherService
    {
        private readonly ILogger<DirectoryWatcherService> _logger;
        private readonly IMediator _bus;

        private FileSystemWatcher _fileSystemWatcher;

        public DirectoryWatcherService(ILogger<DirectoryWatcherService> logger, IMediator bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public void Start(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = path,

                NotifyFilter =  NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName,

                IncludeSubdirectories = true
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Created += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;
            _fileSystemWatcher.Renamed += OnChanged;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }


        /// <summary>
        /// Evento al producirse un cambio en un archivo o directorio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                var change = FileSystemEventArgsMapper
                    .MapToAppFileSystemEntryChange(e);
                
                var notification = new FileSystemEntryChangedEvent(change);

                _bus.Publish(notification);
            }
            catch (Exception ex)
            {
                _logger
                    .LogError(ex, "Error al lanzar el evento " +
                                  "FileSystemEntryChanged {@Event}", e);
            }
        }

        public void Dispose()
        {
            _fileSystemWatcher?.Dispose();
        }
    }

    public interface IDirectoryWatcherService
    {
        void Start(string path);
    }
}