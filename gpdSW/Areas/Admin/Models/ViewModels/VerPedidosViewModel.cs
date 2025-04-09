namespace gpdSW.Areas.Admin.Models.ViewModels
{
    public class VerPedidosViewModel
    {
        public int IdPedido { get; set; }
        public string? NombreCliente { get; set; } 
        public DateOnly Fecha { get; set; }
        public string? Telefono { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }
    }
}
