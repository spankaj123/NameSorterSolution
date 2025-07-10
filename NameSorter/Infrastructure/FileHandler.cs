// Handles file input/output operations for name
using Microsoft.Extensions.Logging;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Models;
using NameSorter.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace NameSorter.Infrastructure
{
    public class FileHandler : IFileHandler
    {
        private readonly ILogger<FileHandler> _logger;

        public FileHandler(ILogger<FileHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Reads names from a file and converts them to Name objects
        public IEnumerable<Name> ReadNames(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new NameSorterException("File path cannot be empty.");
            if (!File.Exists(filePath))
                throw new NameSorterException($"The file '{filePath}' was not found.");

            var names = new List<Name>();
            try
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        names.Add(NameParser.Parse(line));
                    }
                }
                _logger.LogInformation("Loaded {NameCount} names from {FilePath}", names.Count, filePath);
            }
            catch (IOException ex)
            {
                throw new NameSorterException($"Failed to read the file '{filePath}'. Please ensure the file is accessible.", ex);
            }

            return names;
        }

        // Writes sorted names to a file
        public void WriteNames(string filePath, IEnumerable<Name> names)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new NameSorterException("Output file path cannot be empty.");
            if (names == null)
                throw new NameSorterException("The list of names cannot be empty.");

            try
            {
                File.WriteAllLines(filePath, names.Select(n => n.ToString()));
            }
            catch (IOException ex)
            {
                throw new NameSorterException($"Failed to write to the file '{filePath}'. Please ensure the file path is valid and writable.", ex);
            }
        }
    }
}