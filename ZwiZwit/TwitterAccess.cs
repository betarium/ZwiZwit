using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;

namespace ZwiZwit
{
    public class TwitterAccess
    {
        public const string URL_API_SERVER = "https://api.twitter.com";

        protected const string METHOD_GET = "GET";
        protected const string METHOD_POST = "POST";

        protected const string URL_REQUEST_TOKEN = "/oauth/request_token";
        protected const string URL_AUTHORIZE = "/oauth/authorize";
        protected const string URL_ACCESS_TOKEN = "/oauth/access_token";

        protected const string GET_ACCOUNT_VERIFY_CREDENTIALS_URL = "/1.1/account/verify_credentials.json";
        protected const string STATUSES_UPDATE_URL = "/1.1/statuses/update.json";

        public class StatusInfo
        {
            public string screen_name { get; set; }
            public string text { get; set; }
            public DateTime created_at { get; set; }
            public long id { get; set; }
            public long in_reply_to_status_id { get; set; }
            public long user_id { get; set; }
            public string name { get; set; }
            public string profile_image_url { get; set; }
            public string url { get; set; }
            public Dictionary<string, string> attributes;
            public bool retweeted;
            public StatusInfo retweeted_status;
            public long retweeted_status_id;
            public UserInfo retweeted_user;
            public long in_reply_to_user_id { get; set; }
            public string source { get; set; }
        }

        public class UserInfo
        {
            public long id { get; set; }
            public string name { get; set; }
            public string screen_name { get; set; }
            public string url { get; set; }
        }

        public class TwitterException : WebException
        {
            public TwitterException(string message)
                : base(message)
            {
            }
            public TwitterException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public string OauthToken { get; set; }
        public string OauthTokenSecret { get; set; }

#if DEBUG
        public string DebugNonce { get; set; }
        public string DebugTimestamp { get; set; }
#endif

        public bool IsAuth
        {
            get
            {
                return !String.IsNullOrEmpty(OauthToken) && !String.IsNullOrEmpty(OauthTokenSecret);
            }
        }

        public UserInfo CurrentUser { get; protected set; }

        protected Dictionary<string, string> MakeParams()
        {
            string nonce = (new Random().Next(999999999) + 1000000000).ToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("oauth_consumer_key", ConsumerKey);
            requestParams.Add("oauth_nonce", nonce);
            requestParams.Add("oauth_signature_method", "HMAC-SHA1");
            requestParams.Add("oauth_timestamp", timestamp);
            requestParams.Add("oauth_version", "1.0");

            if (OauthToken != null)
            {
                requestParams.Add("oauth_token", OauthToken);
            }

#if DEBUG
            if (DebugNonce != null)
            {
                requestParams["oauth_nonce"] = DebugNonce;
            }
            if (DebugNonce != null)
            {
                requestParams["oauth_timestamp"] = DebugTimestamp;
            }
#endif
            return requestParams;
        }

        protected static string[] GetKeysSorted(ICollection<string> keys)
        {
            string[] bufs = new string[keys.Count];
            int index = 0;
            foreach (var item in keys)
            {
                bufs[index++] = item;
            }
            Array.Sort(bufs);
            return bufs;
        }

        protected static string UrlEncodeUpper(string value)
        {
            StringBuilder buf = new StringBuilder();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);

            foreach (var b in bytes)
            {
                if ((b >= 'a' && b <= 'z') || (b >= 'A' && b <= 'Z') || (b >= '0' && b <= '9'))
                {
                    buf.Append((char)b);
                }
                else if ((b == '-') || (b == '.') || (b == '_') || (b == '~'))
                {
                    buf.Append((char)b);
                }
                else
                {
                    buf.Append(String.Format("%{0,2:X}", (uint)b));
                }
            }
            return buf.ToString();
        }

        protected delegate void SendRequestParse(HttpWebResponse response, Dictionary<string, object> results);

