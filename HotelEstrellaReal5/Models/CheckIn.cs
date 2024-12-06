using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class CheckIn
{
    [Key]
    [Column("ID_CheckIn")]
    public int IdCheckIn { get; set; }

    [Column("ID_Reserva")]
    public int? IdReserva { get; set; }

    [Column("ID_Huesped")]
    public int? IdHuesped { get; set; }

    [Column("ID_Habitacion")]
    public int? IdHabitacion { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [ForeignKey("IdHabitacion")]
    [InverseProperty("CheckIns")]
    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    [ForeignKey("IdHuesped")]
    [InverseProperty("CheckIns")]
    public virtual Huespede? IdHuespedNavigation { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("CheckIns")]
    public virtual Reserva? IdReservaNavigation { get; set; }
}
