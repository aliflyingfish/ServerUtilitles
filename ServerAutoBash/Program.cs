using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Console = Colorful.Console;
using System.Drawing;
using System.Diagnostics;

namespace ServerAutoBash
{
    class Program
    {
        private static void Execute(string command)
        {
            try
            {
                if (command == "exit")
                {
                    Log("Enviroment - Exit", "Applicaltion Done", ColorStyle.Complete);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                else if (command == "stop netthread")
                {
                    NetworkThreadLive = false;
                }
                else if (command == "stop gsthread")
                {
                    GameServerThreadLive = false;
                }
                else if (command == "start netthread")
                {
                    NetworkThreadLive = true;
                    new Thread(NetworkThread).Start();
                }
                else if (command == "start gsthread")
                {
                    GameServerThreadLive = true;
                    new Thread(GameServerThread).Start();
                }
                else if (command == "start all")
                {
                    GameServerThreadLive = true;
                    new Thread(GameServerThread).Start();
                    NetworkThreadLive = true;
                    new Thread(NetworkThread).Start();
                }
                else
                {
                    string cmd = command.Replace("send ", "");
                    if (command.Contains("send "))
                    {
                        GameThreadInput.WriteLine(cmd);
                        GameThreadInput.Flush();
                    }
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
            }
            catch (Exception ex)
            {
                Log("Execute - Error", "Error has throwned.\n" + ex.ToString(), ColorStyle.Error);
            }
        }
        private enum ColorStyle
        {
            Error,
            Information,
            Warning,
            Question,
            Complete
        }

        private static StreamWriter streamWriter = new StreamWriter(".\\logs\\" + DateTime.Now.ToFileTime() + ".log");
        private static void Log(string title,string text,ColorStyle colorStyle)
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
            streamWriter.Write("[" + DateTime.Now.ToString() + "]");
            streamWriter.Write("[");
            streamWriter.Write(title);
            streamWriter.Write("]");
            streamWriter.WriteLine(text);
            streamWriter.Flush();
        }


        private static bool NetworkThreadLive = false;
        private static bool GameServerThreadLive = false;

        private static void NetworkThread()
        {
            try
            {
                foreach(Process p in Process.GetProcessesByName("frpc"))
                {
                    Log("Thread - Init", "Kill process at frpc.exe", ColorStyle.Information);
                    p.Kill();
                }
                Log("Thread - Init", "Pre-Initialzing Network Process ...", ColorStyle.Information);
                Process process = new Process();
                Log("Thread - Init", "Pre-Initialzing Network Process Settings ...", ColorStyle.Information);
                process.StartInfo.FileName = ".\\frpc.exe";
                if (File.Exists(".\\config.ini"))
                {
                    process.StartInfo.Arguments = File.ReadAllText("config.ini");
                }
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                Log("Thread - Init", "Starting Network Process ...", ColorStyle.Information);
                process.Start();
                Log("Thread - Init", "Pre-Initialzing Network Process Listener ...", ColorStyle.Information);
                StreamReader COPor = process.StandardOutput;
                StreamWriter CIPor = process.StandardInput;
                Log("Network Thread - Listener Feedback", "Starting Network Process Listener ...", ColorStyle.Information);
                for (; ; )
                {
                    if (process.HasExited == true)
                    {
                        break;
                    }
                    if (NetworkThreadLive == true)
                    {

                        try
                        {
                            Log("Network Thread - Listener Feedback", COPor.ReadLine(), ColorStyle.Question);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    Log("Network Thread - Stop", "Stopping Network Thread", ColorStyle.Information);
                    Log("Network Thread - Stop", "Stopping Process", ColorStyle.Information);
                    process.Kill();
                    Log("Network Thread - Stop", "Disposing Standard Input Port ...", ColorStyle.Information);
                    CIPor.Dispose();
                    Log("Network Thread - Stop", "Closing Standard Input Port ...", ColorStyle.Information);
                    CIPor.Close();
                    Log("Network Thread - Stop", "Done.", ColorStyle.Complete);
                    Log("Network Thread - Stop", "Disposing Standard Output Port ...", ColorStyle.Information);
                    COPor.Dispose();
                    Log("Network Thread - Stop", "Closing Standard Output Port ...", ColorStyle.Information);
                    COPor.Close();
                    Log("Network Thread - Stop", "Done.", ColorStyle.Complete);
                    Log("Network Thread - Stop", "Everything Was end", ColorStyle.Complete);
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                Log("Trust - Error", "Error has throwned.\n" + ex.ToString(), ColorStyle.Error);
            }

        }

        private static StreamWriter GameThreadInput;

        private static void GameServerThread()
        {
            try
            {
                Log("Thread - Init", "Pre-Initialzing Game Server Process ...", ColorStyle.Information);
                Process process = new Process();
                Log("Thread - Init", "Pre-Initialzing Game Server Process Settings ...", ColorStyle.Information);
                if (File.Exists(".\\config.ini"))
                {
                    process.StartInfo.FileName = File.ReadAllText(".\\javapath.ini");
                }
                else
                {
                    File.Create(".\\javapath.ini");
                    process.StartInfo.FileName = "C:\\Program Files\\Java\\jre1.8.0_271\\bin\\javaw.exe";
                }
                process.StartInfo.Arguments = "-jar .\\server.jar";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                Log("Thread - Init", "Starting Game Server Process ...", ColorStyle.Information);
                process.Start();
                Log("Thread - Init", "Pre-Initialzing Game Server Process Listener ...", ColorStyle.Information);
                StreamReader COPor = process.StandardOutput;
                GameThreadInput = process.StandardInput;
                Log("Game Server Thread - Listener Feedback", "Starting Game Server Process Listener ...", ColorStyle.Information);
                for (; ; )
                {
                    if (process.HasExited == true)
                    {
                        break;
                    }
                    if (GameServerThreadLive == true)
                    {
                        try
                        {
                            Log("Game Server Thread - Listener Feedback", COPor.ReadLine(), ColorStyle.Question);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        break;
                    }
                }
                try
                {
                    //Log("Game Server Thread - Stop", "Stopping Game Server Thread", ColorStyle.Information);
                    //Log("Game Server Thread - Stop", "Stopping Process", ColorStyle.Information);
                    //process.Kill();
                    //Log("Game Server Thread - Stop", "Disposing Standard Input Port ...", ColorStyle.Information);
                    //GameThreadInput.Dispose();
                    //Log("Game Server Thread - Stop", "Closing Standard Input Port ...", ColorStyle.Information);
                    //GameThreadInput.Close();
                    //Log("Game Server Thread - Stop", "Done.", ColorStyle.Complete);
                    //Log("Game Server Thread - Stop", "Disposing Standard Output Port ...", ColorStyle.Information);
                    //COPor.Dispose();
                    //Log("Game Server Thread - Stop", "Closing Standard Output Port ...", ColorStyle.Information);
                    //COPor.Close();
                    //Log("Game Server Thread - Stop", "Done.", ColorStyle.Complete);
                    //Log("Game Server Thread - Stop", "Everything Was end", ColorStyle.Complete);
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                Log("Trust - Error", "Error has throwned.\n" + ex.ToString(), ColorStyle.Error);
            }
        }

        static void Main(string[] args)
        {

            Console.WriteAscii("SABash");
            Console.WriteLine("Verison 1.0.0.7 12-19-2020\n\n");
            Log("Trust - Execute", "Pre-Initialzing Trust Commands ...", ColorStyle.Information);
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                string[] trustcommands = File.ReadAllText(".\\trust.sabcmd").Split('\n');
                foreach(string command in trustcommands)
                {
                    Log("Trust - Execute - "+command.Replace("\r",""), "Execute command \""+command.Replace("\r", "") + "\" with Trust Executer", ColorStyle.Information);
                    Execute(command);
                    Log("Trust - Execute - " + command.Replace("\r", ""), "Done.", ColorStyle.Complete);
                }
                Log("Trust - Execute - Time","All processes was end in "+stopwatch.ElapsedMilliseconds.ToString()+" ms", ColorStyle.Complete);
            }
            catch(Exception ex)
            {
                Log("Trust - Error", "Error has throwned.\n" + ex.ToString(), ColorStyle.Error);
            }

            for(; ; )
            {
                Console.ForegroundColor = Color.Gray;
                Console.Write("$ADMIN$~1 !sh~ ");
                string command = Console.ReadLine();
                Execute(command);
            }

        }
    }
}
