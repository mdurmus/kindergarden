namespace kindergarden
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("activitiesPicture")]
    public partial class activitiesPicture
    {
        public int Id { get; set; }

        public int? activitiesId { get; set; }

        public string filePath { get; set; }

        public bool? isActive { get; set; }

        public virtual activities activities { get; set; }
    }
}
