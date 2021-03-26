using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class CalendarActivity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Text { get; set; }

        public int SchoolId { get; set; }
        public virtual School School { get; set; }

    }
}