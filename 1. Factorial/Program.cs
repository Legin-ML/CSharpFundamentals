using System;

class FactorialCalculator
{
    static void Main()
    {
        int number;


        Console.Write("Enter a positive integer: ");
        while (!int.TryParse(Console.ReadLine(), out number) || number < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            Console.Write("Enter a positive integer: ");
        }


        long factorial = 1;
        for (int i = 1; i <= number; i++)
        {
            factorial *= i;
        }

        Console.WriteLine($"The factorial of {number} is: {factorial}");
    }
}
