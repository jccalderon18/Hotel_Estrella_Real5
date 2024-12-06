using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Estado_Habitaciones")]
public partial class EstadoHabitacione
{
    [Key]
    [Column("ID_Estado_Habitacion")]
    [Display(Name = "ID Estado Habitación")]
    public int IdEstadoHabitacion { get; set; }

    [Column("Nombre_estado_Habitacion")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre")]
    public string? NombreEstadoHabitacion { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ,.]+$", ErrorMessage = "La descripción solo puede tener letras, espacios, puntos y comas.")]
    [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [InverseProperty("IdEstadoHabitacionNavigation")]
    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();
}
