using System;
using System.Collections.Generic;
using System.Linq;

namespace SPOTIFYFINAL
{
    public class helper
    {
        Random _random = new Random();
        
        public int RandomNumber(int min, int max) {
            return _random.Next(min, max);
        }
        

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
            while (true) foreach(int i in Enumerable.Range(9222,11111)) yield return i;
        }
    }
    
}