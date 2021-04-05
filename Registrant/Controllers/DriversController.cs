using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Registrant.Controllers
{
    public class DriversController
    {
        List<Models.Drivers> Driver { get; set; }

        public DriversController()
        {
            Driver = new List<Models.Drivers>();
        }

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
            catch (Exception)
            {

                throw;
            }
            return Driver;
        }

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
            catch (Exception)
            {

                throw;
            }
            return Driver;
        }

    }
}
