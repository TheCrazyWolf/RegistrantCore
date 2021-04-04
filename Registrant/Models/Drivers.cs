using System;
using System.Collections.Generic;
using System.Text;

namespace Registrant.Models
{
    public class Drivers
    {
        public int IdDriver { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Contragent { get; set; }
        public string Attorney { get; set; }

        public Drivers(DB.Driver driver)
        {
            IdDriver = driver.IdDriver;
            FIO = driver.Family + " " + driver.Name + " " + driver.Patronymic;
            Phone = driver.Phone;
            Contragent = driver.IdContragentNavigation?.Name;
            Attorney = driver.Attorney;
        }
    }
}
