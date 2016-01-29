using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace chuanqi_tool
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.dateTimePicker1.Value = DateTime.Now;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (btn.Name == "button1")
                    this.textBox1.Text = dialog.SelectedPath;
                if (btn.Name == "button2")
                    this.textBox2.Text = dialog.SelectedPath;
                if (btn.Name == "button3")
                    this.textBox3.Text = dialog.SelectedPath;
                if (btn.Name == "button4")
                    this.textBox4.Text = dialog.SelectedPath;
                if (btn.Name == "button5")
                    this.textBox5.Text = dialog.SelectedPath;
                if (btn.Name == "button6")
                    this.textBox6.Text = dialog.SelectedPath;
                if (btn.Name == "button7")
                    this.textBox7.Text = dialog.SelectedPath;
                if (btn.Name == "button8")
                    this.textBox8.Text = dialog.SelectedPath;
            }

        }

        public void AutoRun(string path)
        {
            //1.开始做一个判断

            //            判断指定路径下\GameCenter.exe是否在运行(就是遍历一边当前运行进程中有没有这个路径的，或者你有更好的方法都可以)

            //如果\GameCenter.exe没有在运行，直接执行第三步就行(启动)

            //如果\GameCenter.exe在运行就按下面123步顺序执行(关闭，初始化，启动)

            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                Console.WriteLine(p.MainModule.FileName);
                string pathexe = string.Format("{0}\\GameCenter.exe", path);
                if (p.MainModule.FileName == pathexe)
                {
                    SetForegroundWindow(p.MainWindowHandle);
                    Thread.Sleep(1000);
                    SendKeys.SendWait("%t");

                    SendKeys.SendWait("%y");

                    SendKeys.SendWait("%t");

                    SendKeys.SendWait("%y");
                    SendKeys.Flush();
                    Thread.Sleep(30000);
                    p.Kill();

                    //第2步，初始化
                    string FileName = string.Format("{0}\\{1}", path, "清理数据正式开区.bat");

                    Process.Start(FileName);
                    Thread.Sleep(3000);
                }

            }
            //第3步，启动服务端
            //找到目录下的GameCenter.exe，运行
            Process process = new Process();
            process.StartInfo.FileName = string.Format("{0}\\GameCenter.exe", path);
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WorkingDirectory = path;
            process.Start();
            Thread.Sleep(1000);
            SetForegroundWindow(process.MainWindowHandle);
            Console.WriteLine(process.MainModule.FileName);
            SendKeys.SendWait("%s");
            Thread.Sleep(1000);
            SendKeys.SendWait("%y");
            SendKeys.Flush();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (this.btn.Text == "启动")
            {
                if (!string.IsNullOrEmpty(this.textBox1.Text))
                    this.timer1.Start();
                if (!string.IsNullOrEmpty(this.textBox2.Text))
                    this.timer2.Start();
                if (!string.IsNullOrEmpty(this.textBox3.Text))
                    this.timer3.Start();
                if (!string.IsNullOrEmpty(this.textBox4.Text))
                    this.timer4.Start();
                if (!string.IsNullOrEmpty(this.textBox5.Text))
                    this.timer5.Start();
                if (!string.IsNullOrEmpty(this.textBox6.Text))
                    this.timer6.Start();
                if (!string.IsNullOrEmpty(this.textBox7.Text))
                    this.timer7.Start();
                if (!string.IsNullOrEmpty(this.textBox8.Text))
                    this.timer8.Start();
                this.dateTimePicker1.Enabled = false;
                this.dateTimePicker2.Enabled = false;
                this.dateTimePicker3.Enabled = false;
                this.dateTimePicker4.Enabled = false;
                this.dateTimePicker5.Enabled = false;
                this.dateTimePicker6.Enabled = false;
                this.dateTimePicker7.Enabled = false;
                this.dateTimePicker8.Enabled = false;
                this.btn.Text = "停止";
            }
            else

            {
                this.dateTimePicker1.Enabled = true;
                this.dateTimePicker2.Enabled = true;
                this.dateTimePicker3.Enabled = true;
                this.dateTimePicker4.Enabled = true;
                this.dateTimePicker5.Enabled = true;
                this.dateTimePicker6.Enabled = true;
                this.dateTimePicker7.Enabled = true;
                this.dateTimePicker8.Enabled = true;
                this.timer1.Stop();
                this.timer2.Stop();
                this.timer3.Stop();
                this.timer4.Stop();
                this.timer5.Stop();
                this.timer6.Stop();
                this.timer7.Stop();
                this.timer8.Stop();
                this.btn.Text = "启动";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox1.Text);
                this.timer1.Stop();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox2.Text);
                this.timer2.Stop();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox3.Text);
                this.timer3.Stop();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox4.Text);
                this.timer4.Stop();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {

            if (this.dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox5.Text);
                this.timer5.Stop();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker6.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox6.Text);
                this.timer6.Stop();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker7.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox7.Text);
                this.timer7.Stop();
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker8.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox8.Text);
                this.timer8.Stop();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请选择第一个文本的路径");
                return;
            }
            AutoRun(this.textBox1.Text);
        }


    }

}
