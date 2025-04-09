using gpdSW.Models.Entities;

namespace gpdSW.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int? Id { get; set; }
        public DateOnly Fecha { get; set; }
        public string? Instrucciones { get; set; }
        public string? Telefono { get; set; }
        public List<ProductosCarrito> ProductosCarrito { get; set; } = new List<ProductosCarrito>();
    }

    public class ProductosCarrito
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Precio { get; set; }
    }
}
