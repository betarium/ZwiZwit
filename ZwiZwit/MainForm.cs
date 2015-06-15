using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Media;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Library;

namespace ZwiZwit
{
    public partial class MainForm : Form
    {
        private readonly string INI_PATH = AppUtil.IniPath;

        private TwitterAccessExtend twitterObj = new TwitterAccessExtend();
        private TweetCache tweetCache = new TweetCache();
        private Timer refreshTimer = new Timer();

        //private Dictionary<long, bool> StatusHash = new Dictionary<long, bool>();

        private bool reloadForce;
        private long reloadLastId;

        private bool replyMode;
        private long replyTargetId;

        //private Dictionary<string, bool> ignoreTweetTable = new Dictionary<string,bool>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            twitterObj.ConsumerKey = Properties.Settings.Default.ConsumerKey;
            twitterObj.ConsumerSecret = Properties.Settings.Default.ConsumerSecret;

            columnHeader0.Width = 20;
            columnHeader1.Width = 100;
            columnHeader2.Width = 350;
            columnHeader3.Width = 120;

            userText.Text = "";

#if DEBUG
            //Icon = Properties.Resources.icon2;
#endif

            var iconFile = Path.Combine(Application.StartupPath, "app.ico");
            if (File.Exists(iconFile))
            {
                Icon = new System.Drawing.Icon(iconFile);
            }

            trayIcon.Icon = Icon;
            trayIcon.Text = Application.ProductName + AppUtil.DebugTitle;

            string version = Win32Api.GetPrivateProfileString("COMMON", "version", null, INI_PATH);
            if (version == "")
            {
                Win32Api.WritePrivateProfileString("COMMON", "version", Application.ProductName + " " + Application.ProductVersion, INI_PATH);
                Win32Api.WritePrivateProfileString("TIMELINE", "reload_interval", "600", INI_PATH);
                Win32Api.WritePrivateProfileString("TIMELINE", "get_count", "100", INI_PATH);
                Win32Api.WritePrivateProfileString("ACCOUNT", "oauth_token", "", INI_PATH);
                Win32Api.WritePrivateProfileString("ACCOUNT", "oauth_token_secret", "", INI_PATH);
            }

            bool trace_log = Win32Api.GetPrivateProfileInt("COMMON", "trace_log", 0, INI_PATH) != 0;
            if (trace_log)
            {
                AppUtil.InitTraceLog();
                System.Diagnostics.Trace.WriteLine("start:" + DateTime.Now.ToString());
            }

            string consumerKeyCustom = Win32Api.GetPrivateProfileString("DEBUG", "consumer_key", null, INI_PATH);
            string consumerSecretCustom = Win32Api.GetPrivateProfileString("DEBUG", "consumer_secret", null, INI_PATH);
            if (!string.IsNullOrEmpty(consumerKeyCustom))
            {
                twitterObj.ConsumerKey = consumerKeyCustom;
                twitterObj.ConsumerSecret = consumerSecretCustom;
            }
            bool hide_taskbar = Win32Api.GetPrivateProfileInt("DEBUG", "hide_taskbar", 0, INI_PATH) != 0;
            if (hide_taskbar)
            {
                ShowInTaskbar = false;
            }

            timeLineList.SmallImageList = twitterObj.UserIconList;

            refreshTimer.Tick += new EventHandler(OnTimer);
            refreshTimer.Interval = (int)Win32Api.GetPrivateProfileInt("TIMELINE", "reload_interval", 600, INI_PATH) * 1000;
            refreshTimer.Start();

            tweetCache.Load();


            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
            else
            {
                Show();
            }

            string oauth_token = Win32Api.GetPrivateProfileString("ACCOUNT", "oauth_token", null, INI_PATH);
            string oauth_token_secret = Win32Api.GetPrivateProfileString("ACCOUNT", "oauth_token_secret", null, INI_PATH);

            Login(oauth_token, oauth_token_secret);

            if (!twitterObj.IsAuth)
            {
                return;
            }

