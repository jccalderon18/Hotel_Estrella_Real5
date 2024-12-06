using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Detalles_Reserva_Servicio")]
public partial class DetallesReservaServicio
{
    [Key]
    [Column("ID_Detalles_Reserva_Servicio")]
    public int IdDetallesReservaServicio { get; set; }

    [Column("ID_Reserva")]
    public int? IdReserva { get; set; }

    [Column("ID_Servicio")]
    public int? IdServicio { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("DetallesReservaServicios")]
    public virtual Reserva? IdReservaNavigation { get; set; }

    [ForeignKey("IdServicio")]
    [InverseProperty("DetallesReservaServicios")]
    public virtual Servicio? IdServicioNavigation { get; set; }
}
