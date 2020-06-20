using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using 通用架构._2.子界面;
using 通用架构._3.基础函数;
using 通用架构._4.算子库;


namespace 通用架构._2.子界面
{
    public partial class InspectionStandard : Form
    {
        ///<summary>
        ///变量定义
        ///</summary>
        #region  
        XmlRW XmlRW;
        public delegate void SaveParamsDelegate();//直线轮廓显示委托                              
        public event SaveParamsDelegate SaveParamsEvent; //定义直线轮廓显示事件

        public double Standard_ACInMin = 0;
        public double Standard_ACInMax = 0;
        public double Standard_SSInMin = 0;
        public double Standard_SSInMax = 0;
        public double Standard_CSInMin = 0;
        public double Standard_CSInMax = 0;
        public double Standard_ASInMin = 0;
        public double Standard_ASInMax = 0;

        public double Standard_ACOutMin = 0;
        public double Standard_ACOutMax = 0;
        public double Standard_SSOutMin = 0;
        public double Standard_SSOutMax = 0;
        public double Standard_CSOutMin = 0;
        public double Standard_CSOutMax = 0;
        public double Standard_ASOutMin = 0;
        public double Standard_ASOutMax = 0;
        #endregion

        ///<summary>
        ///主函数
        ///</summary>
        public InspectionStandard()
        {
            InitializeComponent();
            Initialization();
        }


        ///<summary>
        ///初始化函数
        ///</summary>
        private void Initialization()
        {
            XmlRW = new XmlRW();

            if (!ReadParams())
                MessageBox.Show("参数读取失败!");   
                
            RefreshParams();
        }


        ///<summary>
        ///参数刷新
        ///</summary>
        private void RefreshParams()
        {
            try
            {
                TextBox_ACInMin.Text = Standard_ACInMin.ToString("#0.00");
                TextBox_ACInMax.Text = Standard_ACInMax.ToString("#0.00");
                TextBox_SSInMin.Text = Standard_SSInMin.ToString("#0.00");
                TextBox_SSInMax.Text = Standard_SSInMax.ToString("#0.00");
                TextBox_CSInMin.Text = Standard_CSInMin.ToString("#0.00");
                TextBox_CSInMax.Text = Standard_CSInMax.ToString("#0.00");
                TextBox_ASInMin.Text = Standard_ASInMin.ToString("#0.00");
                TextBox_ASInMax.Text = Standard_ASInMax.ToString("#0.00");

                TextBox_ACOutMin.Text = Standard_ACOutMin.ToString("#0.00");
                TextBox_ACOutMax.Text = Standard_ACOutMax.ToString("#0.00");
                TextBox_SSOutMin.Text = Standard_SSOutMin.ToString("#0.00");
                TextBox_SSOutMax.Text = Standard_SSOutMax.ToString("#0.00");
                TextBox_CSOutMin.Text = Standard_CSOutMin.ToString("#0.00");
                TextBox_CSOutMax.Text = Standard_CSOutMax.ToString("#0.00");
                TextBox_ASOutMin.Text = Standard_ASOutMin.ToString("#0.00");
                TextBox_ASOutMax.Text = Standard_ASOutMax.ToString("#0.00");

            }
            catch (Exception )
            {
                MessageBox.Show("刷新参数失败！" );
            }
        }


        ///<summary>
        ///读取xml文件参数
        ///</summary>
        private bool ReadParams()
        {
            try
            {
                Standard_ACInMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AC_min"));
                Standard_ACInMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AC_max"));
                Standard_CSInMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_CS_min"));
                Standard_CSInMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_CS_max"));

                Standard_ASInMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AS_min"));
                Standard_ASInMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AS_max"));
                Standard_SSInMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_SS_min"));
                Standard_SSInMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_SS_max"));

                Standard_ACOutMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AC_min"));
                Standard_ACOutMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AC_max"));
                Standard_CSOutMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_CS_min"));
                Standard_CSOutMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_CS_max"));

                Standard_ASOutMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AS_min"));
                Standard_ASOutMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AS_max"));
                Standard_SSOutMin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_SS_min"));
                Standard_SSOutMax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_SS_max"));
            
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        ///<summary>
        ///写入xml文件参数
        ///</summary>
        private bool WriteParams()
        {

            try
            {
                XmlRW.Update("Parameters/Standards/standard_in_AC_min", TextBox_ACInMin.Text);
                XmlRW.Update("Parameters/Standards/standard_in_AC_max", TextBox_ACInMax.Text);
                XmlRW.Update("Parameters/Standards/standard_in_CS_min", TextBox_CSInMin.Text);
                XmlRW.Update("Parameters/Standards/standard_in_CS_max", TextBox_CSInMax.Text);

                XmlRW.Update("Parameters/Standards/standard_in_AS_min", TextBox_ASInMin.Text);
                XmlRW.Update("Parameters/Standards/standard_in_AS_max", TextBox_ASInMax.Text);
                XmlRW.Update("Parameters/Standards/standard_in_SS_min", TextBox_SSInMin.Text);
                XmlRW.Update("Parameters/Standards/standard_in_SS_max", TextBox_SSInMax.Text);

                XmlRW.Update("Parameters/Standards/standard_out_AC_min", TextBox_ACOutMin.Text);
                XmlRW.Update("Parameters/Standards/standard_out_AC_max", TextBox_ACOutMax.Text);
                XmlRW.Update("Parameters/Standards/standard_out_CS_min", TextBox_CSOutMin.Text);
                XmlRW.Update("Parameters/Standards/standard_out_CS_max", TextBox_CSOutMax.Text);

                XmlRW.Update("Parameters/Standards/standard_out_AS_min", TextBox_ASOutMin.Text);
                XmlRW.Update("Parameters/Standards/standard_out_AS_max", TextBox_ASOutMax.Text);
                XmlRW.Update("Parameters/Standards/standard_out_SS_min", TextBox_SSOutMin.Text);
                XmlRW.Update("Parameters/Standards/standard_out_SS_max", TextBox_SSOutMax.Text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SaveParams_Click(object sender, EventArgs e)
        {
            if (!WriteParams())
                MessageBox.Show("参数写入失败!");
            else if(SaveParamsEvent!=null)
                SaveParamsEvent();
        }
    }
}
