using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Registrant.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageKPP.xaml
    /// </summary>
    public partial class PageKPP : Page
    {
        Controllers.KPPShipmentsController kPP;
        Controllers.PlanShipmentController plan;


        public PageKPP()
        {
            InitializeComponent();
            kPP = new Controllers.KPPShipmentsController();
            plan = new Controllers.PlanShipmentController();

            DatePicker.SelectedDate = DateTime.Now;
        }


        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }


        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_Plan.ItemsSource = null;
            DataGrid_Drivers.ItemsSource = null;

            DataGrid_Plan.ItemsSource = plan.GetPlanShipments(DatePicker.SelectedDate.Value);
            DataGrid_Drivers.ItemsSource = kPP.GetShipments(DatePicker.SelectedDate.Value);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_refresh_Click(sender, e);
        }

        private void btn_arrive_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.KPPShipments;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                temp.IdTimeNavigation.DateTimeArrive = DateTime.Now;
                ef.SaveChanges();
                btn_refresh_Click(sender, e);
            }
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.KPPShipments;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                temp.IdTimeNavigation.DateTimeLeft = DateTime.Now;
                ef.SaveChanges();
                btn_refresh_Click(sender, e);
            }
        }

        private void btn_adddriver_Click(object sender, RoutedEventArgs e)
        {
            ClearTextboxes();
            //Forms.KPPAddNew form = new Forms.KPPAddNew();
            //form.ShowDialog();
            //btn_refresh_Click(sender, e);
            ContentAdd.ShowAsync();
        }

        void ClearTextboxes()
        {
            tb_autonum.Clear();
            tb_family.Clear();
            tb_info.Clear();
            tb_name.Clear();
            tb_passport.Clear();
            tb_patronymic.Clear();
            tb_phone.Clear();
        }

        private void btn_registration_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.PlanShipment;
            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
            {
                var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                temp.IdTimeNavigation.DateTimeFactRegist = DateTime.Now;
                ef.SaveChanges();
                btn_refresh_Click(sender, e);
            }

        }

        private void btn_add_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
            btn_refresh_Click(sender, e);
        }

        private void btn_add_add_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    ContentAdd.Hide();
                    btn_refresh_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
