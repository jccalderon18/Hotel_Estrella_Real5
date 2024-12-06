using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Detalles_Habitacion_Comodidad")]
public partial class DetallesHabitacionComodidad
{
    [Key]
    [Column("ID_Detalles_Habitacion_Comodidad")]
    public int IdDetallesHabitacionComodidad { get; set; }

    [Column("ID_Habitacion")]
    public int? IdHabitacion { get; set; }

    [Column("ID_Comodidad")]
    public int? IdComodidad { get; set; }

    [ForeignKey("IdComodidad")]
    [InverseProperty("DetallesHabitacionComodidads")]
    public virtual Comodidade? IdComodidadNavigation { get; set; }

    [ForeignKey("IdHabitacion")]
    [InverseProperty("DetallesHabitacionComodidads")]
    public virtual Habitacione? IdHabitacionNavigation { get; set; }
}
