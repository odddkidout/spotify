using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using OpenQA.Selenium.DevTools.V85.CacheStorage;
using OpenQA.Selenium.DevTools.V89.Accessibility;
using SpotifyAPI.Web;
using Console = Colorful.Console;
using Expression = System.Linq.Expressions.Expression;

namespace SPOTIFYFINAL
{
    public class helper
    {
        Random _random = new Random();
        
        public int RandomNumber(int min, int max) {
            return _random.Next(min, max);
        }
        
        public static IEnumerator<int> port_genrator = portport().GetEnumerator();
        public static IEnumerator<string> UserData = Program.CycleFile(Convert.ToString(constant.UserData)).GetEnumerator();
        public static IEnumerator<string> GEN_DOMAIN = Program.CycleFile(Convert.ToString(constant.GEN_DOMAIN)).GetEnumerator();
        public static IEnumerator<string> stream_proxy = Program.CycleFile(Convert.ToString(constant.stream_proxy)).GetEnumerator();
        public static IEnumerator<string> proxy_gen = Program.CycleFile(constant.proxy_info).GetEnumerator();
        public static IEnumerator<string> Verifier_gen = Program.CycleFile(constant.v_proxy).GetEnumerator();
        
        public static int url_checker(string url)
        {
            if (url.Contains("album"))
            {
                return 0;
            }
            
            return 1;
        }
        
        public static bool proxy_auth_checker(string proxy)
        {
            int x = 0;
            foreach (var _ in proxy.Split(":"))
            {
                x++;
            }

            if (x >= 3) return true;
            return false;
        }

        
        private static IEnumerable<int> portport()
        {
            while (true) foreach(int i in Enumerable.Range(9222,12111)) yield return i;
        }
        
        public static IEnumerator<string> pgen = Program.CycleFile(constant.proxy_info).GetEnumerator();

        public static bool Like_Chances()
        {
            int val = new Random().Next(1, 100);
            if (val < constant.Like_P)
            {
                return true;
            }
            return false;
        }
        
        public static bool Save_Chances()
        {
            int val = new Random().Next(1, 100);
            if (val < constant.Lsave_P) return true;
            return false;
        }

        public static bool Read_Email_CUSTOM(string email, string Forwardmail, int Thread_id)
        {
            string server = "imap.gmail.com";
            if (Forwardmail.ToLower().Contains("gmail"))
            {
                server = "imap.gmail.com";
            }
            else if(Forwardmail.ToLower().Contains("yahoo"))
            {
                server = "imap.mail.yahoo.com";
            }
            else if(Forwardmail.ToLower().Contains("aol"))
            {
                server = "imap.aol.com";
            }
            else
            {
                server = "outlook.office365.com";
            }


            using(Imap imap = new Imap())
            {
                try
                {
                    imap.ConnectSSL(server);
                    imap.Login(Forwardmail.Split(":")[0], Forwardmail.Split(":")[1]);
                }
                catch (Exception)
                {
                    log.Threadlogger(Thread_id,$"| IMAP LOGIN FAIL | {Forwardmail} | ");
                    return false;
                }

                int trie = 0;
                while (trie < 3)
                {
                    trie++;
                    try 
                    {
                        imap.SelectInbox(); 
                        SimpleImapQuery query = new SimpleImapQuery(); 
                        query.To = email; 
                        List<long> uids = imap.Search(query); 
                        foreach (long uid in uids) 
                        {
                            byte[] eml = imap.GetMessageByUID(uid); 
                            IMail message = new MailBuilder()
                                .CreateFromEml(eml); 
                            Console.WriteLine("\n"+message.Text); 
                            string x = message.Text.Split("YOUR ACCOUNT (")[1]; 
                            string y = x.Split(")")[0]; 
                            imap.Close(true); 
                            Console.Write("\n"+y); 
                            if (y.Contains("https://")) 
                            { 
                                if (constant.v_proxyx) 
                                { 
                                    Verifier_gen.MoveNext(); 
                                    if (verifier(y, Verifier_gen.Current,Thread_id)) return true;
                                    
                                }
                                else if (constant.proxy_infox) 
                                { 
                                    proxy_gen.MoveNext(); 
                                    Console.Write("\n"+proxy_gen.Current); 
                                    if (verifier(y, proxy_gen.Current, Thread_id)) return true; 
                                }
                                else if(constant.stream_proxyx) 
                                { 
                                    stream_proxy.MoveNext(); 
                                    if (verifier(y, stream_proxy.Current, Thread_id)) return true; 
                                }
                                else 
                                { 
                                    if (proxylessverifier(y, Thread_id)) return true; 
                                }
                            } 
                        } 
                    }
                    catch (Exception) 
                    { 
                        Thread.Sleep(3000);
                    }
                    log.Threadlogger(Thread_id,$"| IMAP MAIL EXTRACT EXCEPTION FAIL RETRYING| {email} | ");
                    Thread.Sleep(30000);
                }
                return false;
            }
            
        }

