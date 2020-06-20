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
using 通用架构._3.基础函数;
using 通用架构._4.算子库;


namespace 通用架构._2.子界面
{
    public partial class LineDetect : Form
    {
        #region  变量定义
        FittingLine FittingLine;
        ImageProcessing ImageProcessing;

        public HWndCtrl HWndCtrl;
        public string SaveLineDataName;
        public bool Refresh_Params;
        public bool ProcessImageflag;
        public HObject ho_Image;                 //源图像

        //测量参数
        public HTuple hv_MetrologyHandle;        //测量句柄
        public HTuple hv_Width;
        public HTuple hv_Height;


        //矩形roi参数
        public HTuple hv_MeasureRow1;             //roi直线行坐标1
        public HTuple hv_MeasureColumn1;          //roi直线列坐标1
        public HTuple hv_MeasureRow2;             //roi直线行坐标2
        public HTuple hv_MeasureColumn2;          //roi直线列坐标2
        public HTuple hv_MeasureLength1;          //roi半长
        public HTuple hv_MeasureLength2;          //roi半宽


        //图像预处理变量
        public HTuple hv_MaskWidth;              //均值滤波宽度
        public HTuple hv_MaskHeight;             //均值滤波高度
        public HTuple hv_MaskType;               //中值滤波形状
        public HTuple hv_Radius;                 //中值滤波半径
        public HTuple hv_MaskSize;               //Laplace滤波模型大小
        public HTuple hv_FilterMask;             //Laplace滤波模型
        public HTuple hv_MultImage_Mult;         //图像乘法增益
        public HTuple hv_AddImage_Mult;          //图像加法增益
        public HTuple hv_ScaleImage_Add;         //阈值拉伸偏移
        public HTuple hv_ScaleImage_Mult;        //阈值拉伸增益

        public HTuple hv_MeasureSigma;            //平滑系数
        public HTuple hv_MeasureThreshold;        //灰度阈值
        public HTuple hv_MeasureInterpolation;     //插值类型
        public HTuple hv_MeasureTransition;       //明暗方向
        public HTuple hv_MeasureSelect;           //边缘选取
        public HTuple hv_MeasurePointsNum;        //Roi数量
        public HTuple hv_MeasureDistance;         //Roi距离
        public HTuple hv_MeasureMinScore;         //最低得分
        public HTuple hv_MeasureNumInstances;     //直线拟合数量
        public HTuple hv_MeasureDistThreshold;    //距离剔除
        public HTuple hv_MaxNumIterations;        //迭代次数
        public HTuple hv_MeasureIORegions;        //对测量结果的验证
        public HTuple hv_ImageProcessing;

        public HTuple hv_Index;                   //测量结果标志
        public bool DetectLineFlag;               //直线拟合标志

        public HObject ho_Contours;               //测量矩形轮廓
        public HObject ho_Cross;                  //测量结果轮廓点
        public HTuple hv_CrossRow;                //测量结果轮廓点行坐标
        public HTuple hv_CrossColumn;             //测量结果轮廓点列坐标

        public HObject ho_RegionLines;             //测量结果直线轮廓
        public HTuple hv_LineRowBegin;            //拟合直线像素坐标
        public HTuple hv_LineColumnBegin;
        public HTuple hv_LineRowEnd;
        public HTuple hv_LineColumnEnd;

        public HTuple hv_LineRowBegin_Real;       //拟合直线实际坐标
        public HTuple hv_LineColumnBegin_Real;
        public HTuple hv_LineRowEnd_Real;
        public HTuple hv_LineColumnEnd_Real;

        XmlRW xmlRW;

        public delegate void SaveParamsDelegate();//保存参数委托                              
        public event SaveParamsDelegate SaveParamsEvent; //保存参数委托事件

        public delegate void ReviewLineDelegate();//直线轮廓显示委托                              
        public event ReviewLineDelegate ReviewLineEvent; //定义直线轮廓显示事件
        #endregion


