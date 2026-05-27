using ComputerManagerApi.DTOs;

namespace ComputerManagerApi.Services;

public interface IPcService
{
    Task<List<PcListItemDto>> GetAllAsync();
    Task<PcDetailDto?>        GetByIdWithComponentsAsync(int id);
    Task<PcListItemDto>       CreateAsync(PcCreateDto dto);
    Task<bool>                UpdateAsync(int id, PcUpdateDto dto);
    Task<bool>                DeleteAsync(int id);
}
