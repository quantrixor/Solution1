using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataCenter.PharmacyModel
{
    public partial class dbPharmacy : DbContext
    {
        public dbPharmacy()
            : base("name=dbPharmacy")
        {
        }

        public virtual DbSet<InvoiceMedicine> InvoiceMedicines { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<MedicineRunningOut> MedicineRunningOuts { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineTransfer> MedicineTransfers { get; set; }
        public virtual DbSet<MedicineWriteOff> MedicineWriteOffs { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceMedicine>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceMedicines)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medicine>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Medicine>()
                .HasMany(e => e.InvoiceMedicines)
                .WithRequired(e => e.Medicine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medicine>()
                .HasOptional(e => e.MedicineRunningOut)
                .WithRequired(e => e.Medicine);

            modelBuilder.Entity<Medicine>()
                .HasMany(e => e.MedicineTransfers)
                .WithRequired(e => e.Medicine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medicine>()
                .HasMany(e => e.MedicineWriteOffs)
                .WithRequired(e => e.Medicine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Warehouse>()
                .HasMany(e => e.MedicineTransfers)
                .WithRequired(e => e.Warehouse)
                .HasForeignKey(e => e.destinationWarehouseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Warehouse>()
                .HasMany(e => e.MedicineTransfers1)
                .WithRequired(e => e.Warehouse1)
                .HasForeignKey(e => e.sourceWarehouseId)
                .WillCascadeOnDelete(false);
        }
    }
}