        ///<summary>
        ///主函数
        ///</summary>
        public LineDetect()
        {
            InitializeComponent();
            Initialization();
        }


        ///<summary>
        ///初始化函数
        ///</summary>
        public void Initialization()
        {
            FittingLine = new FittingLine();
            ImageProcessing = new ImageProcessing();
            hv_MeasureRow1 = new HTuple();
            hv_MeasureColumn1 = new HTuple();
            hv_MeasureRow2 = new HTuple();
            hv_MeasureColumn2 = new HTuple();

            hv_MeasureLength2 = new HTuple();
            hv_MeasureDistance = new HTuple();
            hv_MetrologyHandle = new HTuple();


            hv_MeasureLength1 = 20;
            hv_MeasureSigma = 1.0;
            hv_MeasureThreshold = 30;

            hv_MeasureInterpolation = "bicubic";     //插值类型
            hv_MeasureTransition = "positive";       //明暗方向
            hv_MeasureSelect = "first";              //边缘选取
            hv_MeasurePointsNum = 20;                //Roi数量
            hv_MeasureMinScore = 0.3;                //最低得分
            hv_MeasureNumInstances = 1;              //直线拟合数量
            hv_MeasureDistThreshold = 2;             //距离剔除
            hv_MaxNumIterations = -1;                //迭代次数
            hv_MeasureIORegions = "false";           //对测量结果的验证
            hv_ImageProcessing = "None";            //对图像预处理

            hv_MaskWidth = 1;              //均值滤波宽度
            hv_MaskHeight = 10;             //均值滤波高度
            hv_MaskType = "square";               //中值滤波形状
            hv_Radius = 10;                 //中值滤波半径
            hv_MaskSize = 7;               //Laplace滤波模型大小
            hv_FilterMask = "n_4";             //Laplace滤波模型
            hv_MultImage_Mult = 0.03;         //图像乘法增益
            hv_AddImage_Mult = 0.8;          //图像加法增益
            hv_ScaleImage_Add = 0;         //阈值拉伸偏移
            hv_ScaleImage_Mult = 2;        //阈值拉伸增益

            hv_Index = null;
            DetectLineFlag = false;
            ho_Contours = null;
            ho_Cross = null;
            hv_CrossRow = null;
            hv_CrossColumn = null;
            ho_RegionLines = null;

            xmlRW = new XmlRW();

            hv_LineRowBegin = 0;                       //拟合直线像素坐标
            hv_LineColumnBegin = 0;
            hv_LineRowEnd = 0;
            hv_LineColumnEnd = 0;

            hv_LineRowBegin_Real = 0;                //拟合直线实际坐标
            hv_LineColumnBegin_Real = 0;
            hv_LineRowEnd_Real = 0;
            hv_LineColumnEnd_Real = 0;

            RefreshParams();

        }


