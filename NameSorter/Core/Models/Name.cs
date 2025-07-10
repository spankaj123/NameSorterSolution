// Represents a person's name with given names and a last name, enforcing validation rules
using NameSorter.Core.Exceptions;
namespace NameSorter.Core.Models
{
    public class Name
    {
        // Array of given names (1 to 3)
        public string[] GivenNames { get; }
        // Last name of the person
        public string LastName { get; }

        // Constructor with validation for given names and last name
        public Name(string[] givenNames, string lastName)
        {
            if (givenNames == null || givenNames.Length == 0 || givenNames.Length > 3)
                throw new NameSorterException("Names must include 1 to 3 given names.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new NameSorterException("Last name cannot be empty.");

            GivenNames = givenNames;
            LastName = lastName;
        }

        // Returns the full name as a string for display or file output
        public override string ToString() => $"{string.Join(" ", GivenNames)} {LastName}";
    }
}