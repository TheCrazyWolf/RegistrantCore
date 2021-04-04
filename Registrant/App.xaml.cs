using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Registrant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ActiveUser;
        public static string LevelAccess;

        public App() { }
        
        public static void SetLevelAccess(string type)
        {
            LevelAccess = type;
        }
        public static string GetLevelAccess()
        {
            return LevelAccess;
        }
        
        public static string GetActiveUser()
        {
            return ActiveUser;
        }

        public static void SetActiveUser(string type)
        {
            ActiveUser = type;
        }

    }
}
