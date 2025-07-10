using NameSorter.Core.Models;

namespace NameSorter.Core.Interfaces
{
    public interface IFileHandler
    {
        // Reads names from a file
        IEnumerable<Name> ReadNames(string filePath);
        // Writes names to a file
        void WriteNames(string filePath, IEnumerable<Name> names);
    }
}
