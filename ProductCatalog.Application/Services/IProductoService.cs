using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Requests;
using ProductCatalog.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDetalleDto>> GetAllAsync(string? nombre, string? descripcion, string? categoria, string? orden, bool asc);
        Task<ProductoDetalleDto?> GetByIdAsync(int id);
        Task AddAsync(ProductoDto dto);
        Task UpdateAsync(int id, ProductoDto dto);
        Task DeleteAsync(int id);
        Task<PagedResult<ProductoDetalleDto>> GetPagedAsync(PagedQueryRequest request);
    }
}
