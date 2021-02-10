﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRBTools
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
            Data.form1 = this;
            AutoSave.form1 = this;
            MouseLeftClick.form1 = this;
            AutoSave.HotKey(this.Handle, true);
            MouseLeftClick.HotKey(this.Handle, true);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            AutoSave.HotKey(this.Handle, false);
            MouseLeftClick.HotKey(this.Handle, false);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    if (m.WParam.ToInt32() == 1)
                    {
                        AutoSave.Run();
                    }
                    else if (m.WParam.ToInt32() == 2)
                    {
                        MouseLeftClick.Run();
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public delegate void SetTextBoxValue(String str);

        public void SetTextBox1Value(string str)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextBoxValue objSetTextBoxValue = new SetTextBoxValue(SetTextBox1Value);
                IAsyncResult result = this.textBox1.BeginInvoke(objSetTextBoxValue, new object[] { str });
                try
                {
                    objSetTextBoxValue.EndInvoke(result);
                }
                catch { }
            }
            else
            {
                this.textBox1.Text = str;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 13)
            {
                if (SpeedInput.Text == "") return;
                double speed = Convert.ToDouble(SpeedInput.Text);
                Speed speedObj = new Speed(this, speed);
                Thread th = new Thread(speedObj.Run);
                th.Start();
                //Console.WriteLine(speed);
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            String about = ""
                + "1. 任何操作都需要时间，请勿重复执行\n"
                + "2. 按下F3后游戏开始存档，8s后开始备份存档\n"
                + "3. 如果存档时间超过8s会备份失败，这时需要手动备份\n"
                + "4. 存档未结束前不能开始新的存档命令，即两次F3命令要间隔8s才能有效\n"
                + "5. 速度只能是整数，不建议太大\n"
                + "6. 速度输入框仅用于设置速度，不是游戏实时速度"
                + "7. 不同难度的最后一波有可能叠加";

            MessageBox.Show(this, about);
        }

        private void FinalSwarmButton_Click(object sender, EventArgs e)
        {
            FinalSwarm finalSwarm = new FinalSwarm();
            if (this.FinalSwarmE.Checked == true)
            {
                finalSwarm.extend = true;
            }
            finalSwarm.Run();
        }

        private void ShowMapButton_Click(object sender, EventArgs e)
        {
            ShowMap showMap = new ShowMap();
            showMap.Run();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 10;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(this.FinalSwarmButton, "如果无效，请选中前面的小框");
            toolTip.SetToolTip(this.FinalSwarmE, "修复最后一波无效");
        }
    }
}
