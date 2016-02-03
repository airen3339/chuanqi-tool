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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            log.Info(string.Format("目录{0}:开始执行自动开区", path));
            this.Cursor = Cursors.WaitCursor;
            //1.开始做一个判断

            //            判断指定路径下\GameCenter.exe是否在运行(就是遍历一边当前运行进程中有没有这个路径的，或者你有更好的方法都可以)

            //如果\GameCenter.exe没有在运行，直接执行第三步就行(启动)

            //如果\GameCenter.exe在运行就按下面123步顺序执行(关闭，初始化，启动)

            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                try
                {
                    Console.WriteLine(p.MainModule.FileName);
                }
                catch (Exception)
                {
                    continue;
                }

                string pathexe = string.Format("{0}\\GameCenter.exe", path);
                if (p.MainModule.FileName == pathexe)
                {
                    log.Info(string.Format("找到已运行进程{0},通过发送按键消息停止服务并结束进程", pathexe));
                    SetForegroundWindow(p.MainWindowHandle);

                    Thread.Sleep(1000);
                    SendKeys.SendWait("%t");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("%y");
                    SendKeys.Flush();
                    Thread.Sleep(20000);
                    //检查相关进程是否都已经关闭，如果没有强制关闭
                    if (this.CheckClose(p, path))
                    {
                        log.Info("正常关闭" + p.MainModule.FileName + "的子进程");
                    }
                    else
                    {
                        log.Info("第二次尝试发送按键进行关闭");
                        this.CheckClose(p, path);
                    }
                    p.Kill();
                    Process pro = new Process();
                    //第2步，初始化
                    string FileName = string.Format("{0}\\{1}", path, "清理数据正式开区.bat");
                    log.Info("执行批处理" + FileName);
                    pro.StartInfo.FileName = FileName;
                    pro.StartInfo.WorkingDirectory = path;
                    pro.Start();
                    pro.WaitForExit();
                    Thread.Sleep(10000);
                }

            }
            //第3步，启动服务端
            //找到目录下的GameCenter.exe，运行

            log.Info("启动服务端"+ string.Format("{0}\\GameCenter.exe", path));
            Process process = new Process();
            process.StartInfo.FileName = string.Format("{0}\\GameCenter.exe", path);
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WorkingDirectory = path;
            process.Start();
            Thread.Sleep(1000);
            SetForegroundWindow(process.MainWindowHandle);
            SendKeys.SendWait("%s");
            Thread.Sleep(1000);
            SetForegroundWindow(process.MainWindowHandle);
            SendKeys.SendWait("%y");
            SendKeys.Flush();
            Thread.Sleep(20000);
            if (this.CheckOpen(process, path))
            {
                log.Info("正常开启" + path);
            }
            else
            {
                log.Info("第二次检查是否正常启动");
                this.CheckOpen(process, path);
            }
            this.Cursor = Cursors.Default;
        }

        private bool CheckOpen(Process process, string path)
        {
            log.Info("检查是否正常启动进程" + string.Format("{0}\\GameCenter.exe", path));
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                try
                {
                    Console.WriteLine(p.MainModule.FileName);
                }
                catch (Exception)
                {
                    continue;
                }
                string pathexe = string.Format("{0}\\DBServer\\DBServer.exe", path);
                if (p.MainModule.FileName.ToLower() == pathexe.ToLower())
                {
                    //找到目录下的进程，证明已通过按键去打开子进程退出
                    log.Info("查到已运行进程：" + pathexe);
                    return true;
                }
            }
            //未找到应用，重新发送命令
            log.Info("未找到相关子进程，重新发送按键消息,控制台：");
            log.Info("控制台进程" + process.MainModule.FileName+"前置");
            SetForegroundWindow(process.MainWindowHandle);
            log.Info("开始发送ALT+S");
            SendKeys.SendWait("%s");
            Thread.Sleep(1000);
            log.Info("控制台进程" + process.MainModule.FileName + "前置");
            log.Info("开始发送ALT+Y");
            SetForegroundWindow(process.MainWindowHandle);
            SendKeys.SendWait("%y");
            SendKeys.Flush();
            Thread.Sleep(20000);
            return false;
        }

        private bool CheckClose(Process process, string path)
        {
            string strpro = ConfigurationManager.AppSettings["process"];
            string[] arr = strpro.Split('|');
            log.Info(string.Format("正在检查{0}是否已正常关闭进程...", path));
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                try
                {
                    Console.WriteLine(p.MainModule.FileName);
                }
                catch (Exception)
                {
                    continue;
                }
                string proName = p.MainModule.FileName;
                //找到目录下的进程，证明没有通过按键去关闭，重新发送按键关闭
                foreach (var item in arr)
                {
                    string fullpath = (path + item);
                    if (proName.ToLower() == fullpath.ToLower())
                    {
                        log.Info(string.Format("找到进程{0},发送按键进行关闭", proName));

                        log.Info("控制台进程名：" + process.MainModule.FileName);
                        log.Info("开始发送ALT+T");
                        SetForegroundWindow(process.MainWindowHandle);
                        SendKeys.SendWait("%t");
                        Thread.Sleep(1000);
                        log.Info("开始发送ALT+Y");
                        SetForegroundWindow(process.MainWindowHandle);
                        SendKeys.SendWait("%y");
                        SendKeys.Flush();
                        return false;
                    }
                }
            }
            return true;
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
                this.dateTimePicker1_1.Enabled = false;
                this.dateTimePicker2_1.Enabled = false;
                this.dateTimePicker3_1.Enabled = false;
                this.dateTimePicker4_1.Enabled = false;
                this.dateTimePicker5_1.Enabled = false;
                this.dateTimePicker6_1.Enabled = false;
                this.dateTimePicker7_1.Enabled = false;
                this.dateTimePicker8_1.Enabled = false;
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
                this.dateTimePicker1_1.Enabled = true;
                this.dateTimePicker2_1.Enabled = true;
                this.dateTimePicker3_1.Enabled = true;
                this.dateTimePicker4_1.Enabled = true;
                this.dateTimePicker5_1.Enabled = true;
                this.dateTimePicker6_1.Enabled = true;
                this.dateTimePicker7_1.Enabled = true;
                this.dateTimePicker8_1.Enabled = true;
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

            if (this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") || this.dateTimePicker1_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox1.Text);

            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") || this.dateTimePicker2_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox2.Text);

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                || this.dateTimePicker3_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox3.Text);

            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                || this.dateTimePicker4_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox4.Text);

            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {

            if (this.dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                || this.dateTimePicker5_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox5.Text);

            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker6.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") || this.dateTimePicker6_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox6.Text);

            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker7.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ||
                this.dateTimePicker7_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox7.Text);

            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker8.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                || this.dateTimePicker8_1.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                AutoRun(this.textBox8.Text);

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

        private void CloseProcess(string path)
        {
            

        }

    }

}
