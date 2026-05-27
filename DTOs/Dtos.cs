namespace ComputerManagerApi.DTOs;

// ─────────────────────────────────────────────────────────────────────────────
// GET /api/pcs  →  list item
// ─────────────────────────────────────────────────────────────────────────────
public class PcListItemDto
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = null!;
    public double   Weight    { get; set; }
    public int      Warranty  { get; set; }
    public DateTime CreatedAt { get; set; }
    public int      Stock     { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// GET /api/pcs/{id}/components  →  detail with nested components
// ─────────────────────────────────────────────────────────────────────────────
public class PcDetailDto
{
    public int      Id         { get; set; }
    public string   Name       { get; set; } = null!;
    public double   Weight     { get; set; }
    public int      Warranty   { get; set; }
    public DateTime CreatedAt  { get; set; }
    public int      Stock      { get; set; }
    public List<PcComponentDto> Components { get; set; } = new();
}

public class PcComponentDto
{
    public int          Amount    { get; set; }
    public ComponentDto Component { get; set; } = null!;
}

public class ComponentDto
{
    public string          Code         { get; set; } = null!;
    public string          Name         { get; set; } = null!;
    public string          Description  { get; set; } = null!;
    public ManufacturerDto Manufacturer { get; set; } = null!;
    public ComponentTypeDto Type        { get; set; } = null!;
}

public class ManufacturerDto
{
    public int      Id             { get; set; }
    public string   Abbreviation   { get; set; } = null!;
    public string   FullName       { get; set; } = null!;
    public DateOnly FoundationDate { get; set; }
}

public class ComponentTypeDto
{
    public int    Id           { get; set; }
    public string Abbreviation { get; set; } = null!;
    public string Name         { get; set; } = null!;
}

// ─────────────────────────────────────────────────────────────────────────────
// POST /api/pcs  (request body)
// ─────────────────────────────────────────────────────────────────────────────
public class PcCreateDto
{
    public string   Name      { get; set; } = null!;
    public double   Weight    { get; set; }
    public int      Warranty  { get; set; }
    public DateTime CreatedAt { get; set; }
    public int      Stock     { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// PUT /api/pcs/{id}  (request body — same fields, separate DTO for clarity)
// ─────────────────────────────────────────────────────────────────────────────
public class PcUpdateDto
{
    public string   Name      { get; set; } = null!;
    public double   Weight    { get; set; }
    public int      Warranty  { get; set; }
    public DateTime CreatedAt { get; set; }
    public int      Stock     { get; set; }
}