        private static bool verifier(string link, string proxy, int Thread_id)
        {
            try
            {
                string ip = proxy.Split(":")[0];
                int port = Convert.ToInt32(proxy.Split(":")[1]);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
                request.Proxy = new WebProxy(ip, port);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                    
                {
                    if (reader.ReadToEnd().Contains("email-verify-container"))
                    {
                        return true;
                    }
                    
                }
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id,"| MAIL VERIFYING ERROR | RETRYING");
                if (proxylessverifier(link, Thread_id)) return true;
            }
            return false;
        }
        
        public static void song_n_e(string link)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(link);
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
                WebResponse response = httpRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd();
                var x = result.Split(" songs.")[0];
                Console.WriteLine(result);  
            }
            catch (Exception)
            {
                log.Threadlogger(0,"| HOW MANY SONGS EXTRACTING ERROR | RETRYING");
            }
        }

        private static bool proxylessverifier(string link, int Thread_id)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
            try
            {
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    if (reader.ReadToEnd().Contains("email-verify-container"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id,"| MAIL VERIFYING ERROR |");
            }
            return false;
        }
        
        public static void Cleaner()
        {
            var x = Environment.GetEnvironmentVariable("LocalAppData");
            var x2 = Environment.GetEnvironmentVariable("APPDATA");

            string DeleteThis = "Spotify";
            string[] Files = Directory.GetDirectories(x);
            string[] Files2 = Directory.GetDirectories(x2);
            
            try
            {
                foreach (string file in Files)
                {
                    if (file.ToUpper().Contains(DeleteThis.ToUpper()))
                    {
                        if (Directory.Exists(file)) Directory.Delete(file, true);
                    }
                }
            }
            catch (Exception)
            {
                log.Threadlogger(0, "FILE DELETE ERROR -- == ++ ;; :: [[ }}");
            }
            try
            {
                foreach (string file in Files2)
                {
                    if (file.ToUpper().Contains(DeleteThis.ToUpper()))
                    {
                        if (Directory.Exists(file)) Directory.Delete(file, true);
                    }
                }
            }
            catch (Exception)
            {
                log.Threadlogger(0, "FILE DELETE ERROR -- == ++ ;; :: [[ }}");
            }
            
        }
        
        public static void log_cleaner()
        {
            string DeleteThis = "thread";
            string configPath = Path.Combine(constant.currentPath, $"logs");
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
                return;
            }
            string[] Files = Directory.GetFiles(configPath);
            try
            {
                foreach (string file in Files)
                {
                    if (file.ToUpper().Contains(DeleteThis.ToUpper()))
                    {
                        if (File.Exists(file)) File.Delete(file);
                    }
                }
            }
            catch (Exception)
            {
                log.Threadlogger(0, "FILE DELETE ERROR -- == ++ ;; :: [[ }}");
            }
            
        }

        public static async Task<int> Song_ext(string link)
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest("31dcdca81f8c4541b860286153dafe19", "279582191490441aaddbbdd280dbe7c0");
            var response = await new OAuthClient(config).RequestToken(request);

            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            string id = link.Split("/")[4];
            try
            {
                if (link.ToLower().Contains("album".ToLower()))
                {
                    var x = spotify.Albums.Get(id);
                    Nullable<int> total = x.Result.TotalTracks;
                    await Task.Delay(1000);
                    return total.Value;
                }
                else
                {
                    var x = spotify.Playlists.Get(id);
                    Nullable<int> total = x.Result.Tracks.Total;
                    await Task.Delay(1000);
                    return total.Value;
                    
                }
            }
            catch (Exception)
            {
                log.Threadlogger(0, "SONG EXTRACTION FAIL");
            }
            return await Task.FromResult(0);

        }
        
    }
    
}