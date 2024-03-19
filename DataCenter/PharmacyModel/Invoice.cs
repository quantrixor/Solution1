namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceMedicines = new HashSet<InvoiceMedicine>();
        }

        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime documentDate { get; set; }

        [Required]
        [StringLength(255)]
        public string provider { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceMedicine> InvoiceMedicines { get; set; }
    }
}
