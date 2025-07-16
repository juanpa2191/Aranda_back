namespace ProductCatalog.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
