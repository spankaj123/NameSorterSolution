// Utility class for parsing name strings into Name objects
using NameSorter.Core.Exceptions;
using NameSorter.Core.Models;

namespace NameSorter.Core.Utilities
{
    public static class NameParser
    {
        // Parses a name string into a Name object with given names and last name
        public static Name Parse(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NameSorterException("Name cannot be empty or whitespace.");

            var parts = name.Trim().Split(' ');
            if (parts.Length < 2 || parts.Length > 4)
                throw new NameSorterException("Names must include 1 to 3 given names followed by a last name.");

            var givenNames = parts.Take(parts.Length - 1).ToArray();
            var lastName = parts.Last();

            return new Name(givenNames, lastName);
        }
    }
}