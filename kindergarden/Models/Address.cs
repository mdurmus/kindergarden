using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class Address
    {
        public int Id { get; set; }
      
        public int? PersonId { get; set; }

        public string State { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }

        public virtual Person Person { get; set; }
       


    }
}