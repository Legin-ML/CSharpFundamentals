using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileProcessingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = "input.txt";
            string outputFilePath = "output.txt";

            try
            {

                if (!File.Exists(inputFilePath))
                {
                    CreateSampleFile(inputFilePath);
                    Console.WriteLine($"Created sample file: {inputFilePath}");
                }

                string fileContent = ReadFile(inputFilePath);
                Console.WriteLine("File content read successfully!");


                Dictionary<string, int> statistics = ProcessTextData(fileContent);

                WriteResults(outputFilePath, statistics);
                Console.WriteLine($"Results written to: {outputFilePath}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File not found. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: Issue with file access. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error occurred: {ex.Message}");
            }

            Console.ReadLine();
        }

        static void CreateSampleFile(string filePath)
        {
            string sampleText = 
                "This is a sample text file.\n" +
                "It contains multiple lines.\n" +
                "We will count words and lines in this file.\n" +
                "This file is created for demonstration purposes.\n" +
                "Each line has various words to count.";

            File.WriteAllText(filePath, sampleText);
        }

        static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            return File.ReadAllText(filePath);
        }

        static Dictionary<string, int> ProcessTextData(string content)
        {
            Dictionary<string, int> statistics = new Dictionary<string, int>();

            string[] lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            statistics["Lines"] = lines.Length;

            string[] words = content.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, 
                                          StringSplitOptions.RemoveEmptyEntries);
            statistics["Words"] = words.Length;

            HashSet<string> uniqueWords = new HashSet<string>(
                words.Select(word => word.ToLower())
            );
            statistics["UniqueWords"] = uniqueWords.Count;

            statistics["Characters"] = content.Count(c => !char.IsWhiteSpace(c));

            return statistics;
        }

        static void WriteResults(string filePath, Dictionary<string, int> statistics)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Text File Analysis Results");
                    writer.WriteLine("-------------------------");
                    
                    foreach (var stat in statistics)
                    {
                        writer.WriteLine($"{stat.Key}: {stat.Value}");
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Error writing to output file: {ex.Message}", ex);
            }
        }
    }
}