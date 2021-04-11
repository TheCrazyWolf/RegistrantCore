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
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public PageUser(int id)
        {
            InitializeComponent();

            tb_refresher.Text = Settings.App.Default.RefreshContent.ToString();
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
                        MessageBox.Show("Пароль не совпадает со старым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Новый логин не должен быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ContentSave.Hide();
        }
    }
}
