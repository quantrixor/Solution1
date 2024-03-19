namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicineWriteOff
    {
        public int id { get; set; }

        public int medicineId { get; set; }

        public int quantity { get; set; }

        [Required]
        [StringLength(255)]
        public string reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime writeOffDate { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
