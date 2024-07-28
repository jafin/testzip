namespace TestZip;

public class ExecutionSummary
{
    private List<string> InvalidArchives { get; } = new();

    private List<string> ValidArchives { get; } = new();

    public void PrintSummary()
    {
        Console.WriteLine($"Valid archives: {ValidArchives.Count}");
        Console.WriteLine($"Invalid archives: {InvalidArchives.Count}");
    }


    public void AddInvalidArchive(string archive)
    {
        InvalidArchives.Add(archive);
    }

    public void AddValidArchive(string archive)
    {
        ValidArchives.Add(archive);
    }

    public void DisplayInvalidArchives()
    {
        Console.WriteLine("\n--- List of Invalid Archives ---");
        if (InvalidArchives.Count == 0)
        {
            Console.WriteLine("No invalid archives found.");
        }
        else
        {
            foreach (var archive in InvalidArchives)
            {
                Console.WriteLine(archive);
            }
        }
    }
}