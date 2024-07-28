using CommandLine;

namespace TestZip;

public class Options
{
    [Value(0, HelpText = "Path to scan")] public string Path { get; set; }
}