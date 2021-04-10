using System;
using System.Collections.Generic;
using System.Text;

namespace Registrant.Models
{
    public class PrintShipments : DB.Shipment
    {
        public string FIOTelephone { get; set; }
        public DateTime DatePlan { get; set; }

        public string Contragent { get; set; }
        public string PlanDate { get; set; }
        public string PlanTime { get; set; }

        public string DateFact { get; set; }
        public string DateLeft { get; set; }

        public string Sklad { get; set; }

        public string Attorney { get; set; }
        public string Auto { get; set; }
        public string TochkaLoad { get; set; }

        public PrintShipments(DB.Shipment shipment)
        {
            IdShipment = shipment.IdShipment;
            FIOTelephone = shipment.IdDriverNavigation?.Family + " " + shipment.IdDriverNavigation?.Name + " " + shipment.IdDriverNavigation?.Patronymic + " " + shipment.IdDriverNavigation?.Phone;

            DatePlan = (DateTime)shipment.IdTimeNavigation?.DateTimePlanRegist;

            Contragent = shipment.IdDriverNavigation?.IdContragentNavigation?.Name;
            Attorney = shipment.IdDriverNavigation?.Attorney;
            Auto = shipment.IdDriverNavigation?.Auto + " " + shipment.IdDriverNavigation?.AutoNumber;

            PlanDate = DatePlan.ToShortDateString();
            PlanTime = DatePlan.ToShortTimeString();

            DateFact = shipment.IdTimeNavigation?.DateTimeFactRegist.ToString();
            DateLeft = shipment.IdTimeNavigation?.DateTimeLeft.ToString();
            Sklad = "МВП";

            NumRealese = shipment.NumRealese;
            PacketDocuments = shipment.PacketDocuments;
            TochkaLoad = shipment.TochkaLoad;
        }


    }
}
