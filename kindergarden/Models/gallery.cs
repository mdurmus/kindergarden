using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class gallery
    {
        public int Id { get; set; }

        public string name { get; set; }

       
        public int SchoolId { get; set; }
        public virtual School School { get; set; }

        public List<galleryImage> images { get; set; }
    }
}