using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataCenter.Model
{
    public partial class dbModel : DbContext
    {
        public dbModel()
            : base("name=dbModel")
        {
        }

        public virtual DbSet<CodeHospitalization> CodeHospitalization { get; set; }
        public virtual DbSet<DiseaseHistory> DiseaseHistory { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Hospitalization> Hospitalization { get; set; }
        public virtual DbSet<InsuranseCompany> InsuranseCompany { get; set; }
        public virtual DbSet<InsuransePolicy> InsuransePolicy { get; set; }
        public virtual DbSet<MedicalCard> MedicalCard { get; set; }
        public virtual DbSet<Passport> Passport { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<Referrals> Referrals { get; set; }
        public virtual DbSet<ResultEvent> ResultEvent { get; set; }
        public virtual DbSet<Speciality> Speciality { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TherapeuticDiagnosticMeasures> TherapeuticDiagnosticMeasures { get; set; }
        public virtual DbSet<TypeEvent> TypeEvent { get; set; }
        public virtual DbSet<TypeHospitalization> TypeHospitalization { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CodeHospitalization>()
                .HasMany(e => e.Hospitalization)
                .WithRequired(e => e.CodeHospitalization)
                .HasForeignKey(e => e.IDCodeHospitalization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DiseaseHistory>()
                .HasMany(e => e.Patient)
                .WithOptional(e => e.DiseaseHistory)
                .HasForeignKey(e => e.IDDiseaseHistory);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.Referrals)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.IDDoctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.TherapeuticDiagnosticMeasures)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.IDDoctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Gender>()
                .HasMany(e => e.Doctor)
                .WithRequired(e => e.Gender)
                .HasForeignKey(e => e.IDGender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Gender>()
                .HasMany(e => e.Patient)
                .WithOptional(e => e.Gender)
                .HasForeignKey(e => e.IDGender);

            modelBuilder.Entity<InsuranseCompany>()
                .HasMany(e => e.Patient)
                .WithRequired(e => e.InsuranseCompany)
                .HasForeignKey(e => e.IDInsuranseCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InsuransePolicy>()
                .HasMany(e => e.Patient)
                .WithOptional(e => e.InsuransePolicy)
                .HasForeignKey(e => e.IDInsuransePolicy);

            modelBuilder.Entity<MedicalCard>()
                .HasMany(e => e.Patient)
                .WithOptional(e => e.MedicalCard)
                .HasForeignKey(e => e.IDMedicalCard);

            modelBuilder.Entity<MedicalCard>()
                .HasMany(e => e.Prescription)
                .WithRequired(e => e.MedicalCard)
                .HasForeignKey(e => e.IDMedicalCard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalCard>()
                .HasMany(e => e.Referrals)
                .WithRequired(e => e.MedicalCard)
                .HasForeignKey(e => e.IDMedicalCard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Passport>()
                .HasMany(e => e.Patient)
                .WithRequired(e => e.Passport)
                .HasForeignKey(e => e.IDPassport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Hospitalization)
                .WithRequired(e => e.Patient)
                .HasForeignKey(e => e.IDPatient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.TherapeuticDiagnosticMeasures)
                .WithRequired(e => e.Patient)
                .HasForeignKey(e => e.IDPatient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResultEvent>()
                .HasMany(e => e.TherapeuticDiagnosticMeasures)
                .WithRequired(e => e.ResultEvent)
                .HasForeignKey(e => e.IDResultEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Speciality>()
                .HasMany(e => e.Doctor)
                .WithRequired(e => e.Speciality)
                .HasForeignKey(e => e.IDSpeciality)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeEvent>()
                .HasMany(e => e.TherapeuticDiagnosticMeasures)
                .WithRequired(e => e.TypeEvent)
                .HasForeignKey(e => e.IDTypeEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeHospitalization>()
                .HasMany(e => e.CodeHospitalization)
                .WithRequired(e => e.TypeHospitalization)
                .HasForeignKey(e => e.IDTypeHospitalization)
                .WillCascadeOnDelete(false);
        }
    }
}
