using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagerApi.Models;

/// <summary>
/// Junction table between PCs and Components with an extra Amount column.
/// Composite primary key (PcId, ComponentCode) is configured in AppDbContext.
/// </summary>
[Table("PCComponents")]
public class PcComponent
{
    public int PcId { get; set; }
    public Pc Pc { get; set; } = null!;

    [Column(TypeName = "char(10)")]
    public string ComponentCode { get; set; } = null!;
    public Component Component { get; set; } = null!;

    public int Amount { get; set; }
}
