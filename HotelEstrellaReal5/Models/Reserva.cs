using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class Reserva
{
    [Key]
    [Column("ID_Reserva")]
    [Display(Name = "ID Reserva")]
    public int IdReserva { get; set; }

    [DataType(DataType.Date)] // Indica que solo se desea la fecha
    [Column("Fecha_Entrada")]
    [Required(ErrorMessage = "El campo Fecha Entrada es obligatorio.")]
    [Display(Name = "Fecha Entrada")]
    public DateTime FechaEntrada { get; set; }

    [DataType(DataType.Date)] // Indica que solo se desea la fecha
    [Column("Fecha_Salida")]
    [Required(ErrorMessage = "El campo Fecha Salida es obligatorio.")]
    [Display(Name = "Fecha Salida")]
    public DateTime FechaSalida { get; set; }

    [Column("Nombre_Completo")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo NombreCompleto es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\sÀ-ÿ]+$", ErrorMessage = "EL nombre solo puede tener letras y espacios.")]
    public string? NombreCompleto { get; set; }

    [Column("ID_Cliente")]
    [Range(0, 999999999999999, ErrorMessage = "El campo Numero de Identificación no pude tener mas de 15 digitos.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Numero de Identificación solo puede tener números.")]
    [Display(Name = "Numero de Identificación")]
    public int? IdCliente { get; set; }

    [Column("Comprobante_Adelanto")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ComprobanteAdelanto { get; set; }

    [Column("Fecha_Registro", TypeName = "datetime")]
    [Display(Name = "Fecha de registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("ID_Categoria")]
    public int? IdCategoria { get; set; }

    [Column("ID_Estado_Reserva")]
    
    [Required(ErrorMessage = "El campo Estado Reserva es obligatorio.")]
    [Display(Name = "Estado Reserva")]
    public int? IdEstadoReserva { get; set; }

    [Column("ID_Habitacion")]
    [Required(ErrorMessage = "El campo Habitación es obligatorio")]
    [Display(Name = "Habitación")]
    public int? IdHabitacion { get; set; }

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<CheckOut> CheckOuts { get; set; } = new List<CheckOut>();

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<DetallesReservaHuesped> DetallesReservaHuespeds { get; set; } = new List<DetallesReservaHuesped>();

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<DetallesReservaServicio> DetallesReservaServicios { get; set; } = new List<DetallesReservaServicio>();

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<EncuestaSatisfaccion> EncuestaSatisfaccions { get; set; } = new List<EncuestaSatisfaccion>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("Reservas")]
    [Display(Name = "Categoría")]
    public virtual Categoria? IdCategoriaNavigation { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Reservas")]
    [Display(Name = "Identificación Cliente")]
    public virtual Cliente? IdClienteNavigation { get; set; }

    [ForeignKey("IdEstadoReserva")]
    [InverseProperty("Reservas")]
    [Display(Name = "Estado Reserva")]
    public virtual EstadoReserva? IdEstadoReservaNavigation { get; set; }

    [ForeignKey("IdHabitacion")]
    [InverseProperty("Reservas")]
    [Display(Name = "Habitación")]
    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
