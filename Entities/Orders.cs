using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Exam.Entities;

public class Orders
{
    [Key]
    public int Id { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    [Column("order_date")]
    public DateTime? OrderDate { get; set; }

    [Column("total_amount")]
    public int TotalAmount { get; set; }
}
