namespace ProductCatalog.Application.DTOs
{
    public class ProductoDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}
