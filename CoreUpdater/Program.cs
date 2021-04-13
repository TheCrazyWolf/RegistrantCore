using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace CoreUpdater
{
    class Program
    {
        private string _actualVer { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Registrant Core Updater");
            Console.WriteLine("Checking for updates");
        }

        public void CheckVersion()
        {
            WebClient web = new WebClient();//Для использования WebClient подключи System.Net 
            _actualVer = web.DownloadString("Адрес к txt файлу");
        }
    }
}
