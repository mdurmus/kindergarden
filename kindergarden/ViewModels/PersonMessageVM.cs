using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.ViewModels
{
    public class PersonMessageVM
    {
        public int PersonMessageId { get; set; }
        public string User { get; set; }
        public bool IsUnread { get; set; }
    }
}