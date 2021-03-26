using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class answerActivityMessage
    {
        public int Id { get; set; }

        public int activitiesId { get; set; }

        public string messageText { get; set; }

        public bool? isActive { get; set; }

        public string messageOwner { get; set; }

        public DateTime messageDate { get; set; }

        public int activitiesMessageId { get; set; }
        public virtual activitiesMessage ActivitiesMessage { get; set; }
    }
}