using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void LightingShaking()
        {
            for(; ; )
            {
                lighting.Visible = false;
                Thread.Sleep(120);
                lighting.Visible = true;
                Thread.Sleep(180);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Thread(LightingShaking).Start();
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
            if (e.KeyChar == '\n'|| e.KeyChar == '\r')
            {
                ExecuteValue = EnterValue;
                statusText.Text = "就绪";
            }
            else if (e.KeyChar == '\b')
            {
                EnterValue.Remove(EnterValue.Length - 1);
                statusText.Text = EnterValue;
            }
            else
            {
                EnterValue += e.KeyChar;
                statusText.Text = EnterValue;
            }
        }

        private void marqueeProgressBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
