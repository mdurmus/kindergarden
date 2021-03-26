using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int? OwnerId { get; set; }
        public virtual Person Owner { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }

        public string sender { get; set; }
        

        public DateTime LeaveDate { get; set; }

        public int PersonMessageId { get; set; }
        public virtual personMessage PersonMessage { get; set; }
    }
}