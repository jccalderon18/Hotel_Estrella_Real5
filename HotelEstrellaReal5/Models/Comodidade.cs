using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Comodidade
{
    [Key]
    [Column("ID_Comodidad")]
    [Display(Name = "ID Comodidad")]
    public int IdComodidad { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ,.]+$", ErrorMessage = "La descripción solo puede tener letras, espacios, puntos y comas.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [InverseProperty("IdComodidadNavigation")]
    public virtual ICollection<DetallesHabitacionComodidad> DetallesHabitacionComodidads { get; set; } = new List<DetallesHabitacionComodidad>();
}
