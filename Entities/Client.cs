using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Exam.Entities;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    public List<Car> Cars { get; set; } = new List<Car>();
    public List<ClientAddress> ClientAddresses = new List<ClientAddress>();
}
