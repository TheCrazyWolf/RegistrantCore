﻿using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EditConnect.xaml
    /// </summary>
    public partial class EditConnect
    {
        public EditConnect()
        {
            InitializeComponent();

            tb_ip.Text = Settings.Connection.Default.IP;
            tb_port.Text = Settings.Connection.Default.Port;
            tb_db.Text = Settings.Connection.Default.Database;
            tb_login.Text = Settings.Connection.Default.Login;
            tb_pass.Text = Settings.Connection.Default.Password;

        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Connection.Default.IP = tb_ip.Text;
            Settings.Connection.Default.Port = tb_port.Text;
            Settings.Connection.Default.Database = tb_db.Text;
            Settings.Connection.Default.Login = tb_login.Text;
            Settings.Connection.Default.Password = tb_pass.Text;
            Settings.Connection.Default.Save();
            this.Close();
        }
    }
}
