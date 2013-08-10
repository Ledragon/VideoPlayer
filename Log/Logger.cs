using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public static class Logger
    {
        private static StreamWriter writer = new StreamWriter(@"D:\Log.txt");
        //public static Logger()
        //{
        //    //writer = new StreamWriter(@"D:\Log.txt");
        //}

        public static void Write(string logText)
        {
            if (writer != null)
            {
                writer.WriteLine(logText);
                writer.Flush();
            }
        }

        public static void Close()
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
        
    }
}
