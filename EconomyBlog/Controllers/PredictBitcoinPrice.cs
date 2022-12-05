using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("predict_bitcoin_price")]
public class PredictBitcoinPriceController : Controller
{
    [HttpGET]
    public static ActionResult GetPredictBitcoinPricePage(Guid sessionId, string path) => ProcessStatic("predict_bitcoin_price", path);
}