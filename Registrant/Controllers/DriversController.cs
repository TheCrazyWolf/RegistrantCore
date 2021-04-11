using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Registrant.Controllers
{
    public class DriversController
    {
        public List<Models.Drivers> Driver { get; set; }

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
            catch (Exception)
            {

                throw;
            }
            return Driver;
        }

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

        internal IEnumerable GetDriversСurrent(int? idDriver)
        {
            throw new NotImplementedException();
        }
    }
}
