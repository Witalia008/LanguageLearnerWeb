using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LanguageLearnerWeb.Extensions.Search
{
    public class Yandex : SearchEngine
    {
        private const string request_url =
            "https://translate.yandex.net/api/v1.5/tr.json/translate";
        private const string key =
            "trnsl.1.1.20160321T160539Z.dacf231b32bd8fd2.e2cfbd9c65ae288726e1d378fadd1b5833f362d9";

        private object locker = new object();

        private HttpClient client = null;
        private HttpClient Client
        {
            get
            {
                lock (locker)
                {
                    if (client == null)
                    {
                        client = new HttpClient();
                    }
                }
                return client;
            }
        }

        public override async Task<List<string>> TranslateAsync(
            string text, string langFrom, string langTo)
        {
            List<string> res = new List<string>();
            try
            {
                var uri = new Uri(request_url);
                var parameters = new Dictionary<string, string>{
                                 {"key", key},
                                 {"lang", langFrom + "-" + langTo},
                                 {"format", "plain"},
                                 {"text", text.ToLower()}
                             };
                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                var result = await Client.PostAsync(uri, content);
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(json);
                    if (obj != null || obj.code == 200)
                    {
                        res = obj.text.ToObject<List<string>>();
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return res;
        }
    }
}