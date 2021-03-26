namespace kindergarden
{
    using kindergarden.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("personMessage")]
    public class personMessage
    {
        public int Id { get; set; }
       
        public bool IsUnRead { get; set; }
        

        [ForeignKey("Sender")]
        public int? SenderId { get; set; }
        public virtual Person Sender { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public virtual ICollection<Message> Message { get; set; }

    }
}
