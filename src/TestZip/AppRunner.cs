namespace TestZip;

public class AppRunner
{
    private readonly ExecutionSummary _executionSummary;
    private readonly Commands.TestArchiveCommandHandler _testArchiveCommandHandler;

    public AppRunner(ExecutionSummary executionSummary, Commands.TestArchiveCommandHandler testArchiveCommandHandler)
    {
        _testArchiveCommandHandler = testArchiveCommandHandler;
        _executionSummary = executionSummary;
    }

    public async Task RunOptions(Options opts)
    {
        if (Directory.Exists(opts.Path))
        {
            await _testArchiveCommandHandler.EnumerateAndTestArchives(opts.Path);
            _executionSummary.PrintSummary();
            _executionSummary.DisplayInvalidArchives();
        }
        else
        {
            Console.WriteLine("Invalid directory path.");
        }
    }
}