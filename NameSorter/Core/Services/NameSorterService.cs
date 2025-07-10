// Service for sorting names
using Microsoft.Extensions.Logging;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameSorter.Core.Services
{
    public class NameSorterService : INameSorterService
    {
        private readonly ILogger<NameSorterService>? _logger;

        public NameSorterService(ILogger<NameSorterService>? logger)
        {
            _logger = logger;
        }

        public IEnumerable<Name> SortNames(IEnumerable<Name> names)
        {
            _logger?.LogInformation("Sort triggered for name list.");
            if (names == null || !names.Any())
            {
                _logger?.LogError("The list of names cannot be empty.");
                throw new NameSorterException("The list of names cannot be empty.");
            }

            return names.OrderBy(n => n.LastName)
                        .ThenBy(n => string.Join(" ", n.GivenNames))
                        .ToList();
        }
    }
}