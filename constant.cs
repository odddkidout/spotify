using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Specialized;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Amib.Threading;
using System.Net;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SPOTIFYFINAL
{
    public class constant
    {
        static public string currentPath =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        static public int PPA_MIN = 5;
        static public int PPA_MAX = 6;
        static public int Thread = 1;
        static public bool AGE = false;
        static public int MIN = 30;
        static public int MAX = 60;
        static public int MIN_ = 30;
        static public int MAX_ = 60;
        static public bool VERIFY_GO = false;
        static public bool GEN_R = false;
        static public bool GEN_WHICH = false;
        static public string GEN_DOMAIN;
        static public string UserData = "acc.txt";
        static public bool proxy_infox = false;
        static public string proxy_info;
        static public string proxy_info_p = "http";
        static public string song_url_list_file = "spotifylink.txt";
        static public bool stream_proxyx = false;
        static public string stream_proxy;
        static public string stream_proxy_p = "http";
    }

    class ASCII
    {
        public static void ASCIII()
        {


            Colorful.Console.WriteLine("");
            Colorful.Console.WriteLine(" ▄████████    ▄████████    ▄████████    ▄█    █▄    ▀█████████▄      ███     ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███    ███   ███    ███   ███    ███   ███    ███     ███    ███  ▀█████████▄ ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███    █▀    ███    ███   ███    █▀    ███    ███     ███    ███   ▀███▀▀██ ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███          ███    ███   ███         ▄███▄▄▄▄███▄▄  ▄███▄▄▄██▀     ███   ▀ ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███        ▀███████████ ▀███████████ ▀▀███▀▀▀▀███▀  ▀▀███▀▀▀██▄     ███     ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███    █▄    ███    ███          ███   ███    ███     ███    ██▄    ███     ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("███    ███   ███    ███    ▄█    ███   ███    ███     ███    ███    ███     ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("████████▀    ███    █▀   ▄████████▀    ███    █▀    ▄█████████▀    ▄████▀   ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("                                                                            ",
                Color.BlueViolet);
            Colorful.Console.WriteLine("");
            Colorful.Console.WriteLine("");

        }
    }

    class user
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }

    class Configuration
    {
        public string name { get; set; }
        public int PPA_MIN { get; set; }
        public int PPA_MAX { get; set; }
        public int minPlaytime { get; set; }
        public int maxPlaytime { get; set; }
        public int maxThreads { get; set; }
        public bool proxy_infox { get; set; }
        public string proxy_info { get; set; }
        
        public string proxy_info_p { get; set; }
        public bool stream_proxyx { get; set; }
        public string stream_proxy { get; set; }
        
        public string stream_proxy_p { get; set; }
        public bool VERIFY_GO { get; set; }
        public bool GEN_WHICH { get; set; }
        public string GEN_DOMAIN { get; set; }
        public string UserData { get; set; }
        public string song_url_list_file { get; set; }
        public bool GEN_R { get; set; }

        public void readconfig()
        {
            constant.Thread = maxThreads;
            constant.PPA_MAX = PPA_MAX;
            constant.PPA_MIN = PPA_MIN;
            constant.MIN_ = minPlaytime;
            constant.MAX_ = maxPlaytime;
            constant.GEN_R = GEN_R;
            constant.song_url_list_file = song_url_list_file;
            if (GEN_R)
            {
                constant.VERIFY_GO = VERIFY_GO;
                constant.GEN_WHICH = GEN_WHICH;
                if (GEN_WHICH) constant.GEN_DOMAIN = GEN_DOMAIN;
                constant.proxy_infox = proxy_infox;
                if (proxy_infox)
                {
                    constant.proxy_info = proxy_info;
                    constant.proxy_info_p = proxy_info_p;
                }
            }
            
            constant.stream_proxyx = stream_proxyx;
            if (stream_proxyx)
            {
                constant.stream_proxy = stream_proxy;
                constant.stream_proxy_p = stream_proxy_p;
            }

        }
    }

    public class log
        {
            public static void Threadlogger(int Threadid, String update)
            {

                string path = Path.Combine(constant.currentPath, "logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, $"THREAD_{Threadid}.txt");
                try
                {
                    Console.Write("\n"+update);
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(update);
                    }

                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }

            private static Mutex errorMutex = new Mutex();

            public static void errorlogger(Exception ex, bool console)
            {
                string path = Path.Combine(constant.currentPath, "error.txt");
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

        }

    }
    
