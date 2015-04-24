﻿using System;
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
                try
                {
                    if (lineParts[1] == COMMAND_OUTPUT)
                    {
                        MessageBox.Show(processor.GetJsonVal("data", lineParts[2]));
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
                        var xhr = processor.GetJsonVal("xhr", lineParts[2]);
                        string data = processor.GetJsonVal("data", lineParts[2]);
                        object resp = new object();
                        jsProc.StandardInput.WriteLine(JsonConvert.SerializeObject(resp));
                    }
                    else
                    {
                        throw new Exception("Unknown terminal command: " + lineParts[1]);
                    }
                    jsProc.StandardInput.WriteLine(DELIMITER_INPUT + RESULT_OK);
                }
                catch (Exception e)
                {
                    jsProc.StandardInput.WriteLine(DELIMITER_INPUT + RESULT_ERROR + DELIMITER_INPUT + e.Message);
                }
            }
            if (!String.IsNullOrEmpty(notCommandText))
            {
                MessageBox.Show(notCommandText);
            }
            jsProc.WaitForExit();
            //MessageBox.Show(jsFilePath);
            System.IO.File.Delete(jsFilePath);
        }

    }
}
