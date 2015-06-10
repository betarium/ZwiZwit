using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ZwiZwit
{
    class Win32Api
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint
          GetPrivateProfileStringA(string lpAppName,
          string lpKeyName, string lpDefault,
          [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpReturnedString, uint nSize,
          string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
          GetPrivateProfileIntA(string lpAppName,
          string lpKeyName,
          int nDefault, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint WritePrivateProfileStringA(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFileName);

        public static string GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpFileName)
        {
            StringBuilder buf = new StringBuilder(1024);
            uint result = GetPrivateProfileStringA(lpAppName, lpKeyName, lpDefault, buf, (uint)buf.Capacity, lpFileName);
            return buf.ToString();
        }

        public static void WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName)
        {
            WritePrivateProfileStringA(lpAppName, lpKeyName, lpString, lpFileName);
        }

        public static uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName)
        {
            uint result = GetPrivateProfileIntA(lpAppName, lpKeyName, nDefault, lpFileName);
            return result;
        }
    }
}
