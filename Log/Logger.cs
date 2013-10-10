using System;
using System.IO;

namespace Log
{
    public static class Logger
    {
        private static StreamWriter _writer;
        public static void SetPath(String path)
        {
            _writer = new StreamWriter(path);
        }
        //public static Logger()
        //{
        //    //writer = new StreamWriter(@"D:\Log.txt");
        //}

        public static void Write(string logText)
        {
            if (_writer != null)
            {
                _writer.WriteLine(logText);
                _writer.Flush();
            }
        }

        public static void Close()
        {
            if (_writer != null)
            {
                _writer.Close();
            }
        }
        
    }
}
