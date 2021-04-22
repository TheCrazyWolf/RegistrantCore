using System;
using System.Collections.Generic;
using System.Text;

namespace Registrant.Models
{
    public class KPPShipments
    {
        public int IdShipment { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public DateTime PlanDateFact { get; set; }
        public string PlanDateFactString { get; set; }
        public string TextStatus { get; set; }

        public string NumAuto { get; set; }
        public string btn_left { get; set; }
        public string btn_arrive { get; set; }

        public KPPShipments(DB.Shipment shipment)
        {
            IdShipment = shipment.IdShipment;
            FIO = shipment.IdDriverNavigation?.Family + " " + shipment.IdDriverNavigation?.Name + " " + shipment.IdDriverNavigation?.Patronymic;
            Phone = shipment.IdDriverNavigation?.Phone;
            if (shipment.IdTimeNavigation.DateTimeFactRegist.HasValue)
            {
                PlanDateFact = shipment.IdTimeNavigation.DateTimeFactRegist.Value;
                PlanDateFactString = PlanDateFact.ToString();
            }

            if (shipment.IdDriverNavigation.AutoNumber != null)
            {
                NumAuto = shipment.IdDriverNavigation.AutoNumber;
            }

            if (shipment.IdTimeNavigation?.DateTimeLeft != null)
            {
                TextStatus = "Покинул склад";
                btn_arrive = "Collapsed";
                btn_left = "Collapsed";
            }
            else if (shipment.IdTimeNavigation?.DateTimeLeft == null && shipment.IdTimeNavigation?.DateTimeEndLoad != null)
            {
                TextStatus = "Отгрузка завершена";
                btn_left = "Visible";
                btn_arrive = "Collapsed";
            }
            else if (shipment.IdTimeNavigation?.DateTimeEndLoad == null && shipment.IdTimeNavigation?.DateTimeLoad != null)
            {
                TextStatus = "Отгрузка";
                btn_arrive = "Collapsed";
                btn_left = "Collapsed";
            }
            else if (shipment.IdTimeNavigation?.DateTimeLoad == null && shipment.IdTimeNavigation?.DateTimeArrive != null)
            {
                TextStatus = "На территории склада";
                btn_left = "Visible";
                btn_arrive = "Collapsed";
            }
            else if (shipment.IdTimeNavigation?.DateTimeArrive == null && shipment.IdTimeNavigation?.DateTimeFactRegist != null)
            {
                TextStatus = "Зарегистрирован";
                btn_arrive = "Visible";
                btn_left = "Collapsed";

            }
            else if (shipment.IdTimeNavigation?.DateTimeFactRegist == null && shipment.IdTimeNavigation?.DateTimePlanRegist != null)
            {
                TextStatus = "";
                btn_arrive = "Collapsed";
                btn_left = "Collapsed";
            }
            else if (shipment.IdTimeNavigation?.DateTimePlanRegist == null && shipment.IdTimeNavigation?.DateTimeFactRegist != null)
            {
                TextStatus = "Зарегистрирован";
                btn_arrive = "Visible";
                btn_left = "Collapsed";
            } 
        }
    }
}
