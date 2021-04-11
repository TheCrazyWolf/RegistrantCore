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
    /// Логика взаимодействия для PageContragents.xaml
    /// </summary>
    public partial class PageContragents : Page
    {
        public PageContragents()
        {
            InitializeComponent();
            FirstLoad();

            Thread thread = new Thread(new ThreadStart(RefreshThread));
            thread.Start();
        }

        /// <summary>
        /// Обновление в потоке
        /// </summary>
        void RefreshThread()
        {
            while (true)
            {
                Thread.Sleep(Settings.App.Default.RefreshContent);
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var temp = ef.Contragents.Where(x => x.Active != "0").OrderByDescending(x => x.IdContragent).ToList();
                        Dispatcher.Invoke(() => DataGrid_Contragents.ItemsSource = temp);
                        Dispatcher.Invoke(() => DataGrid_Contragents.Items.Refresh());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Упс.. Что-то пошло не так\n\nВозможно произошел разрыв соединения с сервером\n\nОтладочная информация\n" + ex.ToString(), "Ошибка при выполнении кода", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Первый старт, подгрузка данных
        /// </summary>
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
            catch (Exception ex)
            {
                MessageBox.Show("Упс.. Что-то пошло не так\n\nВозможно произошел разрыв соединения с сервером\n\nОтладочная информация\n"+ ex.ToString(), "Ошибка при выполнении кода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Кнопка закрыть из диалг окна добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_close_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }

        /// <summary>
        /// Кнопка редактиировать из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.Contragent;

            if (current != null)
            {
                text_editnamecontragent.Text = "Редактирование элемента " + current.Name;
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == current.IdContragent);
                    tb_idcontragent.Text = temp.IdContragent.ToString();
                    tb_edit_name.Text = temp.Name.ToString();
                    ContentEdit.ShowAsync();
                }
            }
        }

        //Кнопка удалить
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

        //Обновить
        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.Where(x => x.Active != "0").OrderByDescending(x => x.IdContragent).ToList();
                    DataGrid_Contragents.ItemsSource = null;
                    DataGrid_Contragents.ItemsSource = temp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Упс.. Что-то пошло не так\n\nВозможно произошел разрыв соединения с сервером\n\nОтладочная информация\n" + ex.ToString(), "Ошибка при выполнении кода", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //Добавить контрагента, диалог окно
        private void btn_addcontragent_Click(object sender, RoutedEventArgs e)
        {
            ContentAdd.ShowAsync();
            tb_namecontragent.Text = null;

        }

        /// <summary>
        /// Кнопка добавление, реального добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Кнопка закрытия в добавление окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_close_Click_1(object sender, RoutedEventArgs e)
        {
            ContentAdd.Hide();
        }

        /// <summary>
        /// Кнопка сохранить изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //Кнопка закрыть окна редактирования
        private void btn_edit_close_Click(object sender, RoutedEventArgs e)
        {
            ContentEdit.Hide();
        }

        //Закрыть инф окно
        private void btn_info_close_Click(object sender, RoutedEventArgs e)
        {
            ContentInfo.Hide();
        }

        /// <summary>
        /// Показать инфу о контр агенте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_info_Click(object sender, RoutedEventArgs e)
        {
            ContentInfo.ShowAsync();
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as DB.Contragent;

            if (current != null)
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Contragents.FirstOrDefault(x => x.IdContragent == current.IdContragent);
                    text_namecontragent.Text = temp.Name;
                    text_infocontragent.Text = temp.ServiceInfo;
                }
            }
        }
    }
}
