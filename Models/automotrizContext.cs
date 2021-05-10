using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class automotrizContext : DbContext
    {
        public automotrizContext()
        {
        }

        public automotrizContext(DbContextOptions<automotrizContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auto> Autos { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Deuda> Deudas { get; set; }
        public virtual DbSet<EstadosCivile> EstadosCiviles { get; set; }
        public virtual DbSet<Financiamiento> Financiamientos { get; set; }
        public virtual DbSet<Hijo> Hijos { get; set; }
        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<PlanesFinanciamiento> PlanesFinanciamientos { get; set; }
        public virtual DbSet<Solicitude> Solicitudes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Auto>(entity =>
            {
                entity.Property(e => e.IdModelo).HasColumnName("id_modelo");

                entity.Property(e => e.IdPlanFinanciamiento).HasColumnName("id_plan_financiamiento");

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Url_imagen");

                entity.Property(e => e.ValorComecial)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Valor_Comecial");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.Autos)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Autos__id_modelo__45F365D3");

                entity.HasOne(d => d.IdPlanFinanciamientoNavigation)
                    .WithMany(p => p.Autos)
                    .HasForeignKey(d => d.IdPlanFinanciamiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Autos__id_plan_f__44FF419A");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Curp)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("Fecha_nacimiento");

                entity.Property(e => e.IdEstadoCivil).HasColumnName("id_estado_civil");

                entity.Property(e => e.IngresosMensuales)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Ingresos_mensuales");

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Nombre_completo");

                entity.Property(e => e.UrlImagen)
                    .IsUnicode(false)
                    .HasColumnName("Url_imagen");

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .HasConstraintName("FK__Clientes__id_est__46E78A0C");
            });

            modelBuilder.Entity<Deuda>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.IdFinanciamiento).HasColumnName("id_financiamiento");

                entity.Property(e => e.IdSolicitud).HasColumnName("id_solicitud");

                entity.Property(e => e.UltimoAbono)
                    .HasColumnType("datetime")
                    .HasColumnName("Ultimo_abono");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Deuda)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Deudas__id_clien__60A75C0F");

                entity.HasOne(d => d.IdFinanciamientoNavigation)
                    .WithMany(p => p.Deuda)
                    .HasForeignKey(d => d.IdFinanciamiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Deudas__id_finan__619B8048");

                entity.HasOne(d => d.IdSolicitudNavigation)
                    .WithMany(p => p.Deuda)
                    .HasForeignKey(d => d.IdSolicitud)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Deudas__id_solic__628FA481");
            });

            modelBuilder.Entity<EstadosCivile>(entity =>
            {
                entity.ToTable("Estados_civiles");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<Financiamiento>(entity =>
            {
                entity.Property(e => e.CantidadAFinanciar)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Cantidad_a_financiar");

                entity.Property(e => e.Enganche).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.IdAutomovil).HasColumnName("id_automovil");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.Mensualidad).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.ValorDelAuto)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Valor_del_auto");

                entity.HasOne(d => d.IdAutomovilNavigation)
                    .WithMany(p => p.Financiamientos)
                    .HasForeignKey(d => d.IdAutomovil)
                    .HasConstraintName("FK__Financiam__id_au__6383C8BA");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Financiamientos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Financiam__id_cl__5CD6CB2B");
            });

            modelBuilder.Entity<Hijo>(entity =>
            {
                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("Fecha_nacimiento");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_completo");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Hijos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Hijos__id_client__47DBAE45");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen)
                    .IsUnicode(false)
                    .HasColumnName("Url_imagen");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.Property(e => e.IdMarca).HasColumnName("id_marca");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Modelos)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Modelos__id_marc__48CFD27E");
            });

            modelBuilder.Entity<PlanesFinanciamiento>(entity =>
            {
                entity.ToTable("Planes_financiamientos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.MinIngresoAcumulable)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("min_ingreso_acumulable");

                entity.Property(e => e.PrecioInicial)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Precio_inicial");

                entity.Property(e => e.PrecioLimite)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Precio_limite");
            });

            modelBuilder.Entity<Solicitude>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.IdPlanFinanciamiento).HasColumnName("id_plan_financiamiento");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Solicitud__id_cl__49C3F6B7");

                entity.HasOne(d => d.IdPlanFinanciamientoNavigation)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.IdPlanFinanciamiento)
                    .HasConstraintName("FK__Solicitud__id_pl__4AB81AF0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
