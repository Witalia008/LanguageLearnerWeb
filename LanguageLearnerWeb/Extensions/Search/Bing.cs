using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LanguageLearnerWeb.Extensions.Search
{
    public class Bing : SearchEngine
    {
        const string AZURE_KEY = "Fth01yCpTtX+FTsrguFKd7SVsmTjU59/4bwm2YFE9Lo";
        const string URL = "https://api.datamarket.azure.com/Bing/Search/";
        const string IMAGE_URL = URL + "v1/Image?Query=%27{0}%27&$top={1}&$format=Json&ImageFilters=%27Size%3AMedium%2BAspect%3ASquare%27";
        const string TRANS_URL = "https://api.datamarket.azure.com/Bing/MicrosoftTranslator/v1/Translate?Text=%27{0}%27&From=%27{1}%27&To=%27{2}%27&$format=json";

        private object locker = new object();

        private HttpClientHandler handler = null;
        private HttpClientHandler Handler
        {
            get
            {
                lock (locker)
                {
                    if (handler == null)
                    {
                        handler = new HttpClientHandler()
                        {
                            Credentials = new NetworkCredential(AZURE_KEY, AZURE_KEY)
                        };
                    }
                }
                return handler;
            }
        }

        private HttpClient client = null;
        private HttpClient Client
        {
            get
            {
                lock (locker)
                {
                    if (client == null)
                    {
                        client = new HttpClient(Handler);
                    }
                }
                return client;
            }
        }

        public override async Task<string> ImageSingleAsync(string text)
        {
            return (await ImagesAsync(text, 1)).FirstOrDefault();
        }

        public override async Task<List<string>> ImagesAsync(string text, int count)
        {
            var results = new List<string>();
            try
            {
                string bingSearch = String.Format(IMAGE_URL, text, count);
                var res = await Client.GetAsync(new Uri(bingSearch));

                if (res.IsSuccessStatusCode)
                {
                    var str = await res.Content.ReadAsStringAsync();

                    JObject resObj = JObject.Parse(str);
                    List<JToken> resArr = resObj["d"]["results"].Children().ToList();
                    foreach (var r in resArr)
                    {
                        //System.Diagnostics.Debug.WriteLine(r["MediaUrl"].ToString());
                        results.Add(r["MediaUrl"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return results;

        }

        public override async Task<List<string>> TranslateAsync(
            string text, string langFrom, string langTo)
        {
            List<string> results = new List<string>();
            try
            {
                string bingTrans = String.Format(TRANS_URL, text, langFrom, langTo);
                var res = await Client.GetAsync(new Uri(bingTrans));

                if (res.IsSuccessStatusCode)
                {
                    var str = await res.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(str);
                    List<JToken> jsonArr = json["d"]["results"].Children().ToList();
                    foreach (var r in jsonArr)
                    {
                        results.Add(r["Text"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return results;
        }
    }
}