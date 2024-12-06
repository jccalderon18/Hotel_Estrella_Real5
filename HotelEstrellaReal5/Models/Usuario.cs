using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Usuario
{
    [Key]
    [Column("ID_Usuario")]
    [Display(Name = "ID Usuario")]
    public int IdUsuario { get; set; }

    [Column("Nombre_Completo")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Nombre completo es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    [Display(Name = "Nombre Completo")]
    public string? NombreCompleto { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico válida.")]
    [Display(Name = "Correo electrónico")]
    public string? Email { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$",
    ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo al menos una letra mayúscula y un número.")]
    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Clave { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$",
    ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo al menos una letra mayúscula y un número.")]
    [Required(ErrorMessage = "El campo Confirmar Contraseña es obligatorio.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Contraseña")]
    public string? ConfirmarClave { get; set; }

    [Column("Foto_Perfil_URL")]
    [StringLength(200)]
    [Unicode(false)]
    [Display(Name = "Foto de perfil URL")]
    public string? FotoPerfilUrl { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("ID_Cliente")]
    [Range(0, 999999999999999, ErrorMessage = "El campo Identificación no pude tener mas de 15 digitos.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Identificación solo puede tener números.")]
    [Required(ErrorMessage = "El campo Identificación es obligatorio.")]
    [Display(Name = "Identificación")]
    public int? IdCliente { get; set; }

    [Column("ID_Rol")]
    [Display(Name = "Rol Usuario")]
    public int? IdRol { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Usuarios")]
    [Display(Name = "Cliente")]
    public virtual Cliente? IdClienteNavigation { get; set; }

    [ForeignKey("IdRol")]
    [InverseProperty("Usuarios")]
    [Display(Name = "Rol Usuario")]
    public virtual Role? IdRolNavigation { get; set; }
}
