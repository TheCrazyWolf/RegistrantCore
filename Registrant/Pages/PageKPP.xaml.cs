using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            Thread thread = new Thread(new ThreadStart(RefreshThread));
            thread.Start();
        }

        void RefreshThread()
        {
            while (true)
            {
                Thread.Sleep(Settings.App.Default.RefreshContent);
                Dispatcher.Invoke(() => DataGrid_Plan.ItemsSource = plan.GetPlanShipments(Dispatcher.Invoke(() => DatePicker.SelectedDate.Value)));

                //Пока что так, так как охрана не умеет видимо пролистывать календарик
                //Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = kPP.GetShipments(Dispatcher.Invoke(() => DatePicker.SelectedDate.Value)));

                //Но следующий метод просто выведет ВСЕ зарегистрированные текущие водители 
                Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = kPP.GetShipments());

                Dispatcher.Invoke(() => DataGrid_Plan.Items.Refresh());
                Dispatcher.Invoke(() => DataGrid_Drivers.Items.Refresh());
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }


        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => DataGrid_Plan.ItemsSource = null);
            Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = null);

            Dispatcher.Invoke(() => DataGrid_Plan.ItemsSource = plan.GetPlanShipments(Dispatcher.Invoke(() => DatePicker.SelectedDate.Value)));

            //Пока что так, так как охрана не умеет видимо пролистывать календарик
            //Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = kPP.GetShipments(Dispatcher.Invoke(() => DatePicker.SelectedDate.Value)));

            //Но следующий метод просто выведет ВСЕ зарегистрированные текущие водители 
            Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = kPP.GetShipments());
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_plan.Text = "Запланированные отгрузки за " + DatePicker.SelectedDate.Value.ToShortDateString();
            btn_refresh_Click(sender, e);
        }

        private void btn_arrive_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.KPPShipments;

            if (current != null)
            {
                MessageBoxResult result = MessageBox.Show("Сменить статус водителя " + current.FIO + " на Прибыл?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                        {
                            var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                            temp.IdTimeNavigation.DateTimeArrive = DateTime.Now;
                            ef.SaveChanges();
                            btn_refresh_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                    }
                }
            }

        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.KPPShipments;
            if (current !=null)
            {
                MessageBoxResult result = MessageBox.Show("Сменить статус водителя " + current.FIO + " на Покинул склад?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                        {
                            var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                            temp.IdTimeNavigation.DateTimeLeft = DateTime.Now;
                            ef.SaveChanges();
                            btn_refresh_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                    }
                }
            }
        }

        private void btn_adddriver_Click(object sender, RoutedEventArgs e)
        {
            LoadDrvAndContragents();
            ClearTextboxes();
            cb_drivers.Text = "";
            ContentAdd.ShowAsync();
        }

        void ClearTextboxes()
        {
            tb_autonum.Clear();
            tb_info.Clear();
            tb_passport.Clear();
            tb_phone.Clear();
            tb_attorney.Clear();
        }

        private void btn_registration_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.PlanShipment;
            if (current !=null)
            {
                MessageBoxResult result = MessageBox.Show("Сменить статус водителя " + current.FIO + " на Зарегистрирован?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                        {
                            var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                            temp.IdTimeNavigation.DateTimeFactRegist = DateTime.Now;
                            ef.SaveChanges();
                            btn_refresh_Click(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                    }
                }
            }

        }

        private void btn_add_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
            btn_refresh_Click(sender, e);
        }

        private void btn_add_add_Click(object sender, RoutedEventArgs e)
        {

            if (cb_drivers.Text == "")
            {
                return;
            }
            
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    DB.Shipment shipment = new DB.Shipment();

                    if (cb_drivers.SelectedItem != null)
                    {
                        var test = cb_drivers as ComboBox;
                        var current = test.SelectedItem as Models.Drivers;
                        shipment.IdDriver = current.IdDriver;
                    }
                    else
                    {
                        //Если водителя нет в списках
                        DB.Driver driver = new DB.Driver();
                        shipment.IdDriverNavigation = driver;
                        var temp = SplitNames(cb_drivers.Text + " ");

                        driver.Name = temp.name.Replace(" ", "");
                        driver.Family = temp.family.Replace(" ", "");
                        driver.Patronymic = temp.patronomyc.Replace(" ", "");
                        driver.AutoNumber = tb_autonum.Text;
                        driver.Attorney = tb_attorney.Text;
                        driver.Phone = tb_phone.Text;
                        driver.AutoNumber = tb_autonum.Text;
                        driver.Passport = tb_passport.Text;
                        driver.Active = "1";
                        driver.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил водителя";
                    }

                    DB.Time time = new DB.Time();
                    time.DateTimeFactRegist = DateTime.Now;

                    shipment.IdTimeNavigation = time;

                    shipment.Description = tb_info.Text;
                    shipment.Active = "1";
                    shipment.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил отгрузку";

                    ef.Add(shipment);
                    ef.SaveChanges();
                    ContentAdd.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*
            if (tb_family.Text != "")
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
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }
            }
            else
            {
                MessageBox.Show("Введите хотябы фамилию водителя!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }


        //Разбив фио
        static (string family, string name, string patronomyc) SplitNames(string FullName)
        {
            var partsName = FullName.Split(' ');
            return (partsName[0], partsName[1], partsName[2]);
        }

        private void cb_drivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = cb_drivers as ComboBox;
            var current = test.SelectedItem as Models.Drivers;

            if (current != null)
            {
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var temp = ef.Drivers.FirstOrDefault(x => x.IdDriver == current.IdDriver);

                        //tb_contragent.Text = temp.IdContragentNavigation?.Name;
                        tb_phone.Text = temp.Phone;
                        tb_autonum.Text = temp.AutoNumber;
                        tb_passport.Text = temp.Passport;
                        tb_attorney.Text = temp.Attorney;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ClearTextboxes();
            }

        }

        void LoadDrvAndContragents()
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    Controllers.DriversController driver = new Controllers.DriversController();

                    cb_drivers.ItemsSource = driver.GetDriversСurrent();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            ContentSeach.ShowAsync();
        }
    }
}
