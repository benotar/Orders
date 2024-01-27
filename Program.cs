using Dotnet_Exam.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;


ConfigurationBuilder builder = new ConfigurationBuilder();

builder.SetBasePath(Directory.GetCurrentDirectory());

builder.AddJsonFile("Configuration/appsettings.json");

IConfigurationRoot config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");

DbContextOptionsBuilder<DotnetExambdContext> optionsBuilder = new DbContextOptionsBuilder<DotnetExambdContext>();

DbContextOptions<DotnetExambdContext> options = optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.30-mysql")).Options;

using (DotnetExambdContext db = new DotnetExambdContext(options))
{

}
