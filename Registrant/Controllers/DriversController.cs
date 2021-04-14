using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Registrant.Controllers
{
    public class DriversController
    {
        public List<Models.Drivers> Driver { get; set; }

        public DriversController()
        {
            Driver = new List<Models.Drivers>();
        }

        //Обычные водилы без активных
        public List<Models.Drivers> GetDrivers()
        {
            Driver.Clear();
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Drivers.Where(x => x.Active != "0").OrderByDescending(x => x.IdDriver);

                    foreach (var item in temp)
                    {
                        Models.Drivers driver = new Models.Drivers(item);
                        Driver.Add(driver);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Driver;
        }
        //Только выбранный и остальные не активные
        public List<Models.Drivers> GetDriversСurrent(int id)
        {
            Driver.Clear();
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Drivers.Where(x => x.Active != "0" | x.IdDriver == id).OrderByDescending(x => x.Family);

                    foreach (var item in temp)
                    {
                        Models.Drivers driver = new Models.Drivers(item);
                        Driver.Add(driver);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Driver;
        }
        //Только активные для чекбокса
        public List<Models.Drivers> GetDriversСurrent()
        {
            Driver.Clear();
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Drivers.Where(x => x.Active != "0").OrderByDescending(x => x.Family);

                    foreach (var item in temp)
                    {
                        Models.Drivers driver = new Models.Drivers(item);
                        Driver.Add(driver);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Driver;
        }

        //Все в том числе неактивные
        public List<Models.Drivers> GetDriversAll()
        {
            Driver.Clear();
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Drivers.OrderByDescending(x => x.IdDriver);

                    foreach (var item in temp)
                    {
                        Models.Drivers driver = new Models.Drivers(item);
                        Driver.Add(driver);
                    }
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
            return Driver;
        }

    }
}
