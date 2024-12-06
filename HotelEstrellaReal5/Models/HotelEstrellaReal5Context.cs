using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Models;

public partial class HotelEstrellaReal5Context : DbContext
{
    public HotelEstrellaReal5Context()
    {
    }

    public HotelEstrellaReal5Context(DbContextOptions<HotelEstrellaReal5Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<CheckOut> CheckOuts { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Comodidade> Comodidades { get; set; }

    public virtual DbSet<DetallesHabitacionComodidad> DetallesHabitacionComodidads { get; set; }

    public virtual DbSet<DetallesReservaHuesped> DetallesReservaHuespeds { get; set; }

    public virtual DbSet<DetallesReservaServicio> DetallesReservaServicios { get; set; }

    public virtual DbSet<DetallesRolPermiso> DetallesRolPermisos { get; set; }

    public virtual DbSet<EncuestaSatisfaccion> EncuestaSatisfaccions { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadoHabitacione> EstadoHabitaciones { get; set; }

    public virtual DbSet<EstadoReserva> EstadoReservas { get; set; }

    public virtual DbSet<Habitacione> Habitaciones { get; set; }

    public virtual DbSet<Huespede> Huespedes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-7CUCEV9\\SQLEXPRESS;Initial Catalog=HotelEstrellaReal5;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__02AA07856AAB9AD0");
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.IdCheckIn).HasName("PK__CheckIns__22C18C7D646A26FA");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.CheckIns).HasConstraintName("FK__CheckIns__ID_Hab__787EE5A0");

            entity.HasOne(d => d.IdHuespedNavigation).WithMany(p => p.CheckIns).HasConstraintName("FK__CheckIns__ID_Hue__778AC167");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.CheckIns).HasConstraintName("FK__CheckIns__ID_Res__76969D2E");
        });

        modelBuilder.Entity<CheckOut>(entity =>
        {
            entity.HasKey(e => e.IdCheckOut).HasName("PK__CheckOut__4F04A09539033037");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.CheckOuts).HasConstraintName("FK__CheckOuts__ID_Pa__7C4F7684");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.CheckOuts).HasConstraintName("FK__CheckOuts__ID_Re__0D7A0286");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__E005FBFF2E616C97");

            entity.Property(e => e.IdCliente).ValueGeneratedNever();
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Telefono).IsFixedLength();

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Clientes).HasConstraintName("FK__Clientes__ID_Est__14270015");
        });

        modelBuilder.Entity<Comodidade>(entity =>
        {
            entity.HasKey(e => e.IdComodidad).HasName("PK__Comodida__504C291D0959A5A7");
        });

        modelBuilder.Entity<DetallesHabitacionComodidad>(entity =>
        {
            entity.HasKey(e => e.IdDetallesHabitacionComodidad).HasName("PK__Detalles__72AED72D9AA9AA37");

            entity.HasOne(d => d.IdComodidadNavigation).WithMany(p => p.DetallesHabitacionComodidads).HasConstraintName("FK__Detalles___ID_Co__01142BA1");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.DetallesHabitacionComodidads).HasConstraintName("FK__Detalles___ID_Ha__00200768");
        });

        modelBuilder.Entity<DetallesReservaHuesped>(entity =>
        {
            entity.HasKey(e => e.IdDetallesReservaHuesped).HasName("PK__Detalles__629493A319582D2F");

            entity.HasOne(d => d.IdHuespedNavigation).WithMany(p => p.DetallesReservaHuespeds).HasConstraintName("FK__Detalles___ID_Hu__08B54D69");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetallesReservaHuespeds).HasConstraintName("FK__Detalles___ID_Re__07C12930");
        });

        modelBuilder.Entity<DetallesReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IdDetallesReservaServicio).HasName("PK__Detalles__69C504401DF1AF65");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetallesReservaServicios).HasConstraintName("FK__Detalles___ID_Re__03F0984C");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetallesReservaServicios).HasConstraintName("FK__Detalles___ID_Se__04E4BC85");
        });

        modelBuilder.Entity<DetallesRolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdDetalleRolPermisos).HasName("PK__Detalles__B28111F7AA55E5CA");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.DetallesRolPermisos).HasConstraintName("FK__Detalles___ID_Pe__0C85DE4D");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.DetallesRolPermisos).HasConstraintName("FK__Detalles___ID_Ro__0B91BA14");
        });

        modelBuilder.Entity<EncuestaSatisfaccion>(entity =>
        {
            entity.HasKey(e => e.IdEncuesta).HasName("PK__Encuesta__A5BA10C6EF1AF501");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.EncuestaSatisfaccions).HasConstraintName("FK__EncuestaS__ID_Re__6D0D32F4");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__9CF4939552B3D92F");
        });

        modelBuilder.Entity<EstadoHabitacione>(entity =>
        {
            entity.HasKey(e => e.IdEstadoHabitacion).HasName("PK__Estado_H__FEA1D624A32974B3");
        });

        modelBuilder.Entity<EstadoReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__Estado_R__3633CB6EF5788B49");
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__Habitaci__9B683254940FD967");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Habitaciones).HasConstraintName("FK__Habitacio__ID_Ca__123EB7A3");

            entity.HasOne(d => d.IdEstadoHabitacionNavigation).WithMany(p => p.Habitaciones).HasConstraintName("FK__Habitacio__ID_Es__1332DBDC");
        });

        modelBuilder.Entity<Huespede>(entity =>
        {
            entity.HasKey(e => e.IdHuesped).HasName("PK__Huespede__0881C3E7386DBE87");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pagos__AE88B429A97CFC08");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Pagos).HasConstraintName("FK__Pagos__ID_Reserv__60A75C0F");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__D5B666CC110E1439");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reservas__12CAD9F491C9AF6D");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Reservas).HasConstraintName("FK__Reservas__ID_Cat__0F624AF8");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Reservas).HasConstraintName("FK__Reservas__ID_Cli__0E6E26BF");

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas).HasConstraintName("FK__Reservas__ID_Est__10566F31");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Reservas).HasConstraintName("FK__Reservas__ID_Hab__114A936A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__202AD220B653014F");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Roles).HasConstraintName("FK__Roles__ID_Estado__6383C8BA");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__1932F58406A9C5D0");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Servicios).HasConstraintName("FK__Servicios__ID_Es__151B244E");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__DE4431C5087BC5E0");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Usuarios).HasConstraintName("FK__Usuarios__ID_Cli__693CA210");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios).HasConstraintName("FK__Usuarios__ID_Rol__6A30C649");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
