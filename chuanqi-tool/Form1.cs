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
            Button btn = sender as Button;
            FileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (btn.Name == "button1")
                    this.textBox1.Text = dialog.FileName;
                if (btn.Name == "button2")
                    this.textBox2.Text = dialog.FileName;
                if (btn.Name == "button3")
                    this.textBox3.Text = dialog.FileName;
                if (btn.Name == "button4")
                    this.textBox4.Text = dialog.FileName;
                if (btn.Name == "button5")
                    this.textBox5.Text = dialog.FileName;
                if (btn.Name == "button6")
                    this.textBox6.Text = dialog.FileName;
                if (btn.Name == "button7")
                    this.textBox7.Text = dialog.FileName;
                if (btn.Name == "button8")
                    this.textBox8.Text = dialog.FileName;
            }

        }

        public void Closeprocess(string filename)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                Console.WriteLine(p.MainModule.FileName);
                if (p.MainModule.FileName == filename)
                {
                    p.Kill();
                    return;
                }
            }
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
                Closeprocess(this.textBox1.Text);
                this.timer1.Stop();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox2.Text);
                this.timer2.Stop();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox3.Text);
                this.timer3.Stop();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox4.Text);
                this.timer4.Stop();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {

            if (this.dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox5.Text);
                this.timer5.Stop();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker6.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox6.Text);
                this.timer6.Stop();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker7.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox7.Text);
                this.timer7.Stop();
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (this.dateTimePicker8.Value.ToString("yyyy-MM-dd HH:mm:ss") == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            {
                Closeprocess(this.textBox8.Text);
                this.timer8.Stop();
            }
        }
    }
}
