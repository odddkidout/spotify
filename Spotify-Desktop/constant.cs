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
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SPOTIFYFINAL
{
    public class constant
    {
        static public string currentPath =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static DateTime startz;
        public static int PPA_MIN = 3;
        public static int PPA_MAX = 6;
        public static  int Thread = 1;
        public static bool AGE = false;
        public static int MIN = 30;
        public static int MAX = 60;
        public static bool Like = false;
        public static int Like_P = 10;
        public static bool Lsave = false;
        public static int Lsave_P = 10;
        public static int MIN_ = 30;
        public static int MAX_ = 60;
        public static bool VERIFY_GO = false;
        public static bool GEN_R = false;
        public static bool GEN_WHICH = false;
        public static string GEN_DOMAIN;
        public static string UserData = "acc.txt";
        public static bool proxy_infox = false;
        public static string proxy_info;
        public static string proxy_info_p = "http";
        public static string song_url_list_file = "spotifylink.txt";
        public static bool stream_proxyx = false;
        public static string stream_proxy;
        public static string stream_proxy_p = "http";
        public static bool v_proxyx = false;
        public static string v_proxy;
        public static string v_proxy_p = "http";
        public static string loadedconfigid;
    }

    public class stats
    {
        public static int Like = 0;
        public static int Stream_Total = 0;
        public static int save_p = 0;
        public static int Gen = 0;
        public static int Succes_login = 0;
        public static int fail = 0;
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

    class Configuration
    {
        public string configId { get; set; }
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
        
        public bool v_proxyx { get; set; }
        public string v_proxy { get; set; }
        public string v_proxy_p { get; set; }
        public bool VERIFY_GO { get; set; }
        public bool GEN_WHICH { get; set; }
        public string GEN_DOMAIN { get; set; }
        public string UserData { get; set; }
        public bool Like { get; set; }
        public int Like_P { get; set; }
        
        public bool Lsave { get; set; }
        public int Lsave_P { get; set; }
        public string song_url_list_file { get; set; }
        public bool GEN_R { get; set; }

        public void readconfig()
        {
            try
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
                    if (VERIFY_GO)
                    {
                        constant.v_proxyx = v_proxyx;
                        if (v_proxyx)
                        {
                            constant.v_proxy = v_proxy;
                            constant.v_proxy_p = v_proxy_p;
                        }
                    }

                    constant.GEN_WHICH = GEN_WHICH;
                    if (GEN_WHICH) constant.GEN_DOMAIN = GEN_DOMAIN;
                    constant.proxy_infox = proxy_infox;
                    if (proxy_infox)
                    {
                        constant.proxy_info = proxy_info;
                        constant.proxy_info_p = proxy_info_p;
                    }
                }
                else
                {
                    constant.UserData = UserData;
                }

                constant.stream_proxyx = stream_proxyx;
                if (stream_proxyx)
                {
                    constant.stream_proxy = stream_proxy;
                    constant.stream_proxy_p = stream_proxy_p;
                }

                constant.Like = Like;
                if (Like)
                {
                    constant.Like_P = Like_P;
                }

                constant.Lsave = Lsave;
                if (Lsave)
                {
                    constant.Lsave_P = Lsave_P;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(500000);
            }
        }
    }

    public class log
        {
            public static void Threadlogger(System.Guid Threadid, String update)
            {

                string path = Path.Combine(constant.currentPath, "logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, $"THREAD_{Threadid}.txt");
                try
                {
                    Console.Write($"\n{constant.startz.Date.Day}{constant.startz.Date.Month}-{constant.startz.TimeOfDay.Hours}-{constant.startz.TimeOfDay.Minutes}| THREAD_{Threadid}| "+update);
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
    
