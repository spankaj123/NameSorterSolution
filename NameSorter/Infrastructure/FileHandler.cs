// Handles file operations for reading and writing names
using Microsoft.Extensions.Logging;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Models;
using NameSorter.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameSorter.Infrastructure
{
    public class FileHandler : IFileHandler
    {
        private readonly ILogger<FileHandler>? _logger;

        public FileHandler(ILogger<FileHandler>? logger)
        {
            _logger = logger;
        }

        public IEnumerable<Name> ReadNames(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _logger?.LogError("File path cannot be empty.");
                throw new NameSorterException("File path cannot be empty.");
            }

            if (!File.Exists(filePath))
            {
                _logger?.LogError("The file '{FilePath}' was not found.", filePath);
                throw new NameSorterException($"The file '{filePath}' was not found.");
            }

            var names = File.ReadAllLines(filePath)
                .Select(line => NameParser.Parse(line))
                .ToList();

            _logger?.LogInformation("Loaded {Count} names from {FilePath}", names.Count, filePath);
            return names;
        }

        public void WriteNames(string filePath, IEnumerable<Name> names)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _logger?.LogError("Output file path cannot be empty.");
                throw new NameSorterException("Output file path cannot be empty.");
            }

            if (names == null || !names.Any())
            {
                _logger?.LogError("The list of names cannot be empty.");
                throw new NameSorterException("The list of names cannot be empty.");
            }

            File.WriteAllLines(filePath, names.Select(n => n.ToString()));
        }
    }
}