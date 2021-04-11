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
        Pages.PageUser pageUser;

        public MainWindow()
        {
            InitializeComponent();

           //Подгрузка данных из настроек
            tb_login.Text = Settings.User.Default.login;
            //В проде убрать нижнее
            tb_password.Password = Settings.User.Default.password;

            //Поток наа 1 старт чтобы при старте не тормозилось
            Thread thread = new Thread(TestConnect);
            thread.Start();

        }


        /// <summary>
        /// Открытие дебага
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_debug_Click(object sender, RoutedEventArgs e)
        {
            text_error.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Кнопка повторить попытку соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tryconnect_Click(object sender, RoutedEventArgs e)
        {
            btn_tryconnect.Visibility = Visibility.Collapsed;
            ContentError.Hide();
            Thread thread1 = new Thread(TestConnect);
            thread1.Start();
        }


        /// <summary>
        /// Проверка существует ли вообще подключение к серверу
        /// </summary>
        void TestConnect()
        {
            Thread.Sleep(2000);
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

        /// <summary>
        /// Кнопка с редактированием настроек подключения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_opensettings_Click(object sender, RoutedEventArgs e)
        {
            Forms.EditConnect edit = new Forms.EditConnect();
            edit.ShowDialog();
        }

        /// <summary>
        /// Действие на нажатие на кнопку Войти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            Settings.User.Default.login = tb_login.Text;

            //В проде убрать! --- сохранение паролей
            Settings.User.Default.password = tb_password.Password;
            Settings.User.Default.Save();

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {

                    var user = ef.Users.Where(x => tb_login.Text == x.Login && tb_password.Password == x.Password).FirstOrDefault();

                    if (user != null)
                    {
                        ContentAuth.Hide();
                        App.SetActiveUser(user.Name);
                        App.SetLevelAccess(user.LevelAccess);
                        NavUI.PaneTitle = "РЕГИСТРАНТ (" + user.Name + ")";
                        nav_userset.Content = user.Name;
                        pageUser = new Pages.PageUser(user.IdUser);
                        Verify();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Проверяем кто он по масти
        /// </summary>
        void Verify()
        {
            if (App.LevelAccess == "admin")
            {
                nav_admin.Visibility = Visibility.Visible;
                nav_contragents.Visibility = Visibility.Visible;
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalkpp.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_userset.Visibility = Visibility.Visible;

                //
                pageKPP = new Pages.PageKPP();
                pageContragents = new Pages.PageContragents();
                pageDrivers = new Pages.PageDrivers();
                pageShipments = new Pages.PageShipments();

            }
            else if (App.LevelAccess == "reader")
            {
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_userset.Visibility = Visibility.Visible;

                pageDrivers = new Pages.PageDrivers();
                pageShipments = new Pages.PageShipments();

            }
            else if (App.LevelAccess == "warehouse")
            {
                nav_drivers.Visibility = Visibility.Visible;
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_userset.Visibility = Visibility.Visible;

                pageContragents = new Pages.PageContragents();
                pageDrivers = new Pages.PageDrivers();
                pageShipments = new Pages.PageShipments();

            }
            else if (App.LevelAccess == "shipment")
            {
                nav_jurnalshipment.Visibility = Visibility.Visible;
                nav_contragents.Visibility = Visibility.Visible;
                nav_drivers.Visibility = Visibility.Visible;
                nav_userset.Visibility = Visibility.Visible;

                pageContragents = new Pages.PageContragents();
                pageDrivers = new Pages.PageDrivers();
                pageShipments = new Pages.PageShipments();
            }
            else if (App.LevelAccess == "kpp")
            {
                nav_jurnalkpp.Visibility = Visibility.Visible;
                nav_userset.Visibility = Visibility.Visible;
                pageKPP = new Pages.PageKPP();
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

        private void nav_admin_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AcrylicWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void nav_userset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageUser;
        }
    }
}
