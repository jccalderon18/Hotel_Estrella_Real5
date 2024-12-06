using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Categoria
{
    [Key]
    [Column("ID_Categoria")]
    [Display(Name = "ID Categoría")]
    public int IdCategoria { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ,.]+$", ErrorMessage = "La descripción solo puede tener letras, espacios, puntos y comas.")]
    [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [InverseProperty("IdCategoriaNavigation")]
    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();

    [InverseProperty("IdCategoriaNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
