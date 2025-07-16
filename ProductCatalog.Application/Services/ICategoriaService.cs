using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(int id);
        Task AddAsync(CategoriaDto dto);
        Task UpdateAsync(int id, CategoriaDto dto);
        Task DeleteAsync(int id);
    }
}