        ///<summary>
        ///参数刷新
        ///</summary>
        public void RefreshParams()
        {
            try
            {

                //基础参数
                NUD_MeasurePointsNum.Value = hv_MeasurePointsNum;
                NUD_MeasureSigma.Value = Convert.ToDecimal(hv_MeasureSigma.ToString());
                NUD_MeasureThreshold.Value = hv_MeasureThreshold;

                if (hv_MeasureTransition == "negative")
                {
                    CB_MeasureTransition.Text = "由明到暗";
                }
                else if (hv_MeasureTransition == "positive")
                {
                    CB_MeasureTransition.Text = "由暗到明";
                }
                else if (hv_MeasureTransition == "all")
                {
                    CB_MeasureTransition.Text = "不区分明暗";
                }

                if (hv_MeasureSelect == "first")
                {
                    CB_MeasureSelect.Text = "第一条边";
                }
                else if (hv_MeasureTransition == "last")
                {
                    CB_MeasureSelect.Text = "最后一条边";
                }
                else if (hv_MeasureTransition == "all")
                {
                    CB_MeasureSelect.Text = "最强边";
                }


                //高级参数
                NUD_MeasureMinScore.Value = Convert.ToDecimal(hv_MeasureMinScore.ToString());
                NUD_MeasureNumInstances.Value = hv_MeasureNumInstances;
                NUD_MeasureDistThreshold.Value = hv_MeasureDistThreshold;
                NUD_MaxNumIterations.Value = hv_MaxNumIterations;

                if (hv_MeasureIORegions == "true")
                {
                    CB_MeasureIORegions.Text = "是";
                }
                else if (hv_MeasureIORegions == "false")
                {
                    CB_MeasureIORegions.Text = "否";
                }

                if (hv_MeasureInterpolation == "nearest_neighbor")
                    CB_MeasureInterpolation.Text = "最近邻域法";
                else if (hv_MeasureInterpolation == "bilinear")
                    CB_MeasureInterpolation.Text = "双线性插值法";
                else if (hv_MeasureInterpolation == "bicubic")
                    CB_MeasureInterpolation.Text = "双三次插值法";

                if (hv_ImageProcessing == "None")
                    CB_ImageProcessing.Text = "无处理";
                else if (hv_ImageProcessing == "SharpenImage")
                    CB_ImageProcessing.Text = "锐化处理";
                else if (hv_ImageProcessing == "ScaleImage")
                    CB_ImageProcessing.Text = "平滑处理";

                //图像预处理参数
                NUD_MeanWidth.Value = hv_MaskWidth;              //均值滤波宽度
                NUD_MeanHeight.Value = hv_MaskHeight;             //均值滤波高度
                NUD_MedianRadius.Value = hv_Radius;                 //中值滤波半径
                NUD_LaplaceSize.Value = hv_MaskSize;               //Laplace滤波模型大小
                NUD_MultImage_Mult.Value = Convert.ToDecimal(hv_MultImage_Mult.ToString());         //图像乘法增益
                NUD_AddImage_Mult.Value = Convert.ToDecimal(hv_AddImage_Mult.ToString());          //图像加法增益
                NUD_ScaleImage_Add.Value = hv_ScaleImage_Add;         //阈值拉伸偏移
                NUD_ScaleImage_Mult.Value = Convert.ToDecimal(hv_ScaleImage_Mult.ToString());        //阈值拉伸增益


            }
            catch (Exception exc)
            {
                MessageBox.Show("初始化参数失败！" + exc.ToString());
            }
        }


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


        ///<summary>
        ///图像预处理函数
        ///</summary>
        public void ProcessImage(HObject ho_Image, HTuple hv_CenterRow, HTuple hv_CenterCol, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2, out HObject ho_Image1)
        {
            try
            {
                ImageProcessing.ImageCrop(ho_Image, hv_CenterRow, hv_CenterCol, hv_Phi, hv_Length1, hv_Length2,
                                             out HObject ho_Rectangle2, out HObject ho_ImagePart);

                ImageProcessing.ImageSharpening(ho_ImagePart, hv_MaskWidth, hv_MaskHeight, hv_MaskType, hv_Radius, hv_MaskSize,
                                                hv_FilterMask, hv_MultImage_Mult, hv_AddImage_Mult, hv_ScaleImage_Mult,
                                                hv_ScaleImage_Add, out HObject ho_ImageResult);

                ImageProcessing.ImageStitching(ho_ImageResult, ho_Image, ho_Rectangle2, hv_Phi, hv_Length1, hv_Length2,
                                               hv_CenterRow, hv_CenterCol, out ho_Image1);
                ProcessImageflag = true;
            }
            catch (Exception)
            {
                ho_Image1 = new HObject();
                ProcessImageflag = false;
            }
        }


