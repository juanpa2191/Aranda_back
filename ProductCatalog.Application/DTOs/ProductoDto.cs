namespace ProductCatalog.Application.DTOs
{
    public class ProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
    }
}