        protected void SendRequest(string method, string url, Dictionary<string, string> requestParams, SendRequestParse parseRequest, Dictionary<string, object> results)
        {
            Dictionary<string, string> defaultRequestParams = MakeParams();

            if (requestParams == null)
            {
                requestParams = new Dictionary<string, string>();
            }

            foreach (var key in defaultRequestParams.Keys)
            {
                if (!requestParams.ContainsKey(key))
                {
                    requestParams.Add(key, defaultRequestParams[key]);
                }
            }
            string[] paramKeys = GetKeysSorted(requestParams.Keys);

            List<string> requestParamList = new List<string>();
            foreach (var key in paramKeys)
            {
                requestParamList.Add(key + "=" + UrlEncodeUpper(requestParams[key]));
            }

            string sigparam = String.Join("&", requestParamList.ToArray());
            string sigurl = method + "&" + UrlEncodeUpper(url) + "&" + UrlEncodeUpper(sigparam);

            string authSecret = "";
            if (requestParams.ContainsKey("oauth_token") && requestParams["oauth_token"] != "")
            {
                authSecret = OauthTokenSecret;
            }
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(ConsumerSecret + "&" + authSecret);
            byte[] hash = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(sigurl));
            string sighash = Convert.ToBase64String(hash);

            requestParams.Add("oauth_signature", sighash);

            paramKeys = GetKeysSorted(requestParams.Keys);

            requestParamList.Clear();
            List<string> tmplist = new List<string>();
            List<string> tmplist2 = new List<string>();
            foreach (var key in paramKeys)
            {
                requestParamList.Add(key + "=" + UrlEncodeUpper(requestParams[key]));
                if (key.StartsWith("oauth_"))
                {
                    tmplist.Add(key + "=\"" + UrlEncodeUpper(requestParams[key]) + "\"");
                }
                else
                {
                    tmplist2.Add(key + "=" + UrlEncodeUpper(requestParams[key]) + "");
                }
            }

            string resultUrl = url;
            if (method != METHOD_POST)
            {
                if (tmplist2.Count > 0)
                {
                    resultUrl += '?' + String.Join("&", tmplist2.ToArray());
                }
            }

            string resultAuth = "OAuth " + String.Join(", ", tmplist.ToArray());