        ///<summary>
        ///直线检测函数
        ///</summary>
        public void DetectLine()
        {


            HOperatorSet.DistancePp(hv_MeasureRow1, hv_MeasureColumn1, hv_MeasureRow2, hv_MeasureColumn2, out HTuple DistancePp);
            DistancePp = DistancePp / 2;
            FittingLine.CreateLineModel(hv_MeasureRow1, hv_MeasureColumn1, hv_MeasureRow2, hv_MeasureColumn2, hv_MeasureLength1,
                                         DistancePp / hv_MeasurePointsNum, hv_MeasureSigma, hv_MeasureThreshold, out hv_MetrologyHandle, out hv_Index);
            FittingLine.FindLine(ho_Image, hv_MetrologyHandle, hv_Index, hv_MeasureNumInstances, hv_MeasureDistThreshold, hv_MeasureTransition, hv_MeasureSelect,
                           hv_MeasurePointsNum, hv_MeasureMinScore, hv_MaxNumIterations, hv_MeasureInterpolation, hv_MeasureIORegions, out hv_CrossRow,
                          out hv_CrossColumn, out hv_LineRowBegin, out hv_LineColumnBegin, out hv_LineRowEnd, out hv_LineColumnEnd);
           if(hv_LineRowBegin!=null)
                DetectLineFlag = true;
            else
                DetectLineFlag = false;
        }



