using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting asynchronous operations...");

        var task1 = FetchDataFromSourceAsync("Source 1");
        var task2 = FetchDataFromSourceAsync("Source 2");
        var task3 = FetchDataFromSourceAsync("Source 3");

        try
        {
            var results = await Task.WhenAll(task1, task2, task3);

            Console.WriteLine("\nAll tasks completed successfully!");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task<string> FetchDataFromSourceAsync(string source)
    {
        Console.WriteLine($"Starting fetch from {source}...");
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            if (source == "Source 2")
            {
                throw new Exception("Network issue occurred while fetching from Source 2.");
            }
            
            return $"{source} - Data fetched successfully!";
        }
        catch (Exception ex)
        {
            return $"{source} - Failed to fetch data: {ex.Message}";
        }
    }
}
