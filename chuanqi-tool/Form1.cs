using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace chuanqi_tool
{
    public partial class Form1 : Form
    {
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
            FileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.FileName;
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Closeprocess();
        }
        public void Closeprocess()
        {
            if (this.textBox1.Text == string.Empty)
                return;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                Console.WriteLine(p.MainModule.FileName);
                if (p.MainModule.FileName == this.textBox1.Text)
                {
                    p.Kill();
                    MessageBox.Show("关闭成功");
                    this.timer1.Stop();
                    return;
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")==DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess();
               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.button3.Text=="启动")
            {
                this.timer1.Start();
                this.dateTimePicker1.Enabled = false;
                this.button3.Text = "停止";
            }
            else

            {
                this.dateTimePicker1.Enabled = true;
                this.timer1.Stop();
                this.button3.Text = "启动";
            }
        }
    }
}