        ///<summary>
        ///读取xml文件参数
        ///</summary>
        public bool ReadParams(String xmlNode)
        {

            try
            {
                string shv_MeasureRow1 = "Parameters/" + xmlNode + "/hv_MeasureRow1";
                string shv_MeasureColumn1 = "Parameters/" + xmlNode + "/hv_MeasureColumn1";
                string shv_MeasureRow2 = "Parameters/" + xmlNode + "/hv_MeasureRow2";
                string shv_MeasureColumn2 = "Parameters/" + xmlNode + "/hv_MeasureColumn2";

                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_MeasureInterpolation = "Parameters/" + xmlNode + "/hv_MeasureInterpolation";
                string shv_MeasureTransition = "Parameters/" + xmlNode + "/hv_MeasureTransition";
                string shv_MeasureSelect = "Parameters/" + xmlNode + "/hv_MeasureSelect";
                string shv_MeasurePointsNum = "Parameters/" + xmlNode + "/hv_MeasurePointsNum";
                string shv_MeasureMinScore = "Parameters/" + xmlNode + "/hv_MeasureMinScore";
                string shv_MeasureNumInstances = "Parameters/" + xmlNode + "/hv_MeasureNumInstances";
                string shv_MeasureDistThreshold = "Parameters/" + xmlNode + "/hv_MeasureDistThreshold";
                string shv_MaxNumIterations = "Parameters/" + xmlNode + "/hv_MaxNumIterations";
                string shv_MeasureIORegions = "Parameters/" + xmlNode + "/hv_MeasureIORegions";
                string shv_ImageProcessing = "Parameters/" + xmlNode + "/hv_ImageProcessing";

                string shv_MaskWidth = "Parameters/" + xmlNode + "/hv_MaskWidth";
                string shv_MaskHeight = "Parameters/" + xmlNode + "/hv_MaskHeight";
                string shv_MaskType = "Parameters/" + xmlNode + "/hv_MaskType";
                string shv_Radius = "Parameters/" + xmlNode + "/hv_Radius";
                string shv_MaskSize = "Parameters/" + xmlNode + "/hv_MaskSize";
                string shv_FilterMask = "Parameters/" + xmlNode + "/hv_FilterMask";
                string shv_MultImage_Mult = "Parameters/" + xmlNode + "/hv_MultImage_Mult";
                string shv_AddImage_Mult = "Parameters/" + xmlNode + "/hv_AddImage_Mult";
                string shv_ScaleImage_Add = "Parameters/" + xmlNode + "/hv_ScaleImage_Add";
                string shv_ScaleImage_Mult = "Parameters/" + xmlNode + "/hv_ScaleImage_Mult";

                hv_MeasureRow1 = Convert.ToDouble(xmlRW.Read(shv_MeasureRow1));
                hv_MeasureColumn1 = Convert.ToDouble(xmlRW.Read(shv_MeasureColumn1));
                hv_MeasureRow2 = Convert.ToDouble(xmlRW.Read(shv_MeasureRow2));
                hv_MeasureColumn2 = Convert.ToDouble(xmlRW.Read(shv_MeasureColumn2));

                hv_MeasureLength1 = Convert.ToDouble(xmlRW.Read(shv_MeasureLength1));
                hv_MeasureSigma = Convert.ToDouble(xmlRW.Read(shv_MeasureSigma));
                hv_MeasureThreshold = Convert.ToInt32(xmlRW.Read(shv_MeasureThreshold));
                hv_MeasureInterpolation = xmlRW.Read(shv_MeasureInterpolation);
                hv_MeasureTransition = xmlRW.Read(shv_MeasureTransition);
                hv_MeasureSelect = xmlRW.Read(shv_MeasureSelect);
                hv_MeasurePointsNum = Convert.ToInt32(xmlRW.Read(shv_MeasurePointsNum));
                hv_MeasureMinScore = Convert.ToDouble(xmlRW.Read(shv_MeasureMinScore));
                hv_MeasureNumInstances = Convert.ToInt32(xmlRW.Read(shv_MeasureNumInstances));
                hv_MeasureDistThreshold = Convert.ToInt32(xmlRW.Read(shv_MeasureDistThreshold));
                hv_MaxNumIterations = Convert.ToInt32(xmlRW.Read(shv_MaxNumIterations));
                hv_MeasureIORegions = xmlRW.Read(shv_MeasureIORegions);
                hv_ImageProcessing = xmlRW.Read(shv_ImageProcessing);

                hv_MaskWidth = Convert.ToInt32(xmlRW.Read(shv_MaskWidth));
                hv_MaskHeight = Convert.ToInt32(xmlRW.Read(shv_MaskHeight));
                hv_MaskType = xmlRW.Read(shv_MaskType);
                hv_Radius = Convert.ToInt32(xmlRW.Read(shv_Radius));
                hv_MaskSize = Convert.ToInt32(xmlRW.Read(shv_MaskSize));
                hv_FilterMask = xmlRW.Read(shv_FilterMask);
                hv_MultImage_Mult = Convert.ToDouble(xmlRW.Read(shv_MultImage_Mult));
                hv_AddImage_Mult = Convert.ToDouble(xmlRW.Read(shv_AddImage_Mult));
                hv_ScaleImage_Add = Convert.ToInt32(xmlRW.Read(shv_ScaleImage_Add));
                hv_ScaleImage_Mult = Convert.ToDouble(xmlRW.Read(shv_ScaleImage_Mult));

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
        public bool WriteParams(String xmlNode)
        {

            try
            {
                string shv_MeasureRow1 = "Parameters/" + xmlNode + "/hv_MeasureRow1";
                string shv_MeasureColumn1 = "Parameters/" + xmlNode + "/hv_MeasureColumn1";
                string shv_MeasureRow2 = "Parameters/" + xmlNode + "/hv_MeasureRow2";
                string shv_MeasureColumn2 = "Parameters/" + xmlNode + "/hv_MeasureColumn2";

                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_MeasureInterpolation = "Parameters/" + xmlNode + "/hv_MeasureInterpolation";
                string shv_MeasureTransition = "Parameters/" + xmlNode + "/hv_MeasureTransition";
                string shv_MeasureSelect = "Parameters/" + xmlNode + "/hv_MeasureSelect";
                string shv_MeasurePointsNum = "Parameters/" + xmlNode + "/hv_MeasurePointsNum";
                string shv_MeasureMinScore = "Parameters/" + xmlNode + "/hv_MeasureMinScore";
                string shv_MeasureNumInstances = "Parameters/" + xmlNode + "/hv_MeasureNumInstances";
                string shv_MeasureDistThreshold = "Parameters/" + xmlNode + "/hv_MeasureDistThreshold";
                string shv_MaxNumIterations = "Parameters/" + xmlNode + "/hv_MaxNumIterations";
                string shv_MeasureIORegions = "Parameters/" + xmlNode + "/hv_MeasureIORegions";
                string shv_ImageProcessing = "Parameters/" + xmlNode + "/hv_ImageProcessing";

                string shv_MaskWidth = "Parameters/" + xmlNode + "/hv_MaskWidth";
                string shv_MaskHeight = "Parameters/" + xmlNode + "/hv_MaskHeight";
                string shv_MaskType = "Parameters/" + xmlNode + "/hv_MaskType";
                string shv_Radius = "Parameters/" + xmlNode + "/hv_Radius";
                string shv_MaskSize = "Parameters/" + xmlNode + "/hv_MaskSize";
                string shv_FilterMask = "Parameters/" + xmlNode + "/hv_FilterMask";
                string shv_MultImage_Mult = "Parameters/" + xmlNode + "/hv_MultImage_Mult";
                string shv_AddImage_Mult = "Parameters/" + xmlNode + "/hv_AddImage_Mult";
                string shv_ScaleImage_Add = "Parameters/" + xmlNode + "/hv_ScaleImage_Add";
                string shv_ScaleImage_Mult = "Parameters/" + xmlNode + "/hv_ScaleImage_Mult";

                xmlRW.Update(shv_MeasureRow1, hv_MeasureRow1.ToString());
                xmlRW.Update(shv_MeasureColumn1, hv_MeasureColumn1.ToString());
                xmlRW.Update(shv_MeasureRow2, hv_MeasureRow2.ToString());
                xmlRW.Update(shv_MeasureColumn2, hv_MeasureColumn2.ToString());

                xmlRW.Update(shv_MeasureLength1, hv_MeasureLength1.ToString());
                xmlRW.Update(shv_MeasureSigma, hv_MeasureSigma.ToString());
                xmlRW.Update(shv_MeasureThreshold, hv_MeasureThreshold.ToString());
                xmlRW.Update(shv_MeasureInterpolation, hv_MeasureInterpolation);
                xmlRW.Update(shv_MeasureTransition, hv_MeasureTransition);
                xmlRW.Update(shv_MeasureSelect, hv_MeasureSelect);
                xmlRW.Update(shv_MeasurePointsNum, hv_MeasurePointsNum.ToString());
                xmlRW.Update(shv_MeasureMinScore, hv_MeasureMinScore.ToString());
                xmlRW.Update(shv_MeasureNumInstances, hv_MeasureNumInstances.ToString());
                xmlRW.Update(shv_MeasureDistThreshold, hv_MeasureDistThreshold.ToString());
                xmlRW.Update(shv_MaxNumIterations, hv_MaxNumIterations.ToString());
                xmlRW.Update(shv_MeasureIORegions, hv_MeasureIORegions);
                xmlRW.Update(shv_ImageProcessing, hv_ImageProcessing);

                xmlRW.Update(shv_MaskWidth, hv_MaskWidth.ToString());
                xmlRW.Update(shv_MaskHeight, hv_MaskHeight.ToString());
                xmlRW.Update(shv_MaskType, hv_MaskType);
                xmlRW.Update(shv_Radius, hv_Radius.ToString());
                xmlRW.Update(shv_MaskSize, hv_MaskSize.ToString());
                xmlRW.Update(shv_FilterMask, hv_FilterMask);
                xmlRW.Update(shv_MultImage_Mult, hv_MultImage_Mult.ToString());
                xmlRW.Update(shv_AddImage_Mult, hv_AddImage_Mult.ToString());
                xmlRW.Update(shv_ScaleImage_Add, hv_ScaleImage_Add.ToString());
                xmlRW.Update(shv_ScaleImage_Mult, hv_ScaleImage_Mult.ToString());

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        ///<summary>
        ///显示检测轮廓
        ///</summary>
        public void Show_Cross_and_Line(HTuple hv_ImageWindow1, HTuple hv_ZoomFactor)
        {
            HOperatorSet.SetDraw(hv_ImageWindow1, "fill");
            HTuple mhv_RowEdges = hv_CrossRow * hv_ZoomFactor;
            HTuple mhv_ColumnEdges = hv_CrossColumn * hv_ZoomFactor;

            HOperatorSet.GenCrossContourXld(out ho_Cross, mhv_RowEdges, mhv_ColumnEdges, 10,
                (new HTuple(45)).TupleRad());
            HOperatorSet.SetColor(hv_ImageWindow1, "green");
            HOperatorSet.SetLineWidth(hv_ImageWindow1, 2);
            HOperatorSet.DispObj(ho_Cross, hv_ImageWindow1);


            HTuple mhv_LineRowBegin = hv_LineRowBegin * hv_ZoomFactor;
            HTuple mhv_LineColumnBegin = hv_LineColumnBegin * hv_ZoomFactor;
            HTuple mhv_LineRowEnd = hv_LineRowEnd * hv_ZoomFactor;
            HTuple mhv_LineColumnEnd = hv_LineColumnEnd * hv_ZoomFactor;

            HOperatorSet.GenRegionLine(out ho_RegionLines, mhv_LineRowBegin, mhv_LineColumnBegin, mhv_LineRowEnd, mhv_LineColumnEnd);
            HOperatorSet.SetLineWidth(hv_ImageWindow1, 1);
            HOperatorSet.SetColor(hv_ImageWindow1, "red");
            HOperatorSet.DispObj(ho_RegionLines, hv_ImageWindow1);
            ho_RegionLines.Dispose();
            ho_Cross.Dispose();

            HOperatorSet.SetDraw(hv_ImageWindow1, "margin");
        }


        ///<summary>
        ///显示边缘梯度
        ///</summary>
        public void ShowMagnitude(HObject ho_Image, double Row, double Column, double phi, HTuple hv_MeasureInterpolation,
                                    HTuple length1, HTuple length2, HTuple Sigma, HTuple MeasureThreshold)
        {
            HObject imaAmp = new HObject();
            HObject imaDir = new HObject();

            HTuple hv_MeasureHandle = new HTuple();
            HTuple hv_GrayValues = new HTuple();
            HTuple hv_Function = new HTuple();
            HTuple hv_Width = new HTuple();
            HTuple hv_Height = new HTuple();
            int Threshold = MeasureThreshold;
            try
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.GenMeasureRectangle2(Row, Column, -phi - Math.PI / 2, length1, length2,
                                                  hv_Width, hv_Height, hv_MeasureInterpolation, out hv_MeasureHandle);
                HOperatorSet.EdgesImage(ho_Image, out imaAmp, out imaDir, "canny", Sigma, "nms", -1, -1);
                HOperatorSet.MeasureProjection(imaAmp, hv_MeasureHandle, out hv_GrayValues);
                HOperatorSet.CreateFunct1dArray(hv_GrayValues, out hv_Function);

                foreach (var series in MagnitudeChart.Series)
                {
                    series.Points.Clear();
                }

                for (int i = 0; i <= hv_Function.Length - 1; i++)
                {
                    MagnitudeChart.Series["边缘梯度"].Points.AddY(hv_Function[i]);
                    MagnitudeChart.Series["阈值"].Points.AddY(Threshold);
                }
                HOperatorSet.CloseMeasure(hv_MeasureHandle);
                imaAmp.Dispose();
                imaDir.Dispose();


            }
            catch (Exception)
            {

            }

        }


        #region 直线测量参数更改，直线重新拟合
        private void NUD_MeasureNumPionts_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasurePointsNum = Convert.ToInt32(NUD_MeasurePointsNum.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasureThreshold = Convert.ToInt32(NUD_MeasureThreshold.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MeasureSigma_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasureSigma = Convert.ToDouble(NUD_MeasureSigma.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void CB_MeasureTransition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_MeasureTransition.Text == "从明到暗")
                hv_MeasureTransition = "negative";
            else if (CB_MeasureTransition.Text == "从暗到明")
                hv_MeasureTransition = "positive";
            else if (CB_MeasureTransition.Text == "不区分明暗")
                hv_MeasureTransition = "all";

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void CB_MeasureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_MeasureSelect.Text == "第一条边")
                hv_MeasureSelect = "first";
            else if (CB_MeasureSelect.Text == "最后一条边")
                hv_MeasureSelect = "last";
            else if (CB_MeasureSelect.Text == "最强边")
                hv_MeasureSelect = "all";

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MeasureMinScore_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasureMinScore = Convert.ToDouble(NUD_MeasureMinScore.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MeasureNumInstances_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasureNumInstances = Convert.ToInt32(NUD_MeasureNumInstances.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MeasureDistThreshold_ValueChanged(object sender, EventArgs e)
        {
            hv_MeasureDistThreshold = Convert.ToInt32(NUD_MeasureDistThreshold.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void NUD_MaxNumIterations_ValueChanged(object sender, EventArgs e)
        {
            hv_MaxNumIterations = Convert.ToInt32(NUD_MaxNumIterations.Value);

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void CB_MeasureIORegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_MeasureIORegions.Text == "是")
                hv_MeasureIORegions = "true";
            else if (CB_MeasureIORegions.Text == "否")
                hv_MeasureIORegions = "false";

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void CB_MeasureInterpolatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_MeasureInterpolation.Text == "最近邻域法")
                hv_MeasureInterpolation = "nearest_neighbor";
            else if (CB_MeasureInterpolation.Text == "双线性插值法")
                hv_MeasureInterpolation = "bilinear";
            else if (CB_MeasureInterpolation.Text == "双三次插值法")
                hv_MeasureInterpolation = "bicubic";

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }

        private void CB_ImageProcessing_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (CB_ImageProcessing.Text == "无处理")
                hv_ImageProcessing = "None";
            else if (CB_ImageProcessing.Text == "锐化处理")
                hv_ImageProcessing = "SharpenImage";
            else if (CB_ImageProcessing.Text == "平滑处理")
                hv_ImageProcessing = "ScaleImage";

            if (ReviewLineEvent != null)
                ReviewLineEvent();
        }
        #endregion


        #region 图像处理参数更改，直线重新拟合
        private void NUD_MedianRadius_ValueChanged(object sender, EventArgs e)
        {
            hv_Radius = Convert.ToInt32(NUD_MedianRadius.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();

        }

        private void NUD_MeanWidth_ValueChanged(object sender, EventArgs e)
        {
            hv_MaskWidth = Convert.ToInt32(NUD_MeanWidth.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_MeanHeight_ValueChanged(object sender, EventArgs e)
        {
            hv_MaskHeight = Convert.ToInt32(NUD_MeanHeight.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_LaplaceSize_ValueChanged(object sender, EventArgs e)
        {
            hv_MaskSize = Convert.ToInt32(NUD_LaplaceSize.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_MultImage_Mult_ValueChanged(object sender, EventArgs e)
        {
            hv_MultImage_Mult = Convert.ToDouble(NUD_MultImage_Mult.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_AddImage_Mult_ValueChanged(object sender, EventArgs e)
        {
            hv_AddImage_Mult = Convert.ToDouble(NUD_AddImage_Mult.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_ScaleImage_Mult_ValueChanged(object sender, EventArgs e)
        {
            hv_ScaleImage_Mult = Convert.ToDouble(NUD_ScaleImage_Mult.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        private void NUD_ScaleImage_Add_ValueChanged(object sender, EventArgs e)
        {
            hv_ScaleImage_Add = Convert.ToInt32(NUD_ScaleImage_Add.Value);
            if (ReviewLineEvent != null && CB_ImageProcessing.Text == "锐化处理")
                ReviewLineEvent();
        }

        #endregion


        ///<summary>
        ///直线参数写入
        ///</summary>
        private void SaveParams_Click(object sender, EventArgs e)
        {
            if (!WriteParams(SaveLineDataName))
            {
                MessageBox.Show("保存参数失败！");
            }
            else if (SaveParamsEvent != null)
                SaveParamsEvent();
        }

    }

}
