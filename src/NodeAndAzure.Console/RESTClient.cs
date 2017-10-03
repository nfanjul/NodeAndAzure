using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NodeAndAzure
{
    public class RESTClient : IDisposable
    {
        private Uri _BaseURL;
        private string _MediaTypeHeader = "application/json";
        private string _ServiceName = "";
        private HttpClient _Client;

        public RESTClient(string baseURL, string pServiceName)
        {
            _BaseURL = new Uri(baseURL);
            _ServiceName = pServiceName;
            _Client = new HttpClient
            {
                BaseAddress = _BaseURL
            };
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_MediaTypeHeader));
        }

        #region Public Methods

        public string Get()
        {
            var result = "";
            HttpResponseMessage response = _Client.GetAsync(_ServiceName).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception(string.Format("{0}: No se ha podido conectar al servicio.", response.StatusCode), new Exception(response.ReasonPhrase)); ;
            }
            return result;
        }

        public static string PostImage(string path)
        {
            var result = "";
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("<API_URL>")
            };
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            form.Add(content, "fileToUpload");
            var imageStream = File.OpenRead(path);
            content = new StreamContent(imageStream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = "file.jpg"
            };
            form.Add(content);
            var response = client.PostAsync("addblob", form).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception(string.Format("{0}: No se ha podido conectar al servicio.", response.StatusCode), new Exception(response.ReasonPhrase)); ;
            }
            return result;
        }

        public static string Post(string text)
        {
            var result = "";
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("<API_URL>"),
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("comment", text),
            });
            var response = client.PostAsync("setredis", new StringContent(text)).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception(string.Format("{0}: No se ha podido conectar al servicio.", response.StatusCode), new Exception(response.ReasonPhrase)); ;
            }
            return result;
        }

        #endregion

        public void Dispose()
        {
            _Client.Dispose();
        }

    }
}

