using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class Connection
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public HttpClient Session { get; private set; }

        public Connection(string host, int port)
        {
            Host = host;
            Port = port;
            Session = new HttpClient();
        }

        private string GetUrl()
        {
            return $"{Host}:{Port}";
        }

        static private EvResponse GetResp(HttpResponseMessage resp)
        {
            Task<byte[]> readTask = Task.Run(
                async () => await resp.Content.ReadAsByteArrayAsync());
            byte[] byteArray = readTask.Result;
            
            string body = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            Dict js = JsonConvert.DeserializeObject<Dict>(body);
            EvResponse evresp = new EvResponse(js);

            if (evresp.GetStatus() == EvResponseStatus.Success)
            {
                return evresp;
            }
            else if (evresp.GetStatus() == EvResponseStatus.Error)
            {
                throw evresp.GetError();
            }
            else
            {
                throw new Exception($"Got invalid response status: {evresp.GetStatus()}");
            }
        }

        public Dict IssueCommand(Dict cmd)
        {
            string body = JsonConvert.SerializeObject(cmd);
            HttpContent content = new StringContent(body, Encoding.UTF8);

            Task<HttpResponseMessage> postTask = Task.Run(
                async () => await Session.PostAsync(GetUrl(), content));
            HttpResponseMessage resp = postTask.Result;

            EvResponse evresp = GetResp(resp);
            return evresp.GetOutput();
        }
    }
}
