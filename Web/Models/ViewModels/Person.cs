using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class PersonViewModel
    {
        public Person Man { set; get; }
        public Person Woman { set; get; }
    }

    public class Person
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }
}