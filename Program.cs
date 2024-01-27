using Dotnet_Exam.Context;
using Dotnet_Exam.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;


ConfigurationBuilder builder = new ConfigurationBuilder();

builder.SetBasePath(Directory.GetCurrentDirectory());

builder.AddJsonFile("Configuration/appsettings.json");

IConfigurationRoot? config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");

DbContextOptionsBuilder<DotnetExambdContext> optionsBuilder = new DbContextOptionsBuilder<DotnetExambdContext>();

DbContextOptions<DotnetExambdContext> options = optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.30-mysql")).Options;

using (DotnetExambdContext db = new DotnetExambdContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    List<Client> clients = new List<Client>
    {
        new Client {FirstName = "Ivan", LastName = "Zhmur"},
        new Client {FirstName = "Leonid", LastName = "Kravchuk"},
        new Client {FirstName = "Leonid", LastName = "Kuchma"},
        new Client {FirstName = "Victor", LastName = "Ushenko"},
        new Client {FirstName = "Victor", LastName = "Yanykovuch"},
        new Client {FirstName = "Petro", LastName = "Poroshenko"},
        new Client {FirstName = "Volodumur", LastName = "Zelenskyi"},
    };

    db.Clients.AddRange(clients);
    db.SaveChanges();

    List<Car> cars = new List<Car>
    {
        new Car
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
            Client =  db.Clients.FirstOrDefault(c => c.LastName == "Poroshenko"),
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
        }
    };

    db.Cars.AddRange(cars);
    db.SaveChanges();
}
