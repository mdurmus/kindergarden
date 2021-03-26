namespace kindergarden
{
    using kindergarden.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("activitiesMessage")]
    public partial class activitiesMessage
    {
        public int Id { get; set; }

        public int? activitiesId { get; set; }

        public string messageText { get; set; }

        public bool? isActive { get; set; }

        public string messageOwner { get; set; }

        public DateTime messageDate { get; set; }

        public virtual activities activities { get; set; }

        public virtual ICollection<answerActivityMessage> AnswerActivityMessages { get; set; }
    }
}
