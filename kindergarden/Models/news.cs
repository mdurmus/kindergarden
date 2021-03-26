namespace kindergarden
{
    using kindergarden.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum Priority
    {
        Prio0,
        Prio1
    }

    public partial class news
    {

        public int Id { get; set; }

        public string subject { get; set; }

        public string newsText { get; set; }

        public string url { get; set; }

        public string filePath { get; set; }

        public Priority priority { get; set; }


        public int SchoolId { get; set; }
        public virtual School School { get; set; }

    }
}
