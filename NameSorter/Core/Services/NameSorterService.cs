// Implements name sorting logic
using Microsoft.Extensions.Logging;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Models;

namespace NameSorter.Core.Services
{
    public class NameSorterService : INameSorterService
    {
        private readonly ILogger<NameSorterService> _logger;

        public NameSorterService(ILogger<NameSorterService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Sorts names by last name, then by given names
        public IEnumerable<Name> SortNames(IEnumerable<Name> names)
        {
            if (names == null)
                throw new NameSorterException("The list of names cannot be empty.");

            _logger.LogInformation("Sort triggered for name list.");
            return names.OrderBy(n => n.LastName)
                       .ThenBy(n => string.Join(" ", n.GivenNames))
                       .ToList();
        }
    }
}