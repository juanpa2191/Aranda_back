using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.Tests.Services
{
    public class ProductoServiceTests
    {
        private readonly Mock<IProductoRepository> _repoMock;
        private readonly ProductoService _service;

        public ProductoServiceTests()
        {
            _repoMock = new Mock<IProductoRepository>();
            _service = new ProductoService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProductoDetalleDtos()
        {
            // Arrange
            var productos = new List<Producto>
            {
                new Producto
                {
                    Id = 1,
                    Nombre = "Teclado",
                    Descripcion = "Teclado mecánico",
                    ImagenUrl = "url.png",
                    Categoria = new Categoria { Id = 1, Nombre = "Tecnología" }
                }
            };

            _repoMock.Setup(r => r.GetAllAsync(null, null, null, null, true))
                     .ReturnsAsync(productos);

            // Act
            var result = await _service.GetAllAsync(null, null, null, null, true);

            // Assert
            Assert.Single(result);
            Assert.Equal("Teclado", result.First().Nombre);
            Assert.Equal("Tecnología", result.First().CategoriaNombre);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Producto?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldCallAddOnRepository()
        {
            // Arrange
            var dto = new ProductoDto
            {
                Nombre = "Mouse",
                Descripcion = "Mouse óptico",
                ImagenUrl = "mouse.jpg",
                CategoriaId = 1
            };

            // Act
            await _service.AddAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Producto>(p => p.Nombre == "Mouse")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrow()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Producto?)null);
            var dto = new ProductoDto
            {
                Nombre = "Update",
                Descripcion = "desc",
                ImagenUrl = "img.jpg",
                CategoriaId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99, dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Act
            await _service.DeleteAsync(1);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
