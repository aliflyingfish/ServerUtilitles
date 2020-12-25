using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace ServerAutoBash
{
    public class Task
    {
        /// <summary>
        /// 要执行的命令
        /// </summary>
        public string[] commands;

        /// <summary>
        /// 时钟周期(执行一次后延迟的时间)
        /// </summary>
        public int ClockInterval = 250;

        private Thread LocalExeThread;

        

        /// <summary>
        /// 要发送信息的进程
        /// </summary>
        public Process EndProcess;

        public Task(string[] commands, Process process)
        {
            this.commands = commands;
            this.EndProcess = process;
            LocalExeThread = new Thread(ExecuteThread);
        }

        public Task(string[] commands, Process process,int interval)
        {
            this.commands = commands;
            this.EndProcess = process;
            this.ClockInterval = interval;
            LocalExeThread = new Thread(ExecuteThread);
        }

        private void ExecuteThread()
        {
                        
        } 

        /// <summary>
        /// 立即启动该任务
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// 立即暂停或挂起任务
        /// </summary>
        public void Suspend()
        {

        }

        /// <summary>
        /// 终止任务并释放资源
        /// </summary>
        public void Abort()
        {

        }

        ~Task()
        {
            commands = null;
            EndProcess.Dispose();
        }
    }
}
