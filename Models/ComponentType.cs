using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagerApi.Models;

[Table("ComponentTypes")]
public class ComponentType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Abbreviation { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public ICollection<Component> Components { get; set; } = new List<Component>();
}
