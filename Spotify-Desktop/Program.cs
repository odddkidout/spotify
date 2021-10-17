using System;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;
using KeyAuth;

namespace SPOTIFYFINAL
{
    class Program
    {
        
        static SmartThreadPool _smartThreadPool;


        static void Main()
        {
            
            string VERSION = "0.1.0";
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            helper.log_cleaner();
            helper.Cleaner();
            // Auth();
            Config();
            
            new Thread(initialiseThreads).Start();
            
            while (true)
            {
                // Console.Clear();
                // ASCII.ASCIII();
                Console.Title = $"SPoTi_{VERSION}_|_TOTAL-STREAM-{stats.Stream_Total}_|_LOGIN_{stats.Succes_login}_|_GENRATED-{stats.Gen}_|_FAIL-{stats.fail}_|_CPU-{cpuCounter.NextValue()+"%"}_|_RAM-{ramCounter.NextValue()+"MB"}";
                // Console.WriteLine("Note \"{0}\"", Console.Title);
                Thread.Sleep(5000);
            }
        }
        
        public static IEnumerable<string> CycleFile(string fileName)
        {
            while (true)
                foreach (var line in File.ReadLines(fileName))
                {
                    yield return line;
                }
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

                if (configuration.VERIFY_GO)
                {
                    // verification procy
                    while (true)
                    { 
                        try
                        { 
                            logo();
                            System.Console.WriteLine("Do you want to use Proxies for verification? (0-No,1-yes");
                            int data = Convert.ToInt32(System.Console.ReadLine()); 
                            if (data == 0)
                            { 
                                configuration.v_proxyx = false;
                                break;
                            }
                            configuration.v_proxyx = true;
                            while (true)
                            {
                                 
                                try
                                 
                                {
                                    logo();
                                    System.Console.WriteLine("verification PROXY PATH");
                                    string datax = Convert.ToString(System.Console.ReadLine());
                                    configuration.v_proxy = datax;
                                    System.Console.WriteLine("Press 1 for Http");
                                    System.Console.WriteLine("Press 2 for Https");
                                    System.Console.WriteLine("Press 3 for Socks5");
                                    int dataxc = Convert.ToInt32(System.Console.ReadLine());
                                    if (dataxc == 1)
                                    { 
                                        configuration.v_proxy_p = "http";
                                    }
                                    else if (dataxc == 2)
                                    { 
                                        configuration.v_proxy_p = "https";
                                    }
                                    else
                                    { 
                                        configuration.v_proxy_p = "socks5";
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
                             
                        }catch (Exception ex) 
                        {
                             errorlogger(ex, true);
                             
                        } 
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
            
            //Like
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("DO YOU WANT TO LIKE SONGS");
                    System.Console.WriteLine("1 - yes");
                    System.Console.WriteLine("0 - NO");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (temp == 1)
                    {
                        configuration.Like = true;
                        while (true)
                        {
                            System.Console.WriteLine("PERCENTAGE %");
                            int P = Convert.ToInt32(System.Console.ReadLine());
                            if (P > 100 || P < 0)
                            {
                                System.Console.WriteLine("WRONG TYPE BETWEEN 1-100 %");
                                continue;
                            }
                            configuration.Like_P = P;
                            break;
                        }
                        break;
                    }
                    configuration.Like = false;
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
            
            // SAVE Playlist and album
            while (true)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("DO YOU WANT TO  SAVE Playlist and Album");
                    System.Console.WriteLine("1 - yes");
                    System.Console.WriteLine("0 - NO");
                    int temp = Convert.ToInt32(System.Console.ReadLine());
                    if (temp == 1)
                    {
                        configuration.Lsave = true;
                        while (true)
                        {
                            System.Console.WriteLine("PERCENTAGE %");
                            int P = Convert.ToInt32(System.Console.ReadLine());
                            if (P > 100 || P < 0)
                            {
                                System.Console.WriteLine("WRONG TYPE BETWEEN 1-100 %");
                                continue;
                            }
                            configuration.Lsave_P = P;
                            break;
                        }

                        break;
                    }
                    configuration.Lsave = false;
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
            
            File.WriteAllText(Path.Combine(currentPath, "config", configuration.name + ".txt"), JsonConvert.SerializeObject(configuration, Formatting.Indented)); 
            configuration.readconfig();
            
        }
        private static void loadConfig()
        {
            logo();
            int selection = -1;
            string configPath = Path.Combine(currentPath, "config");
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            string[] configs = Directory.GetFiles(configPath, "*.txt");
            System.Console.Write("Choose Config Number \n");
            for (int i = 0; i < configs.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(configs[i])}");
            }
            Console.WriteLine("\n\nEnter 0 to go back\n input :- ");
        
            while (true)
            {
                try
                {
                    selection = Convert.ToInt32(System.Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
            }
            if (selection > 0 & selection <= configs.Length)
            {
                var fileStream = File.ReadAllText(configs[selection - 1]);
                Console.Write(fileStream);
                Configuration configuration = new Configuration();
                var config = 
                    JsonConvert.DeserializeObject<Configuration>(fileStream);
        
                configuration.maxThreads = Convert.ToInt32(config.maxThreads);

                configuration.PPA_MAX = Convert.ToInt32(config.PPA_MAX);

                configuration.PPA_MIN = Convert.ToInt32(config.PPA_MIN);

                configuration.minPlaytime = Convert.ToInt32(config.minPlaytime);
                configuration.maxPlaytime = Convert.ToInt32(config.maxPlaytime);
                configuration.GEN_R = Convert.ToBoolean(config.GEN_R);
                configuration.song_url_list_file = Convert.ToString(config.song_url_list_file);
                if (configuration.GEN_R)
                {
                    configuration.VERIFY_GO = Convert.ToBoolean(config.VERIFY_GO);
                    if (configuration.VERIFY_GO)
                    {
                        configuration.v_proxyx = Convert.ToBoolean(config.v_proxyx);
                        if (configuration.v_proxyx)
                        {
                            configuration.v_proxy = Convert.ToString(config.v_proxyx);
                            configuration.v_proxy_p = Convert.ToString(config.v_proxy_p);
                        }
                    }
                   
                    configuration.GEN_WHICH = Convert.ToBoolean(config.GEN_WHICH);
                    if (configuration.GEN_WHICH)
                    {configuration.GEN_DOMAIN = Convert.ToString(config.GEN_DOMAIN);}
                    configuration.proxy_infox = Convert.ToBoolean(config.proxy_infox);
                    if (configuration.proxy_infox)
                    {
                        configuration.proxy_info = Convert.ToString(config.proxy_info);
                        configuration.proxy_info_p = Convert.ToString(config.proxy_info_p);
                    }
                }
                else
                {
                    configuration.UserData = Convert.ToString(config.UserData);
                }
                configuration.stream_proxyx = Convert.ToBoolean(config.stream_proxyx);
                if (configuration.stream_proxyx)
                {
                    configuration.stream_proxy = Convert.ToString(config.stream_proxy);
                    configuration.stream_proxy_p = Convert.ToString(config.stream_proxy_p);
                }

                configuration.Like = Convert.ToBoolean(config.Like);
                if (configuration.Like)
                {
                    configuration.Like_P = Convert.ToInt32(config.Like_P);
                }
                
                configuration.Lsave = Convert.ToBoolean(config.Lsave);
                if (configuration.Lsave)
                {
                    configuration.Lsave_P = Convert.ToInt32(config.Lsave_P);
                }
                configuration.readconfig();

            }
            else if (selection == 0)
            {
                Config();
            }
            else
            {
        
                logo();
                Console.WriteLine("Invalid Choice!! nigga");
                Thread.Sleep(1500);
                loadConfig();
        
            }
        }

        private static void Auth()
        {
            try
            {
                var n = new Programx();
                if (!n.Mainx())
                {
                    Console.WriteLine("\n\n  FAIL LOGIN"); // thought people would be able to find this easier than a ReadLine...
                    Thread.Sleep(3200); // I lied! Jk, it takes the average person 1.8 seconds to read the above output, so I've accounted for that. it would be a huge travesty if it didn't close in five seconds from the ouptput being displayed :)
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("\n\n  Closing in five seconds..."); // thought people would be able to find this easier than a ReadLine...
                Thread.Sleep(3200); // I lied! Jk, it takes the average person 1.8 seconds to read the above output, so I've accounted for that. it would be a huge travesty if it didn't close in five seconds from the ouptput being displayed :)
                Environment.Exit(0);
            }
        }

        private static void Config()
        {
            while (true)
            {
                Console.Clear();
                System.Console.Write("\n");
                ASCII.ASCIII();
                System.Console.Write("\n");
                System.Console.WriteLine("1 => Load config/start bot");
                System.Console.WriteLine("2 => Make config");
                System.Console.Write("3 => setup discord webhook\n");
                int temp = 0;
                try
                {
                    temp = Convert.ToInt32(System.Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                    continue;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                    Console.Clear();
                    continue;
                }
                if (temp == 1)
                {
                    try
                    {
                        loadConfig();
                        break;
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                        Console.Clear();
                    }
                }
                else if (temp == 2)
                {
                    try
                    {
                        makeConfig();
                        System.Console.WriteLine("Config Created");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
                else if (temp == 3)
                {
                    try
                    {
                        System.Console.WriteLine("still in progress");
                        Thread.Sleep(3000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                        Console.Clear();
                    }
                }
                else
                {
                    System.Console.WriteLine("invalid selection");
                }
            }
        }
        
        
    }
    
}