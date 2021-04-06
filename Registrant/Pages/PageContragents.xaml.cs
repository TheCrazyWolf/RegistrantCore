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
    /// Логика взаимодействия для PageContragents.xaml
    /// </summary>
    public partial class PageContragents : Page
    {
        public PageContragents()
        {
            InitializeComponent();

            FirstLoad();
        }

        void FirstLoad()
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.Where(x => x.Active != "0").OrderByDescending(x => x.IdContragent).ToList();
                    DataGrid_Contragents.ItemsSource = temp;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_add_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            ContentEdit.ShowAsync();
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.Contragent;
            text_editnamecontragent.Text = "Каскадное редактирование элемента " + current.Name;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == current.IdContragent);
                    tb_idcontragent.Text = temp.IdContragent.ToString();
                    tb_edit_name.Text = temp.Name.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.Contragent;

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == current.IdContragent);
                    temp.Active = "0";
                    temp.ServiceInfo = temp.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " изменил удалил";
                    ef.SaveChanges();
                    btn_refresh_Click(sender, e);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_Contragents.ItemsSource = null;
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.Where(x => x.Active != "0").OrderByDescending(x => x.IdContragent).ToList();
                    DataGrid_Contragents.ItemsSource = temp;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btn_addcontragent_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.ShowAsync();
            tb_namecontragent.Text = null;

        }

        private void btn_add_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    DB.Contragent contragent = new DB.Contragent();
                    contragent.Name = tb_namecontragent.Text;
                    contragent.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил контрагента";
                    contragent.Active = "1";
                    ef.Add(contragent);
                    ef.SaveChanges();
                    btn_refresh_Click(sender, e);
                    ContentAdd.Hide();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_add_close_Click_1(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }

        private void btn_save_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == Convert.ToInt64(tb_idcontragent.Text));
                    temp.Name = tb_edit_name.Text;
                    temp.ServiceInfo = temp.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " изменил название контрагента";
                    ef.SaveChanges();
                    btn_refresh_Click(sender, e);
                    ContentEdit.Hide();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_edit_close_Click(object sender, RoutedEventArgs e)
        {
            ContentEdit.Hide();
        }

        private void btn_info_close_Click(object sender, RoutedEventArgs e)
        {
            ContentInfo.Hide();
        }

        private void btn_info_Click(object sender, RoutedEventArgs e)
        {
            ContentInfo.ShowAsync();
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.Contragent;

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == current.IdContragent);
                    text_namecontragent.Text = temp.Name;
                    text_infocontragent.Text = temp.ServiceInfo;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
