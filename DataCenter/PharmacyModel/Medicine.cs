namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Medicine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicine()
        {
            InvoiceMedicines = new HashSet<InvoiceMedicine>();
            MedicineTransfers = new HashSet<MedicineTransfer>();
            MedicineWriteOffs = new HashSet<MedicineWriteOff>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string tradeName { get; set; }

        [StringLength(255)]
        public string manufacturer { get; set; }

        public string image { get; set; }

        public decimal price { get; set; }

        public int stockQuantity { get; set; }

        public int? warehouseId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceMedicine> InvoiceMedicines { get; set; }

        public virtual MedicineRunningOut MedicineRunningOut { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicineTransfer> MedicineTransfers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicineWriteOff> MedicineWriteOffs { get; set; }
    }
}
