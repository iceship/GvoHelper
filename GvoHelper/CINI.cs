using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace GvoHelper
{
    class CINI:IDisposable
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private bool bDisposed = false;
        private string _FilePath = string.Empty;

        public string FilePath
        {
            get
            {
                if (_FilePath == null)
                    return string.Empty;
                else
                    return _FilePath;
            }
            set
            {
                if (_FilePath != value)
                    _FilePath = value;
            }
        }

        /// <summary>
        /// 建構子。
        /// </summary>
        /// <param name="path">檔案路徑。</param>      
        public CINI(string path)
        {
            _FilePath = path;
        }
        /// <summary>
        /// 解構子。
        /// </summary>
        ~CINI()
        {
            Dispose(false);
        }
        /// <summary>
        /// 釋放資源(程式設計師呼叫)。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //要求系統不要呼叫指定物件的完成項。
        }
        /// <summary>
        /// 釋放資源(給系統呼叫的)。
        /// </summary>        
        protected virtual void Dispose(bool IsDisposing)
        {
            if (bDisposed)
            {
                return;
            }

            if (IsDisposing)
            {

            }

            bDisposed = true;
        }

        /// <summary>
        /// 設定 KeyValue 值。
        /// </summary>
        /// <param name="IN_Section">Section。</param>
        /// <param name="IN_Key">Key。</param>
        /// <param name="IN_Value">Value。</param>
        public void setKeyValue(string IN_Section, string IN_Key, string IN_Value)
        {
            WritePrivateProfileString(IN_Section, IN_Key, IN_Value, this._FilePath);
        }
        /// <summary>
        /// 取得 Key 相對的 Value 值。
        /// </summary>
        /// <param name="IN_Section">Section。</param>
        /// <param name="IN_Key">Key。</param>        
        public string getKeyValue(string IN_Section, string IN_Key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(IN_Section, IN_Key, "", temp, 255, this._FilePath);
            return temp.ToString();
        }

        /// <summary>
        /// 取得 Key 相對的 Value 值，若沒有則使用預設值(DefaultValue)。
        /// </summary>
        /// <param name="Section">Section。</param>
        /// <param name="Key">Key。</param>
        /// <param name="DefaultValue">DefaultValue。</param>        
        public string getKeyValue(string Section, string Key, string DefaultValue)
        {
            StringBuilder sbResult = new StringBuilder(255);

            try
            {
                GetPrivateProfileString(Section, Key, "", sbResult, 255, this._FilePath);
                return (sbResult.Length > 0) ? sbResult.ToString() : DefaultValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
