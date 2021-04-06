using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Registrant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        Pages.PageContragents pageContragents;
        Pages.PageKPP pageKPP;
        Pages.PageDrivers pageDrivers;
        Pages.PageShipments pageShipments;
        public MainWindow()
        {
            InitializeComponent();

            tb_login.Text = Settings.User.Default.login;
            tb_password.Password = Settings.User.Default.password;


            Thread thread = new Thread(TestConnect);
            thread.Start();

        }


        private void btn_debug_Click(object sender, RoutedEventArgs e)
        {
            text_error.Visibility = Visibility.Visible;
        }

        private void btn_tryconnect_Click(object sender, RoutedEventArgs e)
        {
            btn_tryconnect.Visibility = Visibility.Collapsed;
            ContentError.Hide();
            Thread thread1 = new Thread(TestConnect);
            thread1.Start();
        }


        void  TestConnect()
        {
            Dispatcher.Invoke(() => ContentWait.ShowAsync());

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var Test = ef.Engines.ToList();
                    Dispatcher.Invoke(() => ContentWait.Hide());
                    Dispatcher.Invoke(() => ContentAuth.ShowAsync());
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => ContentWait.Hide());
                Dispatcher.Invoke(() => ContentError.ShowAsync());
                Dispatcher.Invoke(() => text_error.Text = ex.ToString());
            }

        }

        private void btn_opensettings_Click(object sender, RoutedEventArgs e)
        {
            Forms.EditConnect edit = new Forms.EditConnect();
            edit.ShowDialog();
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            Settings.User.Default.login = tb_login.Text;
            Settings.User.Default.password = tb_password.Password;
            Settings.User.Default.Save();

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {

                    var user = ef.Users.Where(x => tb_login.Text == x.Login && tb_password.Password == x.Password).FirstOrDefault();

                    if (user != null)
                    {
                        App.SetActiveUser(user.Login);
                        App.SetLevelAccess(user.LevelAccess);
                        ContentAuth.Hide();
                        Verify();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void Verify()
        {
            if (App.LevelAccess == "admin")
            {
                nav_admin.Visibility = Visibility.Visible;
                nav_contragents.Visibility = Visibility.Visible;
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalkpp.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_jurnalsklad.Visibility = Visibility.Visible;

                //
                pageKPP = new Pages.PageKPP();
                pageContragents = new Pages.PageContragents();
                pageDrivers = new Pages.PageDrivers();
                pageShipments = new Pages.PageShipments();

            }
            else if (App.LevelAccess == "reader")
            {
                nav_contragents.Visibility = Visibility.Visible;
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_jurnalsklad.Visibility = Visibility.Visible;
            }
            else if (App.LevelAccess == "warehouse")
            {
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_jurnalsklad.Visibility = Visibility.Visible;
            }
            else if (App.LevelAccess == "shipment")
            {
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_jurnalsklad.Visibility = Visibility.Visible;
                nav_contragents.Visibility = Visibility.Visible;
                nav_drivers.Visibility = Visibility.Visible;
            }
            else if (App.LevelAccess == "kpp")
            {
                nav_jurnalkpp.Visibility = Visibility.Visible;
            }
        }

        private void nav_jurnalkpp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageKPP;
        }

        private void nav_contragents_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageContragents;
        }

        private void nav_drivers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageDrivers;
        }

        private void nav_jurnalshipment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageShipments;
        }

        private void nav_jurnalsklad_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void nav_admin_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
