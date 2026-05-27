using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagerApi.Models;

[Table("ComponentManufacturers")]
public class ComponentManufacturer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Abbreviation { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string FullName { get; set; } = null!;

    public DateOnly FoundationDate { get; set; }

    public ICollection<Component> Components { get; set; } = new List<Component>();
}
