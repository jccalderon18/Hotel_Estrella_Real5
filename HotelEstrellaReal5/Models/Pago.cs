using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Pago
{
    [Key]
    [Column("ID_Pago")]
    [Display(Name = "ID Pago")]
    public int IdPago { get; set; }

    [Column("Medio_Pago")]
    [StringLength(50)]
    [Unicode(false)]
    [Display(Name = "Medio de Pago")]
    [Required(ErrorMessage = "El campo Medio de Pago es obligatorio.")]
    public string? MedioPago { get; set; }

    [Column("Precio_Inicial", TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo Precio Inicial es obligatorio.")]
    public decimal? PrecioInicial { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo Adelanto es obligatorio.")]
    public decimal? Adelanto { get; set; }

    [Column("Precio_Restante", TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo Precio Restante es obligatorio.")]
    public decimal? PrecioRestante { get; set; }

    [Column("Costo_Penalidad", TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo Costo Penalidad es obligatorio.")]
    public decimal? CostoPenalidad { get; set; }

    [Column("Sub_Total", TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo Sub Total es obligatorio.")]
    public decimal? SubTotal { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El campo IVA es obligatorio.")]
    public decimal? Total { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo Observación es obligatorio.")]
    public string? Observacion { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("ID_Reserva")]
    [Display(Name = "ID Reserva")]
    public int? IdReserva { get; set; }

    [InverseProperty("IdPagoNavigation")]
    public virtual ICollection<CheckOut> CheckOuts { get; set; } = new List<CheckOut>();

    [ForeignKey("IdReserva")]
    [InverseProperty("Pagos")]
    [Display(Name = "Reserva")]
    public virtual Reserva? IdReservaNavigation { get; set; }
}
