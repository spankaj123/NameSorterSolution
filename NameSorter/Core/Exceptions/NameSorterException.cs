// Custom exception for standardized error handling with user-friendly messages
namespace NameSorter.Core.Exceptions
{
    public class NameSorterException : Exception
    {
        // Constructor with message
        public NameSorterException(string message) : base(message)
        {
        }

        // Constructor with message and inner exception
        public NameSorterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}