// Main entry point for the Name Sorter console application
using Microsoft.Extensions.Logging;
using NameSorter.Core.Exceptions;
using NameSorter.Core.Interfaces;
using NameSorter.Core.Services;
using NameSorter.Infrastructure;
using System;
using System.IO;

namespace NameSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure logging
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Application started.");

            try
            {
                // Validate command-line arguments
                if (args.Length != 1)
                {
                    throw new NameSorterException("Please provide the path to the input file. Usage: name-sorter <input-file-path>");
                }

                // Resolve input and output file paths
                string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
                string outputFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../sorted-names-list.txt"));

                // Initialize dependencies with logging
                IFileHandler fileHandler = new FileHandler(loggerFactory.CreateLogger<FileHandler>());
                INameSorterService sorterService = new NameSorterService(loggerFactory.CreateLogger<NameSorterService>());

                // Read, sort, and output names
                var names = fileHandler.ReadNames(inputFilePath);
                var sortedNames = sorterService.SortNames(names);

                foreach (var name in sortedNames)
                {
                    Console.WriteLine(name);
                }

                fileHandler.WriteNames(outputFilePath, sortedNames);
                logger.LogInformation("Sort completed successfully. Names written to {OutputFilePath}", outputFilePath);
            }
            catch (NameSorterException ex)
            {
                logger.LogError(ex, "Error: {Message}", ex.Message);
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                Console.WriteLine($"An unexpected error occurred: {ex.Message}. Please contact support.");
            }
        }
    }
}