using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        //Чтобы потом обратится при нажатие на кнопку
        Pages.PageContragents pageContragents;
        Pages.PageKPP pageKPP;
        Pages.PageDrivers pageDrivers;
        Pages.PageShipments pageShipments;
        Pages.PageUser pageUser;
        Pages.PageAdmin pageAdmin;

        public MainWindow()
        {
            InitializeComponent();

            //Подгрузка данных из настроек
            tb_login.Text = Settings.User.Default.login;
            text_verson.Text = Settings.App.Default.AppVersion;

            //Поток наа 1 старт чтобы при старте не тормозилось
            Thread thread = new Thread(CheckForUpdates);
            thread.Start();
        }

        /// Проверка существует ли вообще подключение к серверу
        void TestConnect()
        {
            //Thread.Sleep(2000);
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

        /// Проверяем кто он по масти
        void Verify()
        {
            if (App.LevelAccess == "admin")
            {
                Dispatcher.Invoke(() => nav_admin.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_contragents.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_drivers.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_jurnalkpp.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_jurnalshipment.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_userset.Visibility = Visibility.Visible);

                //Иниципализация нужных страниц под ролей
                Dispatcher.Invoke(() => pageKPP = new Pages.PageKPP());
                Dispatcher.Invoke(() => pageContragents = new Pages.PageContragents());
                Dispatcher.Invoke(() => pageDrivers = new Pages.PageDrivers());
                Dispatcher.Invoke(() => pageShipments = new Pages.PageShipments());
                Dispatcher.Invoke(() => pageAdmin = new Pages.PageAdmin());

                Dispatcher.Invoke(() => FrameContent.Content = pageShipments);
            }
            else if (App.LevelAccess == "reader")
            {
                Dispatcher.Invoke(() => nav_drivers.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_jurnalshipment.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_userset.Visibility = Visibility.Visible);

                Dispatcher.Invoke(() => pageDrivers = new Pages.PageDrivers());
                Dispatcher.Invoke(() => pageShipments = new Pages.PageShipments());

                Dispatcher.Invoke(() => FrameContent.Content = pageShipments);
            }
            else if (App.LevelAccess == "warehouse")
            {
                Dispatcher.Invoke(() => nav_drivers.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_jurnalshipment.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_userset.Visibility = Visibility.Visible);

                Dispatcher.Invoke(() => pageContragents = new Pages.PageContragents());
                Dispatcher.Invoke(() =>  pageDrivers = new Pages.PageDrivers());
                Dispatcher.Invoke(() => pageShipments = new Pages.PageShipments());

                Dispatcher.Invoke(() => FrameContent.Content = pageShipments);

            }
            else if (App.LevelAccess == "shipment")
            {
                Dispatcher.Invoke(() => nav_jurnalshipment.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_contragents.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_drivers.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_userset.Visibility = Visibility.Visible);

                Dispatcher.Invoke(() => pageContragents = new Pages.PageContragents());
                Dispatcher.Invoke(() => pageDrivers = new Pages.PageDrivers());
                Dispatcher.Invoke(() => pageShipments = new Pages.PageShipments());
                Dispatcher.Invoke(() => pageKPP = new Pages.PageKPP());
                Dispatcher.Invoke(() => nav_jurnalkpp.Visibility = Visibility.Visible);

                Dispatcher.Invoke(() => FrameContent.Content = pageShipments);
            }
            else if (App.LevelAccess == "kpp")
            {
                Dispatcher.Invoke(() => nav_jurnalkpp.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => nav_userset.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => pageKPP = new Pages.PageKPP());

                Dispatcher.Invoke(() => FrameContent.Content = pageKPP);
            }
        }

        void CheckForUpdates()
        {
            Thread.Sleep(1500);
            Dispatcher.Invoke(() => ContentCheckingUpdate.ShowAsync());
            WebClient web = new WebClient();
            try
            {
                string Act = web.DownloadString("https://raw.githubusercontent.com/TheCrazyWolf/RegistrantCore/master/Registrant/ActualVer.txt");
                string ActualText = web.DownloadString("https://raw.githubusercontent.com/TheCrazyWolf/RegistrantCore/master/Registrant/ActualTextDesc.txt");
                Act = Act.Replace("\n", "");
                Act = Act.Replace(".", ",");

                string currentstring = Settings.App.Default.AppVersion;
                currentstring = currentstring.Replace(".", ",");
                decimal Current = decimal.Parse(currentstring);
                decimal Actual = decimal.Parse(Act);
                Dispatcher.Invoke(() => ContentCheckingUpdate.Hide());

                if (Actual > Current)
                {
                    Dispatcher.Invoke(() => ContentUpdate.ShowAsync());
                    Dispatcher.Invoke(() => txt_currver.Text = Current.ToString());
                    Dispatcher.Invoke(() => txt_newver.Text = Act.ToString());
                    Dispatcher.Invoke(() => txt_desc.Text = ActualText);

                    string CanRefuse = web.DownloadString("https://raw.githubusercontent.com/TheCrazyWolf/RegistrantCore/master/Registrant/ActualVerCanRefuse.txt");
                    CanRefuse= CanRefuse.Replace("\n", "");

                    if (CanRefuse == "No")
                    {
                        Dispatcher.Invoke(() => btn_updatelate.Visibility = Visibility.Hidden);
                        Dispatcher.Invoke(() => txt_desc.Text = txt_desc.Text + "\n\nЭто обновление нельзя отложить, т.к. содержит\nкритические правки в коде");
                        Dispatcher.Invoke(() => ContentUpdate.Background = new SolidColorBrush(Color.FromRgb(255, 195, 195)));
                    }

                }
                else
                {
                    TestConnect();
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => ContentCheckingUpdate.Hide());
                TestConnect();
            }
        }


        /// Открытие дебага
        private void btn_debug_Click(object sender, RoutedEventArgs e)
        {
            text_error.Visibility = Visibility.Visible;
        }

        //Кнопка повторить попытку
        private void btn_tryconnect_Click(object sender, RoutedEventArgs e)
        {
            ContentError.Hide();
            Thread thread1 = new Thread(TestConnect);
            thread1.Start();
        }

        /// Кнопка с редактированием настроек подключения
        private void btn_opensettings_Click(object sender, RoutedEventArgs e)
        {
            Forms.EditConnect edit = new Forms.EditConnect();
            edit.ShowDialog();
        }

        /// Действие на нажатие на кнопку Войти
        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            Settings.User.Default.login = tb_login.Text;
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


                        Thread thread = new Thread(Verify);
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("Логин и/или пароль неверный", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => ContentWait.Hide());
                Dispatcher.Invoke(() => ContentError.ShowAsync());
                Dispatcher.Invoke(() => text_error.Text = ex.ToString());
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
            FrameContent.Content = pageAdmin;
        }

        private void AcrylicWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void nav_userset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameContent.Content = pageUser;
        }

        private void btn_about_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAppVer.Hide();
        }

        private void nav_aboutpo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentAppVer.ShowAsync();
        }

        private void btn_updatenow_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("CoreUpdater.exe");
            Environment.Exit(0);
        }

        private void btn_updatelate_Click(object sender, RoutedEventArgs e)
        {
            ContentUpdate.Hide();
            Thread thread = new Thread(TestConnect);
            thread.Start();
        }

        private void btn_debugger_close_Click(object sender, RoutedEventArgs e)
        {
            ContentErrorText.Hide();
        }
    }
}
