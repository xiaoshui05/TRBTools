
namespace TRBTools
{
    partial class Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SpeedInput = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.About = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.FinalSwarmButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.FinalSwarmE = new System.Windows.Forms.CheckBox();
            this.GameWinButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LoadScript = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "1.按F3开始存档";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "2.修改速度需要暂停再继续";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "版本：0.1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "日志：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(52, 215);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(218, 21);
            this.textBox1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "速度：";
            // 
            // SpeedInput
            // 
            this.SpeedInput.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SpeedInput.Location = new System.Drawing.Point(52, 43);
            this.SpeedInput.Name = "SpeedInput";
            this.SpeedInput.Size = new System.Drawing.Size(49, 21);
            this.SpeedInput.TabIndex = 8;
            this.SpeedInput.Text = "1";
            this.SpeedInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpeedInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "游戏版本：1.1.3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(86, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "亿万僵尸工具";
            // 
            // About
            // 
            this.About.Location = new System.Drawing.Point(231, 9);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(39, 23);
            this.About.TabIndex = 12;
            this.About.Text = "关于";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(107, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "注：Enter键确认";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(209, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "3.按F8开启连点器，再按F8关闭连点器";
            // 
            // FinalSwarmButton
            // 
            this.FinalSwarmButton.Enabled = false;
            this.FinalSwarmButton.Location = new System.Drawing.Point(53, 183);
            this.FinalSwarmButton.Name = "FinalSwarmButton";
            this.FinalSwarmButton.Size = new System.Drawing.Size(75, 23);
            this.FinalSwarmButton.TabIndex = 15;
            this.FinalSwarmButton.Text = "最后一波";
            this.FinalSwarmButton.UseVisualStyleBackColor = true;
            this.FinalSwarmButton.Click += new System.EventHandler(this.FinalSwarmButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "6.";
            // 
            // FinalSwarmE
            // 
            this.FinalSwarmE.AutoSize = true;
            this.FinalSwarmE.Enabled = false;
            this.FinalSwarmE.Location = new System.Drawing.Point(32, 188);
            this.FinalSwarmE.Name = "FinalSwarmE";
            this.FinalSwarmE.Size = new System.Drawing.Size(15, 14);
            this.FinalSwarmE.TabIndex = 18;
            this.FinalSwarmE.UseVisualStyleBackColor = true;
            // 
            // GameWinButton
            // 
            this.GameWinButton.Location = new System.Drawing.Point(134, 183);
            this.GameWinButton.Name = "GameWinButton";
            this.GameWinButton.Size = new System.Drawing.Size(40, 23);
            this.GameWinButton.TabIndex = 19;
            this.GameWinButton.Text = "胜利";
            this.GameWinButton.UseVisualStyleBackColor = true;
            this.GameWinButton.Click += new System.EventHandler(this.GameWinButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(155, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "4.按F1显血，按Alt关闭显血";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 164);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(161, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "5.F6全图，F7开打存档文件夹";
            // 
            // LoadScript
            // 
            this.LoadScript.Location = new System.Drawing.Point(179, 183);
            this.LoadScript.Name = "LoadScript";
            this.LoadScript.Size = new System.Drawing.Size(91, 23);
            this.LoadScript.TabIndex = 22;
            this.LoadScript.Text = "生成读档脚本";
            this.LoadScript.UseVisualStyleBackColor = true;
            this.LoadScript.Click += new System.EventHandler(this.LoadScript_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.LoadScript);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.GameWinButton);
            this.Controls.Add(this.FinalSwarmE);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.FinalSwarmButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.About);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.SpeedInput);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form";
            this.Text = "亿万僵尸工具-0.1.0";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SpeedInput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button FinalSwarmButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox FinalSwarmE;
        private System.Windows.Forms.Button GameWinButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button LoadScript;
    }
}

