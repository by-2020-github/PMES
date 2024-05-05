using System.Net.Http;
using DevExpress.DataAccess.Native.Web;
using DevExpress.Skins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

// ReSharper disable InconsistentNaming

namespace PMES.Core;

public class WebService
{
    private static readonly Lazy<WebService> Holder = new(() => new WebService());
    private readonly HttpClient _httpClient;

    private WebService()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = delegate { return true; };
        _httpClient = new HttpClient(handler);
    }

    public static ILogger? Logger { get; set; }
    public static WebService Instance => Holder.Value;

    public async Task<T> Get<T>(string url) where T : class
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie",
                "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<T>(res);
            return product;
        }
        catch (Exception exception)
        {
            Logger.Error(exception.Message);
            return null;
        }
    }


    public async Task<ResponseStruct?> Post(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Cookie",
                @"csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ResponseStruct>(res);
            return product;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    /// <summary>
    ///     发送post请求
    ///     body:x-www-form-urlencoded 
    /// </summary>
    /// <param name="param"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> Post<T>(Dictionary<string, string> param, string url) where T : class, new()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var collection = param.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value)).ToList();
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return null;
            }

            if (code != 200)
            {
                return null;
            }

            var data = jObject["data"]!.ToString();

            var res = JsonConvert.DeserializeObject<T>(data);

            return res;
        }
        catch (Exception exception)
        {
            Logger.Error(exception.Message);
            return null;
        }
    }

    /// <summary>
    ///     发送post请求
    ///     body:json对象
    /// </summary>
    /// <param name="body"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> Post<T>(object body, string url) where T : class, new()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return null;
            }

            if (code != 200)
            {
                return null;
            }

            var data = jObject["data"]!.ToString();

            var res = JsonConvert.DeserializeObject<T>(data);

            return res;
        }
        catch (Exception exception)
        {
            Logger.Error(exception.Message);
            return null;
        }
    }

    public async Task<UploadTemplateRes?> UploadReportTemplate<T>(MultipartFormDataContent content, string url)
    {
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var responseStr = await response.Content.ReadAsStringAsync();
        var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;
        if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
        {
            return null;
        }

        if (code != 200)
        {
            return null;
        }

        var data = jObject["data"]!.ToString();

        var res = JsonConvert.DeserializeObject<T>(data);

        return null;
    }

    #region 直接返回泛型结构体

    /// <summary>
    ///     发送post请求
    ///     body:json对象
    /// </summary>
    /// <param name="body"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> PostJsonT<T>(object body, string url) where T : class, new()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            dynamic responseT = JsonConvert.DeserializeObject<T>(responseStr) ?? throw new InvalidOperationException();

            return responseT.status.code != 200 ? null : (T?)responseT;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }
    
    /// <summary>
    ///     发送post请求
    ///     body:x-www-form-urlencoded 
    /// </summary>
    /// <param name="param"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T?> PostFormT<T>(Dictionary<string, string> param, string url) where T : class, new()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var collection = param.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value)).ToList();
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            dynamic responseT=  JsonConvert.DeserializeObject<T>(responseStr) ?? throw new InvalidOperationException();
            return responseT.status.code != 200 ? null : (T?)responseT;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    #endregion

}

#region post 请求体

/// <summary>
///     验货== ok:particularBillDto
///     接口地址 /api/weight/confirmParticular
/// </summary>
public class CheckProduct
{
    public int checkMethod { get; set; } = 0;
    public int checkPurchaseNumConsistentToRealMateNum { get; set; } = 0;
    public int checkQackaging { get; set; } = 0;
    public int checkQualityBetweenPurchaseAndRealMate { get; set; } = 0;
    public int defectRateValue { get; set; } = 0;
    public string pictures { get; set; } = "";
    public int receiptDetailId { get; set; } = 0;
    public int receiptId { get; set; } = 0;
    public string remark { get; set; } = "";
    public int state { get; set; } = 0;
    public string totalNum { get; set; } = "";
    public int totalRealCheckNum { get; set; } = 0;
    public string userId { get; set; } = "";
}

public class ResponseStruct
{
    public string data { get; set; } = "";
    public status status { get; set; }
}

public class status
{
    public int code { get; set; }
    public string statusMsg { get; set; }
}

/// <summary>
///     上传模板后返回值
/// </summary>
public class UploadTemplateRes
{
    public string fileName { get; set; }
    public int id { get; set; }
}

public static class ApiUrls
{
    #region erp 查询订单

    public static string QueryOrder =
        "https://test.chengzhong-api.site.xiandeng.com:3443/api/product-info?semi_finished=";

    #endregion

    #region 人工线管理

    /// <summary>
    ///     改线入库
    /// </summary>
    public static string ChangePreheaterInStock = "http://8.142.72.79:8089/api/manual/chagePreheaterInStock";

    /// <summary>
    ///     装箱打印
    /// </summary>
    public static string PackingPrinting = "http://8.142.72.79:8089/api/manual/packingPrinting";

    #endregion

    #region 操作工管理

    /// <summary>
    ///     登录
    /// </summary>
    public static string Login = "http://8.142.72.79:8089/api/operator/login";

    #endregion

    #region 条码管理

    /// <summary>
    ///     子母合托
    /// </summary>
    public static string BarcodeMerger = "http://8.142.72.79:8089/api/barcode/childMotherTrayToCocare ";

    /// <summary>
    ///     删除箱码
    /// </summary>
    public static string BarcodeDeleteBoxCode = "http://8.142.72.79:8089/api/barcode/deleteBoxCode";

    /// <summary>
    ///     删除盘码
    /// </summary>
    public static string BarcodeDeletePreheaterCode = "http://8.142.72.79:8089/api/barcode/deletePreheaterCode";

    /// <summary>
    ///     生成箱码
    /// </summary>
    public static string BarcodeGenerateBoxCode = "http://8.142.72.79:8089/api/barcode/generateBoxCode";

    /// <summary>
    ///     生成盘码
    /// </summary>
    public static string BarcodeGeneratePreheaterCode = "http://8.142.72.79:8089/api/barcode/generatePreheaterCode";

    /// <summary>
    ///     查询历史
    /// </summary>
    public static string BarcodeSearchHis = "http://8.142.72.79:8089/api/barcode/searchList";

    #endregion

    #region 标签管理

    /// <summary>
    ///     人工线-标签详情
    /// </summary>
    public static string LabelDetail = "http://8.142.72.79:8089/api/label/getLabelDetail";

    /// <summary>
    ///     人工线-标签列表
    /// </summary>
    public static string LabelManualLineList = "http://8.142.72.79:8089/api/label/getMaualLineOFLabel";

    /// <summary>
    ///     侧贴码-箱码
    /// </summary>
    public static string LabelBoxCode = "http://8.142.72.79:8089/api/label/labelForBoxCode";

    /// <summary>
    ///     顶帖码-盘码
    /// </summary>
    public static string LabelTopCode = "http://8.142.72.79:8089/api/label/labelForPreheaterCode";

    /// <summary>
    ///     上传标签模板
    /// </summary>
    public static string LabelUploadLabelTemplate = "http://8.142.72.79:8089/api/label/pcUploadLabelTemplate";

    #endregion

    #region 文档管理

    public static string ChangedOldToNew = "http://8.142.72.79:8089/api/manual/chagePreheaterInStock";

    #endregion
}

#endregion