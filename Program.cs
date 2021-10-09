using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Specialized;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Amib.Threading;
using System.Net;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SPOTIFYFINAL
{
    class Program
    {
        public static Configuration currentConfig;
        static DateTime start;
        static SmartThreadPool _smartThreadPool;
        public static IEnumerator<int> port_genrator = helper.portport().GetEnumerator();
        public static IEnumerator<string> UserData = CycleFile(constant.UserData).GetEnumerator();
        public static IEnumerator<string> GEN_DOMAIN = CycleFile(constant.GEN_DOMAIN).GetEnumerator();
        public static IEnumerator<string> proxy_info = CycleFile(constant.proxy_info).GetEnumerator();
        public static IEnumerator<string> stream_proxy = CycleFile(constant.stream_proxy).GetEnumerator();
        

        static void Main()
        {
            ASCII.ASCIII();
            
            makeConfig();

            new Thread(initialiseThreads).Start();
            while (true)
            {
                // Console.Clear();
                // ASCII.ASCIII();
                Thread.Sleep(5000);
            }
        }
        
        private static IEnumerable<string> CycleFile(string fileName)
        {
            while (true) foreach(var line in File.ReadLines(fileName)) yield return line;
        }
        static void Initialise()
        {
            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = 1000;
            stpStartInfo.MaxWorkerThreads = Convert.ToInt32(50);
            stpStartInfo.MinWorkerThreads = Convert.ToInt32(1);
            _smartThreadPool = new SmartThreadPool(stpStartInfo);
        }
        
        static void logo()
        {
            Console.Clear();
            ASCII.ASCIII();
            Console.WriteLine("");
        }
        
        private static string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        private static Mutex errorMutex = new Mutex();
        private static void errorlogger(Exception ex, bool console)
        {
            string path = Path.Combine(currentPath, "error.txt");
            if (console)
            {
                Console.Clear();
                Console.BackgroundColor = Color.Blue;
                Console.WriteLine(ex);
                Console.WriteLine("\tERROR\n\nCheck errors.txt");
                Console.ResetColor();
            }
            errorMutex.WaitOne();
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ex + "\n\n");
                }

            }
            finally
            {
                errorMutex.ReleaseMutex();
            }

        }
        static void initialiseThreads()
        {
            Initialise();
            for (int i = 1; i <= constant.Thread; i++)
            {
                try
                {
                    _smartThreadPool.QueueWorkItem(() => new main_THREADLOOPER(i));
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    log.errorlogger(ex, true);
                }
            }
            
        }
        
        private static void makeConfig()
        {
            Configuration configuration = new Configuration();
            logo();
            
            while (true)
            {
                try
                {
                    logo();
                    Console.WriteLine("Enter Config Name");
                    configuration.name = Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            // min max play time
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter minimum playtime (in sec)");
                    configuration.minPlaytime = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("Enter maximum playtime (in sec)");
                    configuration.maxPlaytime = Convert.ToInt32(System.Console.ReadLine());
                    if (configuration.minPlaytime >= configuration.maxPlaytime)
                    {
                        Console.WriteLine("\n MIN is Bigger Than MAX ");
                        Thread.Sleep(2000);
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }

            // gen or input
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Do you want to Gen (0-No,1-yes)");
                    int data = Convert.ToInt32(System.Console.ReadLine());
                    if (data == 0)
                    {
                        configuration.GEN_R = false;
                        break;
                    }
                    configuration.GEN_R = true;
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }

            if (configuration.GEN_R)
            {
                // gen proxy input
                while (true)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("Do you wanna use Proxies? (0-No,1-yes");
                        int data = Convert.ToInt32(System.Console.ReadLine());
                        if (data == 0)
                        {
                            configuration.proxy_infox = false;
                            break;
                        }
                        configuration.proxy_infox = true;
                        while (true)
                        {
                            try
                            {
                                logo();
                                System.Console.WriteLine("GEN PROXY PATH");
                                string datax = Convert.ToString(System.Console.ReadLine());
                                configuration.proxy_info = datax;
                                System.Console.WriteLine("Press 1 for Http");
                                System.Console.WriteLine("Press 2 for Https");
                                System.Console.WriteLine("Press 3 for Socks5");
                                int dataxc = Convert.ToInt32(System.Console.ReadLine());
                                if (dataxc == 1)
                                {
                                    configuration.proxy_info_p = "http";
                                }
                                else if (dataxc == 2)
                                {
                                    configuration.proxy_info_p = "https";
                                }
                                else
                                {
                                    configuration.proxy_info_p = "socks5";
                                }
                                
                                break;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                                Thread.Sleep(2000);
                            }
                            catch (Exception ex)
                            {
                                errorlogger(ex, true);
                            }
                        }

                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
                
                // verify on go
                while (true)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("Verify On Go (0-No,1-yes)");
                        int data = Convert.ToInt32(System.Console.ReadLine());
                        if (data == 0)
                        {
                            configuration.VERIFY_GO = false;
                            break;
                        }
                        configuration.VERIFY_GO = true;
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
                
                //gen which
                while (true)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("Do you want to use CatchALL (0-No,1-yes)");
                        int data = Convert.ToInt32(System.Console.ReadLine());
                        if (data == 0)
                        {
                            configuration.GEN_WHICH = false;
                            break;
                        }
                        configuration.GEN_WHICH = true;
                        while (true)
                        {
                            try
                            {
                                logo();
                                System.Console.WriteLine("ADD txt File .txt");
                                System.Console.WriteLine("/nEX: Domain:forward_mail:password");
                                System.Console.WriteLine("/nEX: gmail.com:example@gmail.com:password");
                                string datax = @""+Convert.ToString(System.Console.ReadLine());
                                configuration.GEN_DOMAIN = datax;
                                break;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("\nOnly path allowed!!!! bitch. ");
                                Thread.Sleep(2000);
                            }
                            catch (Exception ex)
                            {
                                errorlogger(ex, true);
                            }
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
            }
            else
            {
                // userDAta
                while (true)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("Input UserData .txt");
                        string data = Convert.ToString(System.Console.ReadLine());
                    
                        configuration.UserData = data;
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nObitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
            }
            
            // stream proxy input
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Do you wanna use streaming Proxies? (0-No,1-yes)");
                    int data = Convert.ToInt32(System.Console.ReadLine());
                    if (data == 0)
                    {
                        configuration.stream_proxyx = false;
                        break;
                    }
                    configuration.stream_proxyx = true;
                    while (true)
                    {
                        try
                        {
                            logo();
                            System.Console.WriteLine("File Path streaming proxy");
                            string datax = Convert.ToString(System.Console.ReadLine());
                            configuration.stream_proxy = datax;
                            System.Console.WriteLine("Press 1 for Http");
                            System.Console.WriteLine("Press 2 for Https");
                            System.Console.WriteLine("Press 3 for Socks5");
                            int dataxc = Convert.ToInt32(System.Console.ReadLine());
                            if (dataxc == 1)
                            {
                                configuration.stream_proxy_p = "http";
                            }
                            else if (dataxc == 2)
                            {
                                configuration.stream_proxy_p = "https";
                            }
                            else
                            {
                                configuration.stream_proxy_p = "socks5";
                            }
                            break;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }

            // PPA min max
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter minimum play per Account");
                    configuration.PPA_MIN = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("Enter max per Account");
                    configuration.PPA_MAX = Convert.ToInt32(System.Console.ReadLine());
                    if (configuration.PPA_MIN >= configuration.PPA_MAX)
                    {
                        Console.WriteLine("\n MIN is Bigger Than MAX ");
                        Thread.Sleep(2000);
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            
            //songurl file
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Songurl File .txt");
                    configuration.song_url_list_file = @""+Convert.ToString(System.Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            
            //thread
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("How Many THreads");
                    configuration.maxThreads = Convert.ToInt32(System.Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            
            configuration.readconfig();
            
        }
        

    }
}