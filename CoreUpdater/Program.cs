using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;

namespace CoreUpdater
{
    class Program
    {
        private static string _actualVer { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("");
            Console.WriteLine("╦═╗┌─┐┌─┐┬┬─┐┌─┐┌┬┐┌─┐┌┐┌┌┬┐╔═╗┌─┐┬─┐┌─┐");
            Console.WriteLine("╠╦╝├┤ │ ┬│├┬┘└─┐ │ ├─┤│││ │ ║  │ │├┬┘├┤ ");
            Console.WriteLine("╩╚═└─┘└─┘┴┴└─└─┘ ┴ ┴ ┴┘└┘ ┴ ╚═╝└─┘┴└─└─┘");
            Console.WriteLine("");
            Console.WriteLine("============================================");
            Console.WriteLine("{0} > Registrant Core Updater", DateTime.Now);
            Console.WriteLine("{0} > v.0.1 by Alexey Kulagin (TheCrazyWolf)", DateTime.Now);
            Console.WriteLine("{0} > Инициализирован старт обновления", DateTime.Now);
            Console.WriteLine("");
            Console.WriteLine("");
            CheckVersion();
            DownloadPackage();
            Unpack();
            CleanUP();
            Console.WriteLine("{0} > Обновление завершено", DateTime.Now);
            Process.Start("Registrant.exe");
            Console.ReadKey();
        }

        public static void CheckVersion()
        {
            Console.WriteLine("{0} > Получение списка актуальных версии", DateTime.Now);
            try
            {
                WebClient web = new WebClient();
                _actualVer = web.DownloadString("https://raw.githubusercontent.com/TheCrazyWolf/RegistrantCore/master/Registrant/ActualVer.txt");
                Console.WriteLine("{0} > Последняя версия: {1}", DateTime.Now, _actualVer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} > ОШИБКА ОБНОВЛЕНИЯ", DateTime.Now);
                Console.WriteLine("{0} > Не удалось получить список актуальных версии программного обеспечения.", DateTime.Now);
                Console.ReadKey();
                Console.ReadKey();
            }

        }

        public static void DownloadPackage()
        {
            Console.WriteLine("{0} > Загрузка выбранной версии {1}", DateTime.Now, _actualVer);
            try
            {
                string url = "https://github.com/TheCrazyWolf/RegistrantCore/releases/download/" + _actualVer + "/package.zip";
                
                Console.WriteLine("{0} > Загрузка началась ({1})", DateTime.Now,url);
                WebClient web = new WebClient();
                web.DownloadFile(@url, @"package.zip");

                string package = @"package.zip";
                if (File.Exists(package))
                {
                    Console.WriteLine("{0} > Пакет загружен", DateTime.Now, url);
                }
                else
                {
                    Console.WriteLine("{0} > Что то пошло не так", DateTime.Now, url);
                    Console.ReadKey();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} > ОШИБКА ОБНОВЛЕНИЯ", DateTime.Now);
                Console.WriteLine("{0} > Не удалось получить список актуальных версии программного обеспечения.", DateTime.Now);
                Console.ReadKey();
                Console.ReadKey();
            }
        }

        public static void Unpack()
        {
            Console.WriteLine("");
            Console.WriteLine("{0} > ВНИМАНИЕ!", DateTime.Now);
            Console.WriteLine("{0} > Требуется подключение к интернету, во время обновления не пытайтесь", DateTime.Now);
            Console.WriteLine("{0} > закрыть это окно, это может привести к сбою обновления и может откразится на", DateTime.Now);
            Console.WriteLine("{0} > работе программы", DateTime.Now);
            Console.WriteLine("");
            Console.WriteLine("{0} > Убедитесь, что  RegistrantCore сейчас закрыт", DateTime.Now);
            Console.WriteLine("{0} > Ожидание", DateTime.Now);
            Thread.Sleep(3000);
            try
            {
                Console.WriteLine("{0} > Распаковка пакета и применение обновления", DateTime.Now);
                ZipArchiveExtensions.ExtractToDirectory(ZipFile.OpenRead("./package.zip"), "./", true);
                Console.WriteLine("{0} > Развертывание завершено", DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        public static void CleanUP()
        {
            Console.WriteLine("{0} > Очистка кеша", DateTime.Now);
            //File.Delete(@"package.zip");
            Console.WriteLine("{0} > Очистка завершена", DateTime.Now);
        }
    }
    public static class ZipArchiveExtensions
    {

        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }

    }
}
