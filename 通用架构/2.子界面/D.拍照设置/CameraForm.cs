using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using HalconDotNet;
using 通用架构._4.算子库;
using 通用架构._3.基础函数;


namespace 通用架构._2.子界面
{
    public partial class CameraForm : Form
    {
        /// <summary>
        ///变量定义
        /// </summary>
        #region
        public LineDetect LineDetect;        //内侧相机直线检测画面

        public InspectionStandard InspectionStandard;  //检测标准画面

        public HWndCtrl HWndCtrl;
        public ROIController roiController;
        public HTuple hv_ImageWindow;
        HObject ho_Image;
        HTuple ImageWidth, ImageHeight;
        public ROIRectangle2 ROIRectangle2;
        #endregion


        public CameraForm()
        {
            InitializeComponent();
            Initialization();
            //this.tabControl1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance 
            //                    | System.Reflection.BindingFlags.NonPublic).SetValue(tabControl1, true, null);

            //this.tableLayoutPanel2.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            //        | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel2, true, null);

            //this.tableLayoutPanel3.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            //        | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel3, true, null);

            //this.TLPanel_Bottom.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            //        | System.Reflection.BindingFlags.NonPublic).SetValue(TLPanel_Bottom, true, null);

        }


        ///<summary>
        ///程序初始化
        ///</summary>
        public void Initialization()
        {

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


        #region 窗体控件重绘，防止闪烁
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);


        private const int WM_SETREDRAW = 0xB;
        #endregion 




    }
}
