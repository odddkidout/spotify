using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using RandomNameGeneratorLibrary;

namespace SPOTIFYFINAL
{
    public class main_THREADLOOPER
    {
        static int Thread_id = 0;
        
        public main_THREADLOOPER(int Id)
        {
            Thread_id = Id;
            log.Threadlogger(Thread_id, "START" + Thread_id);
            
            while (true)
            {
                if (!run())
                {
                    log.Threadlogger(Thread_id, "FAIL--" + Thread_id);
                }
                Thread.Sleep(3000);
            }
            
        }
        public string get_email_(string GEN_DOMAIN_)
        {
            if (constant.GEN_WHICH)
            {
                var personGenerator = new PersonNameGenerator();
                string a = personGenerator.GenerateRandomFirstName();
                string b = personGenerator.GenerateRandomLastName();
                int c = new Random().Next(10, 10000);
                string email = a + b + c+"@"+GEN_DOMAIN_;
                return email;

            }
            else
            {
                var personGenerator = new PersonNameGenerator();
                string a = personGenerator.GenerateRandomFirstName();
                string b = personGenerator.GenerateRandomLastName();
                int c = new Random().Next(10, 10000);
                string email = a + b + c+"@"+a+".com";
                return email;
            }
        }

        public bool run()
        {
            string email;
            string password;
            string GEN_DOMAIN_ = "false";
            string FORWARD_EMAIL = "false";
            string bind = "false";
            try
            {
                if (constant.GEN_R)
                {
                    if (constant.GEN_WHICH)
                    {
                        var data_ = Program.GEN_DOMAIN;
                        data_.MoveNext();
                        GEN_DOMAIN_ = data_.Current.Split(":")[0];
                        FORWARD_EMAIL = data_.Current.Split(":")[1]+":"+data_.Current.Split(":")[2];
                    }
        
                    email = get_email_(GEN_DOMAIN_);
                    password = email;
                }
                else
                {
                    var user_data = Program.UserData;
                    user_data.MoveNext();
                    email = user_data.Current.Split(":")[0];
                    password = user_data.Current.Split(":")[1];
                    if (helper.proxy_auth_checker(user_data.Current))
                    {
                        bind = user_data.Current.Split(":")[2]+":"+user_data.Current.Split(":")[3];
                    }
                }
                var m = new Main(Thread_id, email+":"+password, FORWARD_EMAIL, bind);

                // account gen
                if (constant.GEN_R)
                {
                    if (!m._main())
                    {
                        log.Threadlogger(Thread_id, $"Fail To GEN With | {email} |");
                        m.force_q();
                        return false;
                    }
                    log.Threadlogger(Thread_id, $"Made | {email} |");
                }
                
                
                // streamer seen
                if (!m._streamer())
                {
                    log.Threadlogger(Thread_id, $"Fail To Stream With | {email} |");
                    m.force_q();
                    return false;
                }
                m.force_q();
                log.Threadlogger(Thread_id, $"Stream Done With | {email} |"); 
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, e.ToString());
            }
            
