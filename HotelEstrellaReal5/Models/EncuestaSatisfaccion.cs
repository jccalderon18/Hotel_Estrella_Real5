using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("EncuestaSatisfaccion")]
public partial class EncuestaSatisfaccion
{
    [Key]
    [Column("ID_Encuesta")]
    [Display(Name = "ID Encuesta")]
    public int IdEncuesta { get; set; }

    [Column("ID_Reserva")]
    [Display(Name = "Titular Reserva")]
    public int? IdReserva { get; set; }

    [Column("Calificacion_Limpieza")]
    [Required(ErrorMessage = "El campo Calificación Limpieza es obligatorio.")]
    [Display(Name = "Limpieza")]
    public int? CalificacionLimpieza { get; set; }

    [Column("Calificacion_Servicios")]
    [Required(ErrorMessage = "El campo Calificación Servicios es obligatorio.")]
    [Display(Name = "Servicios")]
    public int? CalificacionServicios { get; set; }

    [Column("Calificacion_Comida")]
    [Required(ErrorMessage = "El campo Calificación Comida es obligatorio.")]
    [Display(Name = "Comida")]
    public int? CalificacionComida { get; set; }

    [Column("Calificacion_Habitacion")]
    [Required(ErrorMessage = "El campo Calificación Habitación es obligatorio.")]
    [Display(Name = "Habitación")]
    public int? CalificacionHabitacion { get; set; }

    [Column("Calificacion_Atencion")]
    [Required(ErrorMessage = "El campo Calificación Atencion es obligatorio.")]
    [Display(Name = "Atención")]
    public int? CalificacionAtencion { get; set; }

    [Column("Calificacion_Instalaciones")]
    [Required(ErrorMessage = "El campo Calificación Instalaciones es obligatorio.")]
    [Display(Name = "Instalaciones")]
    public int? CalificacionInstalaciones { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("EncuestaSatisfaccions")]
    [Display(Name = "Titular Reserva")]
    public virtual Reserva? IdReservaNavigation { get; set; }
}
