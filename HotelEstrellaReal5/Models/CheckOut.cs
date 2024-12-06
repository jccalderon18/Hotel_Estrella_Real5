using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class CheckOut
{
    [Key]
    [Column("ID_CheckOut")]
    public int IdCheckOut { get; set; }

    [Column("ID_Pago")]
    public int? IdPago { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("ID_Reserva")]
    public int? IdReserva { get; set; }

    [ForeignKey("IdPago")]
    [InverseProperty("CheckOuts")]
    public virtual Pago? IdPagoNavigation { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("CheckOuts")]
    public virtual Reserva? IdReservaNavigation { get; set; }
}
