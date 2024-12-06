using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

[Table("Detalles_Rol_Permisos")]
public partial class DetallesRolPermiso
{
    [Key]
    [Column("ID_Detalle_Rol_Permisos")]
    public int IdDetalleRolPermisos { get; set; }

    [Column("ID_Rol")]
    public int? IdRol { get; set; }

    [Column("ID_Permiso")]
    public int? IdPermiso { get; set; }

    [ForeignKey("IdPermiso")]
    [InverseProperty("DetallesRolPermisos")]
    public virtual Permiso? IdPermisoNavigation { get; set; }

    [ForeignKey("IdRol")]
    [InverseProperty("DetallesRolPermisos")]
    public virtual Role? IdRolNavigation { get; set; }
}
