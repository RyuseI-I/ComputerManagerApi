using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagerApi.Models;

[Table("Components")]
public class Component
{
    /// <summary>char(10) — business code used as primary key</summary>
    [Key]
    [Column(TypeName = "char(10)")]
    [MaxLength(10)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Description { get; set; } = null!;

    // FK → ComponentManufacturers
    public int ComponentManufacturersId { get; set; }
    public ComponentManufacturer Manufacturer { get; set; } = null!;

    // FK → ComponentTypes
    public int ComponentTypesId { get; set; }
    public ComponentType Type { get; set; } = null!;

    public ICollection<PcComponent> PcComponents { get; set; } = new List<PcComponent>();
}
