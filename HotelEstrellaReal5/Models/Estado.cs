using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Estado")]
public partial class Estado
{
    [Key]
    [Column("ID_Estado")]
    public int IdEstado { get; set; }

    [Column("Estado")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Estado1 { get; set; }

    public bool? Activo { get; set; }

    [InverseProperty("IdEstadoNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [InverseProperty("IdEstadoNavigation")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    [InverseProperty("IdEstadoNavigation")]
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
