using System;
using System.Collections.Generic;
using System.Text;

namespace Registrant.Models
{
    public class PlanShipment
    {
        public int IdShipment { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public DateTime PlanDateTime { get; set; }
        public string PlanDateTimeString { get; set; }

        public string NumAuto { get; set; } 

        public PlanShipment(DB.Shipment shipment)
        {
            IdShipment = shipment.IdShipment;
            FIO = shipment.IdDriverNavigation?.Family + " " + shipment.IdDriverNavigation?.Name + " " + shipment.IdDriverNavigation?.Patronymic;
            Phone = shipment.IdDriverNavigation?.Phone;
            if (shipment.IdTimeNavigation.DateTimePlanRegist.HasValue)
            {
                PlanDateTime = shipment.IdTimeNavigation.DateTimePlanRegist.Value;
                PlanDateTimeString = PlanDateTime.ToString();
            }

            if (shipment.IdDriverNavigation.AutoNumber != null)
            {
                NumAuto = shipment.IdDriverNavigation.AutoNumber;
            }

        }
    }
}
