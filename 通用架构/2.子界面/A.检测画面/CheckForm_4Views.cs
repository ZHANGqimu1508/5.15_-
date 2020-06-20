using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using IIRP.Sockets;
using 通用架构._3.基础函数;
using 通用架构._4.算子库;

namespace 通用架构._2.子界面
{
    public partial class CheckForm_4Views : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckForm_2Views));

        #region 参数定义
        //检测参数定义
        public DetectParam DetectParam1_1 = new DetectParam();
        public DetectParam DetectParam1_2 = new DetectParam();
        public DetectParam DetectParam2_1 = new DetectParam();
        public DetectParam DetectParam2_2 = new DetectParam();

        public LineDetect LineDetect_AIn = new LineDetect();
        public LineDetect LineDetect_CIn = new LineDetect();
        public LineDetect LineDetect_UpSIn = new LineDetect();
        public LineDetect LineDetect_DownSIn = new LineDetect();

        public LineDetect LineDetect_AOut = new LineDetect();
        public LineDetect LineDetect_COut = new LineDetect();
        public LineDetect LineDetect_UpSOut = new LineDetect();
        public LineDetect LineDetect_DownSOut = new LineDetect();


        //测量结果定义
        List<string> ResultStyle = new List<string>();  //每圈的测量结果
        public bool DetectResult = false;
        public bool TotalResult = false;      //最终测量结果
        public int TotalNum = 0;
        public int OKNum = 0;
        public int NGNum = 0;
        public double OKRate = 0;

        int OverHangNG_Num = 0;        //OverHangNG 电芯数量
        bool OverHangNG_Flag = true;   //OverHangNG 检测标志
        int OverHangNG_ACInNum = 0;    //OverHangNG 分类次数统计
        int OverHangNG_SSInNum = 0;
        int OverHangNG_ASInNum = 0;
        int OverHangNG_CSInNum = 0;

        int OverHangNG_ACOutNum = 0;
        int OverHangNG_SSOutNum = 0;
        int OverHangNG_ASOutNum = 0;
        int OverHangNG_CSOutNum = 0;

        int FLAbnormal_Num = 0;      //直线拟合异常            电芯数量
        int FLErrAndAngleNG_Num = 0; //直线拟合失败/角度NG交集 电芯数量

        int FLErr_Num = 0;              //直线拟合失败 电芯数量
        bool FLErr_Flag = true;         //直线拟合失败 检测标志
        int FittingLineErr_TotalNum = 0;//直线拟合失败 总次数

        int FittingLineErr_AInNum = 0;  //直线拟合失败 分类次数统计
        int FittingLineErr_CInNum = 0;
        int FittingLineErr_UpSInNum = 0;
        int FittingLineErr_DownSInNum = 0;

        int FittingLineErr_AOutNum = 0;
        int FittingLineErr_COutNum = 0;
        int FittingLineErr_UpSOutNum = 0;
        int FittingLineErr_DownSOutNum = 0;

        int FLAngleNG_Num = 0;              //直线拟合角度NG 电芯数量
        bool FLAngleNG_Flag = true;         //直线拟合角度NG 检测标志
        int FittingLineAngleNG_TotalNum = 0;//直线拟合角度NG 总次数

        int FittingLineAngleNG_AInNum = 0;  //直线拟合角度NG 分类次数统计
        int FittingLineAngleNG_CInNum = 0;
        int FittingLineAngleNG_UpSInNum = 0;
        int FittingLineAngleNG_DownSInNum = 0;

        int FittingLineAngleNG_AOutNum = 0;
        int FittingLineAngleNG_COutNum = 0;
        int FittingLineAngleNG_UpSOutNum = 0;
        int FittingLineAngleNG_DownSOutNum = 0;
        #endregion;


        #region 功能类定义
        //字体颜色定义
        public HTuple FontColor_ACIn = "green";
        public HTuple FontColor_SSIn = "green";
        public HTuple FontColor_ASIn = "green";
        public HTuple FontColor_CSIn = "green";

        public HTuple FontColor_ACOut = "green";
        public HTuple FontColor_SSOut = "green";
        public HTuple FontColor_ASOut = "green";
        public HTuple FontColor_CSOut = "green";

        //画面缩放
        public HWndCtrl HWndCtrl1_1;
        public HWndCtrl HWndCtrl1_2;
        public HWndCtrl HWndCtrl2_1;
        public HWndCtrl HWndCtrl2_2;

        //XML读写
        XmlRW XmlRW = new XmlRW();
        public InspectionStandard InspectionStandard = new InspectionStandard();

        //保存生产记录为CSV文件
        string[,] CsvArray = new string[80, 20];

        //欧姆龙PLC通讯
        string PLC_IP;
        string PLC_Port;
        OmronCip OmronCip;

        //发送结果给PLC
        /// <summary>
        /// CCD_Data_In[0]内侧AD测量均值
        /// CCD_Data_In[1]内侧CD测量均值
        /// CCD_Data_In[2]内侧UpSD测量均值
        /// CCD_Data_In[3]内侧DownSD测量均值
        /// CCD_Data_In[4]内侧AS测量均值
        /// CCD_Data_In[5]内侧CS测量均值
        /// CCD_Data_In[6]内侧SS测量均值
        /// CCD_Data_In[7]内侧AC测量均值
        /// </summary>
        float[] CCD_Data_In = new float[16];
        /// <summary>
        /// CCD_Data_Out[0]外侧AD测量均值
        /// CCD_Data_Out[1]外侧CD测量均值
        /// CCD_Data_Out[2]外侧UpSD测量均值
        /// CCD_Data_Out[3]外侧DownSD测量均值
        /// CCD_Data_Out[4]外侧AS测量均值
        /// CCD_Data_Out[5]外侧CS测量均值
        /// CCD_Data_Out[6]外侧SS测量均值
        /// CCD_Data_Out[7]外侧AC测量均值
        /// </summary>
        float[] CCD_Data_Out = new float[16];
        /// <summary>
        /// CCD_Result[0] 电芯检测最终结果
        /// CCD_Result[1] 内侧AS检测结果
        /// CCD_Result[2] 内侧CS检测结果
        /// CCD_Result[3] 内侧AC检测结果
        /// CCD_Result[4] 内侧SS检测结果
        /// CCD_Result[5] 外侧AS检测结果
        /// CCD_Result[6] 外侧CS检测结果
        /// CCD_Result[7] 外侧AC检测结果
        /// CCD_Result[8] 外侧SS检测结果
        /// CCD_Result[11] 发送结果成功校验位
        /// </summary>
        bool[] CCD_Result = new bool[16];

        //检测数据文件及路径定义
        string LogName = "";
        string DateName = "";
        string ScreenPath = "";
        string ImagePath = "";
        string CsvPath = "";
        public string CsvName = "";

        //检测任务定义
        Task DetectionProcess;

        public delegate void RefreshInformationForm_Delegate();//保存参数 委托                              
        public event RefreshInformationForm_Delegate RefreshInformationForm_Event; //保存参数 委托事件

        public delegate void RefreshInformationForm_Chart_Event_Delegate();//保存参数 委托                              
        public event RefreshInformationForm_Chart_Event_Delegate RefreshInformationForm_Chart_Event; //保存参数 委托事件

        public delegate void ProductionData_Delegate();//统计数据 委托                              
        public event ProductionData_Delegate ProductionData_Event; //统计数据 委托事件
        #endregion;


        #region 标志定义
        public int DetectionNum = 0;  //检测次数计数
        int SettingTime = 50;  //拍照屏蔽时间

        bool DetectStartFlag = false;    //接收到 “检测开始信号” 标志
        bool DetectStopFlag = true;      //接收到 “检测停止信号” 标志
        bool DetectProcessFlag = false;  //检测运行状态标志

        bool startDetect;//开始检测信号
        bool heartSignal;//心跳信号
        #endregion


        ///<summary>
        ///主函数
        ///</summary>
        public CheckForm_4Views()
        {
            InitializeComponent();
            Initialization();
        }


        ///<summary>
        ///初始化函数
        ///</summary>
        private void Initialization()
        {

            //相机检测结果初始化
            DetectParam1_1.Bool_DetectResult = false;
            DetectParam1_2.Bool_DetectResult = false;
            DetectParam2_1.Bool_DetectResult = false;
            DetectParam2_2.Bool_DetectResult = false;

            //相机图像初始化
            DetectParam1_1.HTuple_RotateAngle = -1.6;
            DetectParam1_1.HTuple_CropRow = 200;
            DetectParam1_1.HTuple_CropColumn = 130;
            DetectParam1_1.HTuple_Height = 800;
            DetectParam1_1.HTuple_Width = 1200;


            DetectParam1_2.HTuple_RotateAngle = -0.5;
            DetectParam1_2.HTuple_CropRow = 200;
            DetectParam1_2.HTuple_CropColumn = 195;
            DetectParam1_2.HTuple_Width = 1200;
            DetectParam1_2.HTuple_Height = 800;

            DetectParam2_1.HTuple_RotateAngle = 1.0;
            DetectParam2_1.HTuple_CropRow = 200;
            DetectParam2_1.HTuple_CropColumn = 210;
            DetectParam2_1.HTuple_Width = 1200;
            DetectParam2_1.HTuple_Height = 800;

            DetectParam2_2.HTuple_RotateAngle = 2.0;
            DetectParam2_2.HTuple_CropRow = 200;
            DetectParam2_2.HTuple_CropColumn = 200;
            DetectParam2_2.HTuple_Width = 1200;
            DetectParam2_2.HTuple_Height = 800;

            //检测结果队列实例化
            DetectParam1_1.List_CD = new List<double>();
            DetectParam1_1.List_UpSD = new List<double>();
            DetectParam1_1.List_AngleCD = new List<double>();
            DetectParam1_1.List_AngleUpSD = new List<double>();
            DetectParam1_1.List_CS = new List<double>();
            DetectParam1_1.List_AC = new List<double>();

            DetectParam1_2.List_AD = new List<double>();
            DetectParam1_2.List_DownSD = new List<double>();
            DetectParam1_2.List_AngleAD = new List<double>();
            DetectParam1_2.List_AngleDownSD = new List<double>();
            DetectParam1_2.List_AS = new List<double>();
            DetectParam1_2.List_SS = new List<double>();

            DetectParam2_1.List_CD = new List<double>();
            DetectParam2_1.List_UpSD = new List<double>();
            DetectParam2_1.List_AngleCD = new List<double>();
            DetectParam2_1.List_AngleUpSD = new List<double>();
            DetectParam2_1.List_CS = new List<double>();
            DetectParam2_1.List_AC = new List<double>();

            DetectParam2_2.List_AD = new List<double>();
            DetectParam2_2.List_DownSD = new List<double>();
            DetectParam2_2.List_AngleAD = new List<double>();
            DetectParam2_2.List_AngleDownSD = new List<double>();
            DetectParam2_2.List_AS = new List<double>();
            DetectParam2_2.List_SS = new List<double>();

            //检测图片队列实例化
            DetectParam1_1.List_Image = new List<HObject>();
            DetectParam1_2.List_Image = new List<HObject>();
            DetectParam2_1.List_Image = new List<HObject>();
            DetectParam2_2.List_Image = new List<HObject>();
            DetectParam1_1.List_ImageDate = new List<double>();
            DetectParam1_2.List_ImageDate = new List<double>();
            DetectParam2_1.List_ImageDate = new List<double>();
            DetectParam2_2.List_ImageDate = new List<double>();

            //Lock实例化
            DetectParam1_1.Locker = new object();
            DetectParam1_2.Locker = new object();
            DetectParam2_1.Locker = new object();
            DetectParam2_2.Locker = new object();

            //基准线位置初始化
            DetectParam1_1.HTuple_DatumC_RowBegin = 0;
            DetectParam1_1.HTuple_DatumC_ColumnBegin = 0;
            DetectParam1_1.HTuple_DatumC_RowEnd = 1000;
            DetectParam1_1.HTuple_DatumC_ColumnEnd = 0;

            DetectParam1_2.HTuple_DatumC_RowBegin = 0;
            DetectParam1_2.HTuple_DatumC_ColumnBegin = 0;
            DetectParam1_2.HTuple_DatumC_RowEnd = 1000;
            DetectParam1_2.HTuple_DatumC_ColumnEnd = 0;

            DetectParam2_1.HTuple_DatumC_RowBegin = 0;
            DetectParam2_1.HTuple_DatumC_ColumnBegin = 0;
            DetectParam2_1.HTuple_DatumC_RowEnd = 1000;
            DetectParam2_1.HTuple_DatumC_ColumnEnd = 0;

            DetectParam2_2.HTuple_DatumC_RowBegin = 0;
            DetectParam2_2.HTuple_DatumC_ColumnBegin = 0;
            DetectParam2_2.HTuple_DatumC_RowEnd = 1000;
            DetectParam2_2.HTuple_DatumC_ColumnEnd = 0;

            //检测截图实例化
            DetectParam1_1.List_StitchImage = new List<HObject>();
            DetectParam1_2.List_StitchImage = new List<HObject>();
            DetectParam2_1.List_StitchImage = new List<HObject>();
            DetectParam2_2.List_StitchImage = new List<HObject>();


            //画面缩放实例化
            HWndCtrl1_1 = new HWndCtrl(HWControl1);
            HWndCtrl1_1.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl1_1.setViewState(HWndCtrl.MODE_VIEW_MOVE);

            HWndCtrl1_2 = new HWndCtrl(HWControl2);
            HWndCtrl1_2.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl1_2.setViewState(HWndCtrl.MODE_VIEW_MOVE);

            HWndCtrl2_1 = new HWndCtrl(HWControl3);
            HWndCtrl2_1.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl2_1.setViewState(HWndCtrl.MODE_VIEW_MOVE);

            HWndCtrl2_2 = new HWndCtrl(HWControl4);
            HWndCtrl2_2.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl2_2.setViewState(HWndCtrl.MODE_VIEW_MOVE);


            this.TLP_Text1.Visible = false;
            this.TLP_Text2.Visible = false;
            this.TLP_Text3.Visible = false;
            this.TLP_Text4.Visible = false;

            this.TLPanel_Frame1.SetColumnSpan(this.TLPanel_BG1, 10);
            this.TLPanel_Frame2.SetColumnSpan(this.TLPanel_BG2, 10);
            this.TLPanel_Frame3.SetColumnSpan(this.TLPanel_BG3, 10);
            this.TLPanel_Frame4.SetColumnSpan(this.TLPanel_BG4, 10);

            //XML参数读取
            if (!ReadParams())
            {
                MessageBox.Show("读取XML参数失败！");
            }
            else
            {
                //PLC实例化
                OmronCip = new OmronCip(PLC_IP);

                //相机实例化
                DetectParam1_1.CameraBasler = new CameraBasler(DetectParam1_1.String_CameraName);
                DetectParam1_2.CameraBasler = new CameraBasler(DetectParam1_2.String_CameraName);
                DetectParam2_1.CameraBasler = new CameraBasler(DetectParam2_1.String_CameraName);
                DetectParam2_2.CameraBasler = new CameraBasler(DetectParam2_2.String_CameraName);

                DetectParam1_1.CameraBasler.eventProcessImage += CameraBasler_OverHangle1_1;
                DetectParam1_2.CameraBasler.eventProcessImage += CameraBasler_OverHangle1_2;
                DetectParam2_1.CameraBasler.eventProcessImage += CameraBasler_OverHangle2_1;
                DetectParam2_2.CameraBasler.eventProcessImage += CameraBasler_OverHangle2_2;

                //开始执行循环任务
                RecycleTaskList();
            }
            //进度条初始化
            Switch_count(0, true);

        }


        ///<summary>
        ///读取检测参数
        ///</summary>
        public bool ReadParams()
        {

            if (!LineDetect_AIn.ReadParams("LineParams_InA"))
            {
                return false;
            }
            if (!LineDetect_CIn.ReadParams("LineParams_InC"))
            {
                return false;
            }
            if (!LineDetect_UpSIn.ReadParams("LineParams_InUpS"))
            {
                return false;
            }
            if (!LineDetect_DownSIn.ReadParams("LineParams_InDownS"))
            {
                return false;
            }

            if (!LineDetect_AOut.ReadParams("LineParams_OutA"))
            {
                return false;
            }
            if (!LineDetect_COut.ReadParams("LineParams_OutC"))
            {
                return false;
            }
            if (!LineDetect_UpSOut.ReadParams("LineParams_OutUpS"))
            {
                return false;
            }
            if (!LineDetect_DownSOut.ReadParams("LineParams_OutDownS"))
            {
                return false;
            }

            try
            {
                //PLC参数
                PLC_IP = XmlRW.Read("Parameters/General/PLC_IP");
                PLC_Port = XmlRW.Read("Parameters/General/PLC_Port");

                //相机参数
                DetectParam1_1.String_CameraName = XmlRW.Read("Parameters/CameraParam1_1/name");
                DetectParam1_2.String_CameraName = XmlRW.Read("Parameters/CameraParam1_2/name");
                DetectParam2_1.String_CameraName = XmlRW.Read("Parameters/CameraParam2_1/name");
                DetectParam2_2.String_CameraName = XmlRW.Read("Parameters/CameraParam2_2/name");

                DetectParam1_1.Double_K = Convert.ToDouble(XmlRW.Read("Parameters/CameraParam1_1/K1_1"));
                DetectParam1_2.Double_K = Convert.ToDouble(XmlRW.Read("Parameters/CameraParam1_2/K1_2"));
                DetectParam2_1.Double_K = Convert.ToDouble(XmlRW.Read("Parameters/CameraParam2_1/K2_1"));
                DetectParam2_2.Double_K = Convert.ToDouble(XmlRW.Read("Parameters/CameraParam2_2/K2_2"));


                //检测标准参数
                DetectParam1_1.Double_ACmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AC_min"));
                DetectParam1_1.Double_ACmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AC_max"));
                DetectParam1_1.Double_CSmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_CS_min"));
                DetectParam1_1.Double_CSmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_CS_max"));

                DetectParam1_2.Double_ASmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AS_min"));
                DetectParam1_2.Double_ASmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_AS_max"));
                DetectParam1_2.Double_SSmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_SS_min"));
                DetectParam1_2.Double_SSmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_in_SS_max"));

                DetectParam2_1.Double_ACmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AC_min"));
                DetectParam2_1.Double_ACmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AC_max"));
                DetectParam2_1.Double_CSmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_CS_min"));
                DetectParam2_1.Double_CSmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_CS_max"));

                DetectParam2_2.Double_ASmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AS_min"));
                DetectParam2_2.Double_ASmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_AS_max"));
                DetectParam2_2.Double_SSmin = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_SS_min"));
                DetectParam2_2.Double_SSmax = Convert.ToDouble(XmlRW.Read("Parameters/Standards/standard_out_SS_max"));
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }


        ///<summary>
        ///循环任务列表
        ///</summary>
        public void RecycleTaskList()
        {
            //欧姆龙PLC连接
            Task Task_PLCConnect = new Task(OmronCipConnect);
            Task_PLCConnect.Start();

            //Camera1_1相机连接
            Task Task_Camera1_1 = new Task(() =>
            {
                CameraConnect(DetectParam1_1);
            });
            Task_Camera1_1.Start();

            //Camera1_2相机连接
            Task Task_Camera1_2 = new Task(() =>
            {
                CameraConnect(DetectParam1_2);
            });
            Task_Camera1_2.Start();

            //Camera2_1相机连接
            Task Task_Camera2_1 = new Task(() =>
            {
                CameraConnect(DetectParam2_1);
            });
            Task_Camera2_1.Start();

            //Camera2_2相机连接
            Task Task_Camera2_2 = new Task(() =>
            {
                CameraConnect(DetectParam2_2);
            });
            Task_Camera2_2.Start();

            //接收PLC检测信号
            Task Task_DetectionSignal = new Task(DetectionSignal);
            Task_DetectionSignal.Start();

            //删除旧文件
            Task Task_DeleteData = new Task(DeleteData);
            Task_DeleteData.Start();
        }


        ///<summary>
        ///循环任务：欧姆龙PLC连接
        ///</summary>
        public void OmronCipConnect()
        {

            while (true)
            {
                try
                {
                    if (!OmronCip.IsConnected)
                    {
                        //PLC连接异常，心跳灯-红
                        statusStrip1.Invoke(new MethodInvoker(delegate
                        {
                            this.PLCLinkLight.Image = global::通用架构.Properties.Resources.红灯;
                        }));

                        OmronCip.Open();

                    }
                    else
                    {
                        //PLC连接成功，心跳灯-绿
                        statusStrip1.Invoke(new MethodInvoker(delegate
                        {
                            this.PLCLinkLight.Image = global::通用架构.Properties.Resources.绿灯;
                        }));

                        OmronCip.Write("CCD_Heart", true);
                        Thread.Sleep(1000);

                        //PLC连接成功，心跳灯-黄
                        statusStrip1.Invoke(new MethodInvoker(delegate
                        {
                            this.PLCLinkLight.Image = global::通用架构.Properties.Resources.黄灯;
                        }));

                        OmronCip.Write("CCD_Heart", false);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
            }

        }


        ///<summary>
        ///循环任务：接收PLC检测信号
        ///</summary>
        public void DetectionSignal()
        {

            while (true)
            {
                try
                {
                    var CCD_Start = OmronCip.Read("D100[0]");

                    //开始检测信号
                    if (!DetectStartFlag && CCD_Start == "True")
                    {
                        DetectStartFlag = true;
                        DetectStopFlag = false;
                        DetectStart();
                    }

                    //结束检测信号
                    if (!DetectStopFlag && CCD_Start == "False")
                    {
                        DetectStartFlag = false;
                        DetectStopFlag = true;
                    }

                    Thread.Sleep(5);
                }
                catch (Exception)
                { }

            }
        }


        ///<summary>
        ///循环任务：相机连接
        ///</summary>
        public void CameraConnect(DetectParam DetectParam)
        {

            try
            {
                while (true)
                {
                    if (!DetectParam.CameraBasler.CameraIsOpened())
                    {
                        if (DetectParam.CameraBasler.OpenCam())
                        {
                            if (DetectParam.CameraBasler.ReadCameraParameter(AppDomain.CurrentDomain.BaseDirectory +
                                "Parameters\\" + DetectParam.String_CameraName + ".pfs"))
                            {
                                DetectParam.CameraBasler.StartGrabbing();
                            }
                            else
                            {
                                //相机参数读取失败，心跳灯-红
                                statusStrip1.Invoke(new MethodInvoker(delegate
                                {
                                    this.CameraLinkLight.Image = global::通用架构.Properties.Resources.红灯;
                                }));
                            }
                        }
                        else
                        {
                            //相机参数读取失败，心跳灯-红
                            statusStrip1.Invoke(new MethodInvoker(delegate
                            {
                                this.CameraLinkLight.Image = global::通用架构.Properties.Resources.红灯;
                            }));
                        }
                    }
                    else if (DetectParam.CameraBasler.CameraIsConnected() && DetectParam.CameraBasler.CameraIsOpened())
                    {
                        //相机参数读取失败，心跳灯-绿
                        statusStrip1.Invoke(new MethodInvoker(delegate
                        {
                            this.CameraLinkLight.Image = global::通用架构.Properties.Resources.绿灯;
                        }));
                    }
                    else
                    {
                        //相机参数读取失败，心跳灯-红
                        statusStrip1.Invoke(new MethodInvoker(delegate
                        {
                            this.CameraLinkLight.Image = global::通用架构.Properties.Resources.红灯;
                        }));
                    }
                    Thread.Sleep(2000);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
            }


        }


        ///<summary>
        ///循环任务：定期删除文件
        ///</summary>
        public void DeleteData()
        { }


        ///<summary>
        ///相机图像委托函数：旋转裁切，存入队列
        ///</summary>
        //private void CameraBasler_OverHangle1_1(HObject ho_Image)
        //{
        //    //如果当前线程未结束，异步开启新的线程
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new CameraBasler.delegateProcessHImage(CameraBasler_OverHangle1_1), ho_Image);
        //        return;
        //    }

        //    //待检测图像加入队列，不检测图像直接显示
        //    if (DetectStartFlag)
        //    {
        //        lock (DetectParam1_1.Locker)
        //        {
        //            DetectParam1_1.List_ImageDate.Add(Convert.ToDouble(DateTime.Now.ToString("mmss.ffff")));
        //            DetectParam1_1.List_Image.Add(ho_Image);
        //        }
        //    }
        //    //else
        //    //Tools.HalconTools.showHalconImage(ho_Image, hv_ImageWindow_1_Out, ref hv_StartX_1_Out, ref hv_StartY_1_Out, ref hv_ZoomFactor_1_Out, ref hv_XldHomMat2D_1_Out, hWindowControl_1_Out);

        //}


        private void CameraBasler_OverHangle1_1(HObject ho_Image)
        {
            BeginInvoke(new CameraBasler.delegateProcessHImage(CameraBasler1_1), ho_Image);
        }


        private void CameraBasler1_1(HObject ho_Image)
        {
            //待检测图像加入队列，不检测图像直接显示
            if (DetectStartFlag)
            {
                lock (DetectParam1_1.Locker)
                {
                    DetectParam1_1.List_ImageDate.Add(Convert.ToDouble(DateTime.Now.ToString("mmss.ffff")));
                    DetectParam1_1.List_Image.Add(ho_Image);
                }
            }
            //else
            //Tools.HalconTools.showHalconImage(ho_Image, hv_ImageWindow_1_Out, ref hv_StartX_1_Out, ref hv_StartY_1_Out, ref hv_ZoomFactor_1_Out, ref hv_XldHomMat2D_1_Out, hWindowControl_1_Out);

        }

        private void CameraBasler_OverHangle1_2(HObject ho_Image)
        {
            //如果当前线程未结束，异步开启新的线程
            if (InvokeRequired)
            {
                BeginInvoke(new CameraBasler.delegateProcessHImage(CameraBasler_OverHangle1_2), ho_Image);
                return;
            }

            //待检测图像加入队列，不检测图像直接显示
            if (DetectStartFlag)
            {
                lock (DetectParam1_2.Locker)
                {
                    DetectParam1_2.List_ImageDate.Add(Convert.ToDouble(DateTime.Now.ToString("mmss.ffff")));
                    DetectParam1_2.List_Image.Add(ho_Image);
                }
            }
            //else
            //    //Tools.HalconTools.showHalconImage(ho_Image, hv_ImageWindow_1_Out, ref hv_StartX_1_Out, ref hv_StartY_1_Out, ref hv_ZoomFactor_1_Out, ref hv_XldHomMat2D_1_Out, hWindowControl_1_Out);
        }

        private void CameraBasler_OverHangle2_1(HObject ho_Image)
        {
            //如果当前线程未结束，异步开启新的线程
            if (InvokeRequired)
            {
                BeginInvoke(new CameraBasler.delegateProcessHImage(CameraBasler_OverHangle2_1), ho_Image);
                return;
            }

            //待检测图像加入队列，不检测图像直接显示
            if (DetectStartFlag)
            {
                lock (DetectParam2_1.Locker)
                {
                    DetectParam2_1.List_ImageDate.Add(Convert.ToDouble(DateTime.Now.ToString("mmss.ffff")));
                    DetectParam2_1.List_Image.Add(ho_Image);
                }
            }
            //else
            //    //Tools.HalconTools.showHalconImage(ho_Image, hv_ImageWindow_1_Out, ref hv_StartX_1_Out, ref hv_StartY_1_Out, ref hv_ZoomFactor_1_Out, ref hv_XldHomMat2D_1_Out, hWindowControl_1_Out);
        }

        private void CameraBasler_OverHangle2_2(HObject ho_Image)
        {
            //如果当前线程未结束，异步开启新的线程
            if (InvokeRequired)
            {
                BeginInvoke(new CameraBasler.delegateProcessHImage(CameraBasler_OverHangle2_2), ho_Image);
                return;
            }

            //待检测图像加入队列，不检测图像直接显示
            if (DetectStartFlag)
            {
                lock (DetectParam2_2.Locker)
                {
                    DetectParam2_2.List_ImageDate.Add(Convert.ToDouble(DateTime.Now.ToString("mmss.ffff")));
                    DetectParam2_2.List_Image.Add(ho_Image);
                }
            }
            //else
            //    //Tools.HalconTools.showHalconImage(ho_Image, hv_ImageWindow_1_Out, ref hv_StartX_1_Out, ref hv_StartY_1_Out, ref hv_ZoomFactor_1_Out, ref hv_XldHomMat2D_1_Out, hWindowControl_1_Out);
        }


        ///<summary>
        ///直线拟合函数（双线程）：线线距离/角度计算，NG分类
        ///</summary>
        private void FittingLine_DoubleTask(DetectParam DetectParam, LineDetect LineDetect1, LineDetect LineDetect2, double Dist_L1L2min, double Dist_L1L2max,
                                            int OverHangNG_L1L2Num, int FittingLineErr_DL1Num, int FittingLineErr_DL2Num, int FittingLineAngleNG_DL1Num, int FittingLineAngleNG_DL2Num,
                                            out double Dist_DL1, out double Dist_DL2, out double Dist_L1L2, out double Angle_DL1, out double Angle_DL2, out HTuple FontColor)
        {
            //从图像队列取图
            lock (DetectParam.Locker)
            {
                if (!checkBox2.Checked)
                {
                    HOperatorSet.RotateImage(DetectParam.List_Image[0], out HObject ho_ImageRotate, DetectParam.HTuple_RotateAngle, "constant");
                    HOperatorSet.GenRectangle1(out HObject ho_Rectangle, DetectParam.HTuple_CropRow, DetectParam.HTuple_CropColumn,
                                               DetectParam.HTuple_Height + DetectParam.HTuple_CropRow, DetectParam.HTuple_Width + DetectParam.HTuple_CropColumn);
                    HOperatorSet.ReduceDomain(ho_ImageRotate, ho_Rectangle, out HObject ho_ImageReduced);
                    HOperatorSet.CropDomain(ho_ImageReduced, out DetectParam.HObject_Image);

                }
                else
                {
                    DetectParam.HObject_Image = DetectParam.List_Image[0];
                }
                if (LineDetect1.hv_ImageProcessing == "SharpenImage")
                {
                    HTuple midR_1 = (LineDetect1.hv_MeasureRow1 + LineDetect1.hv_MeasureRow2) / 2;
                    HTuple midC_1 = (LineDetect1.hv_MeasureColumn1 + LineDetect1.hv_MeasureColumn2) / 2;
                    HOperatorSet.AngleLx(LineDetect1.hv_MeasureRow1, LineDetect1.hv_MeasureColumn1, LineDetect1.hv_MeasureRow2, LineDetect1.hv_MeasureColumn2, out HTuple AngleL);
                    HTuple phi_1 = -Convert.ToDouble(AngleL.ToString());
                    HOperatorSet.DistancePp(LineDetect1.hv_MeasureRow1, LineDetect1.hv_MeasureColumn1, LineDetect1.hv_MeasureRow2, LineDetect1.hv_MeasureColumn2, out HTuple LengthL);
                    HTuple length1_1 = Convert.ToDouble(LengthL.ToString()) / 2;
                    HTuple length2_1 = LineDetect1.hv_MeasureLength1;

                    LineDetect1.ProcessImage(DetectParam.HObject_Image, midR_1, midC_1, -phi_1, length1_1, length2_1, out LineDetect1.ho_Image);
                }
                else
                    LineDetect1.ho_Image = DetectParam.HObject_Image;


                if (LineDetect2.hv_ImageProcessing == "SharpenImage")
                {
                    HTuple midR_2 = (LineDetect2.hv_MeasureRow1 + LineDetect2.hv_MeasureRow2) / 2;
                    HTuple midC_2 = (LineDetect2.hv_MeasureColumn1 + LineDetect2.hv_MeasureColumn2) / 2;
                    HOperatorSet.AngleLx(LineDetect2.hv_MeasureRow1, LineDetect2.hv_MeasureColumn1, LineDetect2.hv_MeasureRow2, LineDetect2.hv_MeasureColumn2, out HTuple AngleL2);
                    HTuple phi_2 = -Convert.ToDouble(AngleL2.ToString());
                    HOperatorSet.DistancePp(LineDetect2.hv_MeasureRow1, LineDetect2.hv_MeasureColumn1, LineDetect2.hv_MeasureRow2, LineDetect2.hv_MeasureColumn2, out HTuple LengthL2);
                    HTuple length1_2 = Convert.ToDouble(LengthL2.ToString()) / 2;
                    HTuple length2_2 = LineDetect2.hv_MeasureLength1;

                    LineDetect2.ProcessImage(DetectParam.HObject_Image, midR_2, midC_2, -phi_2, length1_2, length2_2, out LineDetect2.ho_Image);
                }
                else
                    LineDetect2.ho_Image = DetectParam.HObject_Image;

                DetectParam.List_Image.Remove(DetectParam.List_Image[0]);
            }


            //直线拟合
            Task Task_FittingLine1 = new Task(LineDetect1.DetectLine);
            Task Task_FittingLine2 = new Task(LineDetect2.DetectLine);
            Task_FittingLine1.Start();
            Task_FittingLine2.Start();
            Task.WaitAll(Task_FittingLine1, Task_FittingLine2);


            //线线距离/夹角计算，测量NG值为-1
            if (LineDetect1.DetectLineFlag && LineDetect2.DetectLineFlag)
            {
                Angle_DL1 = MeasureDistance.L2LAngle(LineDetect1.hv_LineRowBegin, LineDetect1.hv_LineColumnBegin, LineDetect1.hv_LineRowEnd, LineDetect1.hv_LineColumnEnd,
                            DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd);
                Angle_DL2 = MeasureDistance.L2LAngle(LineDetect2.hv_LineRowBegin, LineDetect2.hv_LineColumnBegin, LineDetect2.hv_LineRowEnd, LineDetect2.hv_LineColumnEnd,
                            DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd);

                Dist_DL1 = MeasureDistance.L2LDistance(LineDetect1.hv_LineRowBegin, LineDetect1.hv_LineColumnBegin, LineDetect1.hv_LineRowEnd, LineDetect1.hv_LineColumnEnd,
                           DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd) * DetectParam.Double_K;
                Dist_DL2 = MeasureDistance.L2LDistance(LineDetect2.hv_LineRowBegin, LineDetect2.hv_LineColumnBegin, LineDetect2.hv_LineRowEnd, LineDetect2.hv_LineColumnEnd,
                           DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd) * DetectParam.Double_K;
                Dist_L1L2 = Dist_DL1 - Dist_DL2;

                if (Angle_DL1 < DetectParam.Double_Anglemin || Angle_DL1 > DetectParam.Double_Anglemax)
                {
                    FittingLineAngleNG_DL1Num++;
                }

                if (Angle_DL2 < DetectParam.Double_Anglemin || Angle_DL2 > DetectParam.Double_Anglemax)
                {
                    FittingLineAngleNG_DL2Num++;
                }

                if (Dist_L1L2 < Dist_L1L2min || Dist_L1L2 > Dist_L1L2max)
                {
                    OverHangNG_L1L2Num++;
                    FontColor = "red";
                }
                else
                {
                    FontColor = "green";
                    DetectResult = true;
                }
            }
            else if (!LineDetect1.DetectLineFlag && LineDetect2.DetectLineFlag)
            {
                Angle_DL2 = MeasureDistance.L2LAngle(LineDetect2.hv_LineRowBegin, LineDetect2.hv_LineColumnBegin, LineDetect2.hv_LineRowEnd, LineDetect2.hv_LineColumnEnd,
                            DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd);
                Dist_DL2 = MeasureDistance.L2LDistance(LineDetect2.hv_LineRowBegin, LineDetect2.hv_LineColumnBegin, LineDetect2.hv_LineRowEnd, LineDetect2.hv_LineColumnEnd,
                           DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd) * DetectParam.Double_K;
                Dist_DL1 = Angle_DL1 = -1;
                Dist_L1L2 = -10;
                FittingLineErr_DL1Num++;
                FontColor = "red";
            }
            else if (LineDetect1.DetectLineFlag && !LineDetect2.DetectLineFlag)
            {
                Angle_DL1 = MeasureDistance.L2LAngle(LineDetect1.hv_LineRowBegin, LineDetect1.hv_LineColumnBegin, LineDetect1.hv_LineRowEnd, LineDetect1.hv_LineColumnEnd,
                            DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd);
                Dist_DL1 = MeasureDistance.L2LDistance(LineDetect1.hv_LineRowBegin, LineDetect1.hv_LineColumnBegin, LineDetect1.hv_LineRowEnd, LineDetect1.hv_LineColumnEnd,
                           DetectParam.HTuple_DatumC_RowBegin, DetectParam.HTuple_DatumC_ColumnBegin, DetectParam.HTuple_DatumC_RowEnd, DetectParam.HTuple_DatumC_ColumnEnd) * DetectParam.Double_K;
                Dist_DL2 = Angle_DL2 = -1;
                Dist_L1L2 = -10;
                FittingLineErr_DL2Num++;
                FontColor = "red";
            }
            else
            {
                Dist_DL1 = Dist_DL2 = Angle_DL1 = Angle_DL2 = -1;
                Dist_L1L2 = -10;
                FittingLineErr_DL1Num++;
                FittingLineErr_DL2Num++;
                FontColor = "red";
            }
        }



        private void HobjectToHimage(HObject hobject, ref HImage image)
        {
            try
            {
                HTuple pointer, type, width, height;
                HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
                image.GenImage1(type, width, height, pointer);
            }
            catch
            {
            }
        }

        ///<summary>
        ///检测结果窗口显示
        ///</summary>
        private void ShowResultInHWindow()
        {
            HImage HImage1 = new HImage();
            HImage HImage2 = new HImage();
            HImage HImage3 = new HImage();
            HImage HImage4 = new HImage();

            HobjectToHimage(LineDetect_CIn.ho_Image, ref HImage1);
            HobjectToHimage(LineDetect_AIn.ho_Image, ref HImage2);
            HobjectToHimage(LineDetect_COut.ho_Image, ref HImage3);
            HobjectToHimage(LineDetect_AOut.ho_Image, ref HImage4);

            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘

            DetectParam1_1.HObject_Image = HImage1;
            HWndCtrl1_1.addIconicVar(DetectParam1_1.HObject_Image);
            HWndCtrl1_1.repaint();
            MeasureDistance.ShowCoordinate(LineDetect_CIn.ho_Image, HWControl1.HalconID, 10, 50, 50, "orange");
            MeasureDistance.WriteStringInHwindown(HWControl1.HalconID, "检测次数:" + DetectionNum.ToString(), 16, "green", 30, 30);
            MeasureDistance.WriteStringInHwindown(HWControl1.HalconID, "正极-负极:" + DetectParam1_1.Double_AC.ToString("0.####"), 16, FontColor_ACIn, 90, 30);
            MeasureDistance.WriteStringInHwindown(HWControl1.HalconID, "正极-隔膜:" + DetectParam1_1.Double_CS.ToString("0.####"), 16, FontColor_CSIn, 150, 30);
            MeasureDistance.ShowMeasureRectangle(HWControl1.HalconID, 1, LineDetect_CIn, "green");
            MeasureDistance.ShowMeasureRectangle(HWControl1.HalconID, 1, LineDetect_UpSIn, "blue");
            MeasureDistance.ShowLine(HWControl1.HalconID, 1, LineDetect_CIn, "green");
            MeasureDistance.ShowLine(HWControl1.HalconID, 1, LineDetect_UpSIn, "blue");

            DetectParam1_2.HObject_Image = HImage2;
            HWndCtrl1_2.addIconicVar(DetectParam1_2.HObject_Image);
            HWndCtrl1_2.repaint();
            MeasureDistance.ShowCoordinate(LineDetect_AIn.ho_Image, HWControl2.HalconID, 10, 50, 50, "orange");
            MeasureDistance.WriteStringInHwindown(HWControl2.HalconID, "检测次数:" + DetectionNum.ToString(), 16, "green", 30, 30);
            MeasureDistance.WriteStringInHwindown(HWControl2.HalconID, "隔膜错位:" + DetectParam1_2.Double_SS.ToString("0.####"), 16, FontColor_SSIn, 90, 30);
            MeasureDistance.WriteStringInHwindown(HWControl2.HalconID, "负极-隔膜:" + DetectParam1_2.Double_AS.ToString("0.####"), 16, FontColor_ASIn, 150, 30);
            MeasureDistance.ShowMeasureRectangle(HWControl2.HalconID, 1, LineDetect_AIn, "red");
            MeasureDistance.ShowMeasureRectangle(HWControl2.HalconID, 1, LineDetect_DownSIn, "blue");
            MeasureDistance.ShowLine(HWControl2.HalconID, 1, LineDetect_AIn, "red");
            MeasureDistance.ShowLine(HWControl2.HalconID, 1, LineDetect_DownSIn, "blue");

            DetectParam2_1.HObject_Image = HImage3;
            HWndCtrl2_1.addIconicVar(DetectParam2_1.HObject_Image);
            HWndCtrl2_1.repaint();
            MeasureDistance.ShowCoordinate(LineDetect_COut.ho_Image, HWControl3.HalconID, 10, 50, 50, "orange");
            MeasureDistance.WriteStringInHwindown(HWControl3.HalconID, "检测次数:" + DetectionNum.ToString(), 16, "green", 30, 30);
            MeasureDistance.WriteStringInHwindown(HWControl3.HalconID, "正极-负极:" + DetectParam2_1.Double_AC.ToString("0.####"), 16, FontColor_ACOut, 90, 30);
            MeasureDistance.WriteStringInHwindown(HWControl3.HalconID, "正极-隔膜:" + DetectParam2_1.Double_CS.ToString("0.####"), 16, FontColor_CSOut, 150, 30);
            MeasureDistance.ShowMeasureRectangle(HWControl3.HalconID, 1, LineDetect_COut, "green");
            MeasureDistance.ShowMeasureRectangle(HWControl3.HalconID, 1, LineDetect_UpSOut, "blue");
            MeasureDistance.ShowLine(HWControl3.HalconID, 1, LineDetect_COut, "green");
            MeasureDistance.ShowLine(HWControl3.HalconID, 1, LineDetect_UpSOut, "blue");

            DetectParam2_2.HObject_Image = HImage4;
            HWndCtrl2_2.addIconicVar(DetectParam2_2.HObject_Image);
            HWndCtrl2_2.repaint();
            MeasureDistance.ShowCoordinate(LineDetect_AOut.ho_Image, HWControl4.HalconID, 10, 50, 50, "orange");
            MeasureDistance.WriteStringInHwindown(HWControl4.HalconID, "检测次数:" + DetectionNum.ToString(), 16, "green", 30, 30);
            MeasureDistance.WriteStringInHwindown(HWControl4.HalconID, "隔膜错位:" + DetectParam2_2.Double_SS.ToString("0.####"), 16, FontColor_SSOut, 90, 30);
            MeasureDistance.WriteStringInHwindown(HWControl4.HalconID, "负极-隔膜:" + DetectParam2_2.Double_AS.ToString("0.####"), 16, FontColor_ASOut, 150, 30);
            MeasureDistance.ShowMeasureRectangle(HWControl4.HalconID, 1, LineDetect_AOut, "red");
            MeasureDistance.ShowMeasureRectangle(HWControl4.HalconID, 1, LineDetect_DownSOut, "blue");
            MeasureDistance.ShowLine(HWControl4.HalconID, 1, LineDetect_AOut, "red");
            MeasureDistance.ShowLine(HWControl4.HalconID, 1, LineDetect_DownSOut, "blue");

            Input_Detect_Num(48, DetectionNum, DetectResult);

            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();//刷新控件
        }


        ///<summary>
        ///直线检测的所有任务
        ///</summary>
        private void LineDetectAllTask()
        {

            while (DetectProcessFlag)
            {
                int[] ImageListLength = new int[]
                          { DetectParam1_1.List_Image.ToArray().Length, DetectParam1_2.List_Image.ToArray().Length, DetectParam2_1.List_Image.ToArray().Length,  DetectParam2_2.List_Image.ToArray().Length };
                int[] ImageDateListLength = new int[]
                          { DetectParam1_1.List_ImageDate.ToArray().Length, DetectParam1_2.List_ImageDate.ToArray().Length, DetectParam2_1.List_ImageDate.ToArray().Length, DetectParam2_2.List_ImageDate.ToArray().Length };

                DetectResult = false;

                if (ImageListLength.Min() > 0)
                {

                    Task Detection1_1 = new Task(() =>
                    {
                        FittingLine_DoubleTask(DetectParam1_1, LineDetect_CIn, LineDetect_UpSIn, DetectParam1_1.Double_CSmin, DetectParam1_1.Double_CSmax, OverHangNG_CSInNum, FittingLineErr_UpSInNum,
                                               FittingLineErr_CInNum, FittingLineAngleNG_UpSInNum, FittingLineAngleNG_CInNum, out DetectParam1_1.Double_CD, out DetectParam1_1.Double_UpSD,
                                               out DetectParam1_1.Double_CS, out DetectParam1_1.Double_AngleCD, out DetectParam1_1.Double_AngleUpSD, out FontColor_CSIn);
                    });
                    Task Detection1_2 = new Task(() =>
                    {
                        FittingLine_DoubleTask(DetectParam1_2, LineDetect_AIn, LineDetect_DownSIn, DetectParam1_2.Double_ASmin, DetectParam1_2.Double_ASmax, OverHangNG_ASInNum, FittingLineErr_DownSInNum,
                                               FittingLineErr_AInNum, FittingLineAngleNG_DownSInNum, FittingLineAngleNG_AInNum, out DetectParam1_2.Double_AD, out DetectParam1_2.Double_DownSD,
                                               out DetectParam1_2.Double_AS, out DetectParam1_2.Double_AngleAD, out DetectParam1_2.Double_AngleDownSD, out FontColor_ASIn);
                    });
                    Task Detection2_1 = new Task(() =>
                    {
                        FittingLine_DoubleTask(DetectParam2_1, LineDetect_UpSOut, LineDetect_COut, DetectParam2_1.Double_CSmin, DetectParam2_1.Double_CSmax, OverHangNG_CSOutNum, FittingLineErr_UpSOutNum,
                                               FittingLineErr_COutNum, FittingLineAngleNG_UpSOutNum, FittingLineAngleNG_COutNum, out DetectParam2_1.Double_UpSD, out DetectParam2_1.Double_CD,
                                               out DetectParam2_1.Double_CS, out DetectParam2_1.Double_AngleUpSD, out DetectParam2_1.Double_AngleCD, out FontColor_CSOut);
                    });
                    Task Detection2_2 = new Task(() =>
                    {
                        FittingLine_DoubleTask(DetectParam2_2, LineDetect_DownSOut, LineDetect_AOut, DetectParam2_2.Double_ASmin, DetectParam2_2.Double_ASmax, OverHangNG_ASOutNum, FittingLineErr_DownSOutNum,
                                              FittingLineErr_AOutNum, FittingLineAngleNG_DownSOutNum, FittingLineAngleNG_AOutNum, out DetectParam2_2.Double_DownSD, out DetectParam2_2.Double_AD,
                                              out DetectParam2_2.Double_AS, out DetectParam2_2.Double_AngleDownSD, out DetectParam2_2.Double_AngleAD, out FontColor_ASOut);
                    });

                    Detection1_1.Start();
                    Detection1_2.Start();
                    Detection2_1.Start();
                    Detection2_2.Start();

                    Task.WaitAll(Detection1_1, Detection1_2, Detection2_1, Detection2_2);

                    //AC距离/SS距离判定，NG分类
                    if (DetectParam1_1.Double_CD != -1 && DetectParam1_2.Double_AD != -1)
                    {
                        DetectParam1_1.Double_AC = DetectParam1_1.Double_CD - DetectParam1_2.Double_AD;

                        if (DetectParam1_1.Double_AC < DetectParam1_1.Double_ACmin || DetectParam1_1.Double_AC > DetectParam1_1.Double_ACmax)
                        {
                            OverHangNG_ACInNum++;
                            FontColor_ACIn = "red";
                            DetectResult = false;
                        }
                        else
                        {
                            FontColor_ACIn = "green";
                        }
                    }
                    else
                    {
                        DetectParam1_1.Double_AC = -10;
                        FontColor_ACIn = "red";
                        DetectResult = false;
                    }

                    if (DetectParam1_1.Double_UpSD != -1 && DetectParam1_2.Double_DownSD != -1)
                    {
                        DetectParam1_2.Double_SS = DetectParam1_1.Double_UpSD - DetectParam1_2.Double_DownSD;

                        if (DetectParam1_2.Double_SS < DetectParam1_2.Double_SSmin || DetectParam1_2.Double_SS > DetectParam1_2.Double_SSmax)
                        {
                            OverHangNG_SSInNum++;
                            FontColor_SSIn = "red";
                            DetectResult = false;
                        }
                        else
                        {
                            FontColor_SSIn = "green";
                        }
                    }
                    else
                    {
                        DetectParam1_2.Double_SS = -10;
                        FontColor_SSIn = "red";
                        DetectResult = false;
                    }

                    if (DetectParam2_1.Double_CD != -1 && DetectParam2_2.Double_AD != -1)
                    {
                        DetectParam2_1.Double_AC = DetectParam2_2.Double_AD - DetectParam2_1.Double_CD;

                        if (DetectParam2_1.Double_AC < DetectParam2_1.Double_ACmin || DetectParam2_1.Double_AC > DetectParam2_1.Double_ACmax)
                        {
                            OverHangNG_ACOutNum++;
                            FontColor_ACOut = "red";
                            DetectResult = false;
                        }
                        else
                        {
                            FontColor_ACOut = "green";
                        }
                    }
                    else
                    {
                        DetectParam2_1.Double_AC = -10;
                        FontColor_ACOut = "red";
                        DetectResult = false;
                    }

                    if (DetectParam2_1.Double_UpSD != -1 && DetectParam2_2.Double_DownSD != -1)
                    {
                        DetectParam2_2.Double_SS = DetectParam2_2.Double_DownSD - DetectParam2_1.Double_UpSD;

                        if (DetectParam2_2.Double_SS < DetectParam2_2.Double_SSmin || DetectParam2_2.Double_SS > DetectParam2_2.Double_SSmax)
                        {
                            OverHangNG_SSOutNum++;
                            FontColor_SSOut = "red";
                            DetectResult = false;
                        }
                        else
                        {
                            FontColor_SSOut = "green";
                        }
                    }
                    else
                    {
                        DetectParam2_2.Double_SS = -10;
                        FontColor_SSOut = "red";
                        DetectResult = false;
                    }


                    //测量结果保存到队列中
                    DetectParam1_1.List_CD.Add(DetectParam1_1.Double_CD);
                    DetectParam1_1.List_UpSD.Add(DetectParam1_1.Double_UpSD);
                    DetectParam1_1.List_AngleCD.Add(DetectParam1_1.Double_AngleCD);
                    DetectParam1_1.List_AngleUpSD.Add(DetectParam1_1.Double_AngleUpSD);
                    DetectParam1_1.List_CS.Add(DetectParam1_1.Double_CS);
                    DetectParam1_1.List_AC.Add(DetectParam1_1.Double_AC);

                    DetectParam1_2.List_AD.Add(DetectParam1_2.Double_AD);
                    DetectParam1_2.List_DownSD.Add(DetectParam1_2.Double_DownSD);
                    DetectParam1_2.List_AngleAD.Add(DetectParam1_2.Double_AngleAD);
                    DetectParam1_2.List_AngleDownSD.Add(DetectParam1_2.Double_AngleDownSD);
                    DetectParam1_2.List_AS.Add(DetectParam1_2.Double_AS);
                    DetectParam1_2.List_SS.Add(DetectParam1_2.Double_SS);

                    DetectParam2_1.List_CD.Add(DetectParam2_1.Double_CD);
                    DetectParam2_1.List_UpSD.Add(DetectParam2_1.Double_UpSD);
                    DetectParam2_1.List_AngleCD.Add(DetectParam2_1.Double_AngleAD);
                    DetectParam2_1.List_AngleUpSD.Add(DetectParam2_1.Double_AngleUpSD);
                    DetectParam2_1.List_CS.Add(DetectParam2_1.Double_CS);
                    DetectParam2_1.List_AC.Add(DetectParam2_1.Double_AC);

                    DetectParam2_2.List_AD.Add(DetectParam2_2.Double_AD);
                    DetectParam2_2.List_DownSD.Add(DetectParam2_2.Double_DownSD);
                    DetectParam2_2.List_AngleAD.Add(DetectParam2_2.Double_AngleAD);
                    DetectParam2_2.List_AngleDownSD.Add(DetectParam2_2.Double_AngleDownSD);
                    DetectParam2_2.List_AS.Add(DetectParam2_2.Double_AS);
                    DetectParam2_2.List_SS.Add(DetectParam2_2.Double_SS);


                    if (DetectResult)
                        ResultStyle.Add("OK");
                    else
                    {
                        ResultStyle.Add("NG");
                        TotalResult = false;
                    }


                    //窗口显示检测结果

                    DetectionNum++;

                    Invoke(new MethodInvoker(delegate
                    {
                        ShowResultInHWindow();
                    }));

                    //当前电芯生产数据更新
                    if (RefreshInformationForm_Chart_Event != null)
                        RefreshInformationForm_Chart_Event();

                    //检测截图加入队列
                    HOperatorSet.DumpWindowImage(out HObject ho_Screen1_1, HWControl1.HalconID);
                    DetectParam1_1.List_StitchImage.Add(ho_Screen1_1);

                    HOperatorSet.DumpWindowImage(out HObject ho_Screen1_2, HWControl2.HalconID);
                    DetectParam1_2.List_StitchImage.Add(ho_Screen1_2);

                    HOperatorSet.DumpWindowImage(out HObject ho_Screen2_1, HWControl3.HalconID);
                    DetectParam2_1.List_StitchImage.Add(ho_Screen2_1);

                    HOperatorSet.DumpWindowImage(out HObject ho_Screen2_2, HWControl4.HalconID);
                    DetectParam2_2.List_StitchImage.Add(ho_Screen2_2);

                    //ho_Screen1_1.Dispose();
                    //ho_Screen1_2.Dispose();
                    //ho_Screen2_1.Dispose();
                    //ho_Screen2_2.Dispose();
                }

                if (DetectStopFlag && ImageDateListLength.Min() == DetectionNum && DetectionNum != 0)
                {

                    DetectProcessFlag = false;
                    Invoke(new MethodInvoker(delegate
                    {
                        DetectStop();
                    }));
                }
                else if (DetectStopFlag && DetectionNum == 0)
                {
                    DetectProcessFlag = false;
                    //检测停止，心跳灯-黄
                    statusStrip1.Invoke(new MethodInvoker(delegate
                    {
                        this.CheckStatusLight.Image = global::通用架构.Properties.Resources.黄灯;
                    }));
                }

            }
        }


        /// <summary>
        /// 发送PLC检测结果
        /// </summary>
        private void SendPLCData()
        {
            //检测结果bool       
            OmronCip.Write("CCD_Result11", true);
            OmronCip.Write("CCD_Result0", CCD_Result[0]);
            OmronCip.Write("CCD_Result1", CCD_Result[1]);
            OmronCip.Write("CCD_Result2", CCD_Result[2]);
            OmronCip.Write("CCD_Result3", CCD_Result[3]);
            OmronCip.Write("CCD_Result6", CCD_Result[6]);
            OmronCip.Write("CCD_Result7", CCD_Result[7]);
            OmronCip.Write("CCD_Result8", CCD_Result[8]);


            ////内侧检测参数（mean值）float
            //OmronCip.Write("CCD_Data_In0", CCD_Data_In[0]);
            //OmronCip.Write("CCD_Data_In1", CCD_Data_In[1]);
            //OmronCip.Write("CCD_Data_In2", CCD_Data_In[2]);
            //OmronCip.Write("CCD_Data_In3", CCD_Data_In[3]);
            //OmronCip.Write("CCD_Data_In4", CCD_Data_In[4]);
            //OmronCip.Write("CCD_Data_In5", CCD_Data_In[5]);
            //OmronCip.Write("CCD_Data_In6", CCD_Data_In[6]);
            //OmronCip.Write("CCD_Data_In7", CCD_Data_In[7]);
            ////外侧检测参数(mean值)float
            //OmronCip.Write("CCD_Data_Out0", CCD_Data_Out[0]);
            //OmronCip.Write("CCD_Data_Out1", CCD_Data_Out[1]);
            //OmronCip.Write("CCD_Data_Out2", CCD_Data_Out[2]);
            //OmronCip.Write("CCD_Data_Out3", CCD_Data_Out[3]);
            //OmronCip.Write("CCD_Data_Out4", CCD_Data_Out[4]);
            //OmronCip.Write("CCD_Data_Out5", CCD_Data_Out[5]);
            //OmronCip.Write("CCD_Data_Out6", CCD_Data_Out[6]);
            //OmronCip.Write("CCD_Data_Out7", CCD_Data_Out[7]);
        }


        ///<summary>
        ///保存检测数据任务
        ///</summary>
        public void SaveCsvAndImage()
        {
            string[,] CsvArray = new string[80, 20];

            try
            {

                //保存CSV文件
                for (int i = 0; i < DetectionNum; i++)
                {
                    CsvArray[i, 0] = (i + 1).ToString();
                    //内侧
                    CsvArray[i, 1] = DetectParam1_1.List_CD[i].ToString("f2");
                    CsvArray[i, 2] = DetectParam1_2.List_AD[i].ToString("f2");
                    CsvArray[i, 3] = DetectParam1_1.List_UpSD[i].ToString("f2");
                    CsvArray[i, 4] = DetectParam1_2.List_DownSD[i].ToString("f2");

                    CsvArray[i, 5] = DetectParam1_1.List_CS[i].ToString("f2");
                    CsvArray[i, 6] = DetectParam1_2.List_AS[i].ToString("f2");
                    CsvArray[i, 7] = DetectParam1_1.List_AC[i].ToString("f2");
                    CsvArray[i, 8] = DetectParam1_2.List_SS[i].ToString("f2");

                    //外侧
                    CsvArray[i, 9] = DetectParam2_1.List_CD[i].ToString("f2");
                    CsvArray[i, 10] = DetectParam2_2.List_AD[i].ToString("f2");
                    CsvArray[i, 11] = DetectParam2_1.List_UpSD[i].ToString("f2");
                    CsvArray[i, 12] = DetectParam2_2.List_DownSD[i].ToString("f2");

                    CsvArray[i, 13] = DetectParam2_1.List_CS[i].ToString("f2");
                    CsvArray[i, 14] = DetectParam2_2.List_AS[i].ToString("f2");
                    CsvArray[i, 15] = DetectParam2_1.List_AC[i].ToString("f2");
                    CsvArray[i, 16] = DetectParam2_2.List_SS[i].ToString("f2");


                    CsvArray[i, 17] = ResultStyle[i];
                }

                if (TotalResult)
                {
                    CsvName = CsvName + "OK";
                    CsvWrite.Write(CsvArray, CsvPath, CsvName, true);
                }
                else
                {
                    CsvName = CsvName + "NG";
                    CsvWrite.Write(CsvArray, CsvPath, CsvName, true);

                    //保存NG图片
                    if (!Directory.Exists(ScreenPath))
                    {
                        Directory.CreateDirectory(ScreenPath);
                    }

                    HObject ho_ObjectsConcat_In = null;
                    HObject ho_TiledImage_In = null;
                    HObject ho_ObjectsConcat_Out = null;
                    HObject ho_TiledImage_Out = null;
                    HObject ho_ObjectsConcat = null;
                    HObject ho_TiledImage = null;

                    //复制内存数据，避免接收到检测信号List数据清空。

                    List<HObject> lScreen_1_1copy = DetectParam1_1.List_StitchImage;
                    List<HObject> lScreen_1_2copy = DetectParam1_2.List_StitchImage;
                    List<HObject> lScreen_2_1copy = DetectParam2_1.List_StitchImage;
                    List<HObject> lScreen_2_2copy = DetectParam2_2.List_StitchImage;

                    for (int i = 0; i < DetectionNum; i++)
                    {
                        if (ResultStyle[i] != "OK")
                        {
                            string screenPath = ScreenPath + i.ToString();
                            //将图标对象Objects1和Objects2的两个元组连接到图标对象ObjectsConcat的新元组中。此元组包含两个输入元组的所有对象：
                            HOperatorSet.ConcatObj(lScreen_1_1copy[i], lScreen_1_2copy[i], out ho_ObjectsConcat_In);
                            //图像拼接
                            HOperatorSet.TileImages(ho_ObjectsConcat_In, out ho_TiledImage_In, 1, "vertical");
                            HOperatorSet.ConcatObj(lScreen_2_1copy[i], lScreen_2_2copy[i], out ho_ObjectsConcat_Out);
                            HOperatorSet.TileImages(ho_ObjectsConcat_Out, out ho_TiledImage_Out, 1, "vertical");
                            HOperatorSet.ConcatObj(ho_TiledImage_In, ho_TiledImage_Out, out ho_ObjectsConcat);
                            HOperatorSet.TileImages(ho_ObjectsConcat, out ho_TiledImage, 2, "horizontal");
                            HOperatorSet.WriteImage(ho_TiledImage, "png", 0, screenPath);
                        }
                    }

                    ho_ObjectsConcat_In.Dispose();
                    ho_TiledImage_In.Dispose();
                    ho_ObjectsConcat_Out.Dispose();
                    ho_ObjectsConcat_Out.Dispose();
                    ho_TiledImage_Out.Dispose();
                    ho_ObjectsConcat.Dispose();
                    ho_TiledImage.Dispose();

                }
            }

            catch (Exception)
            { }
            if (RefreshInformationForm_Event != null)
                RefreshInformationForm_Event();
        }


        ///<summary>
        ///开始检测函数
        ///</summary>
        private void DetectStart()
        {
            //进度条初始化
            Switch_count(0, true);

            LogName = DateTime.Now.ToString("HH_mm_ss");
            DateName = DateTime.Now.ToString("yyyy-MM-dd");
            ScreenPath = AppDomain.CurrentDomain.BaseDirectory + "Screen\\" + DateName + "\\" + LogName + "\\";
            ImagePath = AppDomain.CurrentDomain.BaseDirectory + "Image\\" + DateName + "\\" + LogName + "\\";

            //CSV参数保存卷绕周期内的每次拍照检测参数
            CsvPath = AppDomain.CurrentDomain.BaseDirectory + "CSV\\" + DateName + "\\";
            CsvName = DateTime.Now.ToString("HH_mm_ss");

            DetectionNum = 0;
            TotalResult = true;
            ResultStyle.Clear();

            //检测队列清除
            DetectParam1_1.List_CD.Clear();
            DetectParam1_1.List_UpSD.Clear();
            DetectParam1_1.List_AngleCD.Clear();
            DetectParam1_1.List_AngleUpSD.Clear();
            DetectParam1_1.List_CS.Clear();
            DetectParam1_1.List_AC.Clear();

            DetectParam1_2.List_AD.Clear();
            DetectParam1_2.List_DownSD.Clear();
            DetectParam1_2.List_AngleAD.Clear();
            DetectParam1_2.List_AngleDownSD.Clear();
            DetectParam1_2.List_AS.Clear();
            DetectParam1_2.List_SS.Clear();

            DetectParam2_1.List_CD.Clear();
            DetectParam2_1.List_UpSD.Clear();
            DetectParam2_1.List_AngleCD.Clear();
            DetectParam2_1.List_AngleUpSD.Clear();
            DetectParam2_1.List_CS.Clear();
            DetectParam2_1.List_AC.Clear();

            DetectParam2_2.List_AD.Clear();
            DetectParam2_2.List_DownSD.Clear();
            DetectParam2_2.List_AngleAD.Clear();
            DetectParam2_2.List_AngleDownSD.Clear();
            DetectParam2_2.List_AS.Clear();
            DetectParam2_2.List_SS.Clear();

            DetectParam1_1.List_StitchImage.Clear();
            DetectParam1_2.List_StitchImage.Clear();
            DetectParam2_1.List_StitchImage.Clear();
            DetectParam2_2.List_StitchImage.Clear();

            DetectParam1_1.List_Image.Clear();
            DetectParam1_2.List_Image.Clear();
            DetectParam2_1.List_Image.Clear();
            DetectParam2_2.List_Image.Clear();

            DetectParam1_1.List_ImageDate.Clear();
            DetectParam1_2.List_ImageDate.Clear();
            DetectParam2_1.List_ImageDate.Clear();
            DetectParam2_2.List_ImageDate.Clear();

            //检测开始，心跳灯-绿
            statusStrip1.Invoke(new MethodInvoker(delegate
            {
                this.CheckStatusLight.Image = global::通用架构.Properties.Resources.绿灯;
            }));


            //检测结果初始化
            CCD_Result = new bool[16] { false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false };
            //检测开始，清除当前生产数据曲线
            if (RefreshInformationForm_Chart_Event != null)
                RefreshInformationForm_Chart_Event();
            //开始检测任务
            DetectionProcess = new Task(LineDetectAllTask);
            DetectProcessFlag = true;
            DetectionProcess.Start();
        }


        ///<summary>
        ///停止检测函数
        ///</summary>
        private void DetectStop()
        {
            //进度条显示
            statusStrip1.Invoke(new MethodInvoker(delegate
            {
                Switch_count(21, TotalResult);
            }));

            CCD_Result[0] = !TotalResult;
            //SendPLCData();
            //电芯数量统计
            if (TotalResult)
                OKNum++;                        //良品数量

            TotalNum++;                         //电芯总数量
            OKRate = (double)OKNum / TotalNum;  //良品率

            if (ProductionData_Event != null)
                ProductionData_Event();


            //保存检测数据任务
            Task Task_SaveCsvAndImage = new Task(SaveCsvAndImage);
            Task_SaveCsvAndImage.Start();

            try
            {
                ////测量结果求均值
                //CCD_Data_In[0] = (float)DetectParam1_1.List_CD.Average();
                //CCD_Data_In[1] = (float)DetectParam1_2.List_AD.Average();
                //CCD_Data_In[2] = (float)DetectParam1_1.List_UpSD.Average();
                //CCD_Data_In[3] = (float)DetectParam1_2.List_DownSD.Average();
                //CCD_Data_In[4] = (float)DetectParam1_2.List_AS.Average();
                //CCD_Data_In[5] = (float)DetectParam1_1.List_CS.Average();
                //CCD_Data_In[6] = (float)DetectParam1_2.List_SS.Average();
                //CCD_Data_In[7] = (float)DetectParam1_1.List_AC.Average();

                //CCD_Data_Out[0] = (float)DetectParam2_1.List_CD.Average();
                //CCD_Data_Out[1] = (float)DetectParam2_2.List_AD.Average();
                //CCD_Data_Out[2] = (float)DetectParam2_1.List_UpSD.Average();
                //CCD_Data_Out[3] = (float)DetectParam2_2.List_DownSD.Average();
                //CCD_Data_Out[4] = (float)DetectParam2_2.List_AS.Average();
                //CCD_Data_Out[5] = (float)DetectParam2_1.List_CS.Average();
                //CCD_Data_Out[6] = (float)DetectParam2_2.List_SS.Average();
                //CCD_Data_Out[7] = (float)DetectParam2_1.List_AC.Average();

                ////发送结果给PLC
                //SendPLCData();


                //检测停止，心跳灯-黄
                statusStrip1.Invoke(new MethodInvoker(delegate
                {
                    this.CheckStatusLight.Image = global::通用架构.Properties.Resources.黄灯;
                }));
            }
            catch (Exception)
            {

            }
        }


        #region 进度条功能函数组



        #endregion


        ///<summary>
        ///窗体切换，防止闪烁
        ///</summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }


        #region 窗体控件重绘，防止闪烁
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;
        #endregion


        ///<summary>
        ///延时函数（图标显示用）
        ///</summary>
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {

                //可执行某无聊的操作
            }
        }


        #region Halcon窗体功能按钮
        private void LoadImage1_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.LoadImage1.BackgroundImage = global::通用架构.Properties.Resources.PicImage_Gray;
            Task LoadImage1_Active = new Task(() =>
            {
                Delay(150);
                this.LoadImage1.BackgroundImage = global::通用架构.Properties.Resources.PicImage_White;
            });
            LoadImage1_Active.Start();
            //

            LoadImage1.Refresh();//刷新控件

            //打开指定路径文件夹，加载图片
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.InitialDirectory = "F:\\1.工作文件\\1.程序文件";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DetectParam1_1.HObject_Image = new HImage(FileDialog.FileName);
                    if (DetectStartFlag)
                    {
                        CameraBasler_OverHangle1_1(DetectParam1_1.HObject_Image);
                    }
                    else
                    {
                        HWndCtrl1_1.addIconicVar(DetectParam1_1.HObject_Image);
                        HWndCtrl1_1.repaint();
                    }

                }
                catch (Exception)
                {
                }
            }
            //
        }

        private void Save1_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.Save1.BackgroundImage = global::通用架构.Properties.Resources.PicSave_Gray;
            Task Save_Active = new Task(() =>
            {
                Delay(150);
                this.Save1.BackgroundImage = global::通用架构.Properties.Resources.PicSave_White;
            });
            Save_Active.Start();
            //

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_1.bmp";

            try
            {

                HOperatorSet.WriteImage(DetectParam1_1.HObject_Image, "bmp", 0, filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("图像保存失败！");
            }
        }

        private void SaveAs1_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.SaveAs1.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_Gray;
            Task SaveAs_Active = new Task(() =>
            {
                Delay(150);
                this.SaveAs1.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_White;
            });
            SaveAs_Active.Start();
            //

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "BMP格式（*.bmp）|.bmp;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.WriteImage(DetectParam1_1.HObject_Image, "bmp", 0, filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("图像保存失败！");
                }
            }
        }

        private void ImageReset1_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.ImageReset1.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_Gray;
            Task ImageReset1_Active = new Task(() =>
            {
                Delay(150);
                this.ImageReset1.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_White;
            });
            ImageReset1_Active.Start();
            //

            //重置显示画面
            HWndCtrl1_1.resetAll();
            HWndCtrl1_1.repaint();
            //
        }


        private void LoadImage2_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.LoadImage2.BackgroundImage = global::通用架构.Properties.Resources.PicImage_Gray;
            Task LoadImage2_Active = new Task(() =>
            {
                Delay(150);
                this.LoadImage2.BackgroundImage = global::通用架构.Properties.Resources.PicImage_White;
            });
            LoadImage2_Active.Start();
            //

            LoadImage2.Refresh();//刷新控件

            //打开指定路径文件夹，加载图片
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.InitialDirectory = "F:\\1.工作文件\\1.程序文件";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DetectParam1_2.HObject_Image = new HImage(FileDialog.FileName);
                    if (DetectStartFlag)
                    {
                        CameraBasler_OverHangle1_2(DetectParam1_2.HObject_Image);
                    }
                    else
                    {
                        HWndCtrl1_2.addIconicVar(DetectParam1_2.HObject_Image);
                        HWndCtrl1_2.repaint();
                    }

                }
                catch (Exception)
                {
                }
            }
            //
        }


        private void Save2_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.Save2.BackgroundImage = global::通用架构.Properties.Resources.PicSave_Gray;
            Task Save_Active = new Task(() =>
            {
                Delay(150);
                this.Save2.BackgroundImage = global::通用架构.Properties.Resources.PicSave_White;
            });
            Save_Active.Start();
            //

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_2.bmp";

            try
            {

                HOperatorSet.WriteImage(DetectParam1_2.HObject_Image, "bmp", 0, filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("图像保存失败！");
            }
        }


        private void SaveAs2_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.SaveAs2.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_Gray;
            Task SaveAs_Active = new Task(() =>
            {
                Delay(150);
                this.SaveAs2.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_White;
            });
            SaveAs_Active.Start();
            //

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "BMP格式（*.bmp）|.bmp;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.WriteImage(DetectParam1_2.HObject_Image, "bmp", 0, filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("图像保存失败！");
                }
            }
        }


        private void ImageReset2_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.ImageReset2.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_Gray;
            Task ImageReset2_Active = new Task(() =>
            {
                Delay(150);
                this.ImageReset2.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_White;
            });
            ImageReset2_Active.Start();
            //

            //重置显示画面
            HWndCtrl1_2.resetAll();
            HWndCtrl1_2.repaint();
            //
        }


        private void LoadImage3_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.LoadImage3.BackgroundImage = global::通用架构.Properties.Resources.PicImage_Gray;
            Task LoadImage3_Active = new Task(() =>
            {
                Delay(150);
                this.LoadImage3.BackgroundImage = global::通用架构.Properties.Resources.PicImage_White;
            });
            LoadImage3_Active.Start();
            //

            LoadImage3.Refresh();//刷新控件

            //打开指定路径文件夹，加载图片
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.InitialDirectory = "F:\\1.工作文件\\1.程序文件";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DetectParam2_1.HObject_Image = new HImage(FileDialog.FileName);
                    if (DetectStartFlag)
                    {
                        CameraBasler_OverHangle2_1(DetectParam2_1.HObject_Image);
                    }
                    else
                    {
                        HWndCtrl2_1.addIconicVar(DetectParam2_1.HObject_Image);
                        HWndCtrl2_1.repaint();
                    }


                }
                catch (Exception)
                {
                }
            }
        }


        private void Save3_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.Save3.BackgroundImage = global::通用架构.Properties.Resources.PicSave_Gray;
            Task Save_Active = new Task(() =>
            {
                Delay(150);
                this.Save3.BackgroundImage = global::通用架构.Properties.Resources.PicSave_White;
            });
            Save_Active.Start();
            //

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_1.bmp";

            try
            {

                HOperatorSet.WriteImage(DetectParam2_1.HObject_Image, "bmp", 0, filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("图像保存失败！");
            }
        }

        private void SaveAs3_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.SaveAs3.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_Gray;
            Task SaveAs_Active = new Task(() =>
            {
                Delay(150);
                this.SaveAs3.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_White;
            });
            SaveAs_Active.Start();
            //

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "BMP格式（*.bmp）|.bmp;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.WriteImage(DetectParam2_1.HObject_Image, "bmp", 0, filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("图像保存失败！");
                }
            }
        }

        private void ImageReset3_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.ImageReset3.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_Gray;
            Task ImageReset3_Active = new Task(() =>
            {
                Delay(150);
                this.ImageReset3.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_White;
            });
            ImageReset3_Active.Start();
            //

            //重置显示画面
            HWndCtrl2_1.resetAll();
            HWndCtrl2_1.repaint();
            //

        }


        private void LoadImage4_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.LoadImage4.BackgroundImage = global::通用架构.Properties.Resources.PicImage_Gray;
            Task LoadImage4_Active = new Task(() =>
            {
                Delay(150);
                this.LoadImage4.BackgroundImage = global::通用架构.Properties.Resources.PicImage_White;
            });
            LoadImage4_Active.Start();
            //

            LoadImage4.Refresh();//刷新控件

            //打开指定路径文件夹，加载图片
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.InitialDirectory = "F:\\1.工作文件\\1.程序文件";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DetectParam2_2.HObject_Image = new HImage(FileDialog.FileName);
                    if (DetectStartFlag)
                    {
                        CameraBasler_OverHangle2_2(DetectParam2_2.HObject_Image);
                    }
                    else
                    {
                        HWndCtrl2_2.addIconicVar(DetectParam2_2.HObject_Image);
                        HWndCtrl2_2.repaint();
                    }

                }
                catch (Exception)
                {
                }
            }
        }


        private void Save4_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.Save4.BackgroundImage = global::通用架构.Properties.Resources.PicSave_Gray;
            Task Save_Active = new Task(() =>
            {
                Delay(150);
                this.Save4.BackgroundImage = global::通用架构.Properties.Resources.PicSave_White;
            });
            Save_Active.Start();
            //

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_2.bmp";

            try
            {

                HOperatorSet.WriteImage(DetectParam2_2.HObject_Image, "bmp", 0, filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("图像保存失败！");
            }
        }

        private void SaveAs4_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.SaveAs4.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_Gray;
            Task SaveAs_Active = new Task(() =>
            {
                Delay(150);
                this.SaveAs4.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_White;
            });
            SaveAs_Active.Start();
            //

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "BMP格式（*.bmp）|.bmp;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.WriteImage(DetectParam2_2.HObject_Image, "bmp", 0, filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("图像保存失败！");
                }
            }
        }

        private void ImageReset4_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.ImageReset4.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_Gray;
            Task ImageReset4_Active = new Task(() =>
            {
                Delay(150);
                this.ImageReset4.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_White;
            });
            ImageReset4_Active.Start();
            //

            //重置显示画面
            HWndCtrl2_2.resetAll();
            HWndCtrl2_2.repaint();
        }
        #endregion


        #region 获取图像灰度值（鼠标移动事件）
        private void HWControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (DetectParam1_1.HObject_Image != null)
                try
                {

                    GrayPosition1_1.Text = HWndCtrl1_1.Get_Mouseposition_and_Gray(HWControl1.HalconID, DetectParam1_1.HObject_Image, e);
                }
                catch (Exception)
                {

                }
        }

        private void HWControl2_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (DetectParam1_2.HObject_Image != null)
                try
                {

                    GrayPosition1_2.Text = HWndCtrl1_2.Get_Mouseposition_and_Gray(HWControl2.HalconID, DetectParam1_2.HObject_Image, e);
                }
                catch (Exception)
                {

                }
        }

        private void HWControl3_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (DetectParam2_1.HObject_Image != null)
                try
                {

                    GrayPosition2_1.Text = HWndCtrl2_1.Get_Mouseposition_and_Gray(HWControl3.HalconID, DetectParam2_1.HObject_Image, e);
                }
                catch (Exception)
                {

                }
        }

        private void HWControl4_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (DetectParam2_2.HObject_Image != null)
                try
                {

                    GrayPosition2_2.Text = HWndCtrl2_2.Get_Mouseposition_and_Gray(HWControl4.HalconID, DetectParam2_2.HObject_Image, e);
                }
                catch (Exception)
                {

                }
        }
        #endregion


        ///<summary>
        ///Halcon窗口扩展/回缩
        ///</summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘
            if (this.checkBox1.Checked == true)
            {
                this.TLP_Text1.Visible = false;
                this.TLP_Text2.Visible = false;
                this.TLP_Text3.Visible = false;
                this.TLP_Text4.Visible = false;

                this.TLPanel_Frame1.SetColumnSpan(this.TLPanel_BG1, 10);
                this.TLPanel_Frame2.SetColumnSpan(this.TLPanel_BG2, 10);
                this.TLPanel_Frame3.SetColumnSpan(this.TLPanel_BG3, 10);
                this.TLPanel_Frame4.SetColumnSpan(this.TLPanel_BG4, 10);


            }
            else if (this.checkBox1.Checked == false)
            {
                this.TLPanel_Frame1.SetColumnSpan(this.TLPanel_BG1, 8);
                this.TLPanel_Frame2.SetColumnSpan(this.TLPanel_BG2, 8);
                this.TLPanel_Frame3.SetColumnSpan(this.TLPanel_BG3, 8);
                this.TLPanel_Frame4.SetColumnSpan(this.TLPanel_BG4, 8);
                this.TLP_Text1.Visible = true;
                this.TLP_Text2.Visible = true;
                this.TLP_Text3.Visible = true;
                this.TLP_Text4.Visible = true;

            }
            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();//刷新控件
        }


        private void Button_DetectStart_Click(object sender, EventArgs e)
        {
            if (!DetectStartFlag)
            {
                DetectStartFlag = true;
                DetectStopFlag = false;
                DetectStart();
            }
        }

        private void Button_DetectStop_Click(object sender, EventArgs e)
        {
            DetectStartFlag = false;
            DetectStopFlag = true;
        }


        public void Input_Detect_Num(int TotalNum, int ThisNum, bool ThisResult)
        {
            int step = TotalNum / 20;
            int PB_Num = ThisNum / step + 1;

            if (ThisNum > TotalNum)
                PB_Num = 20;

            Switch_count(PB_Num, ThisResult);
        }

        private void SetProgressBarColor(ToolStripStatusLabel progressbar, bool eachcycleresult)
        {
            if (eachcycleresult && progressbar.BackColor != Color.Red)
            {
                progressbar.BackColor = Color.LimeGreen;
            }
            else
            {
                progressbar.BackColor = Color.Red;
            }
        }

        private void Switch_count(int Count, bool EachCycleResult)
        {

            switch (Count)
            {
                case 0:
                    TSSL_PB1.BackColor = Color.Gainsboro;
                    TSSL_PB2.BackColor = Color.Gainsboro;
                    TSSL_PB3.BackColor = Color.Gainsboro;
                    TSSL_PB4.BackColor = Color.Gainsboro;
                    TSSL_PB5.BackColor = Color.Gainsboro;
                    TSSL_PB6.BackColor = Color.Gainsboro;
                    TSSL_PB7.BackColor = Color.Gainsboro;
                    TSSL_PB8.BackColor = Color.Gainsboro;
                    TSSL_PB9.BackColor = Color.Gainsboro;
                    TSSL_PB10.BackColor = Color.Gainsboro;
                    TSSL_PB11.BackColor = Color.Gainsboro;
                    TSSL_PB12.BackColor = Color.Gainsboro;
                    TSSL_PB13.BackColor = Color.Gainsboro;
                    TSSL_PB14.BackColor = Color.Gainsboro;
                    TSSL_PB15.BackColor = Color.Gainsboro;
                    TSSL_PB16.BackColor = Color.Gainsboro;
                    TSSL_PB17.BackColor = Color.Gainsboro;
                    TSSL_PB18.BackColor = Color.Gainsboro;
                    TSSL_PB19.BackColor = Color.Gainsboro;
                    TSSL_PB20.BackColor = Color.Gainsboro;
                    TSSL_PB21.BackColor = Color.Gainsboro;
                    break;
                case 1:
                    SetProgressBarColor(TSSL_PB1, EachCycleResult);
                    break;
                case 2:
                    SetProgressBarColor(TSSL_PB2, EachCycleResult);
                    break;
                case 3:
                    SetProgressBarColor(TSSL_PB3, EachCycleResult);
                    break;
                case 4:
                    SetProgressBarColor(TSSL_PB4, EachCycleResult);
                    break;
                case 5:
                    SetProgressBarColor(TSSL_PB5, EachCycleResult);
                    break;
                case 6:
                    SetProgressBarColor(TSSL_PB6, EachCycleResult);
                    break;
                case 7:
                    SetProgressBarColor(TSSL_PB7, EachCycleResult);
                    break;
                case 8:
                    SetProgressBarColor(TSSL_PB8, EachCycleResult);
                    break;
                case 9:
                    SetProgressBarColor(TSSL_PB9, EachCycleResult);
                    break;
                case 10:
                    SetProgressBarColor(TSSL_PB10, EachCycleResult);
                    break;
                case 11:
                    SetProgressBarColor(TSSL_PB11, EachCycleResult);
                    break;
                case 12:
                    SetProgressBarColor(TSSL_PB12, EachCycleResult);
                    break;
                case 13:
                    SetProgressBarColor(TSSL_PB13, EachCycleResult);
                    break;
                case 14:
                    SetProgressBarColor(TSSL_PB14, EachCycleResult);
                    break;
                case 15:
                    SetProgressBarColor(TSSL_PB15, EachCycleResult);
                    break;
                case 16:
                    SetProgressBarColor(TSSL_PB16, EachCycleResult);
                    break;
                case 17:
                    SetProgressBarColor(TSSL_PB17, EachCycleResult);
                    break;
                case 18:
                    SetProgressBarColor(TSSL_PB18, EachCycleResult);
                    break;
                case 19:
                    SetProgressBarColor(TSSL_PB19, EachCycleResult);
                    break;
                case 20:
                    SetProgressBarColor(TSSL_PB20, EachCycleResult);
                    break;
                case 21:
                    SetProgressBarColor(TSSL_PB21, EachCycleResult);
                    break;

                default:
                    break;

            }
        }
    }
}

