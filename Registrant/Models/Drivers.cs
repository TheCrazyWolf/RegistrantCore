﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Registrant.Models
{
    public class Drivers
    {
        public int IdDriver { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Attorney { get; set; }

        public string BtnEditVis { get; set; }

        public Drivers(DB.Driver driver)
        {
            IdDriver = driver.IdDriver;
            FIO = driver.Family + " " + driver.Name + " " + driver.Patronymic;
            Phone = driver.Phone;
            //Contragent = driver.IdContragentNavigation?.Name;
            Attorney = driver.Attorney;

            if (App.LevelAccess == "shipment" || App.LevelAccess == "admin")
            {
                BtnEditVis = "Visible";
            }
            else
            {
                BtnEditVis = "Collapsed";
            }
        }
    }
}
