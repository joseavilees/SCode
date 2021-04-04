using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;
using SCode.Client.Teacher.ConsoleApp.IntegrationTests.TestSupport;
using Xunit;

namespace SCode.Client.Teacher.ConsoleApp.IntegrationTests.Application.Pipelines.FileChangePipeline.Steps.
    FileChangePipelineTransformChangeToLogicStepTests
{
    public class PostChangeTest : FunctionalTest
    {
        private ILogicPathRepository _logicPathRepository;

        private IFileChangePipelineTransformChangeToLogicStep _step;

        private List<FileSystemEntryChange> _fileSystemEntryChanges;
        private FileSystemEntryChange _fileSystemEntryChange;
        private AppFileEntryChange _result;

        protected override Task Given()
        {
            var mockPublishChangesStep = new Mock<IFileChangePipelinePublishChangesStep>();
            mockPublishChangesStep.Setup(h => h.Execute(It.IsAny<List<AppFileEntryChange>>()))
                .Callback<List<AppFileEntryChange>>(r => _result = r.FirstOrDefault());
            
            _logicPathRepository = GetService<ILogicPathRepository>();
      
            _step = new FileChangePipelineTransformChangeToLogicStep(_logicPathRepository,
                mockPublishChangesStep.Object);

            var path = GetResourcesPath("FileChangePipelineConvertChangeToLogicStepTests", "demo.txt");

            _fileSystemEntryChange = new FileSystemEntryChange(FileSystemEntryChangeType.Changed, "demo.txt", path);
            _fileSystemEntryChanges = new List<FileSystemEntryChange>
            {
                _fileSystemEntryChange
            };

            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _step.Execute(_fileSystemEntryChanges);

            return Task.CompletedTask;
        }

        [Fact]
        public void Then_It_Should_NotBe_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_It_Properties_Should_Be_Mapped()
        {
            _result
                .ChangeType
                .Should()
                .Be(AppFileEntryChangeType.Changed);

            _result
                .AppFileEntry
                .Name
                .Should()
                .Be(_fileSystemEntryChange.Name);

            _result
                .AppFileEntry
                .IsDirectory
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Then_It_Path_Is_Stored()
        {
            _logicPathRepository
                .Get(_result.AppFileEntry.Id)
                .Should()
                .Be(_fileSystemEntryChange.Path);
        }

        [Fact]
        public void Then_It_Content_Is_Filled()
        {
            _result
                .Content
                .Should()
                .Be("foo foo foo");
        }
    }
}