using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Huespede
{
    [Key]
    [Column("ID_Huesped")]
    [Display(Name = "ID Huesped")]
    public int IdHuesped { get; set; }

    [Range(0, 999999999999999, ErrorMessage = "El campo Identificación no pude tener mas de 15 digitos.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Identificación solo puede tener números.")]
    [Required(ErrorMessage = "El campo Identificación es obligatorio.")]
    [Display(Name = "Identificación")]
    public int? Identificacion { get; set; }

    [Column("Nombre_Completo")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre Completo es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre Completo")]
    public string? NombreCompleto { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Correo electronico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico válida.")]
    [Display(Name = "Correo electronico")]
    public string? Email { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono solo puede tener números.")]
    [Required(ErrorMessage = "El campo Teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [InverseProperty("IdHuespedNavigation")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [InverseProperty("IdHuespedNavigation")]
    public virtual ICollection<DetallesReservaHuesped> DetallesReservaHuespeds { get; set; } = new List<DetallesReservaHuesped>();
}
