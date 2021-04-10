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
using Windows.UI.Xaml.Printing;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Windows.Xps;

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

        }

        private void btn_saveExcel_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Реестр"; // Default file name
            dlg.DefaultExt = ".xls"; // Default file extension
            dlg.Filter = "Говнофайлы (.xls)|*.xls"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result1 = dlg.ShowDialog();

            // Process save file dialog box results
            if (result1 == true)
            {
                // Save document
                string filename = dlg.FileName;
                grid_shipments.SelectAllCells();
                grid_shipments.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, grid_shipments);
                String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                String result = (string)Clipboard.GetData(DataFormats.Text);
                grid_shipments.UnselectAllCells();
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename, false, Encoding.Unicode);
                file.WriteLine(result.Replace(",", " "));
                file.Close();
            }


        }



        public void OnExportGridToCSV(object sender, System.EventArgs e)
        {
           
        }



    }
}
