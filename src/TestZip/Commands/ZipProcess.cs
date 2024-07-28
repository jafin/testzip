using System.Diagnostics;

namespace TestZip.Commands;

public class ZipProcess
{
    private const string SevenZipPath = "7z.exe";

    public async Task<bool> TestArchive(string filePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = SevenZipPath,
                Arguments = $"t -p- \"{Path.Combine(filePath)}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = psi;
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
}