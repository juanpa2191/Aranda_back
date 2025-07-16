using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.Tests.Services
{
    public class CategoriaServiceTests
    {
        private readonly Mock<ICategoriaRepository> _repoMock;
        private readonly CategoriaService _service;

        public CategoriaServiceTests()
        {
            _repoMock = new Mock<ICategoriaRepository>();
            _service = new CategoriaService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCategorias()
        {
            // Arrange
            var categorias = new List<Categoria>
            {
                new Categoria { Id = 1, Nombre = "Tecnología" },
                new Categoria { Id = 2, Nombre = "Ropa" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Nombre == "Ropa");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategoriaDto()
        {
            // Arrange
            var categoria = new Categoria { Id = 1, Nombre = "Hogar" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(categoria);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hogar", result!.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ShouldReturnNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Categoria?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var dto = new CategoriaDto { Nombre = "Salud" };

            // Act
            await _service.AddAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Categoria>(c => c.Nombre == "Salud")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingCategoria()
        {
            // Arrange
            var existente = new Categoria { Id = 1, Nombre = "Viejo" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existente);

            var dto = new CategoriaDto { Id = 1, Nombre = "Nuevo" };

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Categoria>(c => c.Nombre == "Nuevo")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ShouldThrow()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Categoria?)null);
            var dto = new CategoriaDto { Id = 2, Nombre = "Fantasma" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(2, dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            // Act
            await _service.DeleteAsync(3);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(3), Times.Once);
        }
    }
}
