using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

// ReSharper disable InconsistentNaming

namespace PMES.Manual.Net6.Core;

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

    public async Task<T?> Get<T>(string url) where T : class
    {
        Logger?.Verbose($"访问Api url:{url}");
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie",
                "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = await _httpClient.SendAsync(request, new CancellationTokenSource(1500).Token);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync(new CancellationTokenSource(1500).Token);
            var product = JsonConvert.DeserializeObject<T>(res);
            return product;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    public T? Get1<T>(string url) where T : class
    {
        Logger?.Verbose($"访问Api url:{url}");
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie",
                "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var res = response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<T>(res.Result);
            return product;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    public async Task<JObject> GetJObject(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie",
                "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var jobject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(res);
            return jobject;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    public async Task<Tuple<bool, string>> GetJObjectValidate(string url)
    {
        Logger?.Verbose($"访问Api url:{url}");
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie",
                "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new Tuple<bool, string>(true, "");
            }

            //response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            var jobject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(res);
            var detail = jobject?["detail"].First.ToString().Replace('[', ' ').Replace(']', ' ');
            return new Tuple<bool, string>(false, detail);
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return new Tuple<bool, string?>(false, "ApiError");
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
            var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1500));
            var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1500));
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var collection = param.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value)).ToList();
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await _httpClient.SendAsync(request, cts1.Token);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync(cts2.Token);
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
            Logger?.Error(exception.Message);
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
            Logger?.Error(exception.Message);
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
            dynamic responseT = JsonConvert.DeserializeObject<T>(responseStr) ?? throw new InvalidOperationException();
            return responseT.status.code != 200 ? null : (T?)responseT;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return null;
        }
    }

    #endregion

    #region 后台接口

    /// <summary>
    ///     拆垛区域清垛
    /// </summary>
    /// <returns></returns>
    public async Task<bool> EmptyTrayUnStacking(int position)
    {
        try
        {
            Logger?.Verbose($"调用接口[EmptyTrayUnStacking],params:{position}");
            string pos = position.ToString();
            //var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.3.248:8089//api/unstacking/emptyPalletRecycling");
            var request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.16.3.248:8089//api/unstacking/emptyPalletRecycling");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("IsContinuedFeed", "2"));
            collection.Add(new("unstackingWorkshopId", pos));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();

            //var request = new HttpRequestMessage(HttpMethod.Post, "/api/unstacking/emptyPalletRecycling");
            //var collection = new List<KeyValuePair<string, string>>();
            //collection.Add(new("IsContinuedFeed", "2"));
            //collection.Add(new("unstackingWorkshopId", "-1"));
            //var content = new FormUrlEncodedContent(collection);
            //request.Content = content;
            //var response = await _httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Logger?.Error(e.Message);
            return false;
        }

        return true;
    }


    /// <summary>
    ///     TODO:暂未实现，组子母托完毕之后，扫码需要把母托盘编码发送给后台。后台收到此接口直接调度agv放到缓冲区
    /// </summary>
    /// <param name="workShopId"></param>
    /// <param name="borCode"></param>
    /// <returns></returns>
    public async Task<bool> PostMotherTrayCode(int childWorkshopld, int combiateWorkshopld, string motherTrayBarcode,
        int motherWorkshopld)
    {
        try
        {
            Logger?.Verbose(
                $"调用接口[PostMotherTrayCode],params:{childWorkshopld}，{combiateWorkshopld}，{motherTrayBarcode}，{motherWorkshopld}");
            var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.MotherTrayBarCode);
            var collection = new List<KeyValuePair<string, string>>();

            collection.Add(new("childWorkshopId", $"{childWorkshopld}"));
            collection.Add(new("combiateWorkshopId", $"{combiateWorkshopld}"));
            collection.Add(new("motherTrayBarcode", $"{motherTrayBarcode}"));
            collection.Add(new("motherWorkshopId", $"{motherWorkshopld}"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return false;
        }
    }

    /// <summary>
    ///     申请入库
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool ApplyTray2Storage(int position)
    {
        try
        {
            Logger?.Verbose($"调用接口[ApplyTray2Storage],params:{position}");
            string pos = position.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.16.3.248:8089/api/stacking/boxMaterialOfTrayMatIntoWMS");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("stackingWorkshopId", $"{position}"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();

            //var request = new HttpRequestMessage(HttpMethod.Post, "http://172.16.3.248:8089/api/stacking/boxMaterialOfTrayMatIntoWMS");
            //var collection = new List<KeyValuePair<string, string>>();
            //collection.Add(new("stackingWorkshopId", $"{position}"));
            //var content = new FormUrlEncodedContent(collection);
            //request.Content = content;
            //var response = await _httpClient.Send(request);
            //response.EnsureSuccessStatusCode();
            var responseStr = response.Content.ReadAsStringAsync().Result;
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            Logger?.Error(exception.Message);
            return false;
        }
    }

    public bool ClearAndApplyPageBoard()
    {
        try
        {
            Logger?.Verbose($"调用接口[ClearAndApplyPageBoard],params:null");
            var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.RecycleFeedBackEmptyTray);
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("emptyTrayWorkshopId", "-1"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var responseStr = response.Content.ReadAsStringAsync().Result;
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Logger?.Error(e.Message);
            return false;
        }
    }

    public bool ClearErrorStack(int workId)
    {
        try
        {
            Logger?.Verbose($"调用接口[ClearErrorStack],params:{workId}");
            //var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.ApplyExcludePosition);
            var request =
                new HttpRequestMessage(HttpMethod.Post, "http://172.16.3.248:8089/api/stacking/excludePosition");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("emptyTrayWorkshopId", workId.ToString()));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var responseStr = response.Content.ReadAsStringAsync().Result;
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Logger?.Error(e.Message);
            return false;
        }
    }

    public bool ChangeStack(string delivery_sub_tray_spec, string xpzl_spec)
    {
        try
        {
            Logger?.Verbose($"调用接口[ChangeStack],params:{delivery_sub_tray_spec},{xpzl_spec}");
            //var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.ApplyExcludePosition);
            var request = new HttpRequestMessage(HttpMethod.Post, ApiUrls.ChangeStack);
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("delivery_sub_tray_spec", delivery_sub_tray_spec));
            collection.Add(new("xpzl_spec", xpzl_spec));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = _httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var responseStr = response.Content.ReadAsStringAsync().Result;
            var responseStruct = JsonConvert.DeserializeObject<ResponseStruct>(responseStr);
            var jObject = (JObject)JsonConvert.DeserializeObject(responseStr)!;

            if (!int.TryParse(jObject["status"]!["code"]!.ToString(), out int code))
            {
                return false;
            }

            if (code != 200)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Logger?.Error(e.Message);
            return false;
        }
    }

    #endregion
}

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

    //public static string QueryOrder = "http://172.16.3.130:30358/api/product-info?semi_finished=";
    public static string QueryOrder = "https://test-chengzhong-api.xiandeng.com:3443/api/product-info?semi_finished=";
    public static string ValidateOrder = "http://172.16.3.130:30358/api/product-validate?";

    //public static string QueryOrder = "https://test-chengzhong-api.xiandeng.com:3443/api/product-info?semi_finished=";
    //public static string ValidateOrder = "https://test-chengzhong-api.xiandeng.com:3443/api/product-validate?";

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
    //public static string Login = "http://8.142.72.79:8089/api/operator/login";
    public static string Login = "http://172.16.3.248:8089/api/operator/login";

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

    #region 后台接口

    /// <summary>
    ///     生产环境：http://8.142.72.79/
    /// </summary>
    public static string BaseUrl = "http://172.16.3.248:8089";

    /// <summary>
    ///     子母托区域--子母托存入缓存区工位
    /// </summary>
    public static string MotherTrayBarCode = $"{BaseUrl}/api/combination/scanMotherTrayBarcodeIntoCacheArea";


    /// <summary>
    ///     拆垛区域---空托盘回收
    /// </summary>
    public static string UnStackingEmptyTry = $"{BaseUrl}/api/unstacking/emptypalletRecycling";


    /// <summary>
    ///     码垛区域---申请入库
    /// </summary>
    public static string StackingApply2Storage = $"{BaseUrl}/api/stacking/boxMaterialOfTrayMatIntoWMS";

    /// <summary>
    ///     码垛区域---[码垛工位]呼叫子母托盘（不应该调用）
    /// </summary>
    public static string ApplyTry2Stacking = $"{BaseUrl}/api/stacking/applyChildMotherTray";

    /// <summary>
    ///     码垛区域---[码垛工位] - [换垛], 申请一个码垛工位数据的业务逻辑
    /// </summary>
    public static string ChangeStack = $"{BaseUrl}/api/stacking/changeStack";

    /// <summary>
    ///     码垛区域---回收剔除位
    /// </summary>
    public static string RecycleFeedBackEmptyTray = $"{BaseUrl}/api/stacking/feedBackEmptyTray";


    /// <summary>
    ///     码垛区域---回收纸板空托盘（不应该调用）
    /// </summary>
    public static string ApplyExcludePosition = $"{BaseUrl}/api/stacking/excludePosition";


    /// <summary>
    ///     码垛区域---申请纸板（不应该调用）
    /// </summary>
    public static string ApplyPageBoard = $"{BaseUrl}/api/stacking/pageBoard";


    /// <summary>
    ///     码垛区域---发起码垛任务（不应该调用）
    /// </summary>
    public static string ApplySendStackingTask = $"{BaseUrl}/api/stacking/excludePosition";

    #endregion
}