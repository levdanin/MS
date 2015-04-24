using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MagicSubmitter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    class SHJSTerm
    {

        public const string COMMAND_OUTPUT = "OUTPUT";
        public const string COMMAND_DELETE_FILE = "DELETE_FILE";
        public const string COMMAND_WRITE_FILE = "WRITE_FILE";
        public const string COMMAND_WRITE_TMP_FILE = "WRITE_TMP_FILE";
        public const string COMMAND_READ_FILE = "READ_FILE";
        public const string COMMAND_SLEEP = "SLEEP";
        public const string COMMAND_HTTP_REQUEST = "HTTP_REQUEST";
        public const string COMMAND_HTTP_GET = "HTTP_GET";
        public const string COMMAND_HTTP_POST = "HTTP_POST";
        public const string COMMAND_SET_COOKIES = "SET_COOKIES";
        public const string COMMAND_GET_COOKIES = "GET_COOKIES";
        public const string DELIMITER_OUTPUT = "<<<";
        public const string DELIMITER_INPUT = ">>>";
        public const string RESULT_OK = "OK";
        public const string RESULT_ERROR = "ERROR";

        public FastProcessor processor;

        public SHJSTerm()
        {
            processor = new FastProcessor();
        }

        public void run()
        {
            test();
        }

        public void test()
        {
            System.Func<string> runJS = () =>
                {

                    System.Net.CookieContainer cookieJar = new System.Net.CookieContainer();
                    System.Net.WebProxy proxy = null;
                    if ((processor.profile == null) || (MagicSubmitter.Globals.UseProxy))
                    {
                        if (!String.IsNullOrEmpty(MagicSubmitter.Globals.ProxyServer))
                        {
                            proxy = new System.Net.WebProxy(MagicSubmitter.Globals.ProxyServer);
                        }
                        else
                        {
                            proxy = new System.Net.WebProxy("127.0.0.1:8899");
                        }
                    }


                    System.Func<JObject, string, JObject> runHttpRequest = (JObject xhr, string data) =>
                    {
                        string method = (string) xhr["method"];
                        string url = (string) xhr["url"];
                        string postData = data;
                        JArray headers = (JArray)xhr["headers"];
                        string referer = null;
                        string accept = null;
                        string contentType = null;

                        System.Uri actionUri = new System.Uri(url);
                        System.Net.HttpWebRequest httpReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(actionUri);
                        httpReq.CookieContainer = cookieJar;
                        httpReq.KeepAlive = true;
                        httpReq.Proxy = proxy;
                        httpReq.ReadWriteTimeout = 30000;
                        httpReq.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:8.0) Gecko/20100101 Firefox/8.0";
                        httpReq.Headers["Accept-Encoding"] = "";
                        httpReq.Headers["Accept-Language"] = "en-us,en;q=0.5";
                        if (referer == null)
                        {
                            referer = processor.GetLatestURL();
                        }
                        httpReq.Referer = referer;
                        if (accept == null)
                        {
                            accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        }
                        httpReq.Accept = accept;
                        httpReq.AllowAutoRedirect = true;
                        method = method.ToUpper();
                        httpReq.Method = method;
                        if (headers != null)
                        {
                            foreach (string header in headers)
                            {
                                httpReq.Headers[header] = (string) headers[header];
                            }
                        }
                        if (method != "GET")
                        {
                            byte[] arrayPostBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                            httpReq.ContentLength = arrayPostBytes.Length;
                            if (contentType == null)
                            {
                                contentType = "application/x-www-form-urlencoded; charset=UTF-8";
                            }
                            httpReq.ContentType = contentType;
                            httpReq.GetRequestStream().Write(arrayPostBytes, 0, arrayPostBytes.Length);
                            httpReq.GetRequestStream().Close();
                        }

                        System.Net.HttpWebResponse httpResp;
                        try
                        {
                            httpResp = (System.Net.HttpWebResponse)httpReq.GetResponse();
                        }
                        catch (System.Net.WebException ex)
                        {
                            httpResp = (System.Net.HttpWebResponse)ex.Response;
                        }
                        string responseText = (new System.IO.StreamReader(httpResp.GetResponseStream())).ReadToEnd();
                        processor.SetLastText(responseText);
                        processor.SetLatestURL(httpResp.ResponseUri.AbsoluteUri);
                        JObject resp = new JObject();
                        resp["status"] = httpResp.StatusCode.ToString();
                        resp["statusText"] = httpResp.StatusDescription;
                        resp["responseText"] = responseText;
                        JArray responseHeaders = new JArray();
                        foreach (string hName in httpResp.Headers)
                        {
                            responseHeaders[hName] = httpResp.Headers[hName];
                        }
                        resp["responseHeaders"] = responseHeaders;
                        httpResp.Close();
                        return resp;
                    };

                    System.Action<string, string, string> addCookie = (string url, string name, string value) =>
                    {
                        if (!url.Contains("://"))
                        {
                            url = "http://" + url;
                        }
                        System.Uri cUri = new System.Uri(url);
                        cookieJar.Add(cUri, new System.Net.Cookie(name, value));
                    };

                    System.Func<string, string, string> getCookie = (string url, string name) =>
                    {
                        if (!url.Contains("://"))
                        {
                            url = "http://" + url;
                        }
                        System.Uri cUri = new System.Uri(url);
                        if (cookieJar.GetCookies(cUri)[name] == null)
                        {
                            return "";
                        }
                        else
                        {
                            return cookieJar.GetCookies(cUri)[name].Value;
                        }
                    };


                    System.Diagnostics.Process jsProc = new System.Diagnostics.Process();
                    jsProc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + @"\xulrunner\js.exe";
                    if (!System.IO.File.Exists(jsProc.StartInfo.FileName))
                    {
                        jsProc.StartInfo.FileName = @"C:\Program Files\Alexandr Krulik\Magic Submitter\xulrunner\js.exe";
                    }
                    string jsFilePath = System.IO.Path.GetTempFileName();
                    System.IO.File.WriteAllText(jsFilePath, processor.GotoPageNoSet("https://raw.githubusercontent.com/levdanin/MS/master/shjsterm.js"));
                    System.IO.File.AppendAllText(jsFilePath, processor.GotoPageNoSet("https://raw.githubusercontent.com/levdanin/MS/master/shjs_test.js"));
                    jsProc.StartInfo.Arguments = @"-f " + jsFilePath;
                    jsProc.StartInfo.UseShellExecute = false;
                    jsProc.StartInfo.RedirectStandardOutput = true;
                    jsProc.StartInfo.RedirectStandardInput = true;
                    jsProc.StartInfo.CreateNoWindow = true;
                    jsProc.Start();
                    string line = "";
                    string[] lineParts;
                    string notCommandText = "";
                    while (!jsProc.StandardOutput.EndOfStream)
                    {
                        line = jsProc.StandardOutput.ReadLine();
                        lineParts = line.Split(new string[] { DELIMITER_OUTPUT }, StringSplitOptions.None);
                        if (lineParts.Length != 3)
                        {
                            notCommandText += line;
                        }
                        /*
                        try
                        {
                            */
                            if (lineParts[1] == COMMAND_OUTPUT)
                            {
                                notCommandText += processor.GetJsonVal("data", lineParts[2]);
                            }
                            else if (lineParts[1] == COMMAND_HTTP_GET)
                            {
                                jsProc.StandardInput.WriteLine(processor.GotoPageNoSet(processor.GetJsonVal("url", lineParts[2])));
                            }
                            else if (lineParts[1] == COMMAND_SLEEP)
                            {
                                System.Threading.Thread.Sleep(Convert.ToInt32(processor.GetJsonVal("millseconds", lineParts[2])));
                            }
                            else if (lineParts[1] == COMMAND_DELETE_FILE)
                            {
                                string fpath = processor.GetJsonVal("path", lineParts[2]);
                                if (fpath.StartsWith("file://"))
                                {
                                    fpath = fpath.Substring(6);
                                }
                                System.IO.File.Delete(fpath);
                            }
                            else if (lineParts[1] == COMMAND_READ_FILE)
                            {
                                string fpath = processor.GetJsonVal("path", lineParts[2]);
                                if (fpath.StartsWith("file://"))
                                {
                                    fpath = fpath.Substring(7);
                                }
                                string cont;
                                if (fpath.StartsWith("http://") || fpath.StartsWith("https://"))
                                {
                                    cont = processor.GotoPageNoSet(fpath);
                                }
                                else
                                {
                                    cont = System.IO.File.ReadAllText(fpath);
                                }
                                jsProc.StandardInput.WriteLine(cont);
                            }
                            else if (lineParts[1] == COMMAND_WRITE_FILE)
                            {
                                string fpath = processor.GetJsonVal("path", lineParts[2]);
                                if (fpath.StartsWith("file://"))
                                {
                                    fpath = fpath.Substring(7);
                                }
                                string data = processor.GetJsonVal("data", lineParts[2]);
                                System.IO.File.WriteAllText(fpath, data);
                            }
                            else if (lineParts[1] == COMMAND_WRITE_TMP_FILE)
                            {
                                string suffix = processor.GetJsonVal("suffix", lineParts[2]);
                                string data = processor.GetJsonVal("data", lineParts[2]);
                                string fpath = System.IO.Path.GetTempFileName();
                                string newpath = fpath + "." + suffix;
                                System.IO.File.Move(fpath, newpath);
                                System.IO.File.WriteAllText(newpath, data);
                                jsProc.StandardInput.WriteLine(newpath);
                            }
                            else if (lineParts[1] == COMMAND_HTTP_REQUEST)
                            {
                                JObject inObj = (JObject)JsonConvert.DeserializeObject(lineParts[2]);
                                JObject xhr = (JObject)inObj["xhr"];
                                string data = ((JValue)inObj["data"]).Value.ToString();
                                JObject resp = runHttpRequest(xhr, data);
                                jsProc.StandardInput.WriteLine(JsonConvert.SerializeObject(resp));
                            }
                            else
                            {
                                throw new Exception("Unknown terminal command: " + lineParts[1]);
                            }
                            jsProc.StandardInput.WriteLine(DELIMITER_INPUT + RESULT_OK);
                            /*
                        }
                        catch (Exception e)
                        {
                            jsProc.StandardInput.WriteLine(DELIMITER_INPUT + RESULT_ERROR + DELIMITER_INPUT + e.Message);
                        }
                        */
                    }
                    jsProc.WaitForExit();
                    System.IO.File.Delete(jsFilePath);
                    return notCommandText;
                };
            string response = runJS();
            MessageBox.Show("Javascript response: " + response);
        }

    }
}
