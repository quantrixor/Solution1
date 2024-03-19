namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceMedicine
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int invoiceId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int medicineId { get; set; }

        public decimal price { get; set; }

        public int quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime expirationDate { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
