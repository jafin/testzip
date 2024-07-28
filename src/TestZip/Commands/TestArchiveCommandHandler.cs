using System.Text.RegularExpressions;

namespace TestZip.Commands;

public partial class TestArchiveCommandHandler
{
    private readonly ZipProcess _zipProcess;
    private readonly ExecutionSummary _executionSummary;

    public TestArchiveCommandHandler(ExecutionSummary executionSummary, ZipProcess zipProcess)
    {
        _zipProcess = zipProcess;
        _executionSummary = executionSummary;
    }

    public async Task EnumerateAndTestArchives(string path)
    {
        var directoriesToProcess = new Stack<string>();
        directoriesToProcess.Push(path);

        while (directoriesToProcess.Count > 0)
        {
            var currentDirectory = directoriesToProcess.Pop();
            try
            {
                await ScanDir(currentDirectory);

                foreach (var subdirectory in Directory.GetDirectories(currentDirectory))
                {
                    Console.WriteLine($"Folder: {subdirectory}");
                    directoriesToProcess.Push(subdirectory);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private async Task ScanDir(string directory)
    {
        List<string> compressedExtensions = [".zip", ".rar", ".7z"];

        foreach (var file in Directory.GetFiles(directory))
        {
            var extension = Path.GetExtension(file).ToLower();
            if (compressedExtensions.Contains(extension))
            {
                if (extension == ".rar")
                {
                    var partFile = RegexPartMatch().Match(file).Success;
                    if (partFile)
                    {
                        Console.WriteLine($"Skipping partFile: {file}");
                        return;
                    }
                }

                Console.WriteLine($"File: {file}");
                var isValid = await _zipProcess.TestArchive(file);
                Console.WriteLine($"  {Path.GetFileName(file)} - {(isValid ? "Valid" : "Invalid")}");
                if (!isValid)
                {
                    _executionSummary.AddInvalidArchive(file);
                }
                else
                {
                    _executionSummary.AddValidArchive(file);
                }
            }
        }
    }


    [GeneratedRegex(@"\.part(0*[2-9]|[1-9][0-9])\.rar$")]
    private partial Regex RegexPartMatch();
}