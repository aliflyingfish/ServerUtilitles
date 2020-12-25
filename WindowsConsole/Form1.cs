using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsConsole
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string[] BufferValue;
        private string EnterValue;
        private string ExecuteValue;

        private enum ColorStyle
        {
            Error,
            Information,
            Warning,
            Question,
            Complete
        }

        private void Log(string title, string text, ColorStyle colorStyle)
        {
            if (!Directory.Exists(".\\logs"))
            {
                Directory.CreateDirectory(".\\logs");
            }
            Console.Write("[" + DateTime.Now.ToString() + "]");
            if (colorStyle == ColorStyle.Error)
            {
                Console.Write("[");
                Console.Write(title, Color.Red);
                Console.Write("]");
                Console.WriteLine(text);
            }
            else if (colorStyle == ColorStyle.Information)
            {
                Console.Write("[");
                Console.Write(title, Color.White);
                Console.Write("]");
                Console.WriteLine(text);
            }
            else if (colorStyle == ColorStyle.Warning)
            {
                Console.Write("[");
                Console.Write(title, Color.Yellow);
                Console.Write("]");
                Console.WriteLine(text);
            }
            else if (colorStyle == ColorStyle.Question)
            {
                Console.Write("[");
                Console.Write(title, Color.SkyBlue);
                Console.Write("]");
                Console.WriteLine(text);
            }
            else if (colorStyle == ColorStyle.Complete)
            {
                Console.Write("[");
                Console.Write(title, Color.LawnGreen);
                Console.Write("]");
                Console.WriteLine(text);
            }
            memoEdit1.Text+=("[" + DateTime.Now.ToString() + "]");
            memoEdit1.Text+=("[");
            memoEdit1.Text+=(title+"");
            memoEdit1.Text+=("]");
            memoEdit1.Text+=(text+"\r\n");
            streamWriter.Write("[" + DateTime.Now.ToString() + "]");
            streamWriter.Write("[");
            streamWriter.Write(title);
            streamWriter.Write("]");
            streamWriter.WriteLine(text);
            streamWriter.Flush();
        }

        private static StreamWriter streamWriter = new StreamWriter(".\\logs\\" + DateTime.Now.ToFileTime() + ".log");

        private static StreamWriter GameThreadInput;
        private void Monitor()
        {
            PerformanceCounter performanceCounter = new PerformanceCounter("Process", "Working Set", "ServerAutoBash");
            performanceCounter.NextValue();
            for(; ; )
            {
                chartControl1.Invoke(new Action(() =>
                {
                    chartControl1.Series["Java"].Points.Clear();
                }));
                for(int i = 0; i <= 240; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    chartControl1.Series["Java"].Points.AddPoint(i, (double)(performanceCounter.NextValue() / 1024 / 1024));

                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds <= 240)
                    {
                        Thread.Sleep(int.Parse((250 - stopwatch.ElapsedMilliseconds).ToString().Replace("-", "")));
                    }
                }
            }
        }
        private void StartThread()
        {
            statusText.Text = "正在启动";
            Log("Thread - Init", "Pre-Initialzing Application Process ...", ColorStyle.Information);
            Process process = new Process();
            Log("Thread - Init", "Pre-Initialzing Application Process Settings ...", ColorStyle.Information);
            process.StartInfo.FileName = ".\\ServerAutoBash.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            Log("Thread - Init", "Starting Application Process ...", ColorStyle.Information);
            process.Start();
            Log("Thread - Init", "Pre-Initialzing Application Process Listener ...", ColorStyle.Information);
            StreamReader COPor = process.StandardOutput;
            GameThreadInput = process.StandardInput;
            Log("Application Thread - Listener Feedback", "Starting Application Process Listener ...", ColorStyle.Information);
            Thread thread = new Thread(Monitor);
            thread.Start();
            statusText.Text = "就绪";
            for (; ; )
            {
                if (process.HasExited == true)
                {
                    break;
                }
                    try
                    {
                        Log("Application Thread - Listener Feedback", COPor.ReadLine(), ColorStyle.Question);
                    }
                    catch
                    {

                    }
            }
            try
            {
                Log("Application Thread - Stop", "Stopping Application Thread", ColorStyle.Information);
                Log("Application Thread - Stop", "Stopping Process", ColorStyle.Information);
                process.Kill();
                Log("Application Thread - Stop", "Disposing Standard Input Port ...", ColorStyle.Information);
                GameThreadInput.Dispose();
                Log("Application Thread - Stop", "Closing Standard Input Port ...", ColorStyle.Information);
                GameThreadInput.Close();
                Log("Application Thread - Stop", "Done.", ColorStyle.Complete);
                Log("Application Thread - Stop", "Disposing Standard Output Port ...", ColorStyle.Information);
                COPor.Dispose();
                Log("Application Thread - Stop", "Closing Standard Output Port ...", ColorStyle.Information);
                COPor.Close();
                Log("Application Thread - Stop", "Done.", ColorStyle.Complete);
                Log("Application Thread - Stop", "Everything Was end", ColorStyle.Complete);
            }
            catch
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(StartThread);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }

        private void Form1_RightToLeftChanged(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void marqueeProgressBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!(textEdit1.Text == ""))
            {
                try
                {
                    GameThreadInput.WriteLine(textEdit1.Text);
                    GameThreadInput.Flush();
                }
                catch(Exception ex)
                {
                    Log("Env - Error", ex.ToString(), ColorStyle.Error);
                }
            }
        }
    }
}
