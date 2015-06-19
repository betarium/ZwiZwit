namespace ZwiZwit
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.表示VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アクションAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reload2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.timeLineList = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timelineMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retweetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.urlOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.userText = new System.Windows.Forms.Label();
            this.messageText = new System.Windows.Forms.TextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.newTweetText = new System.Windows.Forms.TextBox();
            this.TweetButton = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayQuitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.twitter公式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.timelineMenu.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem,
            this.表示VToolStripMenuItem,
            this.アクションAToolStripMenuItem,
            this.reload2ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loginToolStripMenuItem.Text = "ログイン(&L)";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "終了(&Q)";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // 編集EToolStripMenuItem
            // 
            this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
            this.編集EToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.編集EToolStripMenuItem.Text = "編集(&E)";
            // 
            // 表示VToolStripMenuItem
            // 
            this.表示VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem});
            this.表示VToolStripMenuItem.Name = "表示VToolStripMenuItem";
            this.表示VToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.表示VToolStripMenuItem.Text = "表示(&V)";
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hideToolStripMenuItem.Text = "隠す(&H)";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // アクションAToolStripMenuItem
            // 
            this.アクションAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.reloadAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.twitter公式ToolStripMenuItem});
            this.アクションAToolStripMenuItem.Name = "アクションAToolStripMenuItem";
            this.アクションAToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.アクションAToolStripMenuItem.Text = "アクション(&A)";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reloadToolStripMenuItem.Text = "更新(&R)";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // reloadAllToolStripMenuItem
            // 
            this.reloadAllToolStripMenuItem.Name = "reloadAllToolStripMenuItem";
            this.reloadAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reloadAllToolStripMenuItem.Text = "全て更新(&L)";
            this.reloadAllToolStripMenuItem.Click += new System.EventHandler(this.reloadAllToolStripMenuItem_Click);
            // 
            // reload2ToolStripMenuItem
            // 
            this.reload2ToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.reload2ToolStripMenuItem.Name = "reload2ToolStripMenuItem";
            this.reload2ToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.reload2ToolStripMenuItem.Text = "リロード(&R)";
            this.reload2ToolStripMenuItem.Click += new System.EventHandler(this.reload2ToolStripMenuItem_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(632, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(632, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            this.splitContainer1.Panel1MinSize = 70;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(632, 295);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.timeLineList);
            this.splitContainer4.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.Panel1MinSize = 45;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.Panel2MinSize = 34;
            this.splitContainer4.Size = new System.Drawing.Size(632, 248);
            this.splitContainer4.SplitterDistance = 188;
            this.splitContainer4.TabIndex = 1;
            // 
            // timeLineList
            // 
            this.timeLineList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.timeLineList.ContextMenuStrip = this.timelineMenu;
            this.timeLineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLineList.FullRowSelect = true;
            this.timeLineList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.timeLineList.HideSelection = false;
            this.timeLineList.Location = new System.Drawing.Point(0, 0);
            this.timeLineList.Name = "timeLineList";
            this.timeLineList.Size = new System.Drawing.Size(632, 188);
            this.timeLineList.TabIndex = 2;
            this.timeLineList.UseCompatibleStateImageBehavior = false;
            this.timeLineList.View = System.Windows.Forms.View.Details;
            this.timeLineList.SelectedIndexChanged += new System.EventHandler(this.timeLineList_SelectedIndexChanged);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = " ";
            this.columnHeader0.Width = 20;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名前";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "投稿";
            this.columnHeader2.Width = 481;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "日時";
            this.columnHeader3.Width = 85;
            // 
            // timelineMenu
            // 
            this.timelineMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replyToolStripMenuItem,
            this.retweetToolStripMenuItem,
            this.favoriteToolStripMenuItem,
            this.urlOpenToolStripMenuItem,
            this.openStatusToolStripMenuItem});
            this.timelineMenu.Name = "timelineMenu";
            this.timelineMenu.Size = new System.Drawing.Size(164, 92);
            // 
            // replyToolStripMenuItem
            // 
            this.replyToolStripMenuItem.Name = "replyToolStripMenuItem";
            this.replyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.replyToolStripMenuItem.Text = "リプライ(&R)";
            this.replyToolStripMenuItem.Click += new System.EventHandler(this.replyToolStripMenuItem_Click);
            // 
            // retweetToolStripMenuItem
            // 
            this.retweetToolStripMenuItem.Name = "retweetToolStripMenuItem";
            this.retweetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.retweetToolStripMenuItem.Text = "リツイート(&T)";
            this.retweetToolStripMenuItem.Click += new System.EventHandler(this.retweetToolStripMenuItem_Click);
            // 
            // urlOpenToolStripMenuItem
            // 
            this.urlOpenToolStripMenuItem.Name = "urlOpenToolStripMenuItem";
            this.urlOpenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.urlOpenToolStripMenuItem.Text = "URLを開く(&U)...";
            this.urlOpenToolStripMenuItem.Click += new System.EventHandler(this.urlOpenToolStripMenuItem_Click);
            // 
            // openStatusToolStripMenuItem
            // 
            this.openStatusToolStripMenuItem.Name = "openStatusToolStripMenuItem";
            this.openStatusToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openStatusToolStripMenuItem.Text = "ステータスを開く(&S)...";
            this.openStatusToolStripMenuItem.Click += new System.EventHandler(this.openStatusToolStripMenuItem_Click);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.MinimumSize = new System.Drawing.Size(0, 50);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.userText);
            this.splitContainer5.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer5.Panel1MinSize = 12;
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.messageText);
            this.splitContainer5.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer5.Size = new System.Drawing.Size(632, 56);
            this.splitContainer5.SplitterDistance = 12;
            this.splitContainer5.TabIndex = 6;
            // 
            // userText
            // 
            this.userText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userText.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.userText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.userText.Location = new System.Drawing.Point(0, 0);
            this.userText.Name = "userText";
            this.userText.Size = new System.Drawing.Size(632, 12);
            this.userText.TabIndex = 0;
            this.userText.Text = "name";
            this.userText.DoubleClick += new System.EventHandler(this.userText_DoubleClick);
            // 
            // messageText
            // 
            this.messageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageText.Location = new System.Drawing.Point(0, 0);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ReadOnly = true;
            this.messageText.Size = new System.Drawing.Size(632, 40);
            this.messageText.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.newTweetText);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.TweetButton);
            this.splitContainer3.Size = new System.Drawing.Size(632, 43);
            this.splitContainer3.SplitterDistance = 571;
            this.splitContainer3.TabIndex = 6;
            // 
            // newTweetText
            // 
            this.newTweetText.AcceptsReturn = true;
            this.newTweetText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newTweetText.Location = new System.Drawing.Point(0, 0);
            this.newTweetText.MinimumSize = new System.Drawing.Size(100, 12);
            this.newTweetText.Multiline = true;
            this.newTweetText.Name = "newTweetText";
            this.newTweetText.Size = new System.Drawing.Size(571, 43);
            this.newTweetText.TabIndex = 0;
            // 
            // TweetButton
            // 
            this.TweetButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.TweetButton.Location = new System.Drawing.Point(0, 0);
            this.TweetButton.Name = "TweetButton";
            this.TweetButton.Size = new System.Drawing.Size(57, 43);
            this.TweetButton.TabIndex = 0;
            this.TweetButton.Text = "Post";
            this.TweetButton.UseVisualStyleBackColor = true;
            this.TweetButton.Click += new System.EventHandler(this.TweetButton_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Text = "trayIcon";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.trayQuitToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(111, 48);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.showWindowToolStripMenuItem.Text = "表示(&S)";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.showWindowToolStripMenuItem_Click);
            // 
            // trayQuitToolStripMenuItem
            // 
            this.trayQuitToolStripMenuItem.Name = "trayQuitToolStripMenuItem";
            this.trayQuitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.trayQuitToolStripMenuItem.Text = "終了(&Q)";
            this.trayQuitToolStripMenuItem.Click += new System.EventHandler(this.trayQuitToolStripMenuItem_Click);
            // 
            // favoriteToolStripMenuItem
            // 
            this.favoriteToolStripMenuItem.Name = "favoriteToolStripMenuItem";
            this.favoriteToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.favoriteToolStripMenuItem.Text = "お気に入り(&F)";
            this.favoriteToolStripMenuItem.Click += new System.EventHandler(this.favoriteToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingToolStripMenuItem.Text = "設定(&E)";
            this.settingToolStripMenuItem.Click += new System.EventHandler(this.settingToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // twitter公式ToolStripMenuItem
            // 
            this.twitter公式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHomeToolStripMenuItem});
            this.twitter公式ToolStripMenuItem.Name = "twitter公式ToolStripMenuItem";
            this.twitter公式ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.twitter公式ToolStripMenuItem.Text = "Twitter公式";
            // 
            // showHomeToolStripMenuItem
            // 
            this.showHomeToolStripMenuItem.Name = "showHomeToolStripMenuItem";
            this.showHomeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.showHomeToolStripMenuItem.Text = "ホームを表示(&H)...";
            this.showHomeToolStripMenuItem.Click += new System.EventHandler(this.showHomeToolStripMenuItem_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 366);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ZwiZwit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.timelineMenu.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem アクションAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表示VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reload2ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem trayQuitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListView timeLineList;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Label userText;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox newTweetText;
        private System.Windows.Forms.Button TweetButton;
        private System.Windows.Forms.ContextMenuStrip timelineMenu;
        private System.Windows.Forms.ToolStripMenuItem retweetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem urlOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem twitter公式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHomeToolStripMenuItem;
    }
}

