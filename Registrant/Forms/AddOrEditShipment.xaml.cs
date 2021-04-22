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
using System.Windows.Shapes;

namespace Registrant.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditShipment.xaml
    /// </summary>
    public partial class AddOrEditShipment
    {
        /// Новая отгрузка
        public AddOrEditShipment()
        {
            InitializeComponent();
            
            text_title.Text = "Добавление отгрузки";
            btn_edit.Visibility = Visibility.Collapsed;
            btn_delete.Visibility = Visibility.Collapsed;
            LoadDrvAndContragents();
        }

        //Разбив фио
        static (string family, string name, string patronomyc) SplitNames(string FullName)
        {
            var partsName = FullName.Split(' ');
            return (partsName[0], partsName[1], partsName[2]);
        }

        /// Добавить соответственно
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (cb_drivers.Text != "")
            {
                if (cb_contragent.Text != "")
                {
                    if (dt_plan.Value != null)
                    {
                        try
                        {
                            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                            {
                                DB.Shipment shipment = new DB.Shipment();

                                if (cb_drivers.SelectedItem != null)
                                {
                                    var test = cb_drivers as ComboBox;
                                    var current = test.SelectedItem as Models.Drivers;
                                    shipment.IdDriver = current.IdDriver;
                                }
                                else
                                {
                                    //Если водителя нет в списках
                                    DB.Driver driver = new DB.Driver();
                                    shipment.IdDriverNavigation = driver;
                                    var temp = SplitNames(cb_drivers.Text + " ");

                                    driver.Name = temp.name.Replace(" ", "");
                                    driver.Family = temp.family.Replace(" ", "");
                                    driver.Patronymic = temp.patronomyc.Replace(" ", "");
                                    driver.AutoNumber = tb_autonum.Text;
                                    driver.Attorney = tb_attorney.Text;
                                    driver.Phone = tb_phone.Text;
                                    driver.Active = "1";
                                    driver.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил водителя";
                                }

                                if (cb_contragent.SelectedItem != null)
                                {
                                    var test = cb_contragent as ComboBox;
                                    var current = test.SelectedItem as DB.Contragent;
                                    shipment.IdContragent = current.IdContragent;
                                }
                                else
                                {
                                    DB.Contragent contragent = new DB.Contragent();
                                    contragent.Name = cb_contragent.Text;
                                    contragent.Active = "1";
                                    contragent.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил контрагента";
                                    shipment.IdContragentNavigation = contragent;
                                }


                                DB.Time time = new DB.Time();
                                shipment.IdTimeNavigation = time;

                                if (dt_plan.Value != null)
                                {
                                    time.DateTimePlanRegist = dt_plan.Value;
                                }
                                if (dt_fact.Value != null)
                                {
                                    time.DateTimeFactRegist = dt_fact.Value;
                                }
                                if (dt_arrive.Value != null)
                                {
                                    time.DateTimeArrive = dt_arrive.Value;
                                }
                                if (dt_load.Value != null)
                                {
                                    time.DateTimeLoad = dt_load.Value;
                                }
                                if (dt_endload.Value != null)
                                {
                                    time.DateTimeEndLoad = dt_endload.Value;
                                }
                                if (dt_left.Value != null)
                                {
                                    time.DateTimeLeft = dt_left.Value;
                                }

                                if (tb_numrealese.Text != null)
                                {
                                    shipment.NumRealese = tb_numrealese.Text;
                                }
                                if (tb_packetdoc.Text != null)
                                {
                                    shipment.PacketDocuments = tb_packetdoc.Text;
                                }
                                if (tb_tochkaload.Text != null)
                                {
                                    shipment.TochkaLoad = tb_tochkaload.Text;
                                }
                                if (tb_CountPodons.Text != null)
                                {
                                    shipment.CountPodons = tb_CountPodons.Text;
                                }
                                if (tb_nomencluture.Text != null)
                                {
                                    shipment.Nomenclature = tb_nomencluture.Text;
                                }
                                if (tb_size.Text != null)
                                {
                                    shipment.Size = tb_size.Text;
                                }
                                if (tb_Destination.Text != null)
                                {
                                    shipment.Destination = tb_Destination.Text;
                                }
                                if (tb_typeload.Text != null)
                                {
                                    shipment.TypeLoad = tb_typeload.Text;
                                }
                                if (tb_descript.Text != null)
                                {
                                    shipment.Description = tb_descript.Text;
                                }
                                if (tb_storekeeper.Text != null)
                                {
                                    shipment.StoreKeeper = tb_storekeeper.Text;
                                }

                                shipment.Active = "1";
                                shipment.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил отгрузку";


                                ef.Add(shipment);
                                ef.SaveChanges();
                                Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Дата плановой загрузки не заполнена!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Контрагент не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Водитель не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// Выбор водителя
        private void cb_drivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = cb_drivers as ComboBox;
            var current = test.SelectedItem as Models.Drivers;

            if (current != null)
            {
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var temp = ef.Drivers.FirstOrDefault(x => x.IdDriver == current.IdDriver);

                        //tb_contragent.Text = temp.IdContragentNavigation?.Name;
                        tb_phone.Text = temp.Phone;
                        tb_autonum.Text = temp.AutoNumber;
                        tb_attorney.Text = temp.Attorney;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //cb_contragent.Text = null;
                tb_phone.Text = null;
                tb_autonum.Text = null;
            }
            
        }

        /// Подгрузка водителей
        void LoadDrvAndContragents()
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    Controllers.DriversController driver = new Controllers.DriversController();

                    cb_drivers.ItemsSource = driver.GetDriversСurrent();
                    cb_contragent.ItemsSource = ef.Contragents.Where(x => x.Active != "0").OrderBy(x => x.Name).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// Редактирование отгрузок
        public AddOrEditShipment(int id)
        {
            InitializeComponent();
            //LoadDriversBox();
            btn_add.Visibility = Visibility.Collapsed;
            idcont.Text = id.ToString();

            if (App.LevelAccess == "shipment")
            {
                dt_load.IsEnabled = false;
                dt_endload.IsEnabled = false;
                tb_CountPodons.IsEnabled = false;
                tb_size.IsEnabled = false;
                tb_nomencluture.IsEnabled = false;
                tb_Destination.IsEnabled = false;
                tb_typeload.IsEnabled = false;
                tb_storekeeper.IsEnabled = false;
                tb_descript.IsEnabled = true;
            }
            else if (App.LevelAccess == "warehouse")
            {
                dt_plan.IsEnabled = false;
                dt_fact.IsEnabled = false;
                dt_arrive.IsEnabled = false;
                dt_left.IsEnabled = false;

                tb_numrealese.IsEnabled = false;
                tb_packetdoc.IsEnabled = false;
                tb_tochkaload.IsEnabled = false;
                tb_typeload.IsEnabled = true;

                btn_delete.Visibility = Visibility.Collapsed;
            }
            else if (App.LevelAccess == "admin")
            {
            }
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == id);

                    Controllers.DriversController driver = new Controllers.DriversController();
                    cb_drivers.ItemsSource = driver.GetDriversСurrent((int)temp.IdDriver);
                    cb_drivers.SelectedItem = driver.Driver.FirstOrDefault(x => x.IdDriver == temp.IdDriver);

                    cb_contragent.ItemsSource = ef.Contragents.Where(x => x.Active != "0" || (x.IdContragent == temp.IdContragent)).OrderBy(x => x.Name).ToList();
                    cb_contragent.SelectedItem = ef.Contragents.FirstOrDefault(x => x.IdContragent == temp.IdContragent);

                    dt_plan.Value = temp.IdTimeNavigation?.DateTimePlanRegist;
                    dt_fact.Value = temp.IdTimeNavigation?.DateTimeFactRegist;
                    dt_arrive.Value = temp.IdTimeNavigation?.DateTimeArrive;
                    dt_load.Value = temp.IdTimeNavigation?.DateTimeLoad;
                    dt_endload.Value = temp.IdTimeNavigation?.DateTimeEndLoad;
                    dt_left.Value = temp.IdTimeNavigation?.DateTimeLeft;

                    tb_numrealese.Text = temp.NumRealese;
                    tb_packetdoc.Text = temp.PacketDocuments;
                    tb_tochkaload.Text = temp.TochkaLoad;


                    tb_CountPodons.Text = temp.CountPodons;
                    tb_nomencluture.Text = temp.Nomenclature;
                    tb_size.Text = temp.Size;
                    tb_Destination.Text = temp.Destination;
                    tb_typeload.Text = temp.TypeLoad;
                    tb_descript.Text = temp.Description;
                    tb_storekeeper.Text = temp.StoreKeeper;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error); }

            if (dt_plan.Value != null)
            {
                text_title.Text = "Отгрузка №" + id + " от " + dt_plan.Value;
            }
            else
            {
                text_title.Text = "Отгрузка №" + id;
            }
        }

        /// Кнопка редактировать
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (App.LevelAccess == "shipment")
            {
                if (cb_drivers.Text != "")
                {
                    if (cb_contragent.Text != "")
                    {
                        try
                        {
                            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                            {
                                var shipment = ef.Shipments.FirstOrDefault(x => x.IdShipment == Convert.ToInt64(idcont.Text));

                                if (cb_drivers.SelectedItem != null)
                                {
                                    var test = cb_drivers as ComboBox;
                                    var current = test.SelectedItem as Models.Drivers;
                                    shipment.IdDriver = current.IdDriver;
                                }
                                else
                                {

                                    //Если водителя нет в списках
                                    DB.Driver driver = new DB.Driver();
                                    shipment.IdDriverNavigation = driver;
                                    var temp = SplitNames(cb_drivers.Text + " ");

                                    driver.Name = temp.name.Replace(" ", "");
                                    driver.Family = temp.family.Replace(" ", "");
                                    driver.Patronymic = temp.patronomyc.Replace(" ", "");
                                    driver.AutoNumber = tb_autonum.Text;
                                    driver.Attorney = tb_attorney.Text;
                                    driver.Phone = tb_phone.Text;
                                    driver.Active = "1";
                                    driver.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил водителя";
                                }

                                if (cb_contragent.SelectedItem != null)
                                {
                                    var test = cb_contragent as ComboBox;
                                    var current = test.SelectedItem as DB.Contragent;
                                    shipment.IdContragent = current.IdContragent;
                                }
                                else
                                {
                                    DB.Contragent contragent = new DB.Contragent();
                                    contragent.Name = cb_contragent.Text;
                                    contragent.Active = "1";
                                    contragent.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил контрагента";
                                    shipment.IdContragentNavigation = contragent;
                                }



                                if (dt_plan.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimePlanRegist = dt_plan.Value;
                                }
                                if (dt_fact.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeFactRegist = dt_fact.Value;
                                }
                                if (dt_arrive.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeArrive = dt_arrive.Value;
                                }

                                if (dt_left.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeLeft = dt_left.Value;
                                }
                                if (tb_numrealese.Text != "")
                                {
                                    shipment.NumRealese = tb_numrealese.Text;
                                }
                                if (tb_packetdoc.Text != "")
                                {
                                    shipment.PacketDocuments = tb_packetdoc.Text;
                                }
                                if (tb_typeload.Text != "")
                                {
                                    shipment.TypeLoad = tb_typeload.Text;
                                }
                                if (tb_tochkaload.Text != "")
                                {
                                    shipment.TochkaLoad = tb_tochkaload.Text;
                                }
                                if (tb_descript.Text != "")
                                {
                                    shipment.Description = tb_descript.Text;
                                }

                                

                                shipment.ServiceInfo = shipment.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " внес изменения в отгрузку";
                                ef.SaveChanges();
                                Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Контрагент не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Водитель не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (App.LevelAccess == "warehouse")
            {
                try
                {
                    using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                    {
                        var shipment = ef.Shipments.FirstOrDefault(x => x.IdShipment == Convert.ToInt64(idcont.Text));
                        
                        if (dt_load.Value != null)
                        {
                            shipment.IdTimeNavigation.DateTimeLoad = dt_load.Value;
                        }
                        if (dt_endload.Value != null)
                        {
                            shipment.IdTimeNavigation.DateTimeEndLoad = dt_endload.Value;
                        }
                        if (tb_CountPodons.Text != "")
                        {
                            shipment.CountPodons = tb_CountPodons.Text;
                        }
                        if (tb_nomencluture.Text != "")
                        {
                            shipment.Nomenclature = tb_nomencluture.Text;
                        }
                        if (tb_size.Text != "")
                        {
                            shipment.Size = tb_size.Text;
                        }
                        if (tb_Destination.Text != "")
                        {
                            shipment.Destination = tb_Destination.Text;
                        }
                        if (tb_typeload.Text != "")
                        {
                            shipment.TypeLoad = tb_typeload.Text;
                        }
                        if (tb_descript.Text != null)
                        {
                            shipment.Description = tb_descript.Text;
                        }
                        if (tb_storekeeper.Text != "")
                        {
                            shipment.StoreKeeper = tb_storekeeper.Text;
                        }
                        else
                        {
                            shipment.StoreKeeper = App.ActiveUser;
                        }

                        //shipment.StoreKeeper = App.ActiveUser;
                        shipment.ServiceInfo = shipment.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " внес изменения в отгрузку";
                        ef.SaveChanges();
                        Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if (App.LevelAccess == "admin")
            {
                if (cb_drivers.Text != "")
                {
                    if (cb_contragent.Text != "")
                    {
                        try
                        {
                            using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                            {
                                var shipment = ef.Shipments.FirstOrDefault(x => x.IdShipment == Convert.ToInt64(idcont.Text));

                                if (cb_drivers.SelectedItem != null)
                                {
                                    var test = cb_drivers as ComboBox;
                                    var current = test.SelectedItem as Models.Drivers;
                                    shipment.IdDriver = current.IdDriver;
                                }
                                else
                                {

                                    //Если водителя нет в списках
                                    DB.Driver driver = new DB.Driver();
                                    shipment.IdDriverNavigation = driver;
                                    var temp = SplitNames(cb_drivers.Text + " ");

                                    driver.Name = temp.name.Replace(" ", "");
                                    driver.Family = temp.family.Replace(" ", "");
                                    driver.Patronymic = temp.patronomyc.Replace(" ", "");
                                    driver.AutoNumber = tb_autonum.Text;
                                    driver.Attorney = tb_attorney.Text;
                                    driver.Phone = tb_phone.Text;
                                    driver.Active = "1";
                                    driver.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил водителя";
                                }

                                if (cb_contragent.SelectedItem != null)
                                {
                                    var test = cb_contragent as ComboBox;
                                    var current = test.SelectedItem as DB.Contragent;
                                    shipment.IdContragent = current.IdContragent;
                                }
                                else
                                {
                                    DB.Contragent contragent = new DB.Contragent();
                                    contragent.Name = cb_contragent.Text;
                                    contragent.Active = "1";
                                    contragent.ServiceInfo = DateTime.Now + " " + App.ActiveUser + " добавил контрагента";
                                    shipment.IdContragentNavigation = contragent;
                                }

                                if (dt_plan.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimePlanRegist = dt_plan.Value;
                                }
                                if (dt_fact.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeFactRegist = dt_fact.Value;
                                }
                                if (dt_arrive.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeArrive = dt_arrive.Value;
                                }
                                if (dt_load.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeLoad = dt_load.Value;
                                }
                                if (dt_endload.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeEndLoad = dt_endload.Value;
                                }
                                if (dt_left.Value != null)
                                {
                                    shipment.IdTimeNavigation.DateTimeLeft = dt_left.Value;
                                }

                                if (tb_numrealese.Text != "")
                                {
                                    shipment.NumRealese = tb_numrealese.Text;
                                }
                                if (tb_packetdoc.Text != "")
                                {
                                    shipment.PacketDocuments = tb_packetdoc.Text;
                                }
                                if (tb_tochkaload.Text != "")
                                {
                                    shipment.TochkaLoad = tb_tochkaload.Text;
                                }
                                if (tb_CountPodons.Text != "")
                                {
                                    shipment.CountPodons = tb_CountPodons.Text;
                                }
                                if (tb_nomencluture.Text != "")
                                {
                                    shipment.Nomenclature = tb_nomencluture.Text;
                                }
                                if (tb_size.Text != "")
                                {
                                    shipment.Size = tb_size.Text;
                                }
                                if (tb_Destination.Text != "")
                                {
                                    shipment.Destination = tb_Destination.Text;
                                }
                                if (tb_typeload.Text != "")
                                {
                                    shipment.TypeLoad = tb_typeload.Text;
                                }
                                if (tb_descript.Text != "")
                                {
                                    shipment.Description = tb_descript.Text;
                                }
                                if (tb_storekeeper.Text != "")
                                {
                                    shipment.StoreKeeper = tb_storekeeper.Text;
                                }

                                shipment.Active = "1";
                                shipment.ServiceInfo = shipment.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " внес изменения в отгрузку";
                                ef.SaveChanges();
                                Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Программное исключене", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Контрагент не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Водитель не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            ContentConfirmDel.ShowAsync();
        }

        private void btn_del_yes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (DB.RegistrantCoreContext ef = new DB.RegistrantCoreContext())
                {
                    var temp = ef.Shipments.FirstOrDefault(x => x.IdShipment == Convert.ToInt64(idcont.Text));
                    temp.Active = "0";
                    temp.Description = temp.Description + " " + tb_reasofordel.Text;
                    temp.ServiceInfo = temp.ServiceInfo + "\n" + DateTime.Now + " " + App.ActiveUser + " удалил отгрузку";
                    ef.SaveChanges();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_del_close_Click(object sender, RoutedEventArgs e)
        {
            ContentConfirmDel.Hide();
        }
    }
}