            string post = null;
            if (method == METHOD_POST)
            {
                post = "";
                foreach (var key in paramKeys)
                {
                    if (!key.StartsWith("oauth_"))
                    {
                        if (post.Length > 0)
                        {
                            post += "&";
                        }
                        post += key + "=" + UrlEncodeUpper(requestParams[key]);
                    }
                }
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("URL: " + resultUrl);
            System.Diagnostics.Debug.WriteLine("AUTH: " + resultAuth);
#endif

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(resultUrl);

            request.Headers.Add(HttpRequestHeader.Authorization, resultAuth);
            request.Method = method;

            if (method == METHOD_POST)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                using (var s = new StreamWriter(request.GetRequestStream()))
                {
                    s.Write(post);
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                try
                {
                    if (parseRequest != null)
                    {
                        parseRequest(response, results);
                    }
                }
                finally
                {
                    response.Close();
                }
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    using (StreamReader reader = new StreamReader(we.Response.GetResponseStream()))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }
                            System.Diagnostics.Debug.WriteLine("error response:" + line);
                        }
                    }
                }
                throw;
            }
            catch (JsonParser.JsonException je)
            {
                System.Diagnostics.Debug.WriteLine("Json error");
                System.Diagnostics.Debug.WriteLine(je);
                throw;
            }
        }

        protected void PostOauthRequestTokenParse(HttpWebResponse res, Dictionary<string, object> results)
        {
            string[] tokens = new StreamReader(res.GetResponseStream()).ReadToEnd().Split('&');

            string oauth_token = tokens[0].Replace("oauth_token=", "");

            results.Add("request_oauth_token", oauth_token);
        }

        public void PostOauthRequestToken(out string auth_url, out string request_oauth_token)
        {
            if (string.IsNullOrEmpty(ConsumerKey))
            {
                throw new ArgumentNullException("ConsumerKey");
            }
            if (string.IsNullOrEmpty(ConsumerSecret))
            {
                throw new ArgumentNullException("ConsumerSecret");
            }

            const string METHOD = METHOD_POST;
            const string URL = URL_API_SERVER + URL_REQUEST_TOKEN;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("oauth_token", "");
            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, PostOauthRequestTokenParse, results);

                request_oauth_token = (string)results["request_oauth_token"];
                auth_url = URL_API_SERVER + URL_AUTHORIZE + "?oauth_token=" + request_oauth_token;
            }
            catch (WebException we)
            {
                throw new TwitterException("認証トークンの取得に失敗しました。", we);
            }
        }

        //public string ErrorMessage { get; set; }

        protected void PostOauthAccessTokenParse(HttpWebResponse res, Dictionary<string, object> results)
        {
            string result = new StreamReader(res.GetResponseStream()).ReadToEnd();
            string[] tokens = result.Split('&');

            string oauth_token = tokens[0].Split('=')[1];
            string oauth_token_secret = tokens[1].Split('=')[1];

            results["oauth_token"] = oauth_token;
            results["oauth_token_secret"] = oauth_token_secret;
        }

        public bool PostOauthAccessToken(string request_oauth_token, string pin)
        {
            const string METHOD = METHOD_POST;
            const string URL = URL_API_SERVER + URL_ACCESS_TOKEN;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("oauth_verifier", pin);
            requestParams.Add("oauth_token", request_oauth_token);

            Dictionary<string, object> results = new Dictionary<string, object>();
            results["oauth_token"] = null;
            results["oauth_token_secret"] = null;

            try
            {
                SendRequest(METHOD, URL, requestParams, PostOauthAccessTokenParse, results);

                string oauth_token = (string)results["oauth_token"];
                string oauth_token_secret = (string)results["oauth_token_secret"];
                if (oauth_token == null || oauth_token_secret == null)
                {
                    return false;
                }

                OauthToken = oauth_token;
                OauthTokenSecret = oauth_token_secret;

                return true;
            }
            catch (WebException we)
            {
                throw new TwitterException("ログイン時にエラーが発生しました。", we);
            }
        }

        protected static DateTime ParseDate(string created_at)
        {
            created_at = created_at.Substring(4);
            string month = created_at.Substring(0, 3);
            if (month == "Jan")
            {
                created_at = created_at.Replace("Jan", "1");
            }
            else if (month == "Feb")
            {
                created_at = created_at.Replace("Feb", "2");
            }
            else if (month == "Mar")
            {
                created_at = created_at.Replace("Mar", "3");
            }
            else if (month == "Apr")
            {
                created_at = created_at.Replace("Apr", "4");
            }
            else if (month == "May")
            {
                created_at = created_at.Replace("May", "5");
            }
            else if (month == "Jun")
            {
                created_at = created_at.Replace("Jun", "6");
            }
            else if (month == "Jul")
            {
                created_at = created_at.Replace("Jul", "7");
            }
            else if (month == "Aug")
            {
                created_at = created_at.Replace("Aug", "8");
            }
            else if (month == "Sep")
            {
                created_at = created_at.Replace("Sep", "9");
            }
            else if (month == "Oct")
            {
                created_at = created_at.Replace("Oct", "10");
            }
            else if (month == "Nov")
            {
                created_at = created_at.Replace("Nov", "11");
            }
            else if (month == "Dec")
            {
                created_at = created_at.Replace("Dec", "12");
            }
            try
            {
                DateTime createdDate = DateTime.ParseExact(created_at, "M dd HH:mm:ss K yyyy", null);
                return createdDate;
            }
            catch (FormatException fe)
            {
                System.Diagnostics.Debug.Assert(false);
                System.Diagnostics.Trace.WriteLine(fe);
                return DateTime.MinValue;
            }
        }

        protected void GetStatusesHomeTimelineParse(HttpWebResponse response, Dictionary<string, object> results)
        {
            List<StatusInfo> list = new List<StatusInfo>();
            results["list"] = list;

            StringBuilder responseBuffer = new StringBuilder(100000);
            string responseText = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                //char[] buf = new char[100000];
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        return;
                    }
                    //responseText += line;
                    responseBuffer.Append(line);
                    //int len = reader.Read(buf, 0, buf.Length);
                    //responseBuffer.Append(buf, 0, len);
