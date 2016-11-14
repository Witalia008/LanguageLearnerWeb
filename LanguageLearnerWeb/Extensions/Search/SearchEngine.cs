using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LanguageLearnerWeb.Extensions.Search
{
    public abstract class SearchEngine
    {
        protected SearchEngine() { }

        private static object locker = new object();

        private static Bing bing = null;
        public static SearchEngine BingInstance
        {
            get
            {
                lock (locker)
                {
                    if (bing == null)
                    {
                        bing = new Bing();
                    }
                }
                return bing;
            }
        }

        private static Yandex yandex = null;
        public static SearchEngine YandexInstance
        {
            get
            {
                lock (locker)
                {
                    if (yandex == null)
                    {
                        yandex = new Yandex();
                    }
                }
                return yandex;
            }
        }

        public async virtual Task<string> ImageSingleAsync(string text)
        {
            return "";
        }

        public async virtual Task<List<string>> ImagesAsync(string text, int count)
        {
            return new List<string>();
        }

        public async virtual Task<List<string>> TranslateAsync(
            string text, string langFrom, string langTo)
        {
            return new List<string>();
        }
    }
}