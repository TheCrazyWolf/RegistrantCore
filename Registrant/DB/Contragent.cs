using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Contragent
    {
        public Contragent()
        {
            Shipments = new HashSet<Shipment>();
        }

        public int IdContragent { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public string ServiceInfo { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
