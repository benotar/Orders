using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Exam.Entities;

public class Car
{
    [Key]
    public int Id { get; set; }

    [Column("brand")]
    public string? Brand {  get; set; }

    [Column("model")]
    public string? Model { get; set; }

    [Column("year")]
    public int Year { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }
}
