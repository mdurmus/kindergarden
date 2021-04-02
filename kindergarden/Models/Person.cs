namespace kindergarden
{
    using kindergarden.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("person")]
    public class Person
    {
        //public Person()
        //{
        //    personMessage = new HashSet<personMessage>();
            
        //}

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

       // [StringLength(100)]
       // public string Gsm { get; set; }

        [StringLength(50)]
        public string Pass { get; set; }

        public bool? IsAdmin { get; set; } = false;

        public bool? IsTeacher { get; set; } = false;

        public bool? IsMaster { get; set; } = false;

        public bool? IsActive { get; set; } = false;

        public bool? IsParent { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string PerInfo // property
        {
            get { return this.Name+" "+this.LastName; }   // get method
        }

        public int? SchoolId { get; set; }
        public virtual School School { get; set; }

        public virtual ICollection<personMessage> personMessage { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
       
    }
}
