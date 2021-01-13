namespace vrclient
{
    partial class Form1
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
            this.inputTB = new System.Windows.Forms.TextBox();
            this.outputTB = new System.Windows.Forms.TextBox();
            this.sendBTN = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputTB
            // 
            this.inputTB.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inputTB.Location = new System.Drawing.Point(67, 42);
            this.inputTB.Margin = new System.Windows.Forms.Padding(2);
            this.inputTB.Name = "inputTB";
            this.inputTB.Size = new System.Drawing.Size(245, 32);
            this.inputTB.TabIndex = 0;
            // 
            // outputTB
            // 
            this.outputTB.Location = new System.Drawing.Point(67, 127);
            this.outputTB.Margin = new System.Windows.Forms.Padding(2);
            this.outputTB.Multiline = true;
            this.outputTB.Name = "outputTB";
            this.outputTB.ReadOnly = true;
            this.outputTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTB.Size = new System.Drawing.Size(245, 148);
            this.outputTB.TabIndex = 1;
            this.outputTB.WordWrap = false;
            // 
            // sendBTN
            // 
            this.sendBTN.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sendBTN.Location = new System.Drawing.Point(90, 85);
            this.sendBTN.Margin = new System.Windows.Forms.Padding(2);
            this.sendBTN.Name = "sendBTN";
            this.sendBTN.Size = new System.Drawing.Size(86, 33);
            this.sendBTN.TabIndex = 2;
            this.sendBTN.Text = "send";
            this.sendBTN.UseVisualStyleBackColor = true;
            this.sendBTN.Click += new System.EventHandler(this.SendBTN_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(210, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 33);
            this.button1.TabIndex = 3;
            this.button1.Text = "quit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 310);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sendBTN);
            this.Controls.Add(this.outputTB);
            this.Controls.Add(this.inputTB);
            this.Margin = new System.Windows.Forms.Padding(2);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputTB;
        private System.Windows.Forms.Button sendBTN;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox outputTB;
    }
}

