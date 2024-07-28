namespace TestZip;

public class ExecutionSummary
{
    public List<string> InvalidArchives { get; set; } = new();

    public List<string> ValidArchives { get; set; } = new();

    public void PrintSummary()
    {
        Console.WriteLine($"Valid archives: {ValidArchives.Count}");
        Console.WriteLine($"Invalid archives: {InvalidArchives.Count}");
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