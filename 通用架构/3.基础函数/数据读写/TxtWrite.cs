using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace 通用架构._3.基础函数
{
    public class TxtWrite
    {
        /// <summary>
        /// 写入日志到文本文件
        /// </summary>
        /// <param name="strMessage">日志内容</param>

        public static void WriteTextLog(string strMessage)
        {
            DateTime time = DateTime.Now;
            //string sData = time.ToString("yyyy-MM-dd");
            string path = AppDomain.CurrentDomain.BaseDirectory + @".//LOG/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy -MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();

            str.Append("Time:" + time + ";Message: " + strMessage + "\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }
}
