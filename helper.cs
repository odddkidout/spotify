using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using OpenQA.Selenium.DevTools.V85.CacheStorage;
using OpenQA.Selenium.DevTools.V89.Accessibility;
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
        
        public static IEnumerator<int> port_genrator = helper.portport().GetEnumerator();
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

        
        public static IEnumerable<int> portport()
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
            
            using(Imap imap = new Imap())
            {
                try
                {
                    imap.ConnectSSL("imap.gmail.com");
                    imap.Login(Forwardmail.Split(":")[0], Forwardmail.Split(":")[1]);
                }
                catch (Exception)
                {
                    log.Threadlogger(Thread_id,$"| IMAP LOGIN FAIL | {email} | ");
                }


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
                        // Console.WriteLine(message.Text);
                        string x = message.Text.Split("YOUR ACCOUNT (")[1];
                        string y = x.Split(")")[0];
                        imap.Close(true);
                        if (y.Contains("https://"))
                        {
                            if (constant.v_proxyx)
                            {
                                var vp = Verifier_gen.MoveNext();
                                
                               if (verifier(y, Verifier_gen.Current,Thread_id)) return true;
                            }
                            else if (constant.proxy_infox)
                            {
                                var vp = proxy_gen.MoveNext();
                                if (verifier(y, proxy_gen.Current, Thread_id)) return true;
                            }
                            else if(constant.stream_proxyx)
                            {
                                var vp = stream_proxy.MoveNext();
                                if (verifier(y, stream_proxy.Current, Thread_id)) return true;
                            }
                            else
                            {
                                if (proxylessverifier(y, Thread_id)) return true;
                            }
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    log.Threadlogger(Thread_id,$"| IMAP MAIL EXTRACT EXCEPTION FAIL | {email} | ");
                }
                
                
            }

            return false;
        }

        public static bool verifier(string link, string proxy, int Thread_id)
        {
            string ip = proxy.Split(":")[0];
            int port = Convert.ToInt32(proxy.Split(":")[1]);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
            request.Proxy = new WebProxy(ip, port);
            try
            {
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

        private static bool proxylessverifier(string link, int Thread_id)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
            try
            {
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
                log.Threadlogger(Thread_id,"| MAIL VERIFYING ERROR |");
            }
            return false;
        }
        
        
    }
    
}