using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class GetRequest
    {
        public Dictionary<string, string>? Headers { get; set; }
        public Dictionary<string, string>? Cookies { get; set; }
        public string? Domain { get; set; }
        public string? Response { get; private set; }
        public int? StatusCode { get; private set; }
        
        public void Run(string link)
        {
            try
            {
                //Create requset
                HttpWebRequest request;

                request = (HttpWebRequest)WebRequest.Create(link);
                request.Method = "Get";

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
