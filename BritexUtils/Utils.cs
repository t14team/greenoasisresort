using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace britex
{
    public static class Utils
    {
        private static HttpClient client = new HttpClient();
        private static Random rnd = new Random();

        public static async Task<string> Check(HttpRequest request, string country, string site, string gclid = "")
        {
            if (!string.IsNullOrEmpty(gclid))
            {
                gclid += "||";
            }

            string ipToCheck = "";

            if (!string.IsNullOrEmpty(request.Headers["CF-Connecting-IP"].ToString()))
                ipToCheck = request.Headers["CF-Connecting-IP"].ToString();
            else if (!string.IsNullOrEmpty(request.Headers["X-Client-IP"].ToString()))
                ipToCheck = request.Headers["X-Client-IP"].ToString();
            else if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-For"].ToString()))
                ipToCheck = request.Headers["X-Forwarded-For"].ToString();
            else
            {
                logClick("NoIp", "-", false, gclid + "-", site + "-" + country);
                return "";
            }

            string useragent = request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(useragent))
            {
                logClick(ipToCheck, "-", false, gclid + "NoUserAgent", site + "-" + country);
                return "";
            }

            List<string> Crawlers = new List<string>()
            {"bot","spy","crawl","google","ads","python","headless"};
            bool iscrawler = Crawlers.Exists(x => useragent.ToLower().Contains(x));
            if (iscrawler)
            {
                logClick(ipToCheck, "-", false, gclid + "Crwl. - " + useragent, site + "-" + country);
                return "";
            }
             else if (!useragent.ToLower().Contains("mobile"))
            {
                      logClick(ipToCheck, "-", false, gclid + "ManualCheckDetectedmobil. - " + useragent, site + "-" + country);
                   Console.WriteLine("mobil");
                return "";
            }
         
           

            try
            {
                string ipResponse = await client.GetStringAsync($"https://api.ipdata.co/{ipToCheck}?api-key=fbeea6d0e99b32008f3271c834032c85ecca57a3a630e6402caff806");
                IpData ipData = JsonConvert.DeserializeObject<IpData>(ipResponse);

                if (ipResponse.ToLower().Contains("google"))
                {
                    logClick("Ggl. - " + ipToCheck, ipData.country_name, false, gclid + useragent, site + "-" + country);
                    return "";
                }

                if (ipResponse.ToLower().Contains("bing"))
                {
                    logClick("Bing. - " + ipToCheck, ipData.country_name, false, gclid + useragent, site + "-" + country);
                    return "";
                }
                if (ipData.threat.is_datacenter || ipData.threat.is_vpn || ipData.threat.scores.trust_score < 10)
                {
                    logClick("Vpn,Prx,Thr. - " + ipToCheck, ipData.country_name, false, gclid + useragent, site + "-" + country);
                    return "";
                }

                if (!country.Contains(ipData.country_name.ToLower()))
                {
                    logClick(ipToCheck, "Msm. - " + ipData.country_name, false, gclid + useragent, site + "-" + country);
                    return "";
                }

                int userId = logClick(ipToCheck, ipData.country_name, true, gclid + useragent, site + "-" + country);
                return ipData.country_name.ToLower() + "||" + userId.ToString();
            }
            catch (Exception)
            {
                logClick(ipToCheck, "IpData - Exception", false, gclid + useragent, site + "-" + country);
                return "";
            }

        }

        public static int logClick(string ip, string country, bool showrealcontent, string useragent, string site)
        {
            int id;
            SqlConnection conn = new SqlConnection("Data Source=bbps.database.windows.net;Database=bbps314;user id=bbps;password=Britex2020@;\n");
            conn.Open();
            using (SqlCommand command = new SqlCommand($"insert into Logs output INSERTED.Id Values('{ip}','{country}',{Convert.ToByte(showrealcontent).ToString()},'{useragent}',GETDATE(),0,'{site}');", conn))
            {
                id = (int)command.ExecuteScalar();
            }
            conn.Close();
            return id;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