#if DEBUG
                    //System.Diagnostics.Debug.WriteLine("GetStatusesHomeTimelineParse: " + line);
#endif
                }
            }
            responseText = responseBuffer.ToString();
#if DEBUG
            System.Diagnostics.Debug.WriteLine("GetStatusesHomeTimelineParse: " + responseText);
#endif

            var json = JsonParser.Parse(responseText);

            foreach (JsonParser.JsonEntity node in json.Items)
            {
                Dictionary<string, string> attr = new Dictionary<string, string>();
                //System.Diagnostics.Debug.WriteLine("");
                foreach (var node2 in node.Items)
                {
                    attr.Add(node2.Name, node2.Value);
                    //System.Diagnostics.Debug.WriteLine(node2.Name + "="+ node2.InnerText);
                }
                var userNode = node.Eneities["user"];
                //var retweetedStatusNode = node.Eneities["retweeted_status"];
                //if (retweetedStatusNode != null)
                //{
                //    foreach (var node2 in node.Eneities)
                //    {
                //        attr.Add("retweeted_status." + node2.Key, node2.Value.Value);
                //        //System.Diagnostics.Debug.WriteLine("retweeted_status." + node2.Name + "=" + node2.InnerText);
                //    }
                //}

                string user = userNode.Eneities["name"].Value;
                string screen_name = userNode.Eneities["screen_name"].Value;
                string user_id = userNode.Eneities["id"].Value;
                string iconUrl = userNode.Eneities["profile_image_url"].Value;
                string user_url = userNode.Eneities["url"].Value;

                string id = node.Eneities["id"].Value;
                string retweeted = node.Eneities["retweeted"].Value;
                string in_reply_to_status_id = node.Eneities["in_reply_to_status_id"].Value;
                string in_reply_to_user_id = node.Eneities["in_reply_to_user_id"].Value;
                string source = node.Eneities["source"].Value;

                string message = node.Eneities["text"].Value;
                //XmlNode textNode = node.Eneities["text"].Value;
                //if (textNode != null)
                //{
                //    message = textNode.Value;
                //    message = message.Replace("&lt;", "<");
                //    message = message.Replace("&gt;", ">");
                //    message = message.Replace("&quote;", "\"");
                //    message = message.Replace("&amp;", "&");
                //}

                //if (message.IndexOf('&') >= 0)
                //{
                //    message = message.Replace("&lt;", "<");
                //    message = message.Replace("&gt;", ">");
                //    message = message.Replace("&quote;", "\"");
                //    message = message.Replace("&amp;", "&");
                //}
                message = DecodeHtmlEntity(message);

                string created_at = node.Eneities["created_at"].Value;

                DateTime createdDate = ParseDate(created_at);

                StatusInfo item = new StatusInfo();
                item.text = message;
                item.name = user;
                item.screen_name = screen_name;
                item.created_at = createdDate;
                item.id = long.Parse(id);
                item.profile_image_url = iconUrl;
                item.user_id = long.Parse(user_id);
                item.url = user_url;
                item.attributes = attr;
                item.retweeted = (retweeted == "true");
                item.source = source;
                //if (in_reply_to_status_id != "")
                //{
                //    item.in_reply_to_status_id = long.Parse(in_reply_to_status_id);
                //}
                //if (attr.ContainsKey("retweeted_status.id"))
                //{
                //    item.retweeted_status2 = true;
                //    item.retweeted_status_id = long.Parse(attr["retweeted_status.id"]);
                //}

                if (node.Eneities.ContainsKey("retweeted_status"))
                {
                    item.retweeted_status = new StatusInfo();
                    string text2 = node.Eneities["retweeted_status"].Eneities["text"].Value;
                    text2 = DecodeHtmlEntity(text2);
                    item.retweeted_status.text = text2;

                    //if (text2.IndexOf('&') >= 0)
                    //{
                    //    text2 = text2.Replace("&lt;", "<");
                    //    text2 = text2.Replace("&gt;", ">");
                    //    text2 = text2.Replace("&quote;", "\"");
                    //    text2 = text2.Replace("&amp;", "&");
                    //}

                    //item.retweeted_status = true;
                    item.retweeted_status_id = long.Parse(node.Eneities["retweeted_status"].Eneities["id"].Value);
                    item.retweeted_user = new UserInfo();
                    item.retweeted_user.name = node.Eneities["retweeted_status"].Eneities["user"].Eneities["name"].Value;
                    item.retweeted_user.screen_name = node.Eneities["retweeted_status"].Eneities["user"].Eneities["screen_name"].Value;

                    string retweetHead = "RT @" + item.retweeted_user.screen_name + ": ";
                    if (item.text.StartsWith(retweetHead))
                    {
                        item.text = item.text.Substring(retweetHead.Length);
                    }
                }
                if (!string.IsNullOrEmpty(in_reply_to_user_id))
                {
                    item.in_reply_to_user_id = long.Parse(in_reply_to_user_id);
                }

                list.Add(item);
            }

            results["list"] = list;

        }

        public List<StatusInfo> GetStatusesHomeTimeline(Dictionary<string, object> parameters)
        {
            const string METHOD = METHOD_GET;
            const string URL = URL_API_SERVER + "/1.1/statuses/home_timeline.json";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            if (parameters != null)
            {
                if (parameters.ContainsKey("count"))
                {
                    requestParams.Add("count", parameters["count"].ToString());
                }
                if (parameters.ContainsKey("since_id"))
                {
                    requestParams.Add("since_id", parameters["since_id"].ToString());
                }
            }
            requestParams.Add("include_entities", "false");

            Dictionary<string, object> results2 = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetStatusesHomeTimelineParse, results2);

                List<StatusInfo> list2 = (List<StatusInfo>)results2["list"];
                return list2;
            }
            catch (WebException we)
            {
                if (we.Response is HttpWebResponse)
                {
                    int statusCode = (int)((HttpWebResponse)we.Response).StatusCode;
                    if (statusCode == 429)
                    {
                        throw new TwitterException("タイムラインの取得に失敗しました。API制限をオーバーしました。", we);
                    }
                }
                throw new TwitterException("タイムラインの取得に失敗しました。", we);
            }
            catch (IOException ioe)
            {
                throw new TwitterException("タイムラインの取得に失敗しました。受信中にエラーが発生しました。", ioe);
            }
            catch (XmlException xe)
            {
                throw new TwitterException("タイムラインの取得に失敗しました。", xe);
            }
            catch (JsonParser.JsonException je)
            {
                System.Diagnostics.Trace.WriteLine("Json error:" + je.SourceText);
                throw new TwitterException("タイムラインの取得に失敗しました。解析できない形式です。", je);
            }

        }

        protected void GetDirectMessagesParse(HttpWebResponse response, Dictionary<string, object> results)
        {
            List<StatusInfo> list = new List<StatusInfo>();
            XmlDocument doc = new XmlDocument();
            doc.Load(response.GetResponseStream());
            XmlNodeList nodes = doc.SelectNodes("//direct-messages/direct_message");

            foreach (XmlNode node in nodes)
            {
                Dictionary<string, string> attr = new Dictionary<string, string>();
                //System.Diagnostics.Debug.WriteLine("");
                foreach (XmlNode node2 in node)
                {
                    attr.Add(node2.Name, node2.InnerText);
                    //System.Diagnostics.Debug.WriteLine(node2.Name + "="+ node2.InnerText);
                }
                //var userNode = node["user"];
                var retweetedStatusNode = node["retweeted_status"];
                if (retweetedStatusNode != null)
                {
                    foreach (XmlNode node2 in node)
                    {
                        attr.Add("retweeted_status." + node2.Name, node2.InnerText);
                        //System.Diagnostics.Debug.WriteLine("retweeted_status." + node2.Name + "=" + node2.InnerText);
                    }
                }

                System.Diagnostics.Debug.WriteLine(node.InnerXml);

                string id = node["id"].InnerText;
                string sender_id = node["sender_id"].InnerText;
                string text = node["text"].InnerText;
                string recipient_id = node["recipient_id"].InnerText;
                string created_at = node["created_at"].InnerText;
                string sender_screen_name = node["sender_screen_name"].InnerText;
                string recipient_screen_name = node["recipient_screen_name"].InnerText;

                var userNode = node["sender"];
                string name = null;
                if (userNode != null)
                {
                    name = userNode["name"].InnerText;
                    //string screen_name = userNode["screen_name"].InnerText;
                    //string user_id = userNode["id"].InnerText;
                    //string iconUrl = userNode["profile_image_url"].InnerText;
                    //string user_url = userNode["url"].InnerText;
                }


                XmlNode textNode = node["text"].FirstChild;
                if (textNode != null)
                {
                    text = textNode.InnerText;
                    text = text.Replace("&lt;", "<");
                    text = text.Replace("&gt;", ">");
                    text = text.Replace("&quote;", "\"");
                    text = text.Replace("&amp;", "&");
                }

                DateTime createdDate = ParseDate(created_at);

                StatusInfo item = new StatusInfo();
                item.text = text;
                item.name = name;
                item.screen_name = sender_screen_name;
                item.created_at = createdDate;
                item.id = long.Parse(id);
                item.user_id = long.Parse(sender_id);

                item.attributes = attr;

                list.Add(item);
            }

            results["list"] = list;
        }

        public List<StatusInfo> GetDirectMessages(int count)
        {
            const string METHOD = METHOD_GET;
            const string URL = "https://api.twitter.com/1/direct_messages.xml";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            //requestParams.Add("include_entities", "true");
            if (count > 0)
            {
                requestParams.Add("count", count.ToString());
            }

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetDirectMessagesParse, results);

                List<StatusInfo> list = (List<StatusInfo>)results["list"];
                return list;
            }
            catch (WebException we)
            {
                throw new TwitterException("DMの取得に失敗しました。", we);
            }
            catch (XmlException xe)
            {
                throw new TwitterException("DMの取得に失敗しました。", xe);
            }
        }

        public List<StatusInfo> GetDirectMessagesSent(int count)
        {
            const string METHOD = METHOD_GET;
            const string URL = "https://api.twitter.com/1/direct_messages/sent.xml";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            //requestParams.Add("include_entities", "true");
            if (count > 0)
            {
                requestParams.Add("count", count.ToString());
            }

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetDirectMessagesParse, results);

                List<StatusInfo> list = (List<StatusInfo>)results["list"];
                return list;
            }
            catch (WebException we)
            {
                throw new TwitterException("送信済みDMの取得に失敗しました。", we);
            }
        }

        public List<StatusInfo> GetStatusesMentions(int count)
        {
            const string METHOD = METHOD_GET;
            const string URL = URL_API_SERVER + "/1.1/statuses/mentions_timeline.json";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            if (count > 0)
            {
                requestParams.Add("count", count.ToString());
            }

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetStatusesHomeTimelineParse, results);

                List<StatusInfo> list = (List<StatusInfo>)results["list"];
                return list;
            }
            catch (WebException we)
            {
                throw new TwitterException("メンションの取得に失敗しました。", we);
            }
            catch (XmlException xe)
            {
                throw new TwitterException("メンションの取得に失敗しました。", xe);
            }
        }

        public void PostStatusesUpdate(string message)
        {
            const string METHOD = METHOD_POST;
            const string URL = URL_API_SERVER + STATUSES_UPDATE_URL;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("status", message);

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, null, null);
            }
            catch (WebException we)
            {
                throw new TwitterException("ツイートに失敗しました。", we);
            }
        }

        public void PostStatusesUpdate(string message, long in_reply_to_status_id)
        {
            const string METHOD = METHOD_POST;
            const string URL = URL_API_SERVER + STATUSES_UPDATE_URL;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("status", message);
            requestParams.Add("in_reply_to_status_id", in_reply_to_status_id.ToString());

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, null, null);
            }
            catch (WebException we)
            {
                throw new TwitterException("ツイートに失敗しました。", we);
            }
        }

        public void PostDirectMessage(string user, string message)
        {
            const string METHOD = METHOD_POST;
            const string URL = "https://api.twitter.com/1/direct_messages/new.xml";
            //const string URL = "https://api.twitter.com/1.1/direct_messages/new.xml";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("text", message);
            requestParams.Add("screen_name", user);

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, null, null);
            }
            catch (WebException we)
            {
                throw new TwitterException("DMの送信に失敗しました。", we);
            }
        }

        public UserInfo GetAccountVerifyCredentials()
        {
            const string METHOD = METHOD_GET;
            const string URL = URL_API_SERVER + GET_ACCOUNT_VERIFY_CREDENTIALS_URL;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetUsersShowParse, results);

                UserInfo info = (UserInfo)results["user"];
                if (info == null)
                {
                    throw new TwitterException("No user data");
                }
                CurrentUser = info;
                return info;
            }
            catch (WebException we)
            {
                throw new TwitterException("User data error", we);
            }

        }

        protected void GetUsersShowParse(HttpWebResponse response, Dictionary<string, object> results)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(response.GetResponseStream());
            //XmlNode node = doc.SelectSingleNode("//user");
            //UserInfo info = new UserInfo();
            //info.id = long.Parse(node["id"].InnerText);
            //info.name = node["name"].InnerText;
            //info.screen_name = node["screen_name"].InnerText;
            //info.url = node["url"].InnerText;
            //results["user"] = info;

            string responseText = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        return;
                    }
                    responseText += line;
                    System.Diagnostics.Debug.WriteLine("GetUsersShowParse: " + line);
                }
            }

            var json = JsonParser.Parse(responseText);

            UserInfo info = new UserInfo();
            //info.id = long.Parse(node["id"].InnerText);
            //info.name = node["name"].InnerText;
            //info.screen_name = node["screen_name"].InnerText;
            //info.url = node["url"].InnerText;
            //results["user"] = info;

            info.id = long.Parse(json.Eneities["id"].Value);
            info.name = json.Eneities["name"].Value;
            info.screen_name = json.Eneities["screen_name"].Value;
            info.url = json.Eneities["url"].Value;
            results["user"] = info;
        }

        public UserInfo GetUsersShow(long user_id)
        {
            const string METHOD = METHOD_GET;
            const string URL = "https://api.twitter.com/1/users/show.xml";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("user_id", user_id.ToString());

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL, requestParams, GetUsersShowParse, results);

                UserInfo info = (UserInfo)results["user"];
                return info;
            }
            catch (WebException we)
            {
                throw new TwitterException("ユーザー情報の取得に失敗しました。", we);
            }
        }

        public void PostStatusesRetweet(long id)
        {
            const string METHOD = METHOD_POST;
            const string URL = URL_API_SERVER + "/1.1/statuses/retweet/:id.json";

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("trim_user", "true");

            Dictionary<string, object> results = new Dictionary<string, object>();

            try
            {
                SendRequest(METHOD, URL.Replace(":id", id.ToString()), requestParams, null, null);
            }
            catch (WebException we)
            {
                throw new TwitterException("リツイートに失敗しました。", we);
            }
        }

        protected string DecodeHtmlEntity(string text)
        {
            if(text == null)
            {
                return text;
            }
            if (text.IndexOf('&') >= 0)
            {
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&quote;", "\"");
                text = text.Replace("&amp;", "&");
            }
            return text;
        }
    
    }
}
