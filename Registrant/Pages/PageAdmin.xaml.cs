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
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        public PageAdmin()
        {
            InitializeComponent();
            LoadUser();
        }

        void LoadUser()
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    DataGrid_Users.ItemsSource = ef.Users.OrderBy(x => x.IdUser).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_deluser_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.User;

            if (current != null)
            {

                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        ef.Remove(current);
                        ef.SaveChanges();
                        LoadUser();
                    }
                    ContentSave.ShowAsync();
                }
                catch (Exception ex)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }
            }
        }

        private void btn_info_close_Click(object sender, RoutedEventArgs e)
        {
            ContentSave.Hide();
        }

        private void btn_add_add_Click(object sender, RoutedEventArgs e)
        {
            ContentAddUser.Hide();
            if (tb_login.Text != "")
            {
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        
                        DB.User user = new DB.User();
                        user.Name = tb_name.Text;
                        user.Login = tb_login.Text;
                        user.Password = tb_pass.Text;
                        if (cb_access.SelectedIndex == 0)
                        {
                            user.LevelAccess = "kpp";
                        }
                        else if (cb_access.SelectedIndex == 1)
                        {
                            user.LevelAccess = "reader";
                        }
                        else if (cb_access.SelectedIndex == 2)
                        {
                            user.LevelAccess = "warehouse";
                        }
                        else if (cb_access.SelectedIndex == 3)
                        {
                            user.LevelAccess = "shipment";
                        }
                        else if (cb_access.SelectedIndex == 4)
                        {
                            user.LevelAccess = "admin";
                        }
                        ef.Add(user);
                        ef.SaveChanges();
                        LoadUser();
                    }
                }
                catch (Exception ex)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                }
                ContentSave.ShowAsync();
            }
        }

        private void btn_add_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAddUser.Hide();
        }


        private void btn_showaddwindow_Click(object sender, RoutedEventArgs e)
        {
            tb_login.Text = "";
            tb_pass.Text = "";
            tb_name.Text = "";
            ContentAddUser.ShowAsync();
        }
    }
}
