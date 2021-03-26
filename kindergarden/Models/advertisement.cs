namespace kindergarden
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("advertisement")]
    public partial class advertisement
    {
        public int Id { get; set; }

        public string companyName { get; set; }

        public string filePath { get; set; }

        [StringLength(250)]
        public string url { get; set; }

        [StringLength(250)]
        public string mapsLink { get; set; }

        public bool? isActive { get; set; }

        public DateTime? startDate { get; set; }

        public DateTime? finishDate { get; set; }
    }
}
