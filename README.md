# Name Sorter Solution

This repository contains a .NET 9.0 console application for sorting names from an input file (`unsorted-names-list.txt`) and outputting the sorted list to the console and a file (`sorted-names-list.txt`). The solution is designed for the coding assessment, emphasizing clean code, SOLID principles, minimal logging with `ILogger<T>`, and automation via GitHub Actions.

## Project Structure

- **NameSorterSolution.sln**: Solution file containing two projects.
- **NameSorter/**: Main application project.
  - `Program.cs`: Entry point, reads input file, sorts names, and writes output.
  - `Core/`: Business logic (interfaces, models, services).
    - `NameSorterService.cs`: Handles name sorting.
    - `Name.cs`, `INameSorterService.cs`, etc.
  - `Infrastructure/`: File handling.
    - `FileHandler.cs`: Reads/writes names to files.
  - `unsorted-names-list.txt`: Input file with \~300 names (e.g., "Janet Parsons", "Vaughn Lewis").
  - `sorted-names-list.txt`: Output file (generated in `NameSorter/`).
- **NameSorter.Tests/**: Unit tests for `FileHandler`, `NameSorterService`, and `NameParser` (8 tests total).
- **.github/workflows/BuildandExecute.yml**: GitHub Actions workflow to build, test, run, and commit `sorted-names-list.txt`.

## Prerequisites

- **.NET 9.0 SDK**: Install from dotnet.microsoft.com (x64 SDK).
- **Git**: For cloning the repository.
- **Visual Studio 2022** or **VS Code** (optional, for development).
- **GitHub Repository**: Ensure write permissions for GitHub Actions (see Setup).

## Setup

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/spankaj123/NameSorterSolution.git
   cd NameSorterSolution
   ```

2. **Verify .NET 9.0 SDK**:

   ```bash
   dotnet --list-sdks
   ```

   Ensure `9.0.xxx` is listed. If not, install from the link above.

3. **Ensure Input File**:

   - Confirm `unsorted-names-list.txt` is in `NameSorter/` with \~300 names.
   - Verify `NameSorter/NameSorter.csproj` includes:

     ```xml
     <ItemGroup>
       <None Update="unsorted-names-list.txt">
         <CopyToOutputDirectory>Always</CopyToOutputDirectory>
       </None>
     </ItemGroup>
     ```

4. **GitHub Actions Permissions**:

   - Go to `https://github.com/spankaj123/NameSorterSolution` &gt; **Settings** &gt; **Actions** &gt; **General**.
   - Select **Read and write permissions** under **Workflow permissions**.
   - Click **Save** to allow pushing `sorted-names-list.txt`.

## Running the Application Locally

The application sorts names from `unsorted-names-list.txt` and outputs to `sorted-names-list.txt` and the console.

1. **Navigate to NameSorter Folder**:

   ```bash
   cd NameSorterSolution/NameSorter
   ```

2. **Run the Command**:

   ```bash
   dotnet run --project NameSorter.csproj -- ./unsorted-names-list.txt
   ```

   - **Input**: Reads `NameSorter/unsorted-names-list.txt`.
   - **Output**: Writes `NameSorter/sorted-names-list.txt` and prints sorted names to console.
   - **Expected Logs**:

     ```
     info: NameSorter.Program[0]
           Application started.
     info: NameSorter.Infrastructure.FileHandler[0]
           Loaded 300 names from .../NameSorter/unsorted-names-list.txt
     info: NameSorter.Core.Services.NameSorterService[0]
           Sort triggered for name list.
     Adonis Julius Archer
     Marin Alvarez
     ...
     info: NameSorter.Program[0]
           Sort completed successfully. Names written to .../NameSorter/sorted-names-list.txt
     ```

3. **Verify Output File**:

   - Check `NameSorter/sorted-names-list.txt` for \~300 sorted names (e.g., starting with "Adonis Julius Archer").
   - Run:

     ```bash
     type NameSorter/sorted-names-list.txt
     ```

4. **Run in Visual Studio**:

   - Open `NameSorterSolution.sln`.
   - Set `NameSorter` as startup project.
   - In **Properties &gt; Debug &gt; General**, set "Command line arguments" to `./unsorted-names-list.txt`.
   - Press **F5**.
   - Verify console output and `sorted-names-list.txt`.

## Running Tests

1. **From Solution Root**:

   ```bash
   cd NameSorterSolution
   dotnet test NameSorter.Tests/NameSorter.Tests.csproj --configuration Release
   ```
2. **Expected Output**:
   - 8 tests pass (3 for `FileHandler`, 2 for `NameSorterService`, 3 for `NameParser`).
   - Example:

     ```
     Total tests: 8
     Passed: 8
     ```

## GitHub Actions Workflow

The workflow (`BuildandExecute.yml`) automates building, testing, running the application, and committing `sorted-names-list.txt`.

- **File**: `.github/workflows/BuildandExecute.yml`
- **Trigger**: Runs on `push` or `pull_request` to the `main` branch.
- **Steps**:
  1. **Checkout**: Clones the repository.
  2. **Setup .NET**: Installs .NET 9.0 SDK.
  3. **Restore**: Restores NuGet dependencies.
  4. **Build**: Builds `NameSorterSolution.sln` (Release configuration).
  5. **Run NameSorter**:
     - Executes `dotnet run --project NameSorter/NameSorter.csproj -- ./unsorted-names-list.txt` from `NameSorter/`.
     - Generates `NameSorter/sorted-names-list.txt`.
  6. **Run Tests**: Runs tests in `NameSorter.Tests`.
  7. **Calculate Test Metrics**: Logs total tests, passed tests, and pass percentage.
  8. **Commit**: Commits and pushes `NameSorter/sorted-names-list.txt` to the repository.
- **Verify**:
  - Go to **Actions** tab &gt; Select “Build and Execute Name Sorter”.
  - Check logs for build, run, and test success.
  - Confirm `NameSorter/sorted-names-list.txt` in the repository.

## Scaling Improvements

To enhance the application for larger datasets or production use, consider:

1. **Asynchronous File I/O**:

   - Update `FileHandler` to use `File.ReadAllLinesAsync` and `File.WriteAllLinesAsync` for non-blocking I/O.
   - Example:

     ```csharp
     public async Task<IEnumerable<Name>> ReadNamesAsync(string filePath)
     {
         var lines = await File.ReadAllLinesAsync(filePath);
         return lines.Select(line => NameParser.Parse(line)).ToList();
     }
     ```

2. **Parallel Sorting**:

   - For large datasets (&gt;1M names), use `Parallel.ForEach` or PLINQ in `NameSorterService` to parallelize sorting.
   - Example:

     ```csharp
     return names.AsParallel().OrderBy(n => n.LastName).ThenBy(n => string.Join(" ", n.GivenNames)).ToList();
     ```

3. **Configuration Management**:

   - Use `IConfiguration` to load input/output file paths from `appsettings.json` instead of hardcoding.
   - Example:

     ```json
     {
       "FilePaths": {
         "Input": "unsorted-names-list.txt",
         "Output": "sorted-names-list.txt"
       }
     }
     ```

4. **Dependency Injection**:

   - Replace manual DI with Microsoft.Extensions.DependencyInjection for better maintainability.
   - Example:

     ```csharp
     var serviceProvider = new ServiceCollection()
         .AddLogging(builder => builder.AddConsole())
         .AddSingleton<IFileHandler, FileHandler>()
         .AddSingleton<INameSorterService, NameSorterService>()
         .BuildServiceProvider();
     ```

5. **Error Handling and Logging**:

   - Enhance logging with structured data (e.g., Serilog) for better debugging in production.
   - Add retry logic for file operations to handle transient failures.

6. **Input Validation**:

   - Add validation for name formats (e.g., regex for valid characters) in `NameParser`.
   - Example:

     ```csharp
     if (!Regex.IsMatch(line, @"^[a-zA-Z\s]+$")) throw new NameSorterException("Invalid name format.");
     ```

7. **Scalable Storage**:

   - For massive datasets, use a database (e.g., SQLite) instead of text files.
   - Example: Use EF Core to store and query names.

8. **Unit Test Coverage**:

   - Add tests for edge cases (e.g., empty files, invalid paths).
   - Use a mocking framework (e.g., Moq) for `ILogger<T>` to test logging if needed.
