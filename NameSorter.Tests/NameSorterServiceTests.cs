// Unit tests for NameSorterService
using NameSorter.Core;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Models;
using NameSorter.Core.Services;
using System.Linq;
using Xunit;

namespace NameSorter.Tests
{
    public class NameSorterServiceTests
    {
        private readonly NameSorterService _service;

        public NameSorterServiceTests()
        {
            _service = new NameSorterService(null);
        }

        [Fact]
        public void SortNames_SortsByLastNameThenGivenNames()
        {
            // Arrange
            var names = new[]
            {
                     new Name(new[] { "Janet" }, "Parsons"),
                     new Name(new[] { "Vaughn" }, "Lewis"),
                     new Name(new[] { "Adonis", "Julius" }, "Archer")
                 };

            // Act
            var sorted = _service.SortNames(names).ToList();

            // Assert
            Assert.Equal("Archer", sorted[0].LastName);
            Assert.Equal("Lewis", sorted[1].LastName);
            Assert.Equal("Parsons", sorted[2].LastName);
        }

        [Fact]
        public void SortNames_NullInput_ThrowsNameSorterException()
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => _service.SortNames(null));
            Assert.Equal("The list of names cannot be empty.", exception.Message);
        }
    }
}