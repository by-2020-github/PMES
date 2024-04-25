using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MySqlX.XDevAPI;

namespace PMES.Core
{
    public class HttpHelper
    {
        private static readonly object LockObj = new object();
        private static HttpClient client;
        public static HttpHelper Instance => new HttpHelper();

        private HttpHelper()
        {
            client = new HttpClient() { BaseAddress = new Uri("https://test.chengzhong-api.site.xiandeng.com:3443/api/") };

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object sender, X509Certificate certificate, X509Chain
                    chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="strJson">传入的数据</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage res = await client.PostAsync("product-info?semi_finished=G24040656G190008", content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = await res.Content.ReadAsStringAsync();
                    return resMsgStr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 同步Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public string Post(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //client.DefaultRequestHeaders.Connection.Add("keep-alive");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync("product-info?semi_finished=G24040656G190008", content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;
                    return resMsgStr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回参数的数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">传入的数据</param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string url, object data)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage res = await client.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResultDto<TResult>>(resMsgStr);
                    return result != null ? result.Data : default;
                }
                else
                {
                    MessageBox.Show(res.StatusCode.ToString());
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }

        /// <summary>
        /// 同步Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Get(string url)
        {
            try
            {
                var responseString = client.GetStringAsync(url);
                return responseString.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var responseString = await client.GetStringAsync(url);
                return responseString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <typeparam name="TResult">返回参数的数据</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string url)
        {
            try
            {
                var resMsgStr = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ResultDto<TResult>>(resMsgStr);
                return result != null ? result.Data : default;
            }
            catch (Exception ex)
            {
                return default(TResult);
            }
        }
    }

    public class ResultDto<TResult>
    {
        public string Msg { get; set; }
        public TResult Data { get; set; }
        public bool Success { get; set; }
    }
}