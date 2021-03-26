using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class School
    {
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string schoolGuid { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPayWillSchool { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Person> person { get; set; }
        public virtual ICollection<activities> activities { get; set; }
        public virtual ICollection<news> News { get; set; }
        public virtual ICollection<gallery> gallery { get; set; }
        public virtual ICollection<CalendarActivity> CalendarActivities { get; set; }
       




    }
}