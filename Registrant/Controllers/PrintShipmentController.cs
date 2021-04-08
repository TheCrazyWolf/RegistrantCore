using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Registrant.Controllers
{
    public class PrintShipmentController
    {
        List<Models.PrintShipments> PlanShipments { get; set; }

        public PrintShipmentController()
        {
            PlanShipments = new List<Models.PrintShipments>();
        }

        public List<Models.PrintShipments> GetShipmentsDate(DateTime date)
        {
            PlanShipments.Clear();

            date = date.Date;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.Where(x => x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date);
                foreach (var item in temp)
                {
                    Models.PrintShipments shipment = new Models.PrintShipments(item);
                    PlanShipments.Add(shipment);
                }
            }
            return PlanShipments;
        }

       public List<Models.PrintShipments> GetShipmentsMonth(DateTime date)
        {
            PlanShipments.Clear();

            date = date.Date;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.Where(x => x.IdTimeNavigation.DateTimePlanRegist.Value.Month == date.Month);
                foreach (var item in temp)
                {
                    Models.PrintShipments shipment = new Models.PrintShipments(item);
                    PlanShipments.Add(shipment);
                }
            }
            return PlanShipments;
        }
    }
}
