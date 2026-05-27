using ComputerManagerApi.Data;
using ComputerManagerApi.DTOs;
using ComputerManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagerApi.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _db;

    public PcService(AppDbContext db)
    {
        _db = db;
    }

    // ── GET /api/pcs ─────────────────────────────────────────────────────────
    public async Task<List<PcListItemDto>> GetAllAsync()
    {
        return await _db.PCs
            .AsNoTracking()
            .Select(pc => new PcListItemDto
            {
                Id        = pc.Id,
                Name      = pc.Name,
                Weight    = pc.Weight,
                Warranty  = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock     = pc.Stock
            })
            .ToListAsync();
    }

    // ── GET /api/pcs/{id}/components ─────────────────────────────────────────
    public async Task<PcDetailDto?> GetByIdWithComponentsAsync(int id)
    {
        var pc = await _db.PCs
            .AsNoTracking()
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc is null)
            return null;

        return new PcDetailDto
        {
            Id        = pc.Id,
            Name      = pc.Name,
            Weight    = pc.Weight,
            Warranty  = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock     = pc.Stock,
            Components = pc.PcComponents.Select(pcc => new PcComponentDto
            {
                Amount = pcc.Amount,
                Component = new ComponentDto
                {
                    Code        = pcc.Component.Code.Trim(),
                    Name        = pcc.Component.Name,
                    Description = pcc.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id             = pcc.Component.Manufacturer.Id,
                        Abbreviation   = pcc.Component.Manufacturer.Abbreviation,
                        FullName       = pcc.Component.Manufacturer.FullName,
                        FoundationDate = pcc.Component.Manufacturer.FoundationDate
                    },
                    Type = new ComponentTypeDto
                    {
                        Id           = pcc.Component.Type.Id,
                        Abbreviation = pcc.Component.Type.Abbreviation,
                        Name         = pcc.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    // ── POST /api/pcs ─────────────────────────────────────────────────────────
    public async Task<PcListItemDto> CreateAsync(PcCreateDto dto)
    {
        var pc = new Pc
        {
            Name      = dto.Name,
            Weight    = dto.Weight,
            Warranty  = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock     = dto.Stock
        };

        _db.PCs.Add(pc);
        await _db.SaveChangesAsync();

        return new PcListItemDto
        {
            Id        = pc.Id,
            Name      = pc.Name,
            Weight    = pc.Weight,
            Warranty  = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock     = pc.Stock
        };
    }

    // ── PUT /api/pcs/{id} ────────────────────────────────────────────────────
    public async Task<bool> UpdateAsync(int id, PcUpdateDto dto)
    {
        var pc = await _db.PCs.FindAsync(id);
        if (pc is null)
            return false;

        pc.Name      = dto.Name;
        pc.Weight    = dto.Weight;
        pc.Warranty  = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock     = dto.Stock;

        await _db.SaveChangesAsync();
        return true;
    }

    // ── DELETE /api/pcs/{id} ─────────────────────────────────────────────────
    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _db.PCs.FindAsync(id);
        if (pc is null)
            return false;

        // PCComponents rows are removed automatically via cascade delete
        _db.PCs.Remove(pc);
        await _db.SaveChangesAsync();
        return true;
    }
}
