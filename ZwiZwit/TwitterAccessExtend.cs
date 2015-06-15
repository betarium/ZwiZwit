using Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ZwiZwit
{
    public class TwitterAccessExtend : TwitterAccess
    {

        public ImageList UserIconList { get; protected set; }
        public string IconCacheDir { get; set; }

        public TwitterAccessExtend()
        {
            UserIconList = new ImageList();
        }


        public Image LoadIcon(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            string iconHash = MakeHash(url);
            string iconHashPath = null;
            if (!string.IsNullOrEmpty(IconCacheDir))
            {
                iconHashPath = Path.Combine(IconCacheDir, iconHash);
                if (File.Exists(iconHashPath))
                {
                    try
                    {
                        Image image = Image.FromFile(iconHashPath);
                        return image;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }

            try
            {
                WebRequest req = WebRequest.Create(url);
                using (WebResponse res = req.GetResponse())
                {
                    Stream st = res.GetResponseStream();
                    byte[] buf = new byte[res.ContentLength];
                    int index = 0;
                    while (index < buf.Length)
                    {
                        int read = st.Read(buf, index, buf.Length - index);
                        if (read < 0)
                        {
                            break;
                        }
                        index += read;
                    }
                    if (!string.IsNullOrEmpty(IconCacheDir))
                    {
                        File.WriteAllBytes(iconHashPath, buf);
                    }
                    MemoryStream stream2 = new MemoryStream();
                    stream2.Write(buf, 0, buf.Length);
                    stream2.Seek(0, SeekOrigin.Begin);
                    Image image = Image.FromStream(stream2);
                    return image;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;
        }

        protected static string MakeHash(string key)
        {
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(key);
            sourceBytes = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(sourceBytes);
            System.Text.StringBuilder hashBuf = new System.Text.StringBuilder();
            for (int i = 0; i < sourceBytes.Length; i++)
            {
                hashBuf.AppendFormat("{0:X2}", sourceBytes[i]);
            }
            return hashBuf.ToString();
        }

    }
}
