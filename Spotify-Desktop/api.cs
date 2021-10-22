using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeyAuth;
using NickStrupat;
using SPOTIFYFINAL;
using Console = Colorful.Console;



namespace Spotify_Desktop
{
    public class ipdetails
    {
        public string ip { get; set; }
    }

    public class server
    {
        public string id { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public int core { get; set; }
        public int ram { get; set; }
        public bool online { get; set; }
        public DateTime last_update { get; set; }
        public int cpu_usage { get; set; }
        public int ram_usage { get; set; }
        public int streams24h { get; set; }
        public int streams_done { get; set; }
        public int threads { get; set; }
        public int loginsdone { get; set; }
        public int accgenrated { get; set; }
        public int errors { get; set; }
        public string loadedconfig { get; set; }

    }

    public class apiconf
    {
        
        public string configId { get; set; }
        public string name { get; set; }
        public string urls { get; set; }

        public int maxPlaytime { get; set; }
        public int minPlaytime { get; set; }
        public int loop { get; set; }
        public int Like_P { get; set; }
        public int Lsave_P {get;set;}
        public string songsToPlay { get; set; }
        public int delay { get; set; }
        public int maxThreads { get; set; }
        public int PPA_MIN { get; set; }
        public int PPA_MAX { get; set; }
        public bool VERIFY_GO { get; set; }
        public bool GEN_R { get; set; }

        public bool proxy_infox { get; set; }
        public string proxy_info { get; set; }
        public string proxy_info_p { get; set; }
        public bool v_proxyx { get; set; }
        public string v_proxy_p { get; set; }
        public string v_proxy { get; set; }
        public bool GEN_WHICH { get; set; }
        public bool stream_proxyx { get; set; }
        public string stream_proxy { get; set; }
        public string stream_proxy_p { get; set; }
        public string UserData { get; set; }
        public bool Lsave { get; set; }
        public bool Like { get; set; }
        
        public string song_url_list_file { get; set; }
        public string GEN_DOMAIN { get; set; }

    }
    public class api_Django
    {
        public static server thisserver;
        public static ipdetails res;
        public static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        public static string botmasterdomain = "http://127.0.0.1:8000";
        
        public api_Django(
            string url,int delay)
        {if (url != "" && url != null)
            {
                botmasterdomain = url;
            }
            Console.WriteLine("fetching server ip\n");
            initialise().Wait();
            Console.WriteLine($"initiliasing done");
            Console.WriteLine(res.ip+"\ngeting server details");
            getServerList().Wait();
            Console.WriteLine("done");
            Thread.Sleep(5000);
            getConfigList(delay).Wait();
            Console.WriteLine("config fetched returning");
            while (constant.loadedconfigid == null)
            {
                Console.WriteLine("config loaded is null");
                Thread.Sleep(2000);
            }
            /*Thread thread = new Thread(() => getConfigList(delay));
            thread.Start();*/

        }
        static async Task initialise()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.ipify.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("?format=json");
                if (response.IsSuccessStatusCode)
                {
                    res = JsonConvert.DeserializeObject<ipdetails>(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
        static async Task getServerList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(botmasterdomain);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // HTTP GET
                HttpResponseMessage response = await client.GetAsync($"/api/server?ip={res.ip}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.ReadAsStringAsync().Result != "[]")
                    {
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                        List<server> es = JsonConvert.DeserializeObject<List<server>>(response.Content.ReadAsStringAsync().Result);
                        
                        if (es[0].ip == res.ip)
                        {
                            Console.WriteLine("found");
                            thisserver = es[0];
                        }
                        else
                        {
                            Console.WriteLine("not found");
                        }
                    }
                    else
                    {
                        var comp = new ComputerInfo();
                        Console.WriteLine(res.ip);
                        server temp = new server();
                        string myJson = "{"+$"\"ip\": \"{res.ip}\",\"port\":\"{0}\",\"core\":{Environment.ProcessorCount},\"ram\":{Convert.ToInt32((comp.TotalPhysicalMemory)/(1024))},\"online\":true,\"cpu_usage\":0,\"ram_usage\":0," +
                                        "\"streams24h\":0,\"streams_done\":0,\"threads\":0,\"loginsdone\":0,\"accgenrated\":0,\"errors\":0}";
                        using (var client2 = new HttpClient())
                        {
                            var postResponse = await client2.PostAsync(
                                botmasterdomain+"/api/server",
                                new StringContent(myJson, Encoding.UTF8, "application/json"));

                            if (postResponse.IsSuccessStatusCode)
                            {
                                thisserver =
                                    JsonConvert.DeserializeObject<server>(postResponse.Content.ReadAsStringAsync()
                                        .Result);
                            }
                            else
                            {
                                Console.WriteLine(postResponse.Content.ReadAsStringAsync().Result);
                            }
                        }
                    }
                }
            }
        }

        static async Task getConfigList(int delay)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(botmasterdomain + "/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync($"config?serverid={thisserver.id}");
            }

            if (response.IsSuccessStatusCode)
            {
                if (response.Content.ReadAsStringAsync().Result != "[]")
                {
                    List<apiconf> temp =
                        JsonConvert.DeserializeObject<List<apiconf>>(response.Content.ReadAsStringAsync()
                            .Result);
                    string temppath = constant.currentPath + $"\\api\\config\\{temp[0].configId}";
                    if (constant.loadedconfigid == null || constant.loadedconfigid != temp[0].configId)
                    {
                        constant.loadedconfigid = temp[0].configId;
                        if (!Directory.Exists(temppath))
                        {
                            Directory.CreateDirectory(temppath);
                        }

                        Console.WriteLine("writing files to disk");
                        File.WriteAllLinesAsync($"{temppath}\\proxy_info.txt", temp[0].proxy_info.Split(";"));
                        File.WriteAllLinesAsync($"{temppath}\\stream_proxy.txt",
                            temp[0].stream_proxy.Split(";"));
                        File.WriteAllLinesAsync($"{temppath}\\v_proxy.txt", temp[0].v_proxy.Split(";"));
                        File.WriteAllLinesAsync($"{temppath}\\urls.txt", temp[0].urls.Split(";"));
                        temp[0].UserData = "acc.txt";
                        Console.WriteLine("files written on disk");
                        if (temp[0].Like_P > 0) temp[0].Like = true;
                        if (temp[0].Lsave_P > 0) temp[0].Lsave = true;
                        Configuration configuration = new Configuration();
                        Console.WriteLine(JsonConvert.SerializeObject(temp[0]));
                        configuration.configId = temp[0].configId;
                        configuration.name = temp[0].name;
                        configuration.maxThreads = Convert.ToInt32(temp[0].maxThreads);
                        configuration.PPA_MAX = Convert.ToInt32(temp[0].PPA_MAX);
                        configuration.PPA_MIN = Convert.ToInt32(temp[0].PPA_MIN);
                        configuration.minPlaytime = Convert.ToInt32(temp[0].minPlaytime);
                        configuration.maxPlaytime = Convert.ToInt32(temp[0].maxPlaytime);
                        configuration.GEN_R = Convert.ToBoolean(temp[0].GEN_R);
                        configuration.song_url_list_file = Convert.ToString($"{temppath}\\urls.txt");
                        Console.WriteLine("song url list file : " + $"{temppath}\\urls.txt");
                        if (configuration.GEN_R)
                        {
                            configuration.VERIFY_GO = Convert.ToBoolean(temp[0].VERIFY_GO);
                            if (configuration.VERIFY_GO)
                            {
                                configuration.v_proxyx = Convert.ToBoolean(temp[0].v_proxyx);
                                if (configuration.v_proxyx)
                                {
                                    configuration.v_proxy = Convert.ToString($"{temppath}\\v_proxy.txt");
                                    configuration.v_proxy_p = Convert.ToString(temp[0].v_proxy_p);
                                    Console.WriteLine("vproxy file : " + $"{temppath}\\v_proxy.txt");
                                }
                            }

                            configuration.GEN_WHICH = Convert.ToBoolean(temp[0].GEN_WHICH);
                            if (configuration.GEN_WHICH)
                            {
                                configuration.GEN_DOMAIN = Convert.ToString(temp[0].GEN_DOMAIN);
                            }

                            configuration.proxy_infox = Convert.ToBoolean(temp[0].proxy_infox);
                            if (configuration.proxy_infox)
                            {
                                configuration.proxy_info = Convert.ToString($"{temppath}\\proxy_info.txt");
                                configuration.proxy_info_p = Convert.ToString(temp[0].proxy_info_p);
                                Console.WriteLine("proxyinfo file : " + $"{temppath}\\proxy_info.txt");
                            }
                        }
                        else
                        {
                            configuration.UserData = Convert.ToString(temp[0].UserData);
                        }

                        configuration.stream_proxyx = Convert.ToBoolean(temp[0].stream_proxyx);
                        if (configuration.stream_proxyx)
                        {
                            configuration.stream_proxy = Convert.ToString($"{temppath}\\stream_proxy.txt");
                            configuration.stream_proxy_p = Convert.ToString(temp[0].stream_proxy_p);
                        }

                        Console.WriteLine($"streaming proxy path : {temppath}\\stream_proxy.txt");
                        configuration.Like = Convert.ToBoolean(temp[0].Like);
                        if (configuration.Like)
                        {
                            configuration.Like_P = Convert.ToInt32(temp[0].Like_P);
                        }

                        configuration.Lsave = Convert.ToBoolean(temp[0].Lsave);
                        if (configuration.Lsave)
                        {
                            configuration.Lsave_P = Convert.ToInt32(temp[0].Lsave_P);
                        }

                        try
                        {   Console.WriteLine(JsonConvert.SerializeObject(configuration));
                            File.WriteAllText(Path.Combine(constant.currentPath, "config", configuration.name + ".txt"),
                                JsonConvert.SerializeObject(configuration, Formatting.Indented));
                            Console.WriteLine("reading config");
                            configuration.readconfig();
                            Console.WriteLine("read config");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Thread.Sleep(50000000);
                        }

                        constant.loadedconfigid = temp[0].configId;
                    }

                }
                else
                {

                    Thread.Sleep(300000);

                }
            }
        }
        }
    }