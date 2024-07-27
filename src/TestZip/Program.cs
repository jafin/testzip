using System.Diagnostics;
using System.Text.RegularExpressions;
using CommandLine;

namespace TestZip;

partial class Program
{
    static List<string> invalidArchives = new();
    static List<string> validArchives = new();
    static string sevenZipPath = "7z.exe";

    static async Task Main(string[] args)
    {
        (await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunOptions))
            .WithNotParsed(HandleParseError);
    }

    static async Task RunOptions(Options opts)
    {
        if (Directory.Exists(opts.Path))
        {
            await EnumerateAndTestArchives(opts.Path);
            Console.WriteLine($"Valid archives: {validArchives.Count}");
            Console.WriteLine($"Invalid archives: {invalidArchives.Count}");
            DisplayInvalidArchives();
        }
        else
        {
            Console.WriteLine("Invalid directory path.");
        }
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        Console.WriteLine("Command line arguments are invalid. Use --help for usage information.");
    }

    static async Task EnumerateAndTestArchives(string path)
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

    private static async Task ScanDir(string directory)
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

                Console.WriteLine($"  File: {file}");
                var isValid = await TestArchive(file);
                Console.WriteLine($"  {Path.GetFileName(file)} - {(isValid ? "Valid" : "Invalid")}");
                if (!isValid)
                {
                    invalidArchives.Add(file);
                }
                else
                {
                    validArchives.Add(file);
                }
            }
        }
    }

    static async Task<bool> TestArchive(string filePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"t -p- \"{Path.Combine(filePath)}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null) Console.WriteLine(e.Data);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null) Console.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error testing archive {filePath}: {ex.Message}");
            return false;
        }
    }

    static void DisplayInvalidArchives()
    {
        Console.WriteLine("\n--- List of Invalid Archives ---");
        if (invalidArchives.Count == 0)
        {
            Console.WriteLine("No invalid archives found.");
        }
        else
        {
            foreach (var archive in invalidArchives)
            {
                Console.WriteLine(archive);
            }
        }
    }

    [GeneratedRegex(@"\.part(0*[2-9]|[1-9][0-9])\.rar$")]
    private static partial Regex RegexPartMatch();
}