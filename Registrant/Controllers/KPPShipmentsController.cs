using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Registrant.Controllers
{
    public class KPPShipmentsController
    {
        List<Models.KPPShipments> DriverShipments { get; set; }

        public KPPShipmentsController()
        {
            DriverShipments = new List<Models.KPPShipments>();
        }

        //Получение списка отгрузок для КПП
        public List<Models.KPPShipments> GetShipments(DateTime date)
        {
            DriverShipments.Clear();

            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => ((x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date || x.IdTimeNavigation.DateTimeFactRegist.Value.Date == date) && x.IdTimeNavigation.DateTimeLeft == null && x.IdTimeNavigation.DateTimeFactRegist != null && x.Active != "0"));
                    foreach (var item in temp)
                    {
                        Models.KPPShipments shipment = new Models.KPPShipments(item);
                        DriverShipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return DriverShipments;
        }
    }
}