            int skip_startup_refresh = (int)Win32Api.GetPrivateProfileInt("TIMELINE", "skip_startup_refresh", 0, INI_PATH);
            if (skip_startup_refresh == 0)
            {
                BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            }
        }

        protected bool Login(string oauth_token, string oauth_token_secret)
        {
            bool newLogin = false;
            if (String.IsNullOrEmpty(oauth_token) || String.IsNullOrEmpty(oauth_token_secret))
            {
                newLogin = true;
                LoginForm form = new LoginForm();
                form.Twitter = twitterObj;
                Show();
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                string pin = form.PinText.Text;
                if (pin == "")
                {
                    AppUtil.ErrorLog("ログインエラー");
                    AppUtil.ShowError("ログインに失敗しました。");
                    return false;
                }
                string token = form.RequestOauthToken;

                if (!twitterObj.PostOauthAccessToken(token, pin))
                {
                    AppUtil.ShowError("ログインに失敗しました。");
                    return false;
                }

                oauth_token = twitterObj.OauthToken;
                oauth_token_secret = twitterObj.OauthTokenSecret;
            }
            else
            {
                twitterObj.OauthToken = oauth_token;
                twitterObj.OauthTokenSecret = oauth_token_secret;
            }

            TwitterAccess.UserInfo currentUserInfo = null;
            try
            {
                currentUserInfo = twitterObj.GetAccountVerifyCredentials();
            }
            catch (WebException we)
            {
                AppUtil.ErrorLog("ログインエラー", we);
                AppUtil.ShowError("ログインに失敗しました。");
                return false;
            }

            //Text = AppUtil.AssemblyTitle + " @" + currentUserInfo.screen_name + AppUtil.DebugTitle;
            Text = AppUtil.AssemblyTitle + " " + AppUtil.DebugTitle;

            if (newLogin)
            {
                Win32Api.WritePrivateProfileString("ACCOUNT", "oauth_token", oauth_token, INI_PATH);
                Win32Api.WritePrivateProfileString("ACCOUNT", "oauth_token_secret", oauth_token_secret, INI_PATH);
                Win32Api.WritePrivateProfileString("ACCOUNT", "screen_name", currentUserInfo.screen_name, INI_PATH);
                timeLineList.Items.Clear();
                //StatusHash.Clear();
                tweetCache.Clear();
                userText.Text = "";
                messageText.Text = "";
            }

            //BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            return true;
        }

        bool errorDisplayFlag;

