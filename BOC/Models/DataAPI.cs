using BOC.Areas.E_Library.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOC.Models
{
    public class DataAPI
    {
        public async Task<List<T>> GetListOjectAPI<T>(string uri,
                                                     string Token,
                                                     HttpMethod method,
                                                     bool _ConfigureAwait,
                                                     string _GetObject,
                                                     params string[] _arrays)
                                                     where T : class
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                var _parameters = new List<KeyValuePair<string, string>>();

                //if parameters are even
                if (_arrays.Length % 2 == 0)
                {
                    //if there are more 2 pairs of parameters
                    if (_arrays.Length > 2)
                    {
                        for (int i = 0; i < _arrays.Length; i++)
                        {
                            if (i % 2 != 0 && i != 0)
                            {
                                _parameters.Add(new KeyValuePair<string, string>(_arrays[i - 1], _arrays[i]));
                            }
                       }

                    }
                    else
                    {
                        _parameters.Add(new KeyValuePair<string, string>(_arrays[0], _arrays[1]));
                    }
                }
                var req = new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(_parameters) };
                string _Content;
                HttpResponseMessage res;
                res = await _httpClient.SendAsync(req).ConfigureAwait(_ConfigureAwait);
                _Content = await res.Content.ReadAsStringAsync().ConfigureAwait(_ConfigureAwait);
                var Data = JObject.Parse(_Content);
                System.Collections.Generic.List<T> deserializeData = JsonConvert.DeserializeObject<List<T>>(Data[_GetObject].ToString());
                return deserializeData;
            }
        }

          public async Task<T> GetObjectAPI<T>(string uri,
                                            string Token,
                                            HttpMethod method,
                                            bool _ConfigureAwait,
                                            string _GetObject,
                                            params string[] _arrays)
                                            where T : class
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                var _parameters = new List<KeyValuePair<string, string>>();
                //if parameters are even
                if (_arrays.Length % 2 == 0)
                {
                    //if there are more 2 pairs of parameters
                    if (_arrays.Length > 2)
                    {
                        for (int i = 0; i < _arrays.Length; i++)
                        {
                            if (i % 2 != 0 && i != 0)
                            {
                                _parameters.Add(new KeyValuePair<string, string>(_arrays[i - 1], _arrays[i]));
                            }
                        }
                    }
                    else
                    {
                        _parameters.Add(new KeyValuePair<string, string>(_arrays[0], _arrays[1]));
                    }
                }
                var req = new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(_parameters) };
                string _Content;
                HttpResponseMessage res;
                res = await _httpClient.SendAsync(req).ConfigureAwait(_ConfigureAwait);
                _Content = await res.Content.ReadAsStringAsync().ConfigureAwait(_ConfigureAwait);
                var Data = JObject.Parse(_Content);
                T deserializeData = JsonConvert.DeserializeObject<T>(Data[_GetObject].ToString());
                return deserializeData;
            }
        }

        public async Task<eLib_Comment_New> GetStringAPI(string uri,
                                      string Token,
                                      HttpMethod method,
                                      bool _ConfigureAwait,
                                      params string[] _arrays)

        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                var _parameters = new List<KeyValuePair<string, string>>();
               
                if (_arrays.Length % 2 == 0)
                {
                    //if there are more 2 pairs of parameters
                    if (_arrays.Length > 2)
                    {
                        for (int i = 0; i < _arrays.Length; i++)
                        {
                            if (i % 2 != 0 && i != 0)
                            {
                                _parameters.Add(new KeyValuePair<string, string>(_arrays[i - 1], _arrays[i]));
                            }
                        }
                    }
                    else
                    {
                        _parameters.Add(new KeyValuePair<string, string>(_arrays[0], _arrays[1]));
                    }
                }
                var req = new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(_parameters) };
                string _Content;
                HttpResponseMessage res;
                res = await _httpClient.SendAsync(req).ConfigureAwait(_ConfigureAwait);
                _Content = await res.Content.ReadAsStringAsync().ConfigureAwait(_ConfigureAwait);
                var Data = JObject.Parse(_Content);
                //var ConvertToObject = JsonConvert.SerializeObject(Data);
                eLib_Comment_New deserializeData = JsonConvert.DeserializeObject<eLib_Comment_New>(Data.ToString());
                return deserializeData;
            }
        }


        //Get List Object Without Parameters
        public async Task<List<T>> GetListAPIWithoutParams<T>(string uri,
                                                              string Token,
                                                              HttpMethod method,
                                                              bool _ConfigureAwait,
                                                              string _GetObject)
                                                              where T : class
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                var req = new HttpRequestMessage(method, uri);
                string _content;
                HttpResponseMessage res;
                res = await _httpClient.SendAsync(req).ConfigureAwait(false);
                _content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var Data = JObject.Parse(_content);
                List<T> deserializeData = JsonConvert.DeserializeObject<List<T>>(Data[_GetObject].ToString());
                return deserializeData;
            }
        }
        public async Task<T> GetObjectAPIWithoutParams<T>(string uri,
                                                         string Token,
                                                         HttpMethod method, 
                                                         bool _ConfigureAwait,
                                                         string _GetObject)
                                                         where T : class
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Token);
                var req = new HttpRequestMessage(method, uri);
                string _content;
                HttpResponseMessage res;
                res = await _httpClient.SendAsync(req).ConfigureAwait(false);
                _content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var Data = JObject.Parse(_content);
                T deserializeData = JsonConvert.DeserializeObject<T>(Data["Data"].ToString());
                return deserializeData;
            }
        }

        public async Task<T> _GetObjecReturn<T>(string uri,
                                               string Token,
                                               string param1,
                                               string _param1) where T : class
        {
            using (HttpClient _httpclient = new HttpClient())
            {
                _httpclient.DefaultRequestHeaders.Add("Authorization", Token);
                var _parameters = new List<KeyValuePair<string, string>>();
                _parameters.Add(new KeyValuePair<string, string>(param1, _param1));
                var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(_parameters) };
                string _content;
                HttpResponseMessage res;
                res = await _httpclient.SendAsync(req).ConfigureAwait(false);
                _content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var _Data = JObject.Parse(_content);
                //var ConvertToObject = JsonConvert.SerializeObject(_Data);
                T deserializeData = JsonConvert.DeserializeObject<T>(_content);
                return deserializeData;
            }
        }
        public string ReturnValueFromString(string _pattern, 
                                            string _char)
        {
            string result = string.Empty;
            if (_pattern != null)
                for (int i = 0; i < _pattern.Length; i++)
                {
                    if (_pattern[i].ToString() == _char)
                    {
                        result = _pattern.Substring(i + 1, _pattern.Length - (i + 1)).ToString();
                    }
                }
            return result;
        }
        public void DownloadFile(SftpConfig _config,
                                  string remoteFilePath,
                                  string localFilePath)
        {
            using var client = new SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                
                using var s = File.Create(localFilePath);
                client.DownloadFile(remoteFilePath, s);
            }
            catch (Exception exception)
            {
            }
            finally
            {
                client.Disconnect();
            }
        }
        public List<T> list_Pagination_DCS<T>(string currentPage,
                                             string next,
                                             string previous,
                                             int pagesize,
                                             List<T> lst_To_Pagination,
                                             out int ViewBagcurrentpage,
                                             out int ViewBagnumSize)
        {
            ViewBagcurrentpage = 1;
            ViewBagnumSize = 1; 
            string _currentPage;
            if (currentPage != null)
            {
                _currentPage = currentPage;
            }
            else
            {
                _currentPage = "1";
            }
            List<T> list_Pagination = new List<T>();
            if (lst_To_Pagination != null)
            {
                ViewBagnumSize = (int)Math.Ceiling(lst_To_Pagination.Count / (float)pagesize);
                if (previous == "previous")
                {
                    ViewBagcurrentpage = int.Parse(_currentPage) - 1;
                    _currentPage = (int.Parse(_currentPage) - 1).ToString();
                }
                if (next == "next")
                {
                    ViewBagcurrentpage = int.Parse(_currentPage) + 1;
                    _currentPage = (int.Parse(_currentPage) + 1).ToString();
                }
                list_Pagination = (from item in lst_To_Pagination
                                   select item)
                                  .Skip((int.Parse(_currentPage) - 1) * pagesize)
                                  .Take(pagesize).ToList();

            }
            return list_Pagination;
        }

    }
}
