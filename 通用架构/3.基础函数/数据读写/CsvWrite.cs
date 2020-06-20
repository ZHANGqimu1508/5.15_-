using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace 通用架构._3.基础函数
{
    class CsvWrite
    {
        public static void Write(String[] StrArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            DateTime time = DateTime.Now;
            //string sData = time.ToString("yyyy-MM-dd");
            string path = AppDomain.CurrentDomain.BaseDirectory + @".//CSV/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }


            string fileFullPath = path + time.ToString("yyyy -MM-dd") + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("Time");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AC-Gap");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AD-Gap");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Count");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }
        public static void Write(String[,] StrArray, string PATH, string csvName, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类         
            string path = PATH;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }           
            string fileFullPath = path + csvName + ".csv";
            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
               CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream, Encoding.Default);//中文编码

            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("拍照次数");
                DstTxtString0.Append(",");
                DstTxtString0.Append("正极-基准（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("负极-基准（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("上隔膜-基准（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("下隔膜-基准（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("正极-隔膜（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("负极-隔膜（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("正极-负极（内）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("隔膜-隔膜（内）");
                DstTxtString0.Append(",");


                DstTxtString0.Append("正极-基准（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("负极-基准（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("上隔膜-基准（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("下隔膜-基准（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("正极-隔膜（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("负极-隔膜（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("正极-负极（外）");
                DstTxtString0.Append(",");
                DstTxtString0.Append("隔膜-隔膜（外）");
                DstTxtString0.Append(",");

                DstTxtString0.Append("检测结果");            
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            int row = StrArray.GetLength(0);
            int col = StrArray.GetLength(1);
            //拼接字符串
            for (int i = 0; i < row; i++)
            {
                DstTxtString.Clear();
                for (int j = 0; j < col; j++)
                {
                    DstTxtString.Append(StrArray[i, j]);
                    DstTxtString.Append(",");      
                }
                //写入文件
                CsvTxtWriter.WriteLine(DstTxtString);
            }

            //关闭文件
            CsvTxtWriter.Close();

        }
       /* public static void Write(String[] StrArray, String Path, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            string path = AppDomain.CurrentDomain.BaseDirectory + "CSV\\"+ DateTime.Now.ToString("yyyy-MM-dd")+"\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            string fileFullPath = path+Path + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("NeedleNum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("C-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S1-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S2-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-S_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-S_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-A_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("C-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S1-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S2-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-S_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-S_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-A_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();
        }
        */
        public static void WriteTab(String[] StrArray, String Path, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            string fileFullPath = Path + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("NeedleNum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A/C");
                DstTxtString0.Append(",");
                DstTxtString0.Append("area");             
                DstTxtString0.Append(",");
                DstTxtString0.Append("width1");
                DstTxtString0.Append(",");
                DstTxtString0.Append("width2");
                DstTxtString0.Append(",");
                DstTxtString0.Append("angle_Up");
                DstTxtString0.Append(",");
                DstTxtString0.Append("angle_Right");
                DstTxtString0.Append(",");
                DstTxtString0.Append("angle_Down");
                DstTxtString0.Append(",");
                DstTxtString0.Append("OK/NG");
                DstTxtString0.Append(",");
                DstTxtString0.Append("totalResult1");
                DstTxtString0.Append(",");
                DstTxtString0.Append("totalResult");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();
        }

        public static void WriteOverhang(String[] StrArray, String Path, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            string fileFullPath = Path + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("NeedleNum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C");
                DstTxtString0.Append(",");
                DstTxtString0.Append("OK/NG");
                DstTxtString0.Append(",");
                DstTxtString0.Append("totalResult2");
                DstTxtString0.Append(",");
                DstTxtString0.Append("totalResult");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();
        }

        public static void WriteDouble(String[] StrArray, String Path, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            string path = AppDomain.CurrentDomain.BaseDirectory + "CSV\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            string fileFullPath = path + Path + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("Time");
                DstTxtString0.Append(",");
                DstTxtString0.Append("NeedleNum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("C-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S1-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S2-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-Datum_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-S_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-A_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("C-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S1-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S2-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-Datum_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-S_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-A_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();
        }

        public static void WriteSingle(String[] StrArray, String Path, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            string path = AppDomain.CurrentDomain.BaseDirectory + "CSV\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            string fileFullPath = path + Path + ".csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("Time");
                DstTxtString0.Append(",");
                DstTxtString0.Append("NeedleNum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-Datum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("C-Datum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S1-Datum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S2-Datum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-Datum");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-A");
                DstTxtString0.Append(",");
                DstTxtString0.Append("S-C");
                DstTxtString0.Append(",");
                DstTxtString0.Append("A-C");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-S");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AT9-A");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();
        }


        public static void Write(String CsvFilePath, Byte[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int16[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int32[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int64[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Single[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Double[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static List<String[]> ReadCSV(string filePathName)
        {
            List<String[]> ls = new List<String[]>();
            StreamReader fileReader = new StreamReader(filePathName, Encoding.Default);
            string strLine = "";
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                    //Debug.WriteLine(strLine);
                }
            }
            fileReader.Close();
            return ls;
        }

    }

}
