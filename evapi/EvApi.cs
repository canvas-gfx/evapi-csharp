using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvApi
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        private HttpClient Sess { get; set; }

        public EvApi(string port1, string host = "http://localhost", int port = 0)
        {
            Host = host;
            Port = port;
            Sess = new HttpClient();
        }

        private string GetUrl()
        {
            return $"{Host}:{Port}";
        }

        private async Task<EvResponse> GetResp(HttpResponseMessage resp)
        {
            byte[] byteArray = await resp.Content.ReadAsByteArrayAsync();
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

        public async Task AppSetEnvOptions(EnvOptions envOpt)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.set_env_options"},
                {
                    "args", new Dict()
                    {
                        {"units", envOpt.Units}
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            await GetResp(resp);
        }

        public async Task<List<string>> DebugLog(int numEntries = 20)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                        {"num_entries", numEntries}
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return (List<string>) output["log"];
        }

        public async Task<EvDocument> FileOpen(string path)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                        {"path", path}
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return new EvDocument((Dict) output["doc"]);
        }

        public async Task<string> FileSave()
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return (string) output["path"];
        }

        public async Task<string> FileSaveAs(string path)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                        {"path", path}
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return (string) output["path"];
        }

        public async Task<string> FileExport(string path, EvExportOptions options)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                        {"path", path},
                        {
                            "options", new Dict()
                            {
                                {"range", options.PageRange},
                                {"skip_hidden_pages", options.SkipHiddenPages},
                                {"use_objects_bounds", options.UseObjectBounds},
                                {"create_folder", options.CreateFolder},
                                {"resolution", options.Resolution},
                                {"color_mode", options.ColorMode},
                                {"interpolation", options.Interpolation},
                                {"anti_alias", options.AntiAlias}
                            }
                        }
                    }
                }
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return (string) output["path"];
        }

        public async Task<EvObject> Insert3dModel(
            string path,
            EvPoint pos,
            EvSize size,
            EvInsert3DModelOptions options,
            List<string> configurations,
            EnvOptions envOpt = null)
        {
            Dict args = new Dict()
            {
                {"path", path},
                {
                    "pos", new Dict()
                    {
                        {"x", pos.X},
                        {"y", pos.Y}
                    }
                },
                {
                    "size", new Dict()
                    {
                        {"w", size.W},
                        {"h", size.H}
                    }
                },
                {
                    "options", new Dict()
                    {
                        {"tesselation_quality", options.TesselationQuality},
                        {"use_brep", options.UseBrep},
                        {"fit_to_page", options.FitToPage}
                    }
                },
                {"configurations", configurations}
            };

            if (envOpt != null)
            {
                args["env_opt"] = new Dict()
                {
                    {"units", envOpt.Units}
                };
            }

            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {"args", args}
            };

            string body = JsonConvert.SerializeObject(js);
            HttpContent content = new StringContent(body, Encoding.UTF8);
            HttpResponseMessage resp = await Sess.PostAsync(GetUrl(), content);

            EvResponse evresp = await GetResp(resp);
            Dict output = evresp.GetOutput();
            return new EvObject(new Dict()
            {
                {"id", (int) output["obj"]}
            });
        }
    }
}