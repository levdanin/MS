using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MagicSubmitter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    class SHJSTerm
    {

        public FastProcessor processor;

        protected bool _terminateSignalRecieved = false;
        protected bool _processIsPaused = false;
        public SHJSTerm()
        {
            processor = new FastProcessor();
        }

        public void run(Form1 form)
        {
            test(form);
        }

        public void terminateProcess()
        {
            _terminateSignalRecieved = true;
        }

        public void pauseToggle()
        {
            if (_processIsPaused)
            {
                _processIsPaused = false;
            }
            else
            {
                _processIsPaused = true;
            }
        }

        public void test(Form1 form)
        {

            System.Func<string> runJS = () =>
                {

                    string COMMAND_OUTPUT = "OUTPUT";
                    string COMMAND_LOG = "LOG";
                    string COMMAND_DELETE_FILE = "DELETE_FILE";
                    string COMMAND_WRITE_FILE = "WRITE_FILE";
                    string COMMAND_WRITE_TMP_FILE = "WRITE_TMP_FILE";
                    string COMMAND_READ_FILE = "READ_FILE";
                    string COMMAND_SLEEP = "SLEEP";
                    string COMMAND_HTTP_REQUEST = "HTTP_REQUEST";
                    string COMMAND_HTTP_GET = "HTTP_GET";
                    string COMMAND_HTTP_POST = "HTTP_POST";
                    string COMMAND_SET_COOKIES = "SET_COOKIES";
                    string COMMAND_GET_COOKIES = "GET_COOKIES";
                    string COMMAND_READ_JS = "READ_JS";
                    string DELIMITER_OUTPUT = "<<<";
                    string DELIMITER_INPUT = ">>>";
                    string RESULT_OK = "OK";
                    string RESULT_ERROR = "ERROR";
                    string RESULT_TERMINATE = "TERMINATE";


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
                        JObject headers = (JObject)xhr["headers"];
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
                        foreach (KeyValuePair<string, JToken> hName in headers)
                        {
                            httpReq.Headers[hName.Key] = (string) hName.Value;
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
                        resp["status"] = (int)httpResp.StatusCode;
                        resp["statusText"] = httpResp.StatusDescription;
                        resp["responseText"] = responseText;
                        JObject responseHeaders = new JObject();
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

                    System.Func<string, string, string> updateContent = (string url, string cont) =>
                    {
                        if (url.Contains("recaptcha__uk.js"))
                        {
                            /*
                            cont = Regex.Replace(cont, @"return\s+a\.call\s*\(\s*b\.src\s*\,\s*b\.Db\s*\,\s*c\s*\)",
                                        @"console.log(__dumpObject__(a, 1, null, null,""obj a""));"
                                        + @"console.log(__dumpObject__(b, 1, null, null,""obj b""));"
                                        + @"console.log(__dumpObject__(b.src, 1, null, null,""obj b.src""));"
                                        + @"console.log(__dumpObject__(b.Db, 1, null, null,""obj b.Db""));"
                                        + @"console.log(__dumpObject__(c, 1, null, null, ""obj c""));"
                                        + @"eval(__debugInputMacro__);"
                                        + @"return a.call(b.src,b.Db,c);");
                            cont = Regex.Replace(cont, @"throw\s+a\;",
                                       @"eval(__debugInputMacro__);throw a;");
                            */
                            /*cont = Regex.Replace(cont, "(?i)5e3", "50000");*/
                        }
                            
                        else if (url.Contains("lib/lib.js"))
                        {
                            /*cont = Regex.Replace(cont, @"Object\.extend\s*\(\s*a\.Element\s*\,\s*\{\s*extend\s*\:\s*r", "huyeval(__debugInputMacro__);Object.extend(a.Element,{extend:r");*/
                            cont = "";
                        }
                        
                        return cont;
                    };

                    System.Diagnostics.Process jsProc = new System.Diagnostics.Process();
                    jsProc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + @"\xulrunner\js.exe";
                    if (!System.IO.File.Exists(jsProc.StartInfo.FileName))
                    {
                        jsProc.StartInfo.FileName = @"C:\Program Files\Alexandr Krulik\Magic Submitter\xulrunner\js.exe";
                    }
                    string jsFilePath = System.IO.Path.GetTempFileName();
                    /*
                    System.IO.File.WriteAllText(jsFilePath, processor.GotoPageNoSet("https://raw.githubusercontent.com/levdanin/MS/master/shjsterm.js"));
                    System.IO.File.AppendAllText(jsFilePath, processor.GotoPageNoSet("https://raw.githubusercontent.com/levdanin/MS/master/env_term.js"));
                    System.IO.File.AppendAllText(jsFilePath, processor.GotoPageNoSet("https://raw.githubusercontent.com/levdanin/MS/master/shjs_test_events.js"));
                    */
                    System.IO.File.WriteAllText(jsFilePath, System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\..\..\..\SHJSTerm\shjsterm.js"));
                    System.IO.File.AppendAllText(jsFilePath, System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\..\..\..\SHJSTerm\env_term.js"));
                    //System.IO.File.AppendAllText(jsFilePath, System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\..\..\..\SHJSTerm\shjs_test_scriptload.js"));
                    System.IO.File.AppendAllText(jsFilePath, System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\..\..\..\SHJSTerm\shjs_test_events.js"));
                    jsProc.StartInfo.Arguments = @"-f " + jsFilePath;
                    jsProc.StartInfo.UseShellExecute = false;
                    jsProc.StartInfo.RedirectStandardOutput = true;
                    jsProc.StartInfo.RedirectStandardInput = true;
                    jsProc.StartInfo.CreateNoWindow = true;
                    form.log("***************************************************");
                    form.log("******************* STARTED ***********************");
                    form.log("***************************************************");
                    form.debug("started");
                    jsProc.Start();
                    string line = "";
                    string[] lineParts;
                    string notCommandText = "";
                    while (!jsProc.StandardOutput.EndOfStream)
                    {
                        Application.DoEvents();
                        Application.DoEvents();
                        Application.DoEvents();
                        if (_processIsPaused)
                        {
                            form.log("******************* PAUSED ***********************");
                            form.debug("paused");
                            while (_processIsPaused && !_terminateSignalRecieved)
                            {
                                Application.DoEvents();
                                Application.DoEvents();
                                Application.DoEvents();
                            }
                            form.log("******************* CONTINUED ***********************");
                            form.debug("continued");
                        }
                        line = jsProc.StandardOutput.ReadLine();
                        Application.DoEvents();
                        Application.DoEvents();
                        Application.DoEvents();
                        if (_terminateSignalRecieved)
                        {
                            jsProc.StandardInput.WriteLine(DELIMITER_INPUT + RESULT_TERMINATE + DELIMITER_INPUT + "terminate signal recieved");
                            form.log("******************* ABORTED ***********************");
                            form.debug("aborted");
                            break;
                        }
                        lineParts = line.Split(new string[] { DELIMITER_OUTPUT }, StringSplitOptions.None);
                        if (lineParts.Length != 3)
                        {
                            notCommandText += line;
                        }
                        else
                        {
                            /*
                            try
                            {
                                */
                                if (lineParts[1] == COMMAND_OUTPUT)
                                {
                                    string newLine = "Output:" + processor.GetJsonVal("data", lineParts[2]);
                                    notCommandText += newLine;
                                    form.log(newLine);
                                }
                                else if (lineParts[1] == COMMAND_LOG)
                                {
                                    string newLine = "Logged: " + processor.GetJsonVal("message", lineParts[2]);
                                    notCommandText += newLine;
                                    form.log(newLine);
                                }
                                else if (lineParts[1] == COMMAND_HTTP_GET)
                                {
                                    string url = processor.GetJsonVal("url", lineParts[2]);
                                    string cont = processor.GotoPageNoSet(url);
                                    cont = updateContent(url, cont);
                                    jsProc.StandardInput.WriteLine(cont);
                                }
                                else if (lineParts[1] == COMMAND_SLEEP)
                                {
                                    string sleepStr = processor.GetJsonVal("millseconds", lineParts[2]);
                                    int sleepInt = Convert.ToInt32(sleepStr);
                                    if (sleepInt > 0)
                                    {
                                        System.Threading.Thread.Sleep(sleepInt);
                                    }
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
                                    if (fpath.StartsWith("http://") || fpath.StartsWith("https://") || fpath.StartsWith("//"))
                                    {
                                        if (fpath.StartsWith("//"))
                                        {
                                            fpath = "http:" + fpath;
                                        }
                                        string url = fpath;
                                        cont = processor.GotoPageNoSet(url);
                                        cont = updateContent(url, cont);
                                    }
                                    else if (System.IO.File.Exists(fpath))
                                    {
                                        cont = System.IO.File.ReadAllText(fpath);
                                    }
                                    else
                                    {
                                        cont = null;
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
                                    if (System.IO.File.Exists(fpath))
                                    {
                                        System.IO.File.AppendAllText(fpath, data);
                                    }
                                    else
                                    {
                                        System.IO.File.WriteAllText(fpath, data);
                                    }
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
                                    string data = (string)inObj["data"];
                                    JObject resp = runHttpRequest(xhr, data);
                                    if (((string)xhr["method"]).ToLower() == "get")
                                    {
                                        resp["responseText"] = updateContent((string)xhr["url"], (string)resp["responseText"]);
                                    }
                                    jsProc.StandardInput.WriteLine(JsonConvert.SerializeObject(resp));
                                }
                                else if (lineParts[1] == COMMAND_READ_JS)
                                {
                                    string inputJs = form.getInputJS();
                                    jsProc.StandardInput.WriteLine(inputJs);
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
                    }
                    jsProc.WaitForExit(2000);
                    try
                    {
                        System.IO.File.Delete(jsFilePath);
                    }
                    catch (Exception e) { }
                    _terminateSignalRecieved = false;
                    _processIsPaused = false;
                    return notCommandText;
                };
            string response = runJS();
            form.log("*******************************************************************************");
            form.log("************************************ FINISHED *********************************");
            form.log("*******************************************************************************");
            form.debug("finished");
        }

    }
}
