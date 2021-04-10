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
        public string DateArrive { get; set; }
        public string DateLoad { get; set; }
        public string DateEndLoad { get; set; }


        public string Sklad { get; set; }
        public string Attorney { get; set; }
        public string Auto { get; set; }

        /// <summary>
        /// Для склада
        /// </summary>
        public string Family { get; set; }
        public string NumAuto { get; set; }
        public string TimeLoad { get; set; }
        public string TimeEnd { get; set; }
        public string TimeTotal { get; set; }

        public PrintShipments(DB.Shipment shipment)
        {
            IdShipment = shipment.IdShipment;
            FIOTelephone = shipment.IdDriverNavigation?.Family + " " + shipment.IdDriverNavigation?.Name + " " + shipment.IdDriverNavigation?.Patronymic + " " + shipment.IdDriverNavigation?.Phone;

            DatePlan = (DateTime)shipment.IdTimeNavigation?.DateTimePlanRegist;
            DateArrive = shipment.IdTimeNavigation?.DateTimeArrive.ToString();
            DateLoad = shipment.IdTimeNavigation?.DateTimeLoad.ToString();
            DateEndLoad = shipment.IdTimeNavigation?.DateTimeEndLoad.ToString();

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
            ServiceInfo = shipment.ServiceInfo;

            Destination = shipment.Destination;
            Nomenclature = shipment.Nomenclature;
            Size = shipment.Size;
            CountPodons = shipment.CountPodons;
            Family = shipment.IdDriverNavigation?.Family;
            NumAuto = shipment.IdDriverNavigation?.AutoNumber;

            TimeLoad = shipment.IdTimeNavigation?.DateTimeLoad?.ToShortTimeString();
            TimeEnd = shipment.IdTimeNavigation?.DateTimeEndLoad?.ToShortTimeString();

            if (shipment.IdTimeNavigation?.DateTimeLoad !=null && shipment.IdTimeNavigation?.DateTimeEndLoad !=null)
            {
                DateTime date1 = (DateTime)(shipment.IdTimeNavigation.DateTimeLoad);
                DateTime date2 = (DateTime)(shipment.IdTimeNavigation.DateTimeEndLoad);
                var res = date2 - date1;
                TimeTotal = res.ToString(@"hh\:mm");
            }

            StoreKeeper = shipment.StoreKeeper;
            TypeLoad = shipment.TypeLoad;
            Description = shipment.Description;
        }
    }
}
