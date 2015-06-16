namespace ZwiZwit
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.EnterButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.updateInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.updateCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.consumerKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.consumerSecret = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(124, 231);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(75, 23);
            this.EnterButton.TabIndex = 0;
            this.EnterButton.Text = "OK";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(205, 231);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "更新間隔(秒)";
            // 
            // updateInterval
            // 
            this.updateInterval.Location = new System.Drawing.Point(124, 9);
            this.updateInterval.MaxLength = 5;
            this.updateInterval.Name = "updateInterval";
            this.updateInterval.Size = new System.Drawing.Size(50, 19);
            this.updateInterval.TabIndex = 3;
            this.updateInterval.Validating += new System.ComponentModel.CancelEventHandler(this.updateInterval_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "取得行数";
            // 
            // updateCount
            // 
            this.updateCount.Location = new System.Drawing.Point(124, 37);
            this.updateCount.MaxLength = 4;
            this.updateCount.Name = "updateCount";
            this.updateCount.Size = new System.Drawing.Size(50, 19);
            this.updateCount.TabIndex = 5;
            this.updateCount.Validating += new System.ComponentModel.CancelEventHandler(this.reloadCount_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Consumer Key";
            // 
            // consumerKey
            // 
            this.consumerKey.Location = new System.Drawing.Point(124, 171);
            this.consumerKey.Name = "consumerKey";
            this.consumerKey.Size = new System.Drawing.Size(156, 19);
            this.consumerKey.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Consumer Secret";
            // 
            // consumerSecret
            // 
            this.consumerSecret.Location = new System.Drawing.Point(124, 196);
            this.consumerSecret.Name = "consumerSecret";
            this.consumerSecret.Size = new System.Drawing.Size(156, 19);
            this.consumerSecret.TabIndex = 5;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.consumerSecret);
            this.Controls.Add(this.consumerKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.updateCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.updateInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.EnterButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingForm";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox updateInterval;
        public System.Windows.Forms.TextBox updateCount;
        public System.Windows.Forms.TextBox consumerKey;
        public System.Windows.Forms.TextBox consumerSecret;
    }
}