using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Driver
    {
        public Driver()
        {
            Shipments = new HashSet<Shipment>();
        }

        public int IdDriver { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public int? IdContragent { get; set; }
        public string Attorney { get; set; }
        public string Auto { get; set; }
        public string AutoNumber { get; set; }
        public string Passport { get; set; }
        public string Info { get; set; }
        public string Active { get; set; }
        public string ServiceInfo { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
