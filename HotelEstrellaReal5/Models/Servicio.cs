using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Servicio
{
    [Key]
    [Column("ID_Servicio")]
    [Display(Name = "ID Servicio")]
    public int IdServicio { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get; set; }

    [Column(TypeName = "money")]
    [Required(ErrorMessage = "El campo Valor es obligatorio.")]
    [RegularExpression(@"^[0-9.]+$", ErrorMessage = "El valor solo puede tener números y puntos.")]
    [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
    [Display(Name = "Valor")]
    public decimal? Valor { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ,.]+$", ErrorMessage = "La descripción solo puede tener letras, espacios, puntos y comas.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Column("ID_Estado")]
    [Display(Name = "Estado")]
    public int? IdEstado { get; set; }

    [InverseProperty("IdServicioNavigation")]
    public virtual ICollection<DetallesReservaServicio> DetallesReservaServicios { get; set; } = new List<DetallesReservaServicio>();

    [ForeignKey("IdEstado")]
    [InverseProperty("Servicios")]
    [Display(Name = "Estado")]
    public virtual Estado? IdEstadoNavigation { get; set; }
}
