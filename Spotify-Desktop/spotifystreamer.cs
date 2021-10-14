using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Specialized;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading;
using Amib.Threading;
using System.Net;
using System.Linq;
using System.Net.Mime;
using System.Net.Security;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Support.UI;
using RandomNameGeneratorLibrary;
using Network = OpenQA.Selenium.DevTools.V91.Network;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V91.DevToolsSessionDomains;

namespace SPOTIFYFINAL
{
    public class Browser
    {
        private IWebDriver driver;
        private IDevToolsSession session;
        private ChromeOptions options;
        int Thread_id = 0;
        public bool ini = true;
        public bool session_i = false;
        private Process process_;
        private string email__;
        public
            Browser(int id, string email, string bind)
        {
            string proxy__;
            email__ = email;
            try
            {
                string configPath = Path.Combine(constant.currentPath, $"SAVE_SESSION/{email}");
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
                var sourceFile = Path.Combine(configPath, "prefs");
                if (File.Exists(sourceFile))
                {
                    if (Load_Session())
                    {
                        log.Threadlogger(Thread_id, $" SESSION LOADED {email}");
                        session_i = true;
                    }
                }
                
                session_i = false;
                Thread_id = id;
                options = new ChromeOptions();
                // port generator
                IEnumerator<int> port_ = helper.port_genrator;
                port_.MoveNext();

                // proxy func
                if (!constant.stream_proxyx)
                {
                    if (bind != "false")
                    {
                    
                        proxy__ = bind;
                        options.AddArgument($"--proxy-server={proxy__}");
                    }
                    else
                    {
                        if (constant.proxy_infox)
                        {
                            var x = helper.proxy_gen;
                            x.MoveNext();
                            proxy__ = x.Current;
                            if (!helper.proxy_auth_checker(proxy__))
                            {
                                options.AddArgument($"--proxy-server={constant.proxy_info_p}://{proxy__}");
                            }
                            else
                            {
                                if (!Node_auth(proxy__)) ini = false;
                                int po = 4440 + Thread_id;
                                options.AddArgument($"--proxy-server=127.0.0.1:{po}");
                            }
                        
                        }
                    }
                }
                else if (constant.proxy_infox)
                {
                    
                    var xty = helper.pgen;
                    xty.MoveNext();
                    proxy__ = xty.Current;
                    if (!Node_auth(proxy__)) ini = false;
                    int po = 4440 + Thread_id;
                    options.AddArgument($"--proxy-server=127.0.0.1:{po}");
                }
                else if (constant.stream_proxyx)
                {
                    var x = helper.stream_proxy;
                    x.MoveNext();
                    proxy__ = x.Current;
                    if (!helper.proxy_auth_checker(proxy__))
                    {
                        options.AddArgument($"--proxy-server={constant.stream_proxy_p}://{proxy__}");
                    }
                    else
                    {
                        if (!Node_auth(proxy__)) ini = false;
                        int po = 4440 + Thread_id;
                        options.AddArgument($"--proxy-server=127.0.0.1:{po}");
                    }
                }

                options.AddArgument($@"-mu={email}");
                options.AddArgument($"--remote-debugging-port={port_.Current}");
                options.AddArgument("--use-fake-ui-for-media-stream");
                options.AddArgument("--use-fake-device-for-media-stream");
                options.AddArgument("--silent");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--update-endpoint-override='https://google.com'");
                options.AddArgument("force-webrtc-ip-handling-policy");
                options.AddArgument("--disable-crash-reporting");
                options.AddArgument("--transparent-window-controls");
                options.AddArgument("--disable-gpu-compositing");
                options.AddArgument("--disable-audio-output");
                options.AddArgument("--renderer-process-limit=1");
                options.AddArgument("--num-raster-threads=1");
                options.AddArgument("--single-process");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArguments("--no-zygote");
                options.AddArgument("--disable-cache");
                options.AddArguments("--log-level=3");
                options.AddArgument("--disable-accelerated-2d-canvas");
                options.AddArguments("--disable-accelerated-mjpeg-decode");
                options.AddArguments("--disable-d3d11");
                options.AddArguments("--enable-audio-graph");
                options.AddArguments("--disable-2d-canvas-clip-aa");
                options.AddArguments("--disable-gpu");
                options.AddArgument("--blink-settings=imagesEnabled=false");
                options.AddArguments("--enable-low-end-device-mode");
                options.AddArgument("--remote-debugging-address=0.0.0.0");
                options.AddUserProfilePreference("enforce-webrtc-ip-permission-check", true);
                // options.SetLoggingPreference("goog:loggingPrefs", LogLevel.All);
                options.BinaryLocation = Path.Combine(constant.currentPath, "Spotify\\spotify.exe");
                
                ChromeDriverService service= ChromeDriverService.CreateDefaultService(constant.currentPath);
                port_.MoveNext();
                service.Port = port_.Current;
                driver = new ChromeDriver(service, options);
                driver.Navigate().Refresh();
                if (!Clear_head()) ini = false;
                IDevTools devTools = driver as IDevTools;
                session = devTools.GetDevToolsSession();
                var domains = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                domains.Network.Enable(new Network.EnableCommandSettings());
                domains.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
                {
                    Urls = new string[]
                    {
                        "xpui.app.spotify.com/xpui-routes-show.css",
                        "https://xpui.app.spotify.com/xpui.css",
                        "https://xpui.app.spotify.com/vendor~xpui.css"
                    }
                });
            }
            catch (Exception e)
            {
                ini = false;
                log.Threadlogger(Thread_id, e.ToString());
            }
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                string title = (string)js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => false})");
            }
            catch (Exception ex)
            {
                log.Threadlogger(Thread_id, ex.ToString());
                log.errorlogger(ex, true);
                ini = false;
            
            }
            
        }

        public bool Node_auth(string proxy)
        {
            int port = 4440 +Thread_id;
            
            if (helper.proxy_auth_checker(proxy))
            {
                string ip = proxy.Split(":")[0];
                string port_r = proxy.Split(":")[1];
                string usr = proxy.Split(":")[2];
                string pas = proxy.Split(":")[3];
                try
                {
                    process_ = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "node",
                            Arguments =
                                $"proxy/proxy-login-automator.js -local_port {port} -remote_host {ip} -remote_port {port_r} -usr {usr} -pwd {pas}",
                            UseShellExecute = true,
                        }
                    };
                    process_.Start();
                }
                catch (Exception e)
                {
                    log.Threadlogger(Thread_id, e.ToString());
                    return false;
                }
            }
            else
            {
                string ip = proxy.Split(":")[0];
                string port_r = proxy.Split(":")[1];
                try
                {
                    
                    process_ = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "node",
                            Arguments =
                                $"proxy/proxy-login-automator.js -local_port {port} -remote_host {ip} -remote_port {port_r} -usr admin -pwd admin",
                            UseShellExecute = true,
                        }
                    };
                    process_.Start();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    log.Threadlogger(Thread_id, e.ToString());
                    return false;
                }
            }
            
            return true;
        }
        
        public void kill_node()
        {
            try
            {
                process_.Kill();
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, "[IGNORE]ERROR WHILE KILLING NODE ");
            }
            
        }

        private bool Clear_head()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(9));
            try
            {
                wait4.Until(c => c.FindElement(By.CssSelector("head")));

            }
            catch (Exception e)
            {
                return false;
            }
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {

                js.ExecuteScript("document.querySelector('head').remove();");

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private bool Signup_()
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.Id(@"signup-link")));
                driver.FindElement(By.Id(@"signup-link")).Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        private bool Email_input(string email, string password)
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                wait4.Until(c => c.FindElement(By.Name("email")));
                driver.FindElement(By.Name("email")).Clear();
                driver.FindElement(By.Name("email")).SendKeys(email);
                Thread.Sleep(1000);
                driver.FindElement(By.Name("password")).Clear();
                driver.FindElement(By.Name("password")).SendKeys(password);
                var personGenerator = new PersonNameGenerator();
                var name = personGenerator.GenerateRandomFirstName();
                Thread.Sleep(1000);
                driver.FindElement(By.Name("name")).Clear();
                driver.FindElement(By.Name("name")).SendKeys(name);

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        
        public bool Already_email()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            try
            {
                wait4.Until(c => c.FindElement(By.ClassName("FieldError")));
                log.Threadlogger(Thread_id, "EMAIL already exist");
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public bool Continue_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            try
            {
                wait4.Until(c => c.FindElement(By.Id("signup-button")));
                driver.FindElement(By.Id("signup-button")).Click();
                Thread.Sleep(1000);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public bool B_day()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.Id("month-field")).Displayed);
                driver.FindElement(By.Id("month-field")).Click();
                int month = new Random().Next(2, 12);
                driver.FindElement(By.CssSelector($"[name='month'] option:nth-child({month})")).Click();
                
                var date = new Random().Next(1, 27);
                Thread.Sleep(500);
                driver.FindElement(By.Id("day-field")).Clear();
                driver.FindElement(By.Id("day-field")).SendKeys(date.ToString());
                
                var year = new Random().Next(1980, 2002);
                Thread.Sleep(500);
                driver.FindElement(By.Id("year-field")).Clear();
                driver.FindElement(By.Id("year-field")).SendKeys(year.ToString());
                Thread.Sleep(1000);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        private bool check_box()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElements(By.ClassName("GlueCheckbox__input")));
                driver.FindElements(By.ClassName("GlueCheckbox__input"))[1].Click();
                Thread.Sleep(500);
                driver.FindElements(By.ClassName("GlueCheckbox__input"))[0].Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public bool check_box_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.ClassName("GlueCheckbox__input")).Displayed);
                driver.FindElements(By.ClassName("GlueCheckbox__input"))[0].Click();
                Thread.Sleep(500);
                driver.FindElements(By.ClassName("GlueCheckbox__input"))[1].Click();
                Thread.Sleep(500);
                driver.FindElements(By.ClassName("GlueCheckbox__input"))[2].Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Refresh()
        {
            try
            {
                driver.Navigate().Refresh();
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id, "REFRESH ERROR");
            }
            
            Thread.Sleep(3000);
        }
        
        public bool Gender()
        {
            try
            {
                int x = new Random().Next(2, 4);
                driver.FindElement(By.CssSelector($"[class='FormRadioSelect'] span:nth-child({x}) label")).Click();
                return true;

            }
            catch (Exception e)
            {
                
                return false;
            }
        }
        
        private bool Done_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            int trie = 0;
            while (trie < 2)
            {
                trie++;
                try
                {
                    wait4.Until(c =>
                        c.FindElement(By.XPath("//*[@id='main']/div/div[2]/div[1]/header/button[1]")).Displayed);
                    return true;

                }
                catch (Exception e)
                {
                    driver.Navigate().Refresh();
                }

            }
            try
            {
                wait4.Until(c =>
                    c.FindElement(By.XPath("//*[@id='main']/div/div[2]/div[1]/header/button[1]")).Displayed);
                return true;

            }
            catch (Exception e)
            {
                driver.Navigate().Refresh();
            }
            return false;
        }
        
        public bool Done__()
        {
            int trie = 0;
            while (trie <= 1)
            {
                trie++;
                if (GEN_LAST())
                {
                    log.Threadlogger(Thread_id, "| Flagged Proxy | RETRYING ");
                    Continue_l_();
                    Thread.Sleep(2000);
                    continue;
                }
                break;

            }
            

            if (!Done_()) return false;
            Clear_head();
            return true;
        }
        
        private bool Pre_Done_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
            try
            {
                wait4.Until(c =>
                    c.FindElement(By.Id("signup-button")).Displayed);
                return false;

            }
            catch (Exception e)
            {
                return true;
            }
        }
        
        private bool GEN_LAST()
        {
            if (Pre_Done_())
            {
                return false;
            }
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                wait4.Until(c =>
                    c.FindElement(By.CssSelector("#signup > form > div.glue-hidden-visually > div > ul > li")).Displayed);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public bool Thread_c_()
        {
            try
            {
                var title_ = driver.Title;

            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, "? ERROR > NO APP >");
                return false;
            }

            return true;
        }
        
        private bool main_wait()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(21));
            try
            {
                wait4.Until(c => c.FindElement(By.Id("main")));
                Thread.Sleep(2500);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool click_click_()
        {
            Clear_head();
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(9));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                js.ExecuteScript("window.scrollBy(0,1300);");
                Thread.Sleep(1500);
                var x = wait4.Until(c => c.FindElement(By.CssSelector("[aria-label='Play']")));
                x.Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool Continue_l_()
        {
            int trie = 0;
            while (trie < 26)
            {
                trie++;
                var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                try
                {
                    wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("signup-button")));
                }
                catch (Exception e)
                {
                    log.Threadlogger(Thread_id, e.ToString());
                    return false;
                }

                try
                {
                    var element = driver.FindElement(By.Id("signup-button"));
                    element.Click();
                }
                catch (Exception e)
                {
                    log.Threadlogger(Thread_id, "NO SIGNUP BUTTON");
                }
            }

            return true;

        }
        
        public bool back_button_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                var x = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("#signup > form > a")));
                x.Click();
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, e.ToString());
                return false;
            }

            return true;

        }
        
        private bool Loads()
        {
            main_wait();
            Thread.Sleep(5000);
            try
            {
                driver.Navigate().GoToUrl("https://open.spotify.com/playlist/37i9dQZF1E37R227UWHBom");
            }
            catch (Exception e)
            {
                return false;
            }

            if (!main_wait())
            {
                return false;
            }

            return true;

        }

        public bool Loader_()
        {
            int trie = 0;
            while (trie < 3)
            {
                trie++;
                if (!Loads())
                {
                    Refresh();
                    Thread.Sleep(5000);
                    continue;
                }

                if (!click_click_())
                {
                    Refresh();
                    Thread.Sleep(21000);
                    continue;
                }

                return true;
            }

            return false;
        }
        
        //api remaining
        public bool like_song(int number)
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                var x = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector($"[aria-rowindex=\"{number}\"] div:nth-child(4) button")));
                x.Click();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;

        }
        
        //api
        public bool Follow_artist_loop(int number)
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                var x = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector($"[aria-rowindex=\"{number}\"] div:nth-child(4) button")));
                x.Click();
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, e.ToString());
                return false;
            }

            return true;

        }
        
        // Browser
        public bool Follow_artist()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                var x = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-testid=\"artist-page\"] div > div > div > div button:nth-child(2)")));
                x.Click();
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, e.ToString());
                return false;
            }

            return true;

        }
        
        // Browser api remain
        
        public bool like_playlist()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                var x = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("[aria-label=\"Save to Your Library\"]")));
                x.Click();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;

        }
        // streaming stuff
        public bool url_loader_loop(string url)
        {
            int trie = 0;
            while (trie < 3)
            {
                trie++;
                if (url_load(url))
                {
                    Thread.Sleep(5);
                    if (song_url_loaded_check())
                    {
                        return true;
                    }
                    if (song_url_loaded_check_())
                    {
                        return true;
                    }
                    driver.Navigate().Refresh();
                    Thread.Sleep(5000);
                }
            }
            return false;
        }
        
        private bool url_load(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(45);
                Thread.Sleep(11000);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        public bool song_url_loaded_check()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(35));
            try
            {
                wait4.Until(c => c.FindElement(By.XPath("//span[text()=\"Create Playlist\"]")));
                Thread.Sleep(3000);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool song_url_loaded_check_()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(35));
            try
            {
                wait4.Until(c => c.FindElement(By.XPath("//button[text()='Upgrade']")));
                
                Thread.Sleep(3000);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        public string number_of_songs(int whichurl)
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(35));
            try
            {
                wait4.Until(c => c.FindElement(By.XPath("//button[text()='Upgrade']")));
                return "true";

            }
            catch (Exception e)
            {
                return "false";
            }
            
        }
        
        public bool pre_scroll_loop(string url)
        {
            int trie = 0;
            while (trie < 2)
            {
                trie++;
                Thread.Sleep(7);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                if (pre_scroll_check())
                {
                    Thread.Sleep(1500);
                    try
                    {
                        js.ExecuteScript("window.scrollBy(0,2300);");
                        pre_scroll();
                        return true;

                    }
                    catch (Exception)
                    {
                        if (pre_scroll())
                        {
                            return true;
                        }
                        try
                        {
                            js.ExecuteScript("window.scrollBy(0,2300);");
                            return true;

                        }
                        catch (Exception x)
                        {
                            log.Threadlogger(Thread_id, x.ToString());
                            url_loader_loop(url);
                        }
                    }
                }
                url_loader_loop(url);
            }

            return false;

        }
        
        private bool pre_scroll()
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                var pre = wait4.Until(c => c.FindElement(By.CssSelector("[aria-label='Mute']")));
                js.ExecuteScript("arguments[0].scrollIntoView();", pre);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool pre_scroll_check()
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.XPath("//span[text()='Create Playlist']")));
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        public bool Post_check()
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.CssSelector("[type=\"button\"]")));
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        // click stuff
        public bool song_clicker_loop(int number, string url)
        {
            int tries = 0;
            int trie = 0;
            while (tries < 2)
            {
                tries++;
                if (!check_song(number))
                {
                    while (trie < 2)
                    {
                        trie++;
                        driver.Navigate().Refresh();
                        Thread.Sleep(5000);
                        if (!url_loader_loop(url))
                        {
                            continue;
                        }

                        if (!pre_scroll_loop(url))
                        {
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    song_scroll(number);
                    Thread.Sleep(2500);
                    if (click_song(number))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private bool check_song(int number)
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            number++;
            try
            {
                wait4.Until(c => c.FindElement(By.CssSelector($"[aria-rowindex=\"{number}\"]")));
                Thread.Sleep(2500);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool click_song(int number)
        {
            
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            number++;
            try
            {
                var btn =wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector($"[aria-rowindex=\"{number}\"] button")));
                Thread.Sleep(1300);
                btn.Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        private bool song_scroll(int number)
        {
            if (number == 1)
            {
                number++;
            }
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                var btn =wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector($"[aria-rowindex=\"{number}\"] button")));
                js.ExecuteScript("arguments[0].scrollIntoView();", btn);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        // play check
        private bool play_checker_api(string bearer)
        {
            return false;
        }
        
        private bool play_or_pause()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.CssSelector("[aria-label='Pause']")));
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public bool play_checker_loop(string bearer)
        {
            int trie = 0;
            while (trie < 3)
            {
                trie++;
                if (play_or_pause())
                {
                    return true;
                }
                Thread.Sleep(5000);
            }
            return false;
        }

        public string song_name(int number)
        {
            number++;
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                var name = wait4.Until(c => c.FindElement(By.CssSelector($"[aria-rowindex=\"{number}\"] div:nth-child(2) div div")));
                var sname = name.GetAttribute("innerText");
                return sname;

            }
            catch (Exception e)
            {
                return "false";
            }
        }

        public bool Gen(string email, string password)
        {
            if (!Signup_())
            {
                return false;
            }
            int trie = 0;
            while (trie < 3)
            {
                trie++;
                if (trie == 3)
                {
                    return false;
                }
                if (!Email_input(email, password))
                {
                    Refresh();
                    Signup_();
                    Thread.Sleep(4000);
                }
                else
                {
                    break;
                }
            }

            int trie1 = 0;
            check_box();
            while (trie1 < 3)
            {
                trie1++;
                if (trie1 == 3)
                {
                    return false;
                }
                if (!Continue_())
                {
                    Refresh();
                    Signup_();
                    Email_input(email, password);
                    Thread.Sleep(4000);
                }
                else
                {
                    break;
                }
            }

            return true;
        }
        
        // login stuff
        public bool Login_loop(string email, string password)
        {
            int trie = 0;
            int tries = 0;
            while (trie < 2)
            {
                trie++;
                if (!Login_form())
                {
                    driver.Navigate().Refresh();
                    continue;
                }

                while (tries < 2)
                {
                    tries++;
                    if (!Login_Submit(email, password))
                    {
                        driver.Navigate().Refresh();
                        continue;
                    }

                    if (!Loader_Pre_check())
                    {
                        return false;
                    }

                    if (!Loader_Post_check())
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;

        }
        
        private bool Login_form()
        {
            driver.Navigate().Refresh();
            Clear_head();
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                var btn = wait4.Until(c => c.FindElement(By.Id("login-button")));
                btn.Click();

            }
            catch (Exception e)
            {
                return false;
            }
            try
            {
                wait4.Until(c => c.FindElement(By.Id("login-form-title")));

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        
        private bool Login_Submit(string email, string password)
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.Name("username")));
                driver.FindElement(By.Name("username")).Clear();
                driver.FindElement(By.Name("username")).SendKeys(email);
                Thread.Sleep(500);
                driver.FindElement(By.Name("password")).Clear();
                driver.FindElement(By.Name("password")).SendKeys(password);
                Thread.Sleep(1000);

            }
            catch (Exception e)
            {
                return false;
            }
            try
            {
                Thread.Sleep(2000);
                var btn = wait4.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable((By.Id("login-button"))));
                btn.Click();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private bool Loader_Pre_check()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
            try
            {
                wait4.Until(c => c.FindElement(By.ClassName("ErrorMessage")));
                return false;

            }
            catch (Exception e)
            {
                return true;
            }
            
        }
        
        private bool Loader_Post_check()
        {
            int trie = 0;
            while (trie < 2)
            {
                trie++;
                var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(21));
                try
                {
                    wait4.Until(c => c.FindElement(By.XPath("//span[text()='Create Playlist']")));
                    return true;

                }
                catch (Exception e)
                {
                    driver.Navigate().Refresh();
                    log.Threadlogger(Thread_id,($"{e.ToString()}ERROR WHILE CHECKING LOGIN"));
                    if (Loader_Pre_check())
                    {
                        return false;
                    }
                }


            }

            return false;
        }
        
        private bool Session_Pre_check()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait4.Until(c => c.FindElement(By.Name("email")));
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        
        public bool Session()
        {
            Thread.Sleep(5000);
            if (Session_Pre_check())
            {
                return false;
            }

            if (!Loader_Post_check())
            {
                return false;
            }

            return true;
        }

        public void CLOSE()
        {
            try
            {
                driver.Close();
            }
            catch (Exception e)
            {
                int a = 1;
            }
            try
            {
                driver.Quit();
            }
            catch (Exception e)
            {
                int b = 1;
            }
            kill_node();
            RM();
        }

        public bool ad_clicker()
        {
            var wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                var btn = wait4.Until(c => c.FindElement(By.CssSelector("[aria-label=\"Hide Advertisement\"]")));
                btn.Click();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string logp()
        {
            var log = driver.Manage().Logs.GetLog("performance");
            Console.Write(log);
            foreach (var l in log)
            {

                Console.Write("\n"+l);

            }

            return "aa";
        }

        private void RM()
        {
            var x = Environment.GetEnvironmentVariable("LocalAppData");
            var x2 = Environment.GetEnvironmentVariable("APPDATA");
            var path = x+$"/Spotify-{email__}";
            var path2 = x2+$"/Spotify-{email__}";
            try
            {
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (Directory.Exists(path2)) Directory.Delete(path2, true);
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id, "FILE DELETE ERROR -- == ++ ;; :: [[ }}");
            }
            
        }
        
        public void Save_Session()
        {
            var x2 = Environment.GetEnvironmentVariable("APPDATA");
            var path = x2+$"/Spotify-{email__}";
            
            var sourceFile = Path.Combine(path, "prefs");
            string configPath = Path.Combine(constant.currentPath, "SAVE_SESSION");

            try
            {
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }

                string Folder = Path.Combine(configPath, email__);
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                var dewst = Path.Combine(Folder, "prefs");
                File.Copy(sourceFile, dewst, true);
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id, " ERROR SAVING SESSION ");
            }
            
        }
        
        private bool Load_Session()
        {
            var x2 = Environment.GetEnvironmentVariable("APPDATA");
            var path = x2+$"/Spotify-{email__}";
            
            var sourceFile = Path.Combine(path, "prefs");
            string configPath = Path.Combine(constant.currentPath, "SAVE_SESSION");
            string main_f = Path.Combine(x2, $"Spotify-{email__}");
            try
            {
                if (!Directory.Exists(main_f))
                {
                    Directory.CreateDirectory(main_f);
                }

                string Folder = Path.Combine(configPath, email__);
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                    return false;
                }
                
                var dewst = Path.Combine(Folder, "prefs");
                File.Copy(dewst, sourceFile, true);
            }
            catch (Exception)
            {
                log.Threadlogger(Thread_id, " ERROR LOADING SESSION ");
            }

            return true;

        }
        
    }
}