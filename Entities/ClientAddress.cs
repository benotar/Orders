using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Exam.Entities;

public class ClientAddress
{
    [Key]
    public int Id { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    [Column("address_id")]
    public int AddressId {  get; set; }
    public Address? Address { get; set; }
}
