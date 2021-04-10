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
            //Environment.Exit(0);
            controller = new Controllers.ShipmentController();
            DatePicker.SelectedDate = DateTime.Now;

            if (App.LevelAccess == "admin" || App.LevelAccess == "shipment")
            {
                btn_new.Visibility = Visibility.Visible;
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
                catch (Exception)
                {

                    throw;
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentAddEdit.ShowAsync();
            Forms.AddOrEditShipment addOrEditShipment = new Forms.AddOrEditShipment();
            addOrEditShipment.ShowDialog();
            ContentAddEdit.Hide();

        }

        private void ContentAddEdit_Closing(ModernWpf.Controls.ContentDialog sender, ModernWpf.Controls.ContentDialogClosingEventArgs args)
        {

        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                    temp.IdTimeNavigation.DateTimeLoad = DateTime.Now;
                    ef.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

            btn_refresh_Click(sender, e);
        }

        private void btn_endload_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;

            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == current.IdShipment);
                    temp.IdTimeNavigation.DateTimeEndLoad = DateTime.Now;
                    ef.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

            btn_refresh_Click(sender, e);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var bt = e.OriginalSource as Button;
            var current = bt.DataContext as Models.Shipments;

            Forms.AddOrEditShipment addOr = new Forms.AddOrEditShipment(current.IdShipment);
            addOr.ShowDialog();
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
        }


    }
}
