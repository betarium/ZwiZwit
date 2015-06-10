using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace ZwiZwit
{
    public static class AppUtil
    {
        public static string AppTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length == 0)
                {
                    return null;
                }
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title;
            }
        }

        public static string DebugTitle
        {
            get
            {
#if DEBUG
                return " [DEBUG] ";
#else
                return "";
#endif
            }
        }

        public static void ShowError(string message)
        {
            ErrorLog("ShowError: " + message);
            MessageBox.Show(message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowError(string message, Exception ex)
        {
            ErrorLog("ShowError: " + message, ex);
            MessageBox.Show(message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorLog(string message)
        {
            string logPath = Application.ExecutablePath + ".log";
            string message2 = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + message;
            System.Diagnostics.Debug.WriteLine(message2);
            using (StreamWriter writer = File.AppendText(logPath))
            {
                writer.WriteLine(message2);
                writer.WriteLine("");
            }
        }

        public static void ErrorLog(string message, Exception ex)
        {
            if (ex == null)
            {
                ex = new Exception("No exception");
            }
            //System.Diagnostics.Debug.WriteLine(ex);
            ErrorLog(message + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace);
            Exception ex2 = ex.InnerException;
            while (ex2 != null)
            {
                ErrorLog(ex2.ToString());
                ex2 = ex2.InnerException;
            }
        }

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length == 0)
                {
                    return null;
                }
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title;
            }
        }

        public static string IniPath
        {
            get
            {
                return Path.Combine(Application.StartupPath, Application.ProductName + ".ini");
            }
        }

        public static void InitTraceLog()
        {
            string logPath = Application.ExecutablePath + ".trace.log";
            System.Diagnostics.TextWriterTraceListener texttrace =
                new System.Diagnostics.TextWriterTraceListener(logPath);
            System.Diagnostics.Trace.Listeners.Add(texttrace);
            System.Diagnostics.Trace.AutoFlush = true;
        }
    }
}
