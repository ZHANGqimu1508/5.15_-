using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace 通用架构._3.基础函数
{

    ///<summary>
    ///四相机卷绕_检测参数
    ///</summary>
    public  struct DetectParam
    {
        //<相机参数>
        public string String_CameraName;
        public string String_CameraParam;
        public CameraBasler CameraBasler;
        public double Double_K;
        public double Double_B;


        //<测量结果标志>
        public bool Bool_DetectResult;
        public List<bool> List_DetectResult;


        //<直线拟合>Task线程任务
        Task Task_FittingLine_A;
        Task Task_FittingLine_C;
        Task Task_FittingLine_UpS;
        Task Task_FittingLine_DownS;


        //<横纵基准线>HTuple
        public HTuple HTuple_DatumR_RowBegin;
        public HTuple HTuple_DatumR_ColumnBegin;
        public HTuple HTuple_DatumR_RowEnd;
        public HTuple HTuple_DatumR_ColumnEnd;

        public HTuple HTuple_DatumC_RowBegin;
        public HTuple HTuple_DatumC_ColumnBegin;
        public HTuple HTuple_DatumC_RowEnd;
        public HTuple HTuple_DatumC_ColumnEnd;


        //<距离上下限>实数
        public double Double_ASmin;
        public double Double_ASmax;
        public double Double_CSmin;
        public double Double_CSmax;

        public double Double_ACmin;
        public double Double_ACmax;
        public double Double_SSmin;
        public double Double_SSmax;


        //<线线距离>实数
        public double Double_AS;
        public double Double_CS;
        public double Double_AC;
        public double Double_SS;

        public double Double_AD;
        public double Double_CD;
        public double Double_UpSD;
        public double Double_DownSD;


        //<线线距离>队列
        public List<double> List_AS;
        public List<double> List_CS;
        public List<double> List_AC;
        public List<double> List_SS;

        public List<double> List_AD;
        public List<double> List_CD;
        public List<double> List_UpSD;
        public List<double> List_DownSD;

        //<距离上下限>实数
        public double Double_Anglemin;
        public double Double_Anglemax;

        //<直线角度>实数 
        public double Double_AngleAD;
        public double Double_AngleCD;
        public double Double_AngleUpSD;
        public double Double_AngleDownSD;

        //<直线角度>队列 
        public List<double> List_AngleAD;
        public List<double> List_AngleCD;
        public List<double> List_AngleUpSD;
        public List<double> List_AngleDownSD;


        //<测量图像>
        public HObject HObject_Image;          //原图
        public HTuple HTuple_RotateAngle;      //图像旋转角度
        public HTuple HTuple_CropRow;          //图像裁切起始点坐标（左上角）
        public HTuple HTuple_CropColumn;
        public HTuple HTuple_Width;            //图像裁切宽高
        public HTuple HTuple_Height;

        public object Locker;                  //图像队列互斥锁
        public List<double> List_ImageDate;    //原图队列时间标签
        public List<HObject> List_Image;       //原图队列
        public List<HObject> List_StitchImage; //拼接图队列

    }

}
