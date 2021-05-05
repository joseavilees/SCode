using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SCode.Client.Teacher.ConsoleApp.Application.Builders;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Client.Teacher.ConsoleApp.IntegrationTests.TestSupport;
using Xunit;

namespace SCode.Client.Teacher.ConsoleApp.IntegrationTests.Application.Builders.LogicAppFileEntryBuilderTests
{
    public class  BuildAppFileTest : FunctionalTest
    {
        private ILogicAppFileEntryBuilder _logicAppFileEntryBuilder;
        private List<AppFileEntry> _result;
        private string _path;

        protected override Task Given()
        {
            _logicAppFileEntryBuilder = GetService<ILogicAppFileEntryBuilder>();

            _path = GetResourcesPath("BuildAppFileTest", "ExDirectory1");

            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _result = _logicAppFileEntryBuilder
                .Build(_path);

            return Task.CompletedTask;
        }

        
        [Theory]
        [InlineData("b.txt", 0, false)]
        [InlineData("c.txt", 0, false)]
        [InlineData("d", 0, true)]
        [InlineData("e.txt", 3, false)]
        [InlineData("f", 3, true)]
        [InlineData("g.txt", 5, false)]
        public void Then_It_Properties_Are_Expected(string fileName, int expectedParentId, bool expectedIsDirectory)
        {
            var file = _result
                .First(x => x.Name == fileName);
            
            file
                .ParentId
                .Should()
                .Be(expectedParentId);
            
            file
                .IsDirectory
                .Should()
                .Be(expectedIsDirectory);
        }
    }
}