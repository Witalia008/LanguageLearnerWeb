using LanguageLearnerWeb.Extensions.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace LanguageLearnerWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        // GET: api/Search/Images?count={count}&text={text}
        [ResponseType(typeof(List<string>))]
        [Route("Images")]
        public async Task<IHttpActionResult> GetImages(int count, string text)
        {
            var res = await SearchEngine.BingInstance.ImagesAsync(text, count);
            if (res == null || res.Count == 0)
            {
                return NotFound();
            }
            return Ok(res);
        }

        // GET: api/Search/Image?text={text}
        [ResponseType(typeof(string))]
        [Route("Image")]
        public async Task<IHttpActionResult> GetImage(string text)
        {
            var res = await SearchEngine.BingInstance.ImageSingleAsync(text);
            if (res == null || res == "")
            {
                return NotFound();
            }
            return Ok(res);
        }

        // GET: api/Search/Translate?langFrom={langFrom}&langTo={langTo}&text={text}
        [ResponseType(typeof(List<string>))]
        [Route("Translate")]
        public async Task<IHttpActionResult> GetTranslation(
            string langFrom, string langTo, string text)
        {
            var res = await SearchEngine.BingInstance.TranslateAsync(text, langFrom, langTo);
            if (res != null)
            {
                res.AddRange(await SearchEngine.YandexInstance.TranslateAsync(text, langFrom, langTo));
            }
            if (res != null && res.Count == 0)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
