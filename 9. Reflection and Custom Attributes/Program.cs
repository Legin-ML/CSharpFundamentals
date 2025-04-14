using System;
using System.Linq;
using System.Reflection;

// Define the custom attribute
[AttributeUsage(AttributeTargets.Method)]
public class RunnableAttribute : Attribute { }

// Example class with static and instance methods
public class MathUtils
{
    [Runnable]
    public static string StaticAdd() => $"StaticAdd: {3 + 4}";

    [Runnable]
    public string InstanceMultiply() => $"InstanceMultiply: {3 * 5}";

    private string HiddenMethod() => "Not runnable";
}

public class StringUtils
{
    [Runnable]
    public static string StaticHello() => "Hello from static method";

    [Runnable]
    public string InstanceGreeting() => "Greetings from instance method";

    private static string PrivateStatic() => "Not marked as [Runnable]";
}

// Main program
class Program
{
    static void Main()
    {
        Console.WriteLine("== Discovering [Runnable] methods ==");

        var assembly = Assembly.GetExecutingAssembly();

        var allTypes = assembly.GetTypes();

        foreach (var type in allTypes)
        {
            
            var methods = type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                // Check if method is marked with [Runnable]
                if (method.GetCustomAttributes(typeof(RunnableAttribute), false).Any())
                {
                    object instance = null;

                    if (!method.IsStatic)
                    {
                        // Create an instance of the class for instance methods
                        instance = Activator.CreateInstance(type);
                    }

                    try
                    {
                        var result = method.Invoke(instance, null);
                        Console.WriteLine($"{type.Name}.{method.Name}() => {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error invoking {type.Name}.{method.Name}(): {ex.Message}");
                    }
                }
            }
        }
    }
}
