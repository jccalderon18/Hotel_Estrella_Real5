using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Habitacione
{
    [Key]
    [Column("ID_Habitacion")]
    [Display(Name = "ID Habitación")]
    public int IdHabitacion { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ0-9]+$", ErrorMessage = "EL nombre solo puede tener letras, espacios y numeros.")]
    public string? Nombre { get; set; }

    [Column(TypeName = "money")]
    [Required(ErrorMessage = "El campo Precio es Obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
    [RegularExpression(@"^[0-9.]+$", ErrorMessage = "El precio solo puede tener números y puntos.")]
    public decimal? Precio { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ,.]+$", ErrorMessage = "La descripción solo puede tener letras, espacios, puntos y comas.")]
    [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Column("ID_Categoria")]
    [Display(Name = "ID Categoría")]
    public int? IdCategoria { get; set; }

    [Column("ID_Estado_Habitacion")]
    [Display(Name = "Estado Habitación")]
    public int? IdEstadoHabitacion { get; set; }

    [InverseProperty("IdHabitacionNavigation")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [InverseProperty("IdHabitacionNavigation")]
    public virtual ICollection<DetallesHabitacionComodidad> DetallesHabitacionComodidads { get; set; } = new List<DetallesHabitacionComodidad>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("Habitaciones")]
    [Display(Name = "Categoría")]
    public virtual Categoria? IdCategoriaNavigation { get; set; }

    [ForeignKey("IdEstadoHabitacion")]
    [InverseProperty("Habitaciones")]
    [Display(Name = "Estado")]
    public virtual EstadoHabitacione? IdEstadoHabitacionNavigation { get; set; }

    [InverseProperty("IdHabitacionNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
