using Dotnet_Exam.Context;
using Dotnet_Exam.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Exam;

public class T
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
        });

        db.SaveChanges();
    }
}
