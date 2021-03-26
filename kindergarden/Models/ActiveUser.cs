using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class ActiveUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

    }
}