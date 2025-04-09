using System;
using System.Collections.Generic;

namespace gpdSW.Models.Entities;

public partial class Detallepedido
{
    public int Id { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Productos IdProductoNavigation { get; set; } = null!;
}
