using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Registrant.Controllers
{
    public class ShipmentController
    {
        List<Models.Shipments> Shipments { get; set; }

        public ShipmentController()
        {
            Shipments = new List<Models.Shipments>();
        }
        public List<Models.Shipments> GetShipments(DateTime date)
        {
            Shipments.Clear();

            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => ((x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date || x.IdTimeNavigation.DateTimeFactRegist.Value.Date == date) && x.Active != "0")).OrderByDescending(x => x.IdTimeNavigation.DateTimePlanRegist);
                    foreach (var item in temp)
                    {
                        Models.Shipments shipment = new Models.Shipments(item);
                        Shipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Shipments;
        }

        /// <summary>
        /// ВСЕЕ
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Models.Shipments> GetShipmentsAll()
        {
            Shipments.Clear();
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.OrderByDescending(x => x.IdTimeNavigation.DateTimePlanRegist);
                    foreach (var item in temp)
                    {
                        Models.Shipments shipment = new Models.Shipments(item);
                        Shipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Shipments;
        }
        /// <summary>
        /// Только кто фактически реганулся
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Models.Shipments> GetShipmentsFactReg(DateTime date)
        {
            Shipments.Clear();

            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => ((x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date || x.IdTimeNavigation.DateTimeFactRegist.Value.Date == date) && x.IdTimeNavigation.DateTimeLeft == null && x.IdTimeNavigation.DateTimeArrive == null && x.Active != "0")).OrderByDescending(x => x.IdTimeNavigation.DateTimePlanRegist);
                    foreach (var item in temp)
                    {
                        Models.Shipments shipment = new Models.Shipments(item);
                        Shipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Shipments;
        }
        /// <summary>
        /// Только прибывшие
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Models.Shipments> GetShipmentsArrive(DateTime date)
        {
            Shipments.Clear();

            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => ((x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date || x.IdTimeNavigation.DateTimeFactRegist.Value.Date == date) && x.IdTimeNavigation.DateTimeArrive != null && x.IdTimeNavigation.DateTimeLeft == null && x.Active != "0")).OrderByDescending(x => x.IdTimeNavigation.DateTimePlanRegist);
                    foreach (var item in temp)
                    {
                        Models.Shipments shipment = new Models.Shipments(item);
                        Shipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Shipments;
        }
        /// <summary>
        /// Покинули
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// 
        public List<Models.Shipments> GetShipmentsLeft(DateTime date)
        {
            Shipments.Clear();
            date = date.Date;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.Where(x => ((x.IdTimeNavigation.DateTimePlanRegist.Value.Date == date || x.IdTimeNavigation.DateTimeFactRegist.Value.Date == date) && x.IdTimeNavigation.DateTimeLeft != null && x.Active != "0")).OrderByDescending(x => x.IdTimeNavigation.DateTimePlanRegist);
                    foreach (var item in temp)
                    {
                        Models.Shipments shipment = new Models.Shipments(item);
                        Shipments.Add(shipment);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Shipments;
        }
    }
}
