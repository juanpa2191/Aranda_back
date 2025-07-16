using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Producto>> GetAllAsync(string? nombre, string? descripcion, string? categoria, string? orden, bool asc)
        {
            IQueryable<Producto> query = _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(descripcion))
                query = query.Where(p => p.Descripcion.Contains(descripcion));

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria.Nombre.Contains(categoria));

            query = orden?.ToLower() switch
            {
                "nombre" => asc ? query.OrderBy(p => p.Nombre) : query.OrderByDescending(p => p.Nombre),
                "categoria" => asc ? query.OrderBy(p => p.Categoria.Nombre) : query.OrderByDescending(p => p.Categoria.Nombre),
                _ => query
            };

            return await query.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Producto>> GetAllWithCategoriaAsync()
        {
            return await Task.FromResult(
                _context.Productos
                    .Include(p => p.Categoria)
                    .AsNoTracking()
            );
        }
    }
}
