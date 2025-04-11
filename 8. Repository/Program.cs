﻿public interface IEntity {
    int Id {get; set;}
}

public interface IRepository<T> where T : class, IEntity {
    void Add(T entity);
    T Get(int id);

    IEnumerable<T> GetAll();

    void Update(T entity);
    void Delete(int id);

}

public class InMemoryRepository<T> : IRepository<T> where T : class,  IEntity {

    private readonly List<T> _items = new();
    private int _nextId = 1;


    public void Add(T entity) {
        entity.Id = _nextId;
        _items.Add(entity);
    }

    public T Get(int id) {
        return _items.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<T> GetAll() {
        return _items;
    }

    public void Update(T entity) {
        var index = _items.FindIndex(e => e.Id == entity.Id);
        if (index >= 0) {
            _items[index] = entity;
        }
    }

    public void Delete(int id) {
        var entity = Get(id);
        if (entity != null) {
            _items.Remove(entity);
        }
    }

}


public class Product : IEntity {
    public int Id {get;set;}
    public string Name {get;set;}
    public decimal Price {get;set;}
}

class Program
{
    static void Main()
    {
        IRepository<Product> productRepository = new InMemoryRepository<Product>();

        while (true)
        {
            Console.WriteLine("\nChoose an operation:");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. Get Product by ID");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Delete Product");
            Console.WriteLine("6. Exit");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var newProduct = new Product();
                    Console.Write("Enter name: ");
                    newProduct.Name = Console.ReadLine();
                    Console.Write("Enter price: ");
                    newProduct.Price = decimal.Parse(Console.ReadLine());
                    productRepository.Add(newProduct);
                    Console.WriteLine("Product added.");
                    break;

                case "2":
                    foreach (var p in productRepository.GetAll())
                        Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Price: {p.Price}");
                    break;

                case "3":
                    Console.Write("Enter ID: ");
                    var id = int.Parse(Console.ReadLine());
                    var product = productRepository.Get(id);
                    if (product != null)
                        Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
                    else
                        Console.WriteLine("Product not found.");
                    break;

                case "4":
                    Console.Write("Enter ID to update: ");
                    id = int.Parse(Console.ReadLine());
                    product = productRepository.Get(id);
                    if (product != null)
                    {
                        Console.Write("Enter new name: ");
                        product.Name = Console.ReadLine();
                        Console.Write("Enter new price: ");
                        product.Price = decimal.Parse(Console.ReadLine());
                        productRepository.Update(product);
                        Console.WriteLine("Product updated.");
                    }
                    else
                        Console.WriteLine("Product not found.");
                    break;

                case "5":
                    Console.Write("Enter ID to delete: ");
                    id = int.Parse(Console.ReadLine());
                    productRepository.Delete(id);
                    Console.WriteLine("Product deleted.");
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}