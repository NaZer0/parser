using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class PostRequest
    {
        public Dictionary<string, string>? Headers { get; set; }
        public Dictionary<string, string>? Cookies { get; set; }
        public string? Domain { get; set; }
        public string? Data { get; set; }
        public string? Response { get; private set; }
        public int? StatusCode { get; private set; }

        public void Run(string link)
        {
            try
            {
                //Create requset
                HttpWebRequest request;

                request = (HttpWebRequest)WebRequest.Create(link);
                request.Method = "Post";

                //Header
                if (Headers != null)
                {
                    foreach (var header in Headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                //Cookie
                if ((Cookies != null) && (Domain != null))
                {
                    CookieContainer cookieContainer = new CookieContainer();

                    foreach (var cookie in Cookies)
                    {
                        cookieContainer.Add(new Cookie(cookie.Key, cookie.Value, "/", Domain));
                    }

                    request.CookieContainer = cookieContainer;
                }

                //Data
                if (Data != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(Data);

                    //request.ContentType = "application/x-www-form-urlencoded";

                    request.ContentLength = byteArray.Length;

                    using var reqStream = request.GetRequestStream();
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }

                //Request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();


                StatusCode = (int)response.StatusCode;
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Exception {e.Message}");
            }
        }
    }
}
