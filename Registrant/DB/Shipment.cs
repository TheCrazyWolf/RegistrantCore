using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Shipment
    {
        public int IdShipment { get; set; }
        public int? IdDriver { get; set; }
        public int? IdContragent { get; set; }
        public int IdTime { get; set; }
        public string NumRealese { get; set; }
        public string CountPodons { get; set; }
        public string PacketDocuments { get; set; }
        public string TochkaLoad { get; set; }
        public string Nomenclature { get; set; }
        public string Size { get; set; }
        public string Destination { get; set; }
        public string TypeLoad { get; set; }
        public string Description { get; set; }
        public string StoreKeeper { get; set; }
        public string Active { get; set; }
        public string ServiceInfo { get; set; }

        public virtual Contragent IdContragentNavigation { get; set; }
        public virtual Driver IdDriverNavigation { get; set; }
        public virtual Time IdTimeNavigation { get; set; }
    }
}
