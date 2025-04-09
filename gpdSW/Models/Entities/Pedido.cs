using System;
using System.Collections.Generic;

namespace gpdSW.Models.Entities;

public partial class Pedido
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public DateOnly Fecha { get; set; }

    public string? Instrucciones { get; set; }

    public string Telefono { get; set; } = null!;

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Detallepedido> Detallepedido { get; set; } = new List<Detallepedido>();

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
