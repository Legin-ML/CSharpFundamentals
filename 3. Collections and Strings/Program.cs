using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<string> tasks = new List<string>();
        string command;

        do
        {
            Console.WriteLine("############## Task Manager ##############");
            Console.WriteLine("\nAvailable commands: \n1. add \n2. remove \n3. display \n4. quit");
            Console.Write("Enter command: ");
            command = Console.ReadLine()?.ToLower() ?? string.Empty; // Changes made because of warning CS8602

            if (!string.IsNullOrEmpty(command))
            {
                if (command == "add" || command == "1")
                {
                    Console.Write("Enter the task to add: ");
                    string task = Console.ReadLine()?.Trim() ?? string.Empty;
                    if (!string.IsNullOrEmpty(task))
                    {
                        tasks.Add(task);
                    }
                    else
                    {
                        Console.WriteLine("Task cannot be empty.");
                    }
                }
                else if (command == "remove" || command == "2")
                {
                    if (tasks.Count != 0) {
                    Console.Write("Enter the task to remove: ");
                    string task = Console.ReadLine()?.Trim() ?? string.Empty;
                    if (tasks.Contains(task))
                    {
                        tasks.Remove(task);
                        Console.WriteLine($"Task '{task}' removed.");
                    }
                    else
                    {
                        Console.WriteLine($"Task '{task}' not found.");
                    }
                    }
                    else {
                        Console.WriteLine("The Tasks List is empty");
                    }

                }
                else if (command == "display" || command == "3")
                {
                    if (tasks.Count != 0) {
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("\nTasks List:");
                    foreach (var task in tasks)
                    {
                        Console.WriteLine(task.ToUpper());
                    }
                    Console.WriteLine("-----------------------------------------------------------");
                    }
                    else {
                        Console.WriteLine("The Tasks List is empty");
                    }
                }
                else {
                    Console.WriteLine("The entered command is invalid!");
                }
            }

        } while (command != "quit" && command != "exit" && command != "4");
    }
}
