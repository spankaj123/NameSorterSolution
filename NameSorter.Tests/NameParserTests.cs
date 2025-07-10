// Unit tests for NameParser
using NameSorter.Core;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Models;
using NameSorter.Core.Utilities;
using Xunit;

namespace NameSorter.Tests
{
    public class NameParserTests
    {
        [Theory]
        [InlineData("Janet Parsons", 1)]
        [InlineData("Adonis Julius Archer", 2)]
        [InlineData("Beau Tristan Test Bentley", 3)]
        public void Parse_ValidName_ReturnsCorrectName(string input, int givenNameCount)
        {
            // Act
            var name = NameParser.Parse(input);

            // Assert
            Assert.Equal(givenNameCount, name.GivenNames.Length);
            Assert.NotEmpty(name.LastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_EmptyName_ThrowsNameSorterException(string input)
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => NameParser.Parse(input));
            Assert.Equal("Name cannot be empty or whitespace.", exception.Message);
        }

        [Fact]
        public void Parse_TooManyGivenNames_ThrowsNameSorterException()
        {
            // Act & Assert
            var exception = Assert.Throws<NameSorterException>(() => NameParser.Parse("One Two Three Four Five"));
            Assert.Equal("Names must include 1 to 3 given names followed by a last name.", exception.Message);
        }
    }
}