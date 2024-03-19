namespace DataCenter.PharmacyModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MedicineRunningOut")]
    public partial class MedicineRunningOut
    {
        public int id { get; set; }

        public int optimalQuantity { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
