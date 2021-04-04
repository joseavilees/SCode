using System.Collections.Generic;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;
using Xunit;

namespace SCode.Client.Teacher.ConsoleApp.UnitTest.Helpers.FileSystemEntryChangeHelperTests
{
    public class MergeTests
    {
        [Fact]
        public void TestDoNotMergeUniqueChanges()
        {
            var createChange = new FileSystemEntryChange(FileSystemEntryChangeType.Created, "1", "1");
            var updateChange = new FileSystemEntryChange(FileSystemEntryChangeType.Changed, "2", "2");
            var deleteChange = new FileSystemEntryChange(FileSystemEntryChangeType.Deleted, "3", "3");


            var changes = new List<FileSystemEntryChange>
            {
                createChange,
                updateChange,
                deleteChange
            };

            // Act
            var result = FileSystemEntryChangeHelper.Merge(changes);

            // Assert
            // Contiene cambio Creado
            Assert.Contains(createChange, result);

            // Contiene cambio Actualizado
            Assert.Contains(updateChange, result);

            // Contiene cambio Eliminado
            Assert.Contains(deleteChange, result);
        }

        [Fact]
        public void TestMergeDuplicateChanges()
        {
            var createChange = new FileSystemEntryChange(FileSystemEntryChangeType.Created, "1", "1");
            var createChangeClone = new FileSystemEntryChange(FileSystemEntryChangeType.Created, "1", "1");

            var deleteChange = new FileSystemEntryChange(FileSystemEntryChangeType.Deleted, "2", "2");
            var deleteChangeClone = new FileSystemEntryChange(FileSystemEntryChangeType.Deleted, "2", "2");


            var changes = new List<FileSystemEntryChange>
            {
                createChange,
                createChangeClone,
                deleteChange,
                deleteChangeClone
            };

            // Act
            var result = FileSystemEntryChangeHelper.Merge(changes);

            // Assert
            // Contiene un solo cambio Creado
            Assert.Single(result, x => x.Path == createChange.Path
                                       && x.ChangeType == FileSystemEntryChangeType.Created);

            // Contiene un solo cambio Eliminado
            Assert.Single(result, x => x.Path == deleteChange.Path
                                       && x.ChangeType == FileSystemEntryChangeType.Deleted);
        }


        [Fact]
        public void TestMergeCreatedAndDeletedChanges()
        {
            var createChange = new FileSystemEntryChange(FileSystemEntryChangeType.Created, "1", "1");
            var deleteChange = new FileSystemEntryChange(FileSystemEntryChangeType.Deleted, "1", "1");

            var changes = new List<FileSystemEntryChange>
            {
                createChange,
                deleteChange
            };

            // Act
            var result = FileSystemEntryChangeHelper.Merge(changes);

            // Assert
            // Contiene cambio Eliminado
            Assert.Contains(deleteChange, result);

            // No contiene cambio Creado
            Assert.DoesNotContain(createChange, result);
        }

        [Fact]
        public void TestDoesNotMergeChangedRenamedAndDeletedChanges()
        {
            var changedChange = new FileSystemEntryChange(FileSystemEntryChangeType.Changed, "1", "1");
            var renamedChange = new FileSystemEntryChange(FileSystemEntryChangeType.Renamed, "1", "1", "1");
            var deletedChange = new FileSystemEntryChange(FileSystemEntryChangeType.Deleted, "1", "1");

            var changes = new List<FileSystemEntryChange>()
            {
                changedChange,
                renamedChange,
                deletedChange
            };

            // Act
            var result = FileSystemEntryChangeHelper.Merge(changes);

            // Assert
            // Contiene cambio Actualizado
            Assert.Contains(changedChange, result);

            // Contiene cambio Renombrado
            Assert.Contains(renamedChange, result);

            // Contiene cambio Eliminado
            Assert.Contains(deletedChange, result);
        }
    }
}