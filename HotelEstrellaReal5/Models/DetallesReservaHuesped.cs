using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Detalles_Reserva_Huesped")]
public partial class DetallesReservaHuesped
{
    [Key]
    [Column("ID_Detalles_Reserva_Huesped")]
    public int IdDetallesReservaHuesped { get; set; }

    [Column("ID_Reserva")]
    public int? IdReserva { get; set; }

    [Column("ID_Huesped")]
    public int? IdHuesped { get; set; }

    [ForeignKey("IdHuesped")]
    [InverseProperty("DetallesReservaHuespeds")]
    public virtual Huespede? IdHuespedNavigation { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("DetallesReservaHuespeds")]
    public virtual Reserva? IdReservaNavigation { get; set; }
}
