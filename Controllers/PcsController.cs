using ComputerManagerApi.DTOs;
using ComputerManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagerApi.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PcsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    // ── GET /api/pcs ─────────────────────────────────────────────────────────
    /// <summary>Returns a list of all computers with basic information.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PcListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _pcService.GetAllAsync();
        return Ok(result);
    }

    // ── GET /api/pcs/{id}/components ─────────────────────────────────────────
    /// <summary>Returns the selected computer with all its components.</summary>
    [HttpGet("{id:int}/components")]
    [ProducesResponseType(typeof(PcDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetComponents(int id)
    {
        var result = await _pcService.GetByIdWithComponentsAsync(id);
        if (result is null)
            return NotFound($"PC with id {id} was not found.");

        return Ok(result);
    }

    // ── POST /api/pcs ────────────────────────────────────────────────────────
    /// <summary>Adds a new computer to the database.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PcListItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] PcCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _pcService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetComponents), new { id = created.Id }, created);
    }

    // ── PUT /api/pcs/{id} ────────────────────────────────────────────────────
    /// <summary>Updates an existing computer by ID.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] PcUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _pcService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound($"PC with id {id} was not found.");

        return Ok();
    }

    // ── DELETE /api/pcs/{id} ─────────────────────────────────────────────────
    /// <summary>Deletes a computer and its component bindings by ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pcService.DeleteAsync(id);
        if (!deleted)
            return NotFound($"PC with id {id} was not found.");

        return NoContent();
    }
}
