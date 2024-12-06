using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Estado_Reservas")]
public partial class EstadoReserva
{
    [Key]
    [Column("ID_Estado_Reserva")]
    public int IdEstadoReserva { get; set; }

    [Column("Nombre_Estado")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreEstado { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [InverseProperty("IdEstadoReservaNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
