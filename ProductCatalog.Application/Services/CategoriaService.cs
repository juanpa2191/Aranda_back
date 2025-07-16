using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;

        public CategoriaService(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            var categorias = await _repo.GetAllAsync();
            return categorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            });
        }

        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            var categoria = await _repo.GetByIdAsync(id);
            if (categoria == null) return null;

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task AddAsync(CategoriaDto dto)
        {
            var nueva = new Categoria { Nombre = dto.Nombre };
            await _repo.AddAsync(nueva);
        }

        public async Task UpdateAsync(int id, CategoriaDto dto)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null)
                throw new KeyNotFoundException("Categoría no encontrada");

            existente.Nombre = dto.Nombre;
            await _repo.UpdateAsync(existente);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
