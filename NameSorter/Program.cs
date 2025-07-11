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

                // Resolve input file path relative to current working directory
                string inputFilePath = Path.GetFullPath(args[0], Environment.CurrentDirectory);
                // Output to the same directory as input file
                string outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? throw new NameSorterException("Unable to determine input file directory."), "sorted-names-list.txt");

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