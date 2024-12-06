using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Cliente
{
    [Key]
    [Column("ID_Cliente")]
    [Range(0, 999999999999999, ErrorMessage = "El campo Identificación no pude tener mas de 15 digitos.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Identificación solo puede tener números.")]
    [Required(ErrorMessage = "El campo Identificación es obligatorio.")]
    [Display(Name = "Identificación")]
    public int IdCliente { get; set; }

    [Column("Nombre_Completo")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre Completo")]
    public string? NombreCompleto { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    [RegularExpression(@"^[0-9\s]+$", ErrorMessage = "El teléfono solo puede tener números.")]
    [Required(ErrorMessage = "El campo Teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico válida.")]
    [Display(Name = "Correo electrónico")]
    public string? Email { get; set; }

    [Column("Fecha_Nacimiento")]
    [Required(ErrorMessage = "El campo Fecha de nacimiento es obligatorio.")]
    [Display(Name = "Fecha de nacimiento")]
    public DateOnly? FechaNacimiento { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("ID_Estado")]
    [Display(Name = "Estado")]
    public int? IdEstado { get; set; }

    [ForeignKey("IdEstado")]
    [InverseProperty("Clientes")]
    [Display(Name = "Estado")]
    public virtual Estado? IdEstadoNavigation { get; set; }

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
