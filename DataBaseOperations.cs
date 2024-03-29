﻿using Dotnet_Exam.Context;
using Dotnet_Exam.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace Dotnet_Exam;

public class DataBaseOperations
{
    /// <summary>
    /// The data is hardcoded as this is an learning project.
    /// </summary>
    /// <param name="options"></param>
    public static void AddDataToDataBase(DbContextOptions<DotnetExambdContext> options)
    {
        try
        {
            using DotnetExambdContext db = new DotnetExambdContext(options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            AddClients(db);

            AddCars(db);

            AddAddresses(db);

            AddClientAddresses(db);

            AddOrders(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void PrintMinAmountOrder(DbContextOptions<DotnetExambdContext> options)
    {
        using DotnetExambdContext db = new DotnetExambdContext(options);

        Order? order = db.Orders.FromSqlRaw("SELECT Orders.id, Orders.client_id, Orders.order_date, Orders.total_amount, " +
            "Clients.first_name, Clients.last_name " +
            "FROM Orders " +
            "JOIN Clients ON Clients.id = Orders.client_id " +
            "WHERE Orders.total_amount = (SELECT MIN(Orders.total_amount) FROM Orders)")
            .Include(o => o.Client)
            .FirstOrDefault();

        Console.WriteLine($"Order: {order?.Id} | Client {order?.Client?.Id}. {order?.Client?.FirstName} {order?.Client?.LastName} " +
            $"| Order date: {order?.OrderDate} | Total amount: {order?.TotalAmount:N2}");
    }

    public static void PrintMaxAmountOrder(DbContextOptions<DotnetExambdContext> options)
    {
        using DotnetExambdContext db = new DotnetExambdContext(options);

        Order? order = db.Orders.FromSqlRaw("SELECT Orders.id, Orders.client_id, Orders.order_date, Orders.total_amount, " +
            "Clients.first_name, Clients.last_name " +
            "FROM Orders " +
            "JOIN Clients ON Clients.id = Orders.client_id " +
            "WHERE Orders.total_amount = (SELECT MAX(Orders.total_amount) FROM Orders)")
            .Include(o => o.Client)
            .FirstOrDefault();

        Console.WriteLine($"Order: {order?.Id} | Client {order?.Client?.Id}. {order?.Client?.FirstName} {order?.Client?.LastName} " +
            $"| Order date: {order?.OrderDate} | Total amount: {order?.TotalAmount:N2}");
    }

    public static void PrintOrderByYearCar(DbContextOptions<DotnetExambdContext> options)
    {
        using DotnetExambdContext db = new DotnetExambdContext(options);

        var res = db.Cars.FromSqlInterpolated($@" 
            SELECT Cars.id, Cars.client_id, Cars.brand, Cars.model, Cars.year,
            Clients.first_name, Clients.last_name
            FROM Cars
            JOIN Clients ON Clients.id = Cars.client_id
            GROUP BY Cars.id, Cars.client_id, Cars.brand, Cars.model, Cars.year,
            Clients.first_name, Clients.last_name
            ORDER BY Cars.year ASC")
            .Include(c => c.Client)
            .ToList();

        foreach (var car in res)
        {
            Console.WriteLine($"Car {car.Id}. {car.Brand} {car.Model}, {car.Year} | Owner: {car.Client?.FirstName} {car.Client?.LastName}");
        }

    }

    public static void PrintOrdersByClient(DbContextOptions<DotnetExambdContext> options, string? clientLastName)
    {
        using var db = new DotnetExambdContext(options);

        if (!db.Clients.Any(c => c.LastName.Equals(clientLastName)))
        {
            Console.WriteLine($"Client {clientLastName} not found!");

            return;
        }

        List<Order>? orders = db.Orders
            .Where(order => order.Client.LastName!.Equals(clientLastName))
            .Include(order2 => order2.Client)
            .ToList();

        foreach (Order order in orders)
        {
            Console.WriteLine($"Order: {order.Id} | Client: {order.Client?.Id}. {order.Client?.FirstName} {order.Client?.LastName} " +
                $"| Order date: {order.OrderDate} | Total amount: {order.TotalAmount:N2}");
        }
    }

    public static void PrintOrdersByCity(DbContextOptions<DotnetExambdContext> options, string? city)
    {
        using var db = new DotnetExambdContext(options);

        if (!db.Address.Any(address => address.City.Equals(city)))
        {
            Console.WriteLine($"City {city} not found!");

            return;
        }

        List<Order>? orders = db.Orders
            .Where(order => order.Client.ClientAddresses
                .Any(clientAddress => clientAddress.Address.City.Equals(city)))
            .Include(order2 => order2.Client)
                .ThenInclude(client => client.ClientAddresses)
                    .ThenInclude(clientAddress => clientAddress.Address)
            .ToList();

        foreach (Order order in orders)
        {
            var clientCity = order.Client?.ClientAddresses.FirstOrDefault()?.Address?.City ?? "N/A";

            Console.WriteLine($"Order: {order.Id}, City: {clientCity} " +
                $"| Client: {order.Client?.Id}. {order.Client?.FirstName} {order.Client?.LastName} " +
                $"| Order date: {order.OrderDate} | Total amount: {order.TotalAmount:N2}");
        }
    }

    public static void PrintOrdersByDateRange(DbContextOptions<DotnetExambdContext> options, DateTime start, DateTime end)
    {
        using var db = new DotnetExambdContext(options);

        List<Order>? orders = db.Orders
            .Where(order => order.OrderDate >= start && order.OrderDate <= end)
            .Include(order2 => order2.Client)
            .OrderBy(order3 => order3.OrderDate)
            .ToList();

        foreach (var order in orders)
        {
            Console.WriteLine($"Order: {order.Id} | Client: {order.Client.FirstName} {order.Client.LastName} " +
                $"| Order date: {order.OrderDate} | Total amount: {order.TotalAmount:N2}");
        }
    }

    public static void PrintTotalAmountByClient(DbContextOptions<DotnetExambdContext> options, string? clientLastName)
    {
        using var db = new DotnetExambdContext(options);

        Client? client = db.Clients
            .Include(c => c.Orders)
            .FirstOrDefault(c => c.LastName.Equals(clientLastName));

        if (client is null)
        {
            Console.WriteLine($"Client {clientLastName} not found!");

            return;
        }

        decimal total = client.Orders.Sum(order => order.TotalAmount);

        Console.WriteLine($"Client: {client.FirstName} {client.LastName} | Total amount: {total:N2}");
    }

    public static void PrintAllClients(DbContextOptions<DotnetExambdContext> options)
    {
        using var db = new DotnetExambdContext(options);

        var r = db.Clients
            .Include(client => client.Cars)
            .Include(client => client.ClientAddresses)
                .ThenInclude(clientAddresses => clientAddresses.Address)
            .Include(client => client.Orders)
            .ToList();

        foreach (var i in r)
        {
            Console.WriteLine($"Client: {i.Id}. {i.FirstName} {i.LastName}");

            if (i.Cars.Count > 0)
            {
                Console.WriteLine("Cars:");

                foreach (var car in i.Cars)
                {
                    Console.WriteLine($"Car: {car.Id}. Brand: {car.Brand} | Model: {car.Model} | Year: {car.Year}");
                }
            }
            else
            {
                Console.WriteLine("Cars: N/A");
            }

            if (i.ClientAddresses.Count > 0)
            {
                Console.WriteLine("Addresses:");

                foreach (var clientAddress in i.ClientAddresses)
                {
                    Console.WriteLine($"Address: {clientAddress.Address.Id}. City: {clientAddress.Address.City}, {clientAddress.Address.Street} {clientAddress.Address.Number}");
                }
            }
            else
            {
                Console.WriteLine("Addresses: N/A");
            }

            if (i.Orders.Count > 0)
            {
                Console.WriteLine("Orders:");

                foreach (var order in i.Orders)
                {
                    Console.WriteLine($"Order: {order.Id}. Order date: {order.OrderDate}, Total amount: {order.TotalAmount:N2}");
                }
            }
            else
            {
                Console.WriteLine("Orders: N/A");
            }

            Console.WriteLine();
        }
    }

    // Privates methods
    private static void AddClients(DotnetExambdContext db)
    {
        db.Clients.AddRange(new Client { FirstName = "Ivan", LastName = "Zhmur" },
            new Client { FirstName = "Leonid", LastName = "Kravchuk" },
            new Client { FirstName = "Leonid", LastName = "Kuchma" },
            new Client { FirstName = "Victor", LastName = "Yushchenko" },
            new Client { FirstName = "Victor", LastName = "Yanukovych" },
            new Client { FirstName = "Petro", LastName = "Poroshenko" },
            new Client { FirstName = "Volodumur", LastName = "Zelenskyi" });

        db.SaveChanges();
    }

    private static void AddCars(DotnetExambdContext db)
    {

        db.Cars.AddRange(new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Zhmur"),
            Brand = "Toyota",
            Model = "Camry",
            Year = 2020
        },
        new Car
        {
            Client = null,
            Brand = "Ford",
            Model = "Mustang",
            Year = 2018
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Zelenskyi"),
            Brand = "Honda",
            Model = "Civic",
            Year = 2019
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Poroshenko"),
            Brand = "Chevrolet",
            Model = "Impala",
            Year = 2021
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Yanykovuch"),
            Brand = "Tesla",
            Model = "Model S",
            Year = 2022
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Kuchma"),
            Brand = "BMW",
            Model = "X5",
            Year = 2020
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Ushenko"),
            Brand = "Mercedes-Benz",
            Model = "C-Class",
            Year = 2021
        },
        new Car
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName == "Zhmur"),
            Brand = "Mitsubishi",
            Model = "Lancer X",
            Year = 2017
        });

        db.SaveChanges();
    }

    private static void AddAddresses(DotnetExambdContext db)
    {
        db.Address.AddRange(new Address { City = "Kyiv", Street = "Khreshchatyk Street", Number = 23 },
            new Address { City = "Lviv", Street = "Prymiska Street", Number = 46 },
            new Address { City = "Odessa", Street = "Deribasivska Street", Number = 79 },
            new Address { City = "Kharkiv", Street = "Sumska Street", Number = 101 },
            new Address { City = "Dnipro", Street = "Shevchenko Street", Number = 22 },
            new Address { City = "Zaporizhzhia", Street = "Soborna Street", Number = 33 },
            new Address { City = "Vinnytsia", Street = "Teatralna Street", Number = 44 },
            new Address { City = "Ivano-Frankivsk", Street = "Hrushevskoho Street", Number = 55 },
            new Address { City = "Zhytomyr", Street = "Kyivska Street", Number = 66 },
            new Address { City = "Ternopil", Street = "Sagaidachnoho Street", Number = 77 });

        db.SaveChanges();
    }

    private static void AddClientAddresses(DotnetExambdContext db)
    {
        db.ClientAddresses.AddRange(new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Zhmur")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Shevchenko Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Kuchma")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Sagaidachnoho Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Kuchma")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Prymiska Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Zelenskyi")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Deribasivska Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Poroshenko")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Kyivska Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Yushchenko")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Soborna Street"))
        },
        new ClientAddress
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Kravchuk")),
            Address = db.Address.FirstOrDefault(a => a.Street.Equals("Hrushevskoho Street"))
        });

        db.SaveChanges();
    }

    private static void AddOrders(DotnetExambdContext db)
    {
        db.Orders.AddRange(new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Zhmur")),
            OrderDate = new DateTime(2024, 01, 28, 19, 34, 0),
            TotalAmount = 1500
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Zelenskyi")),
            OrderDate = new DateTime(2024, 01, 23, 15, 35, 22),
            TotalAmount = 500
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Kuchma")),
            OrderDate = new DateTime(2007, 03, 12, 22, 59, 59),
            TotalAmount = 2007
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Poroshenko")),
            OrderDate = new DateTime(2014, 04, 22, 10, 30, 15),
            TotalAmount = 1909
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Yushchenko")),
            OrderDate = new DateTime(2004, 11, 4, 01, 01, 11),
            TotalAmount = 1990
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Yanukovych")),
            OrderDate = new DateTime(2010, 10, 8, 20, 03, 22),
            TotalAmount = 5000
        },
        new Order
        {
            Client = db.Clients.FirstOrDefault(c => c.LastName.Equals("Zhmur")),
            OrderDate = new DateTime(2024, 01, 29, 15, 35, 22),
            TotalAmount = 10000.22m
        });

        db.SaveChanges();
    }
}
