using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Registrant.Forms
{
    /// <summary>
    /// Логика взаимодействия для KPPAddNew.xaml
    /// </summary>
    public partial class KPPAddNew
    {
        public KPPAddNew()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                DB.Shipment shipment = new DB.Shipment();
                shipment.IdDriverNavigation = new DB.Driver();
                shipment.IdTimeNavigation = new DB.Time();
                shipment.IdDriverNavigation.Family = tb_family.Text;
                shipment.IdDriverNavigation.Name = tb_name.Text;
                shipment.IdDriverNavigation.Patronymic = tb_patronymic.Text;
                shipment.IdDriverNavigation.Phone = tb_phone.Text;
                shipment.IdDriverNavigation.AutoNumber = tb_autonum.Text;
                shipment.IdDriverNavigation.Passport = tb_passport.Text;
                shipment.IdDriverNavigation.Info = tb_info.Text;
                shipment.IdDriverNavigation.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил карточку водителя";

                shipment.IdTimeNavigation.DateTimeFactRegist = DateTime.Now;

                shipment.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " каскадное добавление с карточкой водителя";

                ef.Add(shipment);
                ef.SaveChanges();
                this.Close();
            }
        }
    }
}
