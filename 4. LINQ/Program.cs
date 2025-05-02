using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Student> students = new List<Student>
            {
                new Student("Alice", 85, 16),
                new Student("Bob", 72, 15),
                new Student("Charlie", 90, 17),
                new Student("Diana", 78, 16),
                new Student("Ethan", 95, 18),
                new Student("Fiona", 68, 15),
                new Student("George", 88, 17),
                new Student("Hannah", 79, 16)
            };

            int gradeThreshold = 80;

            var filteredByName = students
                .Where(s => s.Grade > gradeThreshold)
                .OrderBy(s => s.Name)
                .ToList(); // Method Syntax

            Console.WriteLine($"Students with grade above {gradeThreshold}, sorted by name:");
            foreach (var student in filteredByName)
            {
                Console.WriteLine($"Name: {student.Name}, Grade: {student.Grade}, Age: {student.Age}");
            }
            
            Console.WriteLine();

            var filteredByGrade = (from s in students
                                    where s.Grade > gradeThreshold
                                    orderby s.Grade descending
                                    select s).ToList();  // Query Syntax


            Console.WriteLine($"Students with grade above {gradeThreshold}, sorted by grade (descending):");
            foreach (var student in filteredByGrade)
            {
                Console.WriteLine($"Name: {student.Name}, Grade: {student.Grade}, Age: {student.Age}");
            }
            Console.WriteLine("--------------");
            var filterOnlyNames = students.Where(s => s.Name.Contains('n')).ToList();

            foreach (var student in filterOnlyNames)
            {
                Console.WriteLine($"Name: {student.Name}, Grade: {student.Grade}, Age: {student.Age}");
            }


        }
    }

    class Student
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public int Age { get; set; }

        public Student(string name, int grade, int age)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Grade = grade;
            Age = age;
        }
    }
}