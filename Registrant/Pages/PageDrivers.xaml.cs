using SourceChord.FluentWPF;
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
    /// Логика взаимодействия для PageDrivers.xaml
    /// </summary>
    public partial class PageDrivers : Page
    {
        Controllers.DriversController controller;

        public PageDrivers()
        {
            InitializeComponent();
            controller = new Controllers.DriversController();
            DataGrid_Drivers.ItemsSource = controller.GetDrivers();


            if (App.LevelAccess == "reader")
            {
                btn_add_driver.Visibility = Visibility.Collapsed;
                btn_delete_30day.Visibility = Visibility.Collapsed;
            }

            Thread thread = new Thread(new ThreadStart(RefreshThread));
            thread.Start();
        }

        void RefreshThread()
        {
            while (true)
            {
                Thread.Sleep(Settings.App.Default.RefreshContent);
                if (Dispatcher.Invoke(() => tb_search.Text == ""))
                {
                    Dispatcher.Invoke(() => DataGrid_Drivers.ItemsSource = controller.GetDrivers());
                    Dispatcher.Invoke(() => DataGrid_Drivers.Items.Refresh());
                }
            }
        }

        /// <summary>
        /// Кнопка обновить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_search.Text == "")
            {
                //DataGrid_Drivers.ItemsSource = null;
                DataGrid_Drivers.ItemsSource = controller.GetDrivers();
                DataGrid_Drivers.Items.Refresh();
            }
        }

        /// Кнопка закрытия диалогового окна с редактированием
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAddEdit.Hide();
            btn_edit.Visibility = Visibility.Collapsed;
            btn_add.Visibility = Visibility.Collapsed;
        }

        /// Кнопка добавления водителя
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    DB.Driver driver = new DB.Driver();
                    driver.Family = tb_Family.Text;
                    driver.Name = tb_name.Text;
                    driver.Patronymic = tb_patronomyc.Text;
                    driver.Phone = tb_phone.Text;

                    if (tb_contragent.SelectedItem != null)
                    {
                        var test = tb_contragent as ComboBox;
                        var current = test.SelectedItem as DB.Contragent;
                        if (current != null)
                        { driver.IdContragent = current.IdContragent; }
                    }
                    
                    driver.Attorney = tb_attorney.Text;
                    driver.Auto = tb_auto.Text;
                    driver.AutoNumber = tb_autonum.Text;
                    driver.Passport = tb_passport.Text;
                    driver.Info = tb_info.Text;
                    driver.Active = "1";
                    driver.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил водителя";
                    ef.Add(driver);
                    ef.SaveChanges();
                    btn_close_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
        }

        /// Кнопка открыть окно редактирования
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Drivers;
            ClearTextbox();

            btn_edit.Visibility = Visibility.Visible;
            btn_add.Visibility = Visibility.Collapsed;
            btn_delete.Visibility = Visibility.Visible;
            if (current != null)
            {
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var driver = ef.Drivers.FirstOrDefault(x => x.IdDriver == current.IdDriver);

                        text_namedriver.Text = driver.Family + " " + driver.Name + " " + driver.Patronymic;

                        tb_id.Text = driver.IdDriver.ToString();
                        tb_Family.Text = driver.Family;
                        tb_name.Text = driver.Name;
                        tb_patronomyc.Text = driver.Patronymic;
                        tb_phone.Text = driver.Phone;

                        tb_contragent.ItemsSource = ef.Contragents.Where(x => x.Active != "0").ToList();
                        tb_contragent.SelectedItem = ef.Contragents.FirstOrDefault(x => x.IdContragent == driver.IdContragent);

                        tb_attorney.Text = driver.Attorney;
                        tb_auto.Text = driver.Auto;
                        tb_autonum.Text = driver.AutoNumber;
                        tb_passport.Text = driver.Passport;
                        tb_info.Text = driver.Info;
                        ContentAddEdit.ShowAsync();

                    }
                }
                catch (Exception ex)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }
            }
        }


        /// Кнлпка удалить
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var driver = ef.Drivers.FirstOrDefault(x => x.IdDriver == Convert.ToInt64(tb_id.Text));
                    driver.Active = "0";
                    driver.ServiceInfo = driver.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " удалил водителя";
                    ef.SaveChanges();
                    ContentAddEdit.Hide();
                    btn_refresh_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
        }

        /// Кнопка добавить водителя
        private void btn_add_driver_Click(object sender, RoutedEventArgs e)
        {
            ContentAddEdit.ShowAsync();
            ClearTextbox();
            text_namedriver.Text = "Добавить нового водителя";

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    tb_contragent.ItemsSource = ef.Contragents.Where(x => x.Active != "0").ToList();
                    btn_add.Visibility = Visibility.Visible;
                    btn_delete.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
        }


        /// Очистка
        void ClearTextbox()
        {
            tb_id.Text = null;
            tb_Family.Text = null;
            tb_name.Text = null;
            tb_patronomyc.Text = null;
            tb_phone.Text = null;
            tb_contragent.ItemsSource = null;
            tb_attorney.Text = null;
            tb_auto.Text = null;
            tb_autonum.Text = null;
            tb_passport.Text = null;
            tb_info.Text = null;
        }

        /// Непосредственное редактирование
        private void btn_edit_Click1(object sender, RoutedEventArgs e)
        {
            
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var driver = ef.Drivers.FirstOrDefault(x => x.IdDriver == Convert.ToInt64(tb_id.Text));
                        driver.Family = tb_Family.Text;
                        driver.Name = tb_name.Text;
                        driver.Patronymic = tb_patronomyc.Text;
                        driver.Phone = tb_phone.Text;

                        if (tb_contragent.SelectedItem != null)
                        {
                            var test = tb_contragent as ComboBox;
                            var current = test.SelectedItem as DB.Contragent;
                            if (current !=null)
                            { driver.IdContragent = current.IdContragent; }
                        }
                        
                        driver.Attorney = tb_attorney.Text;
                        driver.Auto = tb_auto.Text;
                        driver.AutoNumber = tb_autonum.Text;
                        driver.Passport = tb_passport.Text;
                        driver.Info = tb_info.Text;
                        driver.ServiceInfo = driver.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " внес изменения";
                        ef.SaveChanges();
                        btn_close_Click(sender, e);
                        ContentAddEdit.Hide();
                        btn_refresh_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
            }
        }

        /// Закрытия окна с информацией
        private void btn_info_close_Click(object sender, RoutedEventArgs e)
        {
            ContentInfo.Hide();
        }

        /// Открыть окно с расширенной информацией
        private void btn_info_Click(object sender, RoutedEventArgs e)
        {

            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Drivers;

            if (current !=null)
            {

                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        ContentInfo.ShowAsync();
                        ContentInfoGrid.DataContext = ef.Drivers.FirstOrDefault(x => x.IdDriver == current.IdDriver);
                        text_info_namedriver.Text = current.FIO;
                    }
                }
                catch (Exception ex)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }

            }
        }


        private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_search.Text == "")
            {
                btn_refresh_Click(sender, e);
            }
            else
            {
                try
                {
                    DataGrid_Drivers.ItemsSource = null;

                    var temp = controller.GetDriversAll();

                    var data = temp.Where(t => t.FIO.ToUpper().StartsWith(tb_search.Text.ToUpper())).ToList();
                    var sDOP = temp.Where(t => t.FIO.ToUpper().Contains(tb_search.Text.ToUpper())).ToList();
                    data.AddRange(sDOP);
                    var noDupes = data.Distinct().ToList();
                    DataGrid_Drivers.ItemsSource = noDupes;

                    if (noDupes.Count == 0)
                    {
                        //Ничекго не нашел
                    }
                    else
                    {
                        // Нашел
                    }
                }
                catch (Exception ex)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }
            }

        }

        private void btn_delete_30day_Click(object sender, RoutedEventArgs e)
        {
            Content30days.ShowAsync();
        }

        private void btn_30d_close_Click(object sender, RoutedEventArgs e)
        {
            Content30days.Hide();
        }

        private void btn_30d_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    DateTime last30 = DateTime.Now.Date;
                    last30.AddDays(-30);
                    DateTime currentMonth = last30.AddDays(30);
                   // var temp = ef.Shipments.Where(x => (x.IdTimeNavigation.DateTimeLeft > last30) && x.IdDriverNavigation.Active != "0" && x.IdTimeNavigation.DateTimeFactRegist == currentMonth.GetDateTimeFormats.);
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
