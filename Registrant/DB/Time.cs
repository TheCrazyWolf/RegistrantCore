using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Time
    {
        public Time()
        {
            Shipments = new HashSet<Shipment>();
        }

        public int IdTime { get; set; }
        public DateTime? DateTimePlanRegist { get; set; }
        public DateTime? DateTimeFactRegist { get; set; }
        public DateTime? DateTimeArrive { get; set; }
        public DateTime? DateTimeLoad { get; set; }
        public DateTime? DateTimeEndLoad { get; set; }
        public DateTime? DateTimeLeft { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
