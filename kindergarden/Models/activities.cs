namespace kindergarden
{
    using kindergarden.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class activities
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string subject { get; set; }

        [StringLength(1000)]
        public string miniText { get; set; }

        public string fullText { get; set; }

        public string adminText { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? startDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? finishDate { get; set; }

        [StringLength(250)]
        public string location { get; set; }

        public bool? isActive { get; set; }

       
        public int SchoolId { get; set; }
        public virtual School School { get; set; }


         public virtual ICollection<activitiesMessage> activitiesMessage { get; set; }

         public virtual ICollection<activitiesPicture> activitiesPicture { get; set; }
    }
}
