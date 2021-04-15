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
    /// Логика взаимодействия для PageShipments.xaml
    /// </summary>
    public partial class PageShipments : Page
    {
        Controllers.ShipmentController controller = new Controllers.ShipmentController();

        public PageShipments()
        {
            InitializeComponent();
            controller = new Controllers.ShipmentController();
            DatePicker.SelectedDate = DateTime.Now;

            Thread thread = new Thread(new ThreadStart(RefreshThread));
            thread.Start();

            if (App.LevelAccess == "admin" || App.LevelAccess == "shipment")
            {
                btn_new.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Потоко
        /// </summary>
        void RefreshThread()
        {
            while (true)
            {
                
                if (Dispatcher.Invoke(() => string.IsNullOrWhiteSpace(tb_search.Text)))
                {
                    if (Dispatcher.Invoke(() => DatePicker.SelectedDate != null))
                    {
                        if (Dispatcher.Invoke(() => cb_sort.SelectedIndex == 0))
                        {
                            Dispatcher.Invoke(() => DataGrid_Shipments.ItemsSource = controller.GetShipments(DatePicker.SelectedDate.Value));
                            Dispatcher.Invoke(() => DataGrid_Shipments.Items.Refresh());
                        }
                        else if (Dispatcher.Invoke(() => cb_sort.SelectedIndex == 1))
                        {
                            Dispatcher.Invoke(() => DataGrid_Shipments.ItemsSource = controller.GetShipmentsFactReg(DatePicker.SelectedDate.Value));
                            Dispatcher.Invoke(() => DataGrid_Shipments.Items.Refresh());
                        }
                        else if(Dispatcher.Invoke(() => cb_sort.SelectedIndex == 2))
                        {
                            Dispatcher.Invoke(() => DataGrid_Shipments.ItemsSource = controller.GetShipmentsArrive(DatePicker.SelectedDate.Value));
                            Dispatcher.Invoke(() => DataGrid_Shipments.Items.Refresh());
                        }
                        else if(Dispatcher.Invoke(() => cb_sort.SelectedIndex == 3))
                        {
                            Dispatcher.Invoke(() => DataGrid_Shipments.ItemsSource = controller.GetShipmentsLeft(DatePicker.SelectedDate.Value));
                            Dispatcher.Invoke(() => DataGrid_Shipments.Items.Refresh());
                        }
                    }
                }
                Thread.Sleep(Settings.App.Default.RefreshContent);
            }
        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid_Shipments.ItemsSource = null;
            Text_date.Text = "Реестр за " + DatePicker.SelectedDate.Value.ToShortDateString();
            btn_refresh_Click(sender, e);
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            
            if (tb_search.Text != null)
            {
                
                if (DatePicker.SelectedDate != null)
                {
                    DataGrid_Shipments.ItemsSource = null;
                    if (cb_sort.SelectedIndex == 0)
                    {
                        DataGrid_Shipments.ItemsSource = controller.GetShipments(DatePicker.SelectedDate.Value);
                    }
                    else if (cb_sort.SelectedIndex == 1)
                    {
                        DataGrid_Shipments.ItemsSource = controller.GetShipmentsFactReg(DatePicker.SelectedDate.Value);
                    }
                    else if (cb_sort.SelectedIndex == 2)
                    {
                        DataGrid_Shipments.ItemsSource = controller.GetShipmentsArrive(DatePicker.SelectedDate.Value);
                    }
                    else if (cb_sort.SelectedIndex == 3)
                    {
                        DataGrid_Shipments.ItemsSource = controller.GetShipmentsLeft(DatePicker.SelectedDate.Value);
                    }
                    else
                    {
                        DataGrid_Shipments.ItemsSource = controller.GetShipments(DatePicker.SelectedDate.Value);
                    }
                }
            }
        }

        private void cb_sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_refresh_Click(sender, e);
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
                    DataGrid_Shipments.ItemsSource = null;

                    var temp = controller.GetShipmentsAll();
                    var data = temp.Where(t => t.FIO.ToUpper().StartsWith(tb_search.Text.ToUpper())).ToList();
                    var sDOP = temp.Where(t => t.FIO.ToUpper().Contains(tb_search.Text.ToUpper())).ToList();
                    data.AddRange(sDOP);
                    var noDupes = data.Distinct().ToList();
                    DataGrid_Shipments.ItemsSource = noDupes;

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


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Forms.AddOrEditShipment addOrEditShipment = new Forms.AddOrEditShipment();
            addOrEditShipment.ShowDialog();
            btn_refresh_Click(sender, e);
        }


        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;

            if (current != null)
            {
                MessageBoxResult result = (MessageBoxResult)ModernWpf.MessageBox.Show("Сменить статус водителя " + current.FIO + " на Загрузка начата?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {

                    try
                    {
                        using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                        {
                            var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                            temp.IdTimeNavigation.DateTimeLoad = DateTime.Now;
                            ef.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                    }
                    
                }
            }

            btn_refresh_Click(sender, e);
        }

        private void btn_endload_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;

            if (current != null)
            {
                MessageBoxResult result = (MessageBoxResult)ModernWpf.MessageBox.Show("Сменить статус водителя " + current.FIO + " на Загрузка окончена?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {

                    try
                    {
                        using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                        {
                            var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                            temp.IdTimeNavigation.DateTimeEndLoad = DateTime.Now;
                            ef.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ContentErrorText.ShowAsync();
                        ((MainWindow)System.Windows.Application.Current.MainWindow).text_debuger.Text = ex.ToString();
                    }

                }
                   
            }

            btn_refresh_Click(sender, e);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;
            if (current !=null)
            {
                Forms.AddOrEditShipment addOr = new Forms.AddOrEditShipment(current.IdShipment);
                addOr.ShowDialog();
            }
            btn_refresh_Click(sender, e);
        }



        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            if (App.LevelAccess == "shipment")
            {
                Forms.PrintShipments print = new Forms.PrintShipments();
                print.ShowDialog();
            }
            else if (App.LevelAccess == "warehouse")
            {
                Forms.PrintWarehouse print = new Forms.PrintWarehouse();
                print.ShowDialog();
            }
            else
            {
                MessageBoxResult result = (MessageBoxResult)ModernWpf.MessageBox.Show("Вам открыть окно вид для сбыта?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (result == MessageBoxResult.Yes)
                {
                    Forms.PrintShipments print = new Forms.PrintShipments();
                    print.ShowDialog();
                }
                else
                {
                    Forms.PrintWarehouse print = new Forms.PrintWarehouse();
                    print.ShowDialog();
                }
            }
        }


    }
}
