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
            Data.hotKeys.Add(Keys.F3, 3);// 保存
            Data.hotKeys.Add(Keys.F8, 8);// 连点器
            Data.hotKeys.Add(Keys.F6, 6);// 全图
            Data.hotKeys.Add(Keys.F1, 1);// 显血
            Data.hotKeys.Add(Keys.F7, 7);// 打开存档文件夹
            Tools.HotKey(this.Handle, true);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Tools.HotKey(this.Handle, false);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    if (m.WParam.ToInt32() == 3)
                    {
                        AutoSave.Run();
                    }
                    else if (m.WParam.ToInt32() == 8)
                    {
                        MouseLeftClick.Run();
                    }
                    else if (m.WParam.ToInt32() == 6)
                    {
                        new ShowMap().Run();
                    }
                    else if (m.WParam.ToInt32() == 1)
                    {
                        ShowBlood.Run();
                    }
                    else if (m.WParam.ToInt32() == 7)
                    {
                        string path = Tools.SavePath();
                        System.Diagnostics.Process.Start(path);
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
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8)
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
                + "6. 速度输入框仅用于设置速度，不是游戏实时速度\n"
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

        private void Form_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 10;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(this.FinalSwarmButton, "如果无效，请选中前面的小框");
            toolTip.SetToolTip(this.FinalSwarmE, "修复最后一波无效");
            toolTip.SetToolTip(this.LoadScript, "在存档文件夹生成读档脚本【快速读档.bat】，用于读最近一次存档");
        }

        private void GameWinButton_Click(object sender, EventArgs e)
        {
            FinalSwarm finalSwarm = new FinalSwarm();
            if (this.FinalSwarmE.Checked == true)
            {
                finalSwarm.extend = true;
            }
            finalSwarm.gameWinMark = true;
            finalSwarm.Run();
        }

        private void LoadScript_Click(object sender, EventArgs e)
        {
            string path = Tools.SavePath();
            StreamWriter sw = new StreamWriter(path + "\\快速读档.bat");
            sw.WriteLine(@"md ""TRBToolsBackup""");
            sw.WriteLine(@"md ""TRBToolsBackup\temp""");
            sw.WriteLine(@"powershell Move-Item -Path .\* -Destination '.\TRBToolsBackup\temp' -Force -Include *.zxcheck,*.zxsav -Exclude *_Backup.zxcheck,*_Backup.zxsav");
            sw.Flush();
            sw.Close();
        }
    }
}
