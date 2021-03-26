using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.Models
{
    public class LogRecord
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public DateTime LoginTime { get; set; }
        public string SchoolName { get; set; }

    }
}