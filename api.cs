using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class api
    {
        public static server thisserver;
        public static ipdetails res;
        public static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        public static string botmasterdomain = "http://127.0.0.1:8000";
        
        public api(
            string url,int delay)
        {if (botmasterdomain != "" && botmasterdomain != null)
            {
                botmasterdomain = url;
            }
            initialise().Wait();
            Console.WriteLine(res.ip);
            getServerList().Wait();
            Thread thread = new Thread(() => getConfigList(delay));
            thread.Start();

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

        static async void getConfigList(int delay)
        {
            while(true){
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(botmasterdomain+"/api/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync($"config?serverid={thisserver.id}");
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.Content.ReadAsStringAsync().Result != "[]")
                        {
                            

                            List<Configuration> temp =
                                JsonConvert.DeserializeObject<List<Configuration>>(response.Content.ReadAsStringAsync()
                                    .Result);
                            if (constant.loadedconfigid != null && constant.loadedconfigid != temp[0].configId)
                            {
                                temp[0].UserData = "acc.txt";
                                if (temp[0].Like_P > 0) temp[0].Like = true;
                                if (temp[0].Lsave_P > 0) temp[0].Lsave = true;
                                
                            }
                            
                            Configuration configuration = new Configuration();
                            Console.WriteLine(JsonConvert.SerializeObject(temp[0]));
                            
                            Environment.Exit(0);
                            temp[0].readconfig();
                            /*Console.WriteLine(temp[0].name);
                            constant.Thread = temp[0].maxThreads;
                            constant.PPA_MAX = temp[0].PPA_MAX;
                            constant.PPA_MIN = temp[0].PPA_MIN;
                            constant.MIN_ = temp[0].minPlaytime;
                            constant.MAX_ = temp[0].maxPlaytime;
                            constant.GEN_R = temp[0].GEN_R;
                            constant.urls = temp[0].urls;
                            if (temp[0].GEN_R)
                            {
                                constant.VERIFY_GO = temp[0].VERIFY_GO;
                                constant.GEN_WHICH = temp[0].GEN_WHICH;
                                if (temp[0].GEN_WHICH) constant.GEN_DOMAIN = temp[0].GEN_DOMAIN;
                                constant.proxy_infox = temp[0].proxy_infox;
                                if (temp[0].proxy_infox)
                                {
                                    constant.proxy_info = temp[0].proxy_info;
                                    constant.proxy_info_p = temp[0].proxy_info_p;
                                }
                            }
            
                            constant.stream_proxyx = temp[0].stream_proxyx;
                            if (temp[0].stream_proxyx)
                            {
                                constant.stream_proxy = temp[0].stream_proxy;
                                constant.stream_proxy_p = temp[0].stream_proxy_p;
                            }
            
                            constant.v_proxyx = temp[0].stream_proxyx;
                            if (temp[0].v_proxyx)
                            {
                                constant.v_proxy = temp[0].stream_proxy;
                                constant.v_proxy_p = temp[0].stream_proxy_p;
                            }
            
                            
                            if (temp[0].Like_P>0)
                            {
                                constant.Like = true;
                                constant.Like_P = temp[0].Like_P;
                            }
            
                            
                            if (temp[0].Lsave_P>0)
                            {
                                constant.Lsave = true;
                                constant.Lsave_P = temp[0].Lsave_P;
                            }
                            */

                            return;
                        }
                        else
                        {
                            
                            Thread.Sleep(300000);
                            
                        }
                    }
                } 
                Thread.Sleep(delay*6000);
            }
        }
    }
}