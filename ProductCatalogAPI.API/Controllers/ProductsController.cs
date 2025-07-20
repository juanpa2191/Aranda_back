using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Filters;
using ProductCatalog.Application.Requests;
using ProductCatalog.Application.Services;

namespace ProductCatalogAPI.API.Controllers
{
    [ValidateModel]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductsController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var producto = await _productoService.GetAllAsync();
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoDto dto)
        {
            await _productoService.AddAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = dto.Nombre }, dto); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoDto dto)
        {
            try
            {
                await _productoService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged([FromBody] PagedQueryRequest request)
        {
            var result = await _productoService.GetPagedAsync(request);
            return Ok(result);
        }
    }
}
