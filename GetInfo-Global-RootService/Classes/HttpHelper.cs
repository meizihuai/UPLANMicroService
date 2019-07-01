﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetInfo_Global_RootService
{
    public class HttpHelper
    {
        
        public static async Task<NormalResponse> Get(string url, Dictionary<string, object> dik = null)
        {           
            string param = "?";
            if (dik == null)
            {
                dik = new Dictionary<string, object>();
            }
            foreach (string k in dik.Keys)
            {
                param = param + k + "=" + dik[k].ToString() + "&";
            }
            param = param.Substring(0, param.Length - 1);
            url = url + param;
            string result = await GetResponse(url);
            if (result == "") return new NormalResponse(false, "接口请求失败", "", "");
            try
            {
                NormalResponse np = JsonConvert.DeserializeObject<NormalResponse>(result);
                if (np == null) np = new NormalResponse(false, "接口请求出错");
                return np;
            }
            catch (Exception e)
            {
                return new NormalResponse(false, "接口请求出错", e.ToString(), "");
            }
        }
        public static async Task<NormalResponse> Post(string url, string data)
        {
            string result = await PostResponse(url, data);
            if (result == "") return new NormalResponse(false, "接口请求失败");
            try
            {
                NormalResponse np = JsonConvert.DeserializeObject<NormalResponse>(result);
                if (np == null) np = new NormalResponse(false, "接口请求出错");
                return np;
            }
            catch (Exception)
            {

            }
            return new NormalResponse(false, "接口请求失败");
        }
        public static async Task<NormalResponse> Post(string url, object data)
        {
            return await Post(url, JsonConvert.SerializeObject(data));
        }


        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>       
        private static async Task<string> GetResponse(string url)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(
                  new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType;
                    if (string.IsNullOrEmpty(contentType.CharSet))
                    {
                        contentType.CharSet = "utf-8";
                    }
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        private static async Task<string> PostResponse(string url, string postData)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = new StringContent(postData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
