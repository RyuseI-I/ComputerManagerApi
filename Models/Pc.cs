using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagerApi.Models;

[Table("PCs")]
public class Pc
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public double Weight { get; set; }

    public int Warranty { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Stock { get; set; }

    public ICollection<PcComponent> PcComponents { get; set; } = new List<PcComponent>();
}
