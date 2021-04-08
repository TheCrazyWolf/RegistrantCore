using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Registrant.Forms
{
    /// <summary>
    /// Логика взаимодействия для PrintShipments.xaml
    /// </summary>
    public partial class PrintShipments 
    {
        Controllers.PrintShipmentController controller;
        public PrintShipments()
        {
            InitializeComponent();
            controller = new Controllers.PrintShipmentController();
            DatePicker.SelectedDate = DateTime.Now;

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            grid_shipments.ItemsSource = null;
            grid_shipments.ItemsSource = controller.GetShipmentsDate(DatePicker.SelectedDate.Value);
        }

        private void btn_month_Click(object sender, RoutedEventArgs e)
        {
            grid_shipments.ItemsSource = null;
            grid_shipments.ItemsSource = controller.GetShipmentsMonth(DatePicker.SelectedDate.Value);
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            var pd = new PrintDialog();
            var result = pd.ShowDialog();
            if (result.HasValue && result.Value)
                pd.PrintVisual(grid_shipments, "My WPF printing a DataGrid");
        }

        private void btn_saveExcel_Click(object sender, RoutedEventArgs e)
        {
            
        }



        public void OnExportGridToCSV(object sender, System.EventArgs e)
        {
           
        }


    }
}
