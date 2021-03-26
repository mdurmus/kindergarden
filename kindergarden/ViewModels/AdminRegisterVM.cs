using kindergarden.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kindergarden.ViewModels
{
    public class AdminRegisterVM
    {
        public int AdminRegisterVMId { get; set; }
        public Person Person { get; set; }
        public School School { get; set; }
    }
}