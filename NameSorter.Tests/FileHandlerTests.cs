// Unit tests for FileHandler
using NameSorter.Core;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Models;
using NameSorter.Infrastructure;
using System.IO;
using System.Linq;
using Xunit;

namespace NameSorter.Tests
{
    public class FileHandlerTests
    {
        private readonly IFileHandler _fileHandler;

        public FileHandlerTests()
        {
            _fileHandler = new FileHandler(null);
        }

        [Fact]
        public void ReadNames_ValidFile_ReturnsNames()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllLines(tempFile, new[] { "Janet Parsons", "Vaughn Lewis" });

            // Act
            var names = _fileHandler.ReadNames(tempFile).ToList();

            // Assert
            Assert.Equal(2, names.Count);
            File.Delete(tempFile);
        }

        [Fact]
        public void ReadNames_NonExistentFile_ThrowsNameSorterException()
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => _fileHandler.ReadNames("nonexistent.txt"));
            Assert.Equal("The file 'nonexistent.txt' was not found.", exception.Message);
        }

        [Fact]
        public void ReadNames_EmptyFilePath_ThrowsNameSorterException()
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => _fileHandler.ReadNames(""));
            Assert.Equal("File path cannot be empty.", exception.Message);
        }

        [Fact]
        public void WriteNames_ValidInput_WritesToFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var names = new[] { new Name(new[] { "Janet" }, "Parsons") };

            // Act
            _fileHandler.WriteNames(tempFile, names);

            // Assert
            var lines = File.ReadAllLines(tempFile);
            Assert.Single(lines);
            Assert.Equal("Janet Parsons", lines[0]);
            File.Delete(tempFile);
        }

        [Fact]
        public void WriteNames_EmptyFilePath_ThrowsNameSorterException()
        {
            // Arrange
            var names = new[] { new Name(new[] { "Janet" }, "Parsons") };

            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => _fileHandler.WriteNames("", names));
            Assert.Equal("Output file path cannot be empty.", exception.Message);
        }

        [Fact]
        public void WriteNames_NullNames_ThrowsNameSorterException()
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => _fileHandler.WriteNames("output.txt", null));
            Assert.Equal("The list of names cannot be empty.", exception.Message);
        }
    }
}