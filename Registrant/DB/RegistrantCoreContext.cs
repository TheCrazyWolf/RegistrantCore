using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Registrant.DB
{
    public partial class RegistrantCoreContext : DbContext
    {
        public RegistrantCoreContext()
        {
        }

        public RegistrantCoreContext(DbContextOptions<RegistrantCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contragent> Contragents { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Engine> Engines { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=" + Settings.Connection.Default.IP + "," + Settings.Connection.Default.Port + ";" + "Database=" + Settings.Connection.Default.Database + ";User ID=" + Settings.Connection.Default.Login + ";" + "Password=" + Settings.Connection.Default.Password + ";MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Contragent>(entity =>
            {
                entity.HasKey(e => e.IdContragent);

                entity.Property(e => e.IdContragent).HasColumnName("idContragent");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.ServiceInfo).HasColumnType("text");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.IdDriver);

                entity.Property(e => e.IdDriver).HasColumnName("idDriver");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.Attorney).HasMaxLength(150);

                entity.Property(e => e.Auto).HasMaxLength(150);

                entity.Property(e => e.AutoNumber).HasMaxLength(150);

                entity.Property(e => e.Family).HasMaxLength(150);

                entity.Property(e => e.IdContragent).HasColumnName("idContragent");

                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Passport).HasColumnType("text");

                entity.Property(e => e.Patronymic).HasMaxLength(150);

                entity.Property(e => e.Phone).HasMaxLength(150);

                entity.Property(e => e.ServiceInfo).HasColumnType("text");

                entity.HasOne(d => d.IdContragentNavigation)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.IdContragent)
                    .HasConstraintName("FK_Drivers_Contragents");
            });

            modelBuilder.Entity<Engine>(entity =>
            {
                entity.ToTable("Engine");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ActualAppVer).HasMaxLength(50);

                entity.Property(e => e.ActualBdVer).HasMaxLength(50);
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasKey(e => e.IdShipment);

                entity.ToTable("Shipment");

                entity.Property(e => e.IdShipment).HasColumnName("idShipment");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.CountPodons).HasMaxLength(150);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Destination).HasMaxLength(150);

                entity.Property(e => e.IdDriver).HasColumnName("idDriver");

                entity.Property(e => e.IdTime).HasColumnName("idTime");

                entity.Property(e => e.Nomenclature).HasMaxLength(150);

                entity.Property(e => e.NumRealese).HasMaxLength(150);

                entity.Property(e => e.PacketDocuments).HasMaxLength(150);

                entity.Property(e => e.ServiceInfo).HasColumnType("text");

                entity.Property(e => e.Size).HasMaxLength(150);

                entity.Property(e => e.StoreKeeper).HasMaxLength(150);

                entity.Property(e => e.TochkaLoad).HasMaxLength(150);

                entity.Property(e => e.TypeLoad).HasMaxLength(150);

                entity.HasOne(d => d.IdDriverNavigation)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.IdDriver)
                    .HasConstraintName("FK_Shipment_Drivers");

                entity.HasOne(d => d.IdTimeNavigation)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.IdTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_Time");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasKey(e => e.IdTime);

                entity.ToTable("Time");

                entity.Property(e => e.IdTime).HasColumnName("idTime");

                entity.Property(e => e.DateTimeArrive).HasColumnType("datetime");

                entity.Property(e => e.DateTimeEndLoad).HasColumnType("datetime");

                entity.Property(e => e.DateTimeFactRegist).HasColumnType("datetime");

                entity.Property(e => e.DateTimeLeft).HasColumnType("datetime");

                entity.Property(e => e.DateTimeLoad).HasColumnType("datetime");

                entity.Property(e => e.DateTimePlanRegist).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.LevelAccess).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
