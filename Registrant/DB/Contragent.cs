using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Contragent
    {
        public Contragent()
        {
            Drivers = new HashSet<Driver>();
        }

        public int IdContragent { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public string ServiceInfo { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
    }
}
