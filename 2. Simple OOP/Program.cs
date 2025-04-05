using System;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }


    public Person(string name, int age, string location)
    {
        Name = name;
        Age = age;
        Location = location;
    }


    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name} and I am {Age} years old. I live in {Location}.");
    }
}

class Program
{
    static void Main()
    {

        Person person1 = new Person("Jane Doe", 29, "New York");
        Person person2 = new Person("John Doe", 36, "London");
        

        person1.Introduce();
        person2.Introduce();
    }
}
