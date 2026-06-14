using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace littleworldadvent.BritexUtils
{
    public static class UrilisResult
    {
        public static async Task<(bool, string)> Check(HttpRequest request, string country, string site, string gclid = "")
        {
            string result = await britex.Utils.Check(request, country, site, gclid);
            string userId = "";
            string countryresult = "";
            if (result.Contains("||"))
            {
                countryresult = result.Split("||")[0];
                userId = result.Split("||")[1];
            }
            return (country == countryresult, userId);
        }
    }
}
