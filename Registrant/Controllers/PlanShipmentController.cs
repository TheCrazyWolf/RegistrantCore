using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Registrant.Controllers
{
    public class PlanShipmentController
    {
        List<Models.PlanShipment> PlanShipments { get; set; }

        public PlanShipmentController()
        {
            PlanShipments = new List<Models.PlanShipment>();
        }

        public List<Models.PlanShipment> GetPlanShipments(DateTime date)
        {
            PlanShipments.Clear();
            
            date = date.Date;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.Where(x => x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date && x.IdTimeNavigation.DateTimeFactRegist.Value == null);
                foreach (var item in temp)
                {
                    Models.PlanShipment shipment = new Models.PlanShipment(item);
                    PlanShipments.Add(shipment);
                }
            }
            return PlanShipments;
        }
    }
}
