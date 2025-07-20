using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Requests;
using ProductCatalog.Application.Responses;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductoDetalleDto>> GetAllAsync(string? nombre, string? descripcion, string? categoria, string? orden, bool asc)
        {
            var productos = await _repo.GetAllAsync(nombre, descripcion, categoria, orden, asc);

            return productos.Select(p => new ProductoDetalleDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                ImagenUrl = p.ImagenUrl,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty
            });
        }

        public async Task<ProductoDetalleDto?> GetByIdAsync(int id)
        {
            var producto = await _repo.GetByIdAsync(id);
            if (producto == null) return null;

            return new ProductoDetalleDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                ImagenUrl = producto.ImagenUrl,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty
            };
        }
        public async Task<List<ProductoDetalleDto>> GetAllAsync()
        {
            var producto = await _repo.GetAllWithCategoriaAsync();
            if (producto == null) return null;

            return producto.Select(p => new ProductoDetalleDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                ImagenUrl = p.ImagenUrl,
                CategoriaNombre = p.Categoria.Nombre
            }).ToList();
        }

        public async Task AddAsync(ProductoDto dto)
        {
            var nuevo = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                ImagenUrl = dto.ImagenUrl,
                CategoriaId = dto.CategoriaId
            };

            await _repo.AddAsync(nuevo);
        }

        public async Task UpdateAsync(int id, ProductoDto dto)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null)
                throw new KeyNotFoundException("Producto no encontrado");

            existente.Nombre = dto.Nombre;
            existente.Descripcion = dto.Descripcion;
            existente.ImagenUrl = dto.ImagenUrl;
            existente.CategoriaId = dto.CategoriaId;

            await _repo.UpdateAsync(existente);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<PagedResult<ProductoDetalleDto>> GetPagedAsync(PagedQueryRequest request)
        {
            var productos = await _repo.GetAllWithCategoriaAsync();

            // Filtrado dinámico
            foreach (var filtro in request.Filters ?? new())
            {
                var key = filtro.Key.ToLower();
                var value = filtro.Value.ToLower();

                productos = key switch
                {
                    "nombre" => productos.Where(p => p.Nombre.ToLower().Contains(value)),
                    "descripcion" => productos.Where(p => p.Descripcion.ToLower().Contains(value)),
                    "categoria" => productos.Where(p => p.Categoria != null && p.Categoria.Nombre.ToLower().Contains(value)),
                    _ => productos
                };
            }

            // Ordenamiento
            if (!string.IsNullOrWhiteSpace(request.Orden))
            {
                productos = request.Orden.ToLower() switch
                {
                    "nombre" => request.Asc ? productos.OrderBy(p => p.Nombre) : productos.OrderByDescending(p => p.Nombre),
                    "descripcion" => request.Asc ? productos.OrderBy(p => p.Descripcion) : productos.OrderByDescending(p => p.Descripcion),
                    "categoria" => request.Asc ? productos.OrderBy(p => p.Categoria!.Nombre) : productos.OrderByDescending(p => p.Categoria!.Nombre),
                    _ => productos
                };
            }

            // Total antes de paginar
            var total = await productos.CountAsync();

            // Paginación
            var items = await productos
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductoDetalleDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    ImagenUrl = p.ImagenUrl,
                    CategoriaNombre = p.Categoria!.Nombre
                })
                .ToListAsync();

            return new PagedResult<ProductoDetalleDto>
            {
                Items = items,
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
