namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicineTransfer
    {
        public int id { get; set; }

        public int medicineId { get; set; }

        public int sourceWarehouseId { get; set; }

        public int destinationWarehouseId { get; set; }

        public int quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime transferDate { get; set; }

        public virtual Medicine Medicine { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Warehouse Warehouse1 { get; set; }
    }
}
