using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MagicSubmitter;

namespace WindowsFormsApplication1
{
    class SHJSTerm
    {

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
                lineParts = line.Split(new string[] { "<<<" }, StringSplitOptions.None);
                if (lineParts.Length != 3)
                {
                    notCommandText += line;
                }
                try
                {
                    if (lineParts[1] == "OUTPUT")
                    {
                        MessageBox.Show(processor.GetJsonVal("data", lineParts[2]));
                    }
                    jsProc.StandardInput.WriteLine(">>>OK");
                }
                catch (Exception e)
                {
                    jsProc.StandardInput.WriteLine(">>>ERROR>>>" + e.Message);
                }
            }
            if (!String.IsNullOrEmpty(notCommandText))
            {
                MessageBox.Show(notCommandText);
            }
            jsProc.WaitForExit();
            System.IO.File.Delete(jsFilePath);
        }

    }
}
