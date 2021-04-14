using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Registrant.Controllers
{
    public class PlanShipmentController
    {
        List<Models.PlanShipment> PlanShipments { get; set; }

        public PlanShipmentController()
        {
            PlanShipments = new List<Models.PlanShipment>();
        }

        //Для кпп по датам
        public List<Models.PlanShipment> GetPlanShipments(DateTime date)
        {
            PlanShipments.Clear();
            
            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date && x.IdTimeNavigation.DateTimeFactRegist.Value == null);
                    foreach (var item in temp)
                    {
                        Models.PlanShipment shipment = new Models.PlanShipment(item);
                        PlanShipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return PlanShipments;
        }
    }
}