            return true;
        }
        

    }

    public class Main
    {
        private string _Email;
        private string _Pass;
        private int _PPA;
        private Browser _BB;
        private int Thread_id;
        private int PPA_COUNTER = 0;
        private string FORWARD_EMAIL;

        public Main(int thread, string userData, string FORWARD_EMAIL,string bind)
        {
            _PPA = new Random().Next(constant.PPA_MIN, constant.PPA_MAX);
            _Email = userData.Split(":")[0];
            _Pass = userData.Split(":")[1];
            _BB = new Browser(thread, _Email, bind);
            Thread_id = thread;
        }

        private bool _page()
        {
            int trie = 0;
            while (trie < 3)
            {
                trie++;
                if (trie == 3)
                {
                    return false;
                }
                if (!_BB.B_day())
                {
                    _BB.Refresh();
                    _BB.Gen(_Email, _Pass);
                    Thread.Sleep(5000);
                    
                }
                else
                {
                    break;
                }
            }
            
            int trie1 = 0;
            while (trie1 < 3)
            {
                trie1++;
                if (trie1 == 3)
                {
                    return false;
                }
                if (!_BB.Gender())
                {
                    _BB.Refresh();
                    _BB.Gen(_Email, _Pass);
                    _BB.B_day();
                    Thread.Sleep(5000);
                    
                }
                else
                {
                    break;
                }
            }
            
            int trie2 = 0;
            _BB.check_box_();
            while (trie2 < 3)
            {
                trie2++;
                if (trie2 == 3)
                {
                    return false;
                }
                if (!_BB.Continue_())
                {
                    _BB.Refresh();
                    _BB.Gen(_Email, _Pass);
                    _BB.B_day();
                    _BB.Gender();
                    Thread.Sleep(5000);
                    
                }
                else
                {
                    break;
                }
            }

            return true;
        }

        private void loader_()
        {
            _BB.Loader_();
        }

        public bool _main()
        {
            if (!_BB.ini)
            {
                _BB.CLOSE();
                log.Threadlogger(Thread_id,$"| ERROR WHILE OPENING THREAD RETRYING_ | {_Email} | ");
                return false;
            }

            int trie = 0;
            while (trie < 5)
            {
                trie++;
                if (!_BB.Gen(_Email, _Pass))
                {
                    _BB.CLOSE();
                    log.Threadlogger(Thread_id,$"| ERROR WHILE LOADING SIGNUP PAGE RETRYING_ | {_Email} | ");
                    return false;
                }

                if (_BB.Already_email())
                {
                    _BB.CLOSE();
                    log.Threadlogger(Thread_id,$"| ERROR ALREADY EMAIL EXIST | {_Email} | ");
                    return false;
                }

                if (!_page())
                {
                    _BB.CLOSE();
                    log.Threadlogger(Thread_id,$"| ERROR SIGNUP | {_Email} | ");
                    return false;
                }

                if (_BB.Already_email())
                {
                    _BB.CLOSE();
                    log.Threadlogger(Thread_id,$"| ERROR ALREADY EMAIL EXIST | {_Email} | ");
                    return false;
                }

                if (!_BB.Done__())
                {
                    _BB.Refresh();
                    if (trie == 2)
                    {
                        _BB.CLOSE();
                        return false;
                    }
                    continue;
                    
                }
                break;
            }
            
            // save email func
            if (constant.AGE)
            {
                Thread.Sleep(5000);
                loader_();
                try
                {
                    int time_ = new Random().Next(constant.MIN, constant.MAX);
                    log.Threadlogger(Thread_id,$"| NOW AGING FOR {time_} | {_Email} | ");
                    Thread.Sleep(time_*1000);
                }
                catch (Exception e)
                {
                    log.Threadlogger(Thread_id, e.ToString());
                }
            }

            if (constant.VERIFY_GO)
            {
                log.Threadlogger(Thread_id,$"| NOW VERIFYING | {_Email} | ");
                int ver = 1;
            }
            else
            {
                int ver = 4;
            }

            return true;
        }

        public bool _streamer()
        {
            // change proxy to stream if yes
            if (constant.stream_proxyx && constant.proxy_infox)
            {
                _BB.kill_node();
                var x = Program.stream_proxy;
                x.MoveNext();

                if (!_BB.Node_auth(x.Current))
                {
                    _BB.ini = false;
                }
                Thread.Sleep(9000);

            }
            
            if (!_BB.ini)
            {
                _BB.CLOSE();
                log.Threadlogger(Thread_id,$"| ERROR WHILE OPENING THREAD RETRYING_ | {_Email} | ");
                return false;
            }

            if (!constant.GEN_R)
            {
                if (_BB.session_ind)
                {
                    if (!_BB.Session())
                    {
                        if (!_BB.Login_loop(_Email, _Pass))
                        {
                            _BB.CLOSE();
                            return false;
                        }
                    }
                }
                else
                {
                    if (!_BB.Login_loop(_Email, _Pass))
                    {
                        _BB.CLOSE();
                        return false;
                    }
                }
                
            }

            int trie = 0;
            string[] url_links = File.ReadAllLines(constant.song_url_list_file);

            int index = 0;
            foreach (var urlx in url_links)
            {
                index++;
                int a_full_songs_ = 0;
                List<int>  songs_list = new List<int>();
                
                if (trie == 3)
                {
                    _BB.CLOSE();
                    return false;
                }

                int which_url = helper.url_checker(urlx);
                string url = urlx;
                if (urlx.Contains(":play_only="))
                {
                    string _list = urlx.Split(":play_only=")[1];
                    foreach (string i in _list.Split(","))
                    {
                        songs_list.Add(Convert.ToInt32(i));
                    }
                    url = urlx.Split(":play_only=")[0];
                }
                else
                {
                    songs_list.Add(1);
                    a_full_songs_ = 1;
                }

                // url loader
                int tries = 0;
                while (tries < 3)
                {
                    tries++;
                    if (tries == 3)
                    {
                        break;
                    }

                    if (index == 1)
                    {
                        _BB.Refresh();
                        _BB.song_url_loaded_check();
                    }
                    
                    if (!_BB.url_loader_loop(url))
                    {
                        log.Threadlogger(Thread_id, $"HAVING ERROR while loading this url: {url}");
                    }
                    else
                    {
                        log.Threadlogger(Thread_id, $"URL LOADED {url}");
                        break;
                    }
                }
                if (tries == 3)
                {
                    trie++;
                    continue;
                }
                
                // pre scroll
                int tries1 = 0;
                while (tries1 < 3)
                {
                    tries1++;
                    if (tries1 == 3)
                    {
                        break;
                    }

                    if (!_BB.pre_scroll_loop(url))
                    {
                        log.Threadlogger(Thread_id, $"UNABLE TO SCROLL THROUGH PAGE :{url}");
                        
                    }
                    else
                    {
                        if (!_BB.Post_check())
                        {
                            _BB.url_loader_loop(url);
                        }
                        break;
                    }
                }
                if (tries1 == 3)
                {
                    trie++;
                    continue;
                }
                
                // MAIN LOOP START
                if (!song_play_loop(songs_list, which_url, url, a_full_songs_))
                {
                    log.Threadlogger(Thread_id, $" ERROR :{url}");
                }
                log.Threadlogger(Thread_id, $"successfully played :{url}");

                if (PPA_COUNTER >= _PPA)
                {
                    _BB.CLOSE();
                    return true;
                }
                
            }
            
            _BB.CLOSE();
            return true;
        }

        private bool song_play_loop(List<int> songs_list, int which_url, string url, int a_full_songs)
        {
            // loop to click
            foreach (var number in songs_list)
            {
                if (PPA_COUNTER >= _PPA)
                {
                    return true;
                }

                // click func
                int trie = 0;
                while (trie < 3)
                {
                    trie++;
                    if (trie == 3)
                    {
                        break;
                    }
                    if (!_BB.song_clicker_loop(number, url))
                    {
                        _BB.ad_clicker();
                        log.Threadlogger(Thread_id, $"PROBLEM BROWSER WHILE PLAYING SONG {number} :{url}");
                        _BB.url_loader_loop(url);
                        Thread.Sleep(10000);
                        _BB.pre_scroll_loop(url);
                        continue;
                    }
                    
                    Thread.Sleep(5000);
                    if (!_BB.play_checker_loop("bearer"))
                    {
                        _BB.ad_clicker();
                        _BB.Refresh();
                        log.Threadlogger(Thread_id, $"PROBLEM BROWSER WHILE CHECKING IS SONG IS PLATING {number} ON {url}");
                        _BB.url_loader_loop(url);
                        Thread.Sleep(10000);
                        _BB.pre_scroll_loop(url);
                        
                    }
                    break;
                }if (trie == 3)
                {
                    continue;
                }

                string Song_name = _BB.song_name(number);
                if (Song_name != "false")
                {
                    log.Threadlogger(Thread_id, $"PLAYING {Song_name}| {number} ON |{url}");
                }
                else
                {
                    log.Threadlogger(Thread_id, $"PLAYING {number} ON |{url}");
                }

                int wait_for = new Random().Next(constant.MIN_, constant.MAX_);
                Thread.Sleep(wait_for*1000);
                PPA_COUNTER++;
            }

            return true;
        }

        public void force_q()
        {
            try
            {
                _BB.CLOSE();
            }
            catch (Exception e)
            {
                log.Threadlogger(Thread_id, e.ToString());
            }
        }

        public void test()
        {
            _BB.Refresh();
            _BB.logp();
        }

    }
}