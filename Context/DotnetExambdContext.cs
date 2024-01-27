using Dotnet_Exam.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Exam.Context;

public class DotnetExambdContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Car> Cars => Set<Car>();

    public DbSet<Address> Addresss => Set<Address>();

    public DbSet<ClientAddress> ClientAddresses => Set<ClientAddress>();
    
    public DbSet<Orders> Orders => Set<Orders>();

    public DotnetExambdContext(DbContextOptions<DotnetExambdContext> options)
        :base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
