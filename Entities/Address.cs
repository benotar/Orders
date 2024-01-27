using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Exam.Entities;

public class Address
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("street")]
    public string? Street { get; set; }

    [Column("number")]
    public int Number { get; set; }

    public List<ClientAddress> ClientAddresses {  get; set; } = new List<ClientAddress>();
}
