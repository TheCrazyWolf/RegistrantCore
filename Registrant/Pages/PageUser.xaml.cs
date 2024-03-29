﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Registrant.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public PageUser(int id)
        {
            InitializeComponent();

            tb_refresher.Text = Settings.App.Default.RefreshContent.ToString();

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var user = ef.Users.FirstOrDefault(x => x.IdUser == id);
                    txt_user.Text = user.Name;
                    tb_name.Text = user.Name;
                    tb_id.Text = user.IdUser.ToString();
                    tb_login.Text = user.Login;
                    tb_role.Text = user.LevelAccess;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (tb_login.Text != "")
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var user = ef.Users.FirstOrDefault(x => x.IdUser == Convert.ToInt64(tb_id.Text));

                    if (tb_pass.Text == user.Password)
                    {
                        user.Login = tb_login.Text;
                        user.Password = tb_passnew.Text;
                        ef.SaveChanges();
                        ContentSave.ShowAsync();

                        tb_pass.Text = "";
                        tb_passnew.Text = "";
                    }
                    else
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = "Пароль не совпадает со старым";
                    }
                }
            }
            else
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = "Новый логин не должен быть пустым";
            }

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ContentSave.Hide();
        }

        private void btn_edit_connectstring_Click(object sender, RoutedEventArgs e)
        {
            Forms.EditConnect edit = new Forms.EditConnect();
            edit.ShowDialog();
        }
    }
}
