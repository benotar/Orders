using Dotnet_Exam.Context;
using Dotnet_Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


ConfigurationBuilder builder = new ConfigurationBuilder();

builder.SetBasePath(Directory.GetCurrentDirectory());

builder.AddJsonFile("Configuration/appsettings.json");

IConfigurationRoot? config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");

DbContextOptionsBuilder<DotnetExambdContext> optionsBuilder = new DbContextOptionsBuilder<DotnetExambdContext>();

DbContextOptions<DotnetExambdContext> options = optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.30-mysql")).Options;

DataBaseOperations.AddDataToDataBase(options);

//DataBaseOperations.PrintMinAmountOrder(options);
//Console.WriteLine();

//DataBaseOperations.PrintMaxAmountOrder(options);
//Console.WriteLine();

//DataBaseOperations.PrintOrderByYearCar(options);

//DataBaseOperations.PrintOrdersByClient(options, "Zhmur");
//Console.WriteLine();

DataBaseOperations.PrintOrdersByCity(options, "Dnipro");