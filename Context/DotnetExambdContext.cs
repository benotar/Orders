using Dotnet_Exam.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Exam.Context;

public class DotnetExambdContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Car> Cars => Set<Car>();

    public DbSet<Address> Address => Set<Address>();

    public DbSet<ClientAddress> ClientAddresses => Set<ClientAddress>();
    
    public DbSet<Order> Orders => Set<Order>();

    public DotnetExambdContext(DbContextOptions<DotnetExambdContext> options)
        :base(options)
    { }
}