        protected void UpdateList()
        {
            try
            {
                TweetButton.Enabled = false;

                int getCount = (int)Win32Api.GetPrivateProfileInt("TIMELINE", "get_count", 100, INI_PATH);
                List<TwitterAccess.StatusInfo> listItem2 = null;

                try
                {
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    param.Add("count", getCount);
                    if (!reloadForce && reloadLastId != 0)
                    {
                        param.Add("since_id", reloadLastId);
                    }
                    reloadForce = false;
                    listItem2 = twitterObj.GetStatusesHomeTimeline(param);
                }
                catch (TwitterAccess.TwitterException te)
                {
                    if (!errorDisplayFlag)
                    {
                        errorDisplayFlag = true;
                        AppUtil.ShowError("タイムラインの取得に失敗しました。", te);
                    }
                    return;
                }
                errorDisplayFlag = false;

                List<TwitterAccess.StatusInfo> listItem1 = new List<TwitterAccess.StatusInfo>();
                try
                {
                    listItem1 = twitterObj.GetStatusesMentions(-1);
                }
                catch (WebException we)
                {
                    AppUtil.ShowError("リプライの取得に失敗しました。", we);
                    return;
                }
                listItem1.AddRange(listItem2);

                long lastId = 0;
                List<TwitterAccess.StatusInfo> listItem = new List<TwitterAccess.StatusInfo>();
                foreach (var statusItem in listItem1)
                {
                    if (!tweetCache.AddTweet(statusItem))
                    {
                        continue;
                    }

                    listItem.Add(statusItem);

                    if (lastId < statusItem.id)
                    {
                        lastId = statusItem.id;
                    }
                }
                reloadLastId = lastId;

                string keyword = Win32Api.GetPrivateProfileString("FILTER", "keyword", null, INI_PATH);

                //int itemIndex = 0;
                foreach (var statusItem in listItem)
                {
                    OnUpdateListItem(statusItem);

                    int itemIndex = timeLineList.Items.Count;
                    for (int i = 0; i < timeLineList.Items.Count; i++)
                    {
                        TwitterAccess.StatusInfo sts = (TwitterAccess.StatusInfo)timeLineList.Items[i].Tag;
                        if ((sts.created_at.Ticks < statusItem.created_at.Ticks) || (sts.created_at.Ticks == statusItem.created_at.Ticks && sts.id <= statusItem.id))
                        {
                            itemIndex = i;
                            break;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(statusItem.created_at.Ticks + " " + itemIndex + " " + statusItem.text);
                    //foreach (ListViewItem item2 in timeLineList.Items)
                    //{
                    //    TwitterAccess.StatusInfo sts = (TwitterAccess.StatusInfo)item2.Tag;
                    //    if (sts.created_at > statusItem.created_at)
                    //    {
                    //        newidx = item2.Index - 1;
                    //        if (newidx < 0)
                    //        {
                    //            newidx = 0;
                    //        }
                    //        break;
                    //    }
                    //}

                    //ListViewItem item = timeLineList.Items.Insert(itemIndex++, "");
                    ListViewItem item = timeLineList.Items.Insert(itemIndex, "");
                    item.Tag = statusItem;
                    item.SubItems.Add(statusItem.name);
                    item.SubItems.Add(statusItem.text);
                    item.SubItems.Add(statusItem.created_at.ToString("yyyy/MM/dd HH:mm:ss"));
                    if (statusItem.user_id == twitterObj.CurrentUser.id)
                    {
                        item.ForeColor = Color.DarkBlue;
                    }
                    else if (statusItem.retweeted_status != null)
                    {
                        item.ForeColor = Color.DarkSlateGray;
                        item.SubItems[1].Text = statusItem.retweeted_user.name;
                        item.SubItems[2].Text = statusItem.retweeted_status.text;
                    }
                    else if (statusItem.in_reply_to_user_id == twitterObj.CurrentUser.id)
                    {
                        item.BackColor = Color.Violet;
                    }

                    if (!string.IsNullOrEmpty(keyword) && statusItem.text.IndexOf(keyword) >= 0)
                    {
                        item.BackColor = Color.Orange;
                    }

                    Image image = twitterObj.LoadIcon(statusItem.profile_image_url);
                    if (!item.ImageList.Images.ContainsKey(statusItem.profile_image_url))
                    {
                        item.ImageList.Images.Add(statusItem.profile_image_url, image);
                    }
                    item.ImageKey = statusItem.profile_image_url;
                }

                int max_items = (int)Win32Api.GetPrivateProfileInt("TIMELINE", "max_items", 30000, INI_PATH);
                if (timeLineList.Items.Count > max_items)
                {
                    int max_items2 = max_items - max_items / 10;
                    while (timeLineList.Items.Count > max_items2)
                    {
                        TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)timeLineList.Items[timeLineList.Items.Count - 1].Tag;
                        tweetCache.RemoveTweet(statusItem);
                        timeLineList.Items.RemoveAt(timeLineList.Items.Count - 1);
                    }
                }

                //try
                //{
                //    listItem = twitter.GetStatusesMentions(-1);
                //}
                //catch (WebException we)
                //{
                //    AppUtil.ShowError("リプライの取得に失敗しました。", we);
                //    return;
                //}

                //listItem2 = new List<TwitterAccess.StatusInfo>();
                //foreach (var statusItem in listItem)
                //{
                //    if (StatusHash2.ContainsKey(statusItem.id))
                //    {
                //        continue;
                //    }
                //    StatusHash2.Add(statusItem.id, true);
                //    listItem2.Add(statusItem);
                //}
                //listItem = listItem2;

                //itemIndex = 0;
                //foreach (var statusItem in listItem)
                //{
                //    if (StatusHash.ContainsKey(statusItem.id))
                //    {
                //        continue;
                //    }

                //    OnUpdateListItem(statusItem);

                //    ListViewItem item = timeLineList.Items.Insert(itemIndex++, "");
                //    item.Tag = statusItem;
                //    item.SubItems.Add(statusItem.name);
                //    item.SubItems.Add(statusItem.text);
                //    item.SubItems.Add(statusItem.created_at.ToString("yyyy/MM/dd HH:mm:ss"));

                //    //if (statusItem.retweeted_status && !StatusHash.ContainsKey(statusItem.retweeted_status_id))
                //    //{
                //    //    StatusHash.Add(statusItem.retweeted_status_id, true);
                //    //}
                //    trayIcon.ShowBalloonTip(3000, Application.ProductName, statusItem.name + "\r\n" + statusItem.text, ToolTipIcon.Info);
                //}




                //                try
                //                {
                //                    listItem = twitter.GetDirectMessages(-1);
                //                }
                //                catch (WebException we)
                //                {
                //                    AppUtil.ErrorLog("DM取得失敗", we);
                //                    return;
                //                }

                //                itemIndex = 0;
                //                foreach (var statusItem in listItem)
                //                {
                //                    if (StatusHash.ContainsKey(statusItem.id))
                //                    {
                //                        continue;
                //                    }

                //                    ListViewItem item = MessageList3.Items.Insert(itemIndex++, "");
                //                    item.Tag = statusItem;
                //                    item.SubItems.Add(statusItem.name);
                //                    item.SubItems.Add(statusItem.text);
                //                    item.SubItems.Add(statusItem.created_at.ToString("yyyy/MM/dd HH:mm:ss"));
                //                    StatusHash.Add(statusItem.id, true);
                //                    if (statusItem.retweeted_status && !StatusHash.ContainsKey(statusItem.retweeted_status_id))
                //                    {
                //                        StatusHash.Add(statusItem.retweeted_status_id, true);
                //                    }
                //                    notifyIcon1.ShowBalloonTip(3000, Application.ProductName, statusItem.name + "\r\n" + statusItem.text, ToolTipIcon.Info);
                //                }

                //                try
                //                {
                //                    listItem = twitter.GetDirectMessagesSent(-1);
                //                }
                //                catch (WebException we)
                //                {
                //                    AppUtil.ErrorLog("送信済みDM取得失敗", we);
                //                    return;
                //                }

                //                itemIndex = 0;
                //                foreach (var statusItem in listItem)
                //                {
                //                    if (StatusHash.ContainsKey(statusItem.id))
                //                    {
                //                        continue;
                //                    }

                //                    ListViewItem item = MessageList3.Items.Insert(itemIndex++, "");
                //                    item.Tag = statusItem;
                //                    item.SubItems.Add(statusItem.name);
                //                    item.SubItems.Add(statusItem.text);
                //                    item.SubItems.Add(statusItem.created_at.ToString("yyyy/MM/dd HH:mm:ss"));
                //                    StatusHash.Add(statusItem.id, true);
                //                    if (statusItem.retweeted_status && !StatusHash.ContainsKey(statusItem.retweeted_status_id))
                //                    {
                //                        StatusHash.Add(statusItem.retweeted_status_id, true);
                //                    }
                //                }

            }
            finally
            {
                TweetButton.Enabled = true;
            }
        }

        protected void OnUpdateListItem(TwitterAccess.StatusInfo statusItem)
        {
            //bool retweetDuplicate = false;
            //if (statusItem.retweeted_status && StatusHash.ContainsKey(statusItem.retweeted_status_id))
            //{
            //    retweetDuplicate = true;
            //}

            string keyword = Win32Api.GetPrivateProfileString("FILTER", "keyword", null, INI_PATH);
            //string send_to = Win32Api.GetPrivateProfileString("FILTER", "send_to", null, IniPath);
            if (!String.IsNullOrEmpty(keyword))
            {
                if (statusItem.text.IndexOf(keyword) >= 0)
                {
                    trayIcon.ShowBalloonTip(30, Application.ProductName, statusItem.name + "(@" + statusItem.screen_name + ")\n\n" + statusItem.text, ToolTipIcon.Warning);

                    var warnFile = Path.Combine(Application.StartupPath, "warn.wav");
                    if (File.Exists(warnFile))
                    {
                        var player = new SoundPlayer(warnFile);
                        player.Load();
                        player.Play();
                    }
                    /*
                    try
                    {
                        //string statusUrl = "https://mobile.twitter.com/" + statusItem.screen_name + "/status/" + statusItem.id.ToString();
                        string statusUrl = "http://twtr.jp/user/" + statusItem.screen_name + "/status/" + statusItem.id.ToString();
                        string message2 = "RT @" + statusItem.screen_name + " ";
                        int addLen = message2.Length + statusUrl.Length + 3;
                        string subMessage = statusItem.text;
                        if (addLen + statusItem.text.Length > 140)
                        {
                            subMessage = statusItem.text.Substring(0, 140 - addLen - 3) + "...";
                        }
                        message2 += subMessage + " ⇒ " + statusUrl + "";
                        if (message2.Length > 140)
                        {
                            System.Diagnostics.Debug.WriteLine("Send DM fail:" + message2);
                        }
                        System.Diagnostics.Debug.WriteLine("Send DM:" + message2);
                        string[] sendList = send_to.Split(',');
                        foreach (var item in sendList)
                        {
                            twitter.PostDirectMessage("@" + item, message2);
                            //twitter.PostDirectMessage(item, message2);
                        }
                    }
                    catch (WebException we)
                    {
                        System.Diagnostics.Debug.WriteLine(we.ToString());
                        AppUtil.ErrorLog("filter error", we);
    //                    AppUtil.ShowError("DMの送信に失敗しました。\n" + we.Message, we);
                    }
                     */
                }
            }
        }

        protected void OnTimer(object sender, EventArgs e)
        {
            if (twitterObj.IsAuth)
            {
                BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (twitterObj.IsAuth)
            {
                BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            }
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void timeLineList_SelectedIndexChanged(object sender, EventArgs e)
        {
            replyMode = false;
            TweetButton.Text = "ツイート";

            ListView senderList = (ListView)sender;
            if (senderList.SelectedIndices.Count != 1)
            {
                messageText.Text = "";
                userText.Text = "";
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;
            messageText.Text = statusItem.text;
            userText.Text = statusItem.name + " (@" + statusItem.screen_name + ")";
            if (statusItem.retweeted_status != null)
            {
                userText.Text = statusItem.retweeted_user.name + " (@" + statusItem.retweeted_user.screen_name + ")" + " [RT " + statusItem.name + " (@" + statusItem.screen_name + ")]";
                messageText.Text = statusItem.retweeted_status.text;
            }

            //TwitterAccess.UserInfo userInfo = null;
            //try
            //{
            //    userInfo = twitter.GetUsersShow(statusItem.user_id);
            //}
            //catch (WebException)
            //{
            //}

            //if (statusItem.profile_image_url != null)
            //{
            //    if (IconCach.ContainsKey(statusItem.profile_image_url))
            //    {
            //        IconPicture.Image = IconCach[statusItem.profile_image_url];
            //    }
            //    else
            //    {
            //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(statusItem.profile_image_url);

            //        HttpWebResponse response = null;
            //        try
            //        {
            //            using (response = (HttpWebResponse)request.GetResponse())
            //            {
            //                byte[] buf = new byte[response.ContentLength];
            //                response.GetResponseStream().Read(buf, 0, buf.Length);
            //                MemoryStream mem = new MemoryStream(buf);
            //                Image image = Image.FromStream(mem);
            //                IconPicture.Image = image;
            //                IconCach.Add(statusItem.profile_image_url, image);
            //            }
            //        }
            //        catch (WebException we)
            //        {
            //            HttpWebResponse res = (HttpWebResponse)we.Response;
            //            string result = new StreamReader(res.GetResponseStream()).ReadToEnd();
            //            //throw we;
            //        }
            //        catch (ArgumentException)
            //        {
            //        }
            //    }
            //}
            //else
            //{
            //    IconPicture.Image = null;
            //}

        }

        private void reload2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (twitterObj.IsAuth)
            {
                BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            Close();
        }

        private void trayQuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            Close();
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Hide();
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void TweetButton_Click(object sender, EventArgs e)
        {
            if (!twitterObj.IsAuth)
            {
                return;
            }

            string message = newTweetText.Text;
            if (message.Length <= 0)
            {
                return;
            }

            try
            {
                if (replyMode)
                {
                    twitterObj.PostStatusesUpdate(message, replyTargetId);
                }
                else
                {
                    twitterObj.PostStatusesUpdate(message);
                }
            }
            catch (WebException we)
            {
                AppUtil.ShowError("送信に失敗しました。", we);
                return;
            }
            newTweetText.Text = "";
            if (replyMode)
            {
                replyMode = false;
                replyTargetId = 0;
                TweetButton.Text = "ツイート";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && Visible == true)
            {
                e.Cancel = true;
                Hide();
                return;
            }
            refreshTimer.Stop();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Visible)
            {
                Visible = false;
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Login(null, null))
            {
                return;
            }
            BeginInvoke(new EventHandler(delegate { UpdateList(); }));
        }

        private void userText_DoubleClick(object sender, EventArgs e)
        {
            ListView senderList = timeLineList;
            if (senderList.SelectedIndices.Count != 1)
            {
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;

            string url = "https://twitter.com/" + statusItem.screen_name + "/";
            Process.Start(url);
        }

        private void retweetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!twitterObj.IsAuth)
            {
                return;
            }

            ListView senderList = timeLineList;
            if (senderList.SelectedIndices.Count != 1)
            {
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;

            try
            {
                twitterObj.PostStatusesRetweet(statusItem.id);
            }
            catch (WebException we)
            {
                AppUtil.ShowError("リツイートに失敗しました。", we);
                return;
            }
        }

        private void replyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!twitterObj.IsAuth)
            {
                return;
            }

            ListView senderList = timeLineList;
            if (senderList.SelectedIndices.Count != 1)
            {
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;

            newTweetText.Text = "@" + statusItem.screen_name + " " + newTweetText.Text;
            replyMode = true;
            replyTargetId = statusItem.id;
            TweetButton.Text = "リプライ";
        }

        private void urlOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView senderList = timeLineList;
            if (senderList.SelectedIndices.Count != 1)
            {
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;
            string text = statusItem.text;
            Match match = new Regex("h?ttps?:\\/\\/[^ 　]+|$").Match(text);
            if (match.Success)
            {
                string url = text.Substring(match.Index, match.Length);
                Process.Start(url);
            }

        }

        private void openStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView senderList = timeLineList;
            if (senderList.SelectedIndices.Count != 1)
            {
                return;
            }

            int selectedIndex = senderList.SelectedIndices[0];
            TwitterAccess.StatusInfo statusItem = (TwitterAccess.StatusInfo)senderList.Items[selectedIndex].Tag;

            string url = "https://twitter.com/" + statusItem.screen_name + "/status/" + statusItem.id;
            Process.Start(url);
        }

        private void reloadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reloadForce = true;
            if (twitterObj.IsAuth)
            {
                BeginInvoke(new EventHandler(delegate { UpdateList(); }));
            }
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;
        }

    }
}
