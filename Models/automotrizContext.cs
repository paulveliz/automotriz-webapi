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
        public virtual DbSet<EstadoCivil> EstadoCivils { get; set; }
        public virtual DbSet<Hijo> Hijos { get; set; }
        public virtual DbSet<PlanSugerido> PlanSugeridos { get; set; }
        public virtual DbSet<PlanesFinanciamiento> PlanesFinanciamientos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:schoolpaulveliz.database.windows.net,1433;Initial Catalog=automotriz;Persist Security Info=False;User ID=paulveliz;Password=qwerty123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Auto>(entity =>
            {
                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PlanFinanciamiento)
                    .HasColumnName("Plan_financiamiento")
                    .HasComment("1.- >0 - <220,000.00, 2.- >220.001.00 - <380,000.00, 3.- >380,001.00 - <680,000.00, 4.- >680,001.00 - <Infinito");

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Url_imagen");

                entity.Property(e => e.ValorComercial)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Valor_comercial");

                entity.HasOne(d => d.PlanFinanciamientoNavigation)
                    .WithMany(p => p.Autos)
                    .HasForeignKey(d => d.PlanFinanciamiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Autos__au_Plan_f__38996AB5");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Apellido_paterno");

                entity.Property(e => e.Curp)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Domicilio)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstadoCivil).HasColumnName("Id_estado_civil");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clientes__Id_est__6754599E");
            });

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.ToTable("Estado_civil");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IdHijos).HasColumnName("Id_hijos");

                entity.Property(e => e.IngresoAcumulable)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Ingreso_acumulable")
                    .HasComment("");

                entity.HasOne(d => d.IdHijosNavigation)
                    .WithMany(p => p.EstadoCivils)
                    .HasForeignKey(d => d.IdHijos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Estado_ci__Id_hi__68487DD7");
            });

            modelBuilder.Entity<Hijo>(entity =>
            {
                entity.Property(e => e.CantidadFin).HasColumnName("Cantidad_fin");

                entity.Property(e => e.CantidadIni).HasColumnName("Cantidad_ini");
            });

            modelBuilder.Entity<PlanSugerido>(entity =>
            {
                entity.ToTable("Plan_sugerido");

                entity.Property(e => e.IngresoAcumulabeFin).HasColumnName("Ingreso_acumulabe_fin");

                entity.Property(e => e.IngresoAcumulableIni).HasColumnName("Ingreso_acumulable_ini");

                entity.Property(e => e.PlanFinanciamientoSug).HasColumnName("Plan_financiamiento_sug");

                entity.HasOne(d => d.PlanFinanciamientoSugNavigation)
                    .WithMany(p => p.PlanSugeridos)
                    .HasForeignKey(d => d.PlanFinanciamientoSug)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Plan_suge__Plan___693CA210");
            });

            modelBuilder.Entity<PlanesFinanciamiento>(entity =>
            {
                entity.ToTable("Planes_financiamiento");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioInicial)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Precio_inicial");

                entity.Property(e => e.PrecioLimite)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Precio_limite");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
