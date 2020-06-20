using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using HalconDotNet;
using 通用架构._4.算子库;
using 通用架构._3.基础函数;


namespace 通用架构._2.子界面
{
    public partial class ChangeParaForm : Form
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


        public ChangeParaForm()
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
            //实例化：检测画面子菜单
            LineDetect = new LineDetect();
            LineDetect.TopLevel = false;
            LineDetect.Dock = DockStyle.Fill;
            LineDetect.FormBorderStyle = FormBorderStyle.None;
            LineDetect.Size = TLPCheckParam.Size;
            ROIRectangle2 = new ROIRectangle2();
            ho_Image = null;
            hv_ImageWindow = HWControl1.HalconID;

            InspectionStandard = new InspectionStandard();
            InspectionStandard.TopLevel = false;
            InspectionStandard.Dock = DockStyle.Fill;
            InspectionStandard.FormBorderStyle = FormBorderStyle.None;
            InspectionStandard.Size = TLPCheckParam.Size;
            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘
            TLPCheckParam.Controls.Clear();
            TLPCheckParam.Controls.Add(InspectionStandard);
            InspectionStandard.Show();
            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();//刷新控件
            //

            //画面缩放
            roiController = new ROIController();
            HWndCtrl = new HWndCtrl(HWControl1);
            HWndCtrl.useROIController(roiController);
            HWndCtrl.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
            HWndCtrl.ReDrawEvent += new HWndCtrl.ReDrawDelegate(ReDraw);//更改测量矩形，重绘直线
            LineDetect.ReviewLineEvent += new LineDetect.ReviewLineDelegate(ReviewLine);//参数更改，重绘直线
            HWndCtrl.ImageProcessingEvent += HWndCtrl_ImageProcessingEvent;

            if (ho_Image != null)
            {
                HWndCtrl.addIconicVar(ho_Image);
                HWndCtrl.repaint();
            }
            //

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


        public void HWndCtrl_ImageProcessingEvent(HObject ho_Image)
        {
            if (LineDetect.hv_ImageProcessing != null)
            {
                if (LineDetect.hv_ImageProcessing == "SharpenImage")
                {
                    LineDetect.ProcessImage(ho_Image, ROIRectangle2.midR, ROIRectangle2.midC, -ROIRectangle2.phi, ROIRectangle2.length1, ROIRectangle2.length2, out HObject ho_Image1);
                    if (LineDetect.ProcessImageflag)
                    {
                        LineDetect.ho_Image = HWndCtrl.ho_Image_Changed = ho_Image1;
                    }
                }
                else
                {
                    LineDetect.ho_Image = HWndCtrl.ho_Image_Changed = ho_Image;
                }
            }
            else
            {
                LineDetect.ho_Image = HWndCtrl.ho_Image_Changed = ho_Image;
            }
        }



        private void ReDraw()
        {

            if (LineDetect.ho_Image != null)
            {
                try
                {
                    ROIRectangle2.PointNum = LineDetect.hv_MeasurePointsNum;
                    LineDetect.hv_MeasureRow1 = ROIRectangle2.midR - ROIRectangle2.length1 * Math.Sin(ROIRectangle2.phi);
                    LineDetect.hv_MeasureColumn1 = ROIRectangle2.midC - ROIRectangle2.length1 * Math.Cos(ROIRectangle2.phi);
                    LineDetect.hv_MeasureRow2 = ROIRectangle2.midR + ROIRectangle2.length1 * Math.Sin(ROIRectangle2.phi);
                    LineDetect.hv_MeasureColumn2 = ROIRectangle2.midC + ROIRectangle2.length1 * Math.Cos(ROIRectangle2.phi);
                    LineDetect.hv_MeasureLength1 = ROIRectangle2.length2;
                    LineDetect.hv_MeasureLength2 = ROIRectangle2.length1 / LineDetect.hv_MeasurePointsNum;
                    LineDetect.ShowMagnitude(LineDetect.ho_Image, ROIRectangle2.midR, ROIRectangle2.midC, ROIRectangle2.phi, LineDetect.hv_MeasureInterpolation,
                                             ROIRectangle2.length2, LineDetect.hv_MeasureLength2, LineDetect.hv_MeasureSigma, LineDetect.hv_MeasureThreshold);
                    //直线拟合
                    LineDetect.DetectLine();
                    LineDetect.Show_Cross_and_Line(HWControl1.HalconID, 1);

                }
                catch (Exception)
                { }
            }
        }


        private void ReviewLine()
        {
            try
            {
                HWndCtrl.repaint();
            }
            catch (Exception)
            { }

        }


        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {

                //可执行某无聊的操作
            }
        }


        private void HWControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (ho_Image != null)
                try
                {

                    TB_PositionAndGray.Text = HWndCtrl.Get_Mouseposition_and_Gray(hv_ImageWindow, HWndCtrl.ho_Image_Changed, e);
                }
                catch (Exception)
                {

                }
        }


        private void Snap_Click(object sender, EventArgs e)
        {
            this.Snap.BackgroundImage = global::通用架构.Properties.Resources.PicCam_Gray;
            Task Snap_Active = new Task(() =>
            {
                Delay(150);
                this.Snap.BackgroundImage = global::通用架构.Properties.Resources.PicCam_White;
            });
            Snap_Active.Start();

        }


        private void LoadImage_Click(object sender, EventArgs e)
        {   //按钮点击效果
            this.LoadImage.BackgroundImage = global::通用架构.Properties.Resources.PicImage_Gray;
            Task LoadImage_Active = new Task(() =>
            {
                Delay(150);
                this.LoadImage.BackgroundImage = global::通用架构.Properties.Resources.PicImage_White;
            });
            LoadImage_Active.Start();
            //

            LoadImage.Refresh();//刷新控件

            //打开指定路径文件夹，加载图片
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.InitialDirectory = "F:\\1.工作文件\\1.程序文件";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ho_Image = new HImage(FileDialog.FileName);
                    HOperatorSet.GetImageSize(ho_Image, out ImageWidth, out ImageHeight);
                    HWndCtrl.addIconicVar(ho_Image);
                    LineDetect.ho_Image = ho_Image;
                    HWndCtrl.repaint();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
            //
        }


        private void SaveAs_Click(object sender, EventArgs e)
        {

            //按钮点击效果
            this.SaveAs.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_Gray;
            Task SaveAs_Active = new Task(() =>
            {
                Delay(150);
                this.SaveAs.BackgroundImage = global::通用架构.Properties.Resources.PicSaveAs_White;
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
                    HOperatorSet.WriteImage(HWndCtrl.ho_Image, "bmp", 0, filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("图像保存失败！");
                }
            }
        }

        private void Button_Standard_Click(object sender, EventArgs e)
        {
            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘
            TLPCheckParam.Controls.Clear();
            TLPCheckParam.Controls.Add(InspectionStandard);
            InspectionStandard.Show();
            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();//刷新控件

            //按钮点击效果
            this.Button_Standard.BackgroundImage = global::通用架构.Properties.Resources.PicChangeParam_Gray;
            Task Button_Standards_Active = new Task(() =>
            {
                Delay(100);
                this.Button_Standard.BackgroundImage = global::通用架构.Properties.Resources.PicChangeParam_White;
            });
            Button_Standards_Active.Start();
            //
        }


        private void ImageReset_Click(object sender, EventArgs e)
        {
            //按钮点击效果
            this.ImageReset.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_Gray;
            Task ImageReset_Active = new Task(() =>
            {
                Delay(150);
                this.ImageReset.BackgroundImage = global::通用架构.Properties.Resources.PicImageReset_White;
            });
            ImageReset_Active.Start();
            //
            //重置显示画面
            HWndCtrl.resetAll();
            HWndCtrl.repaint();
            //
        }


        private void LoadingParams(string CameraLabelText,string LineDetectName,string filePath)
        {

            CameraLabel.Text = CameraLabelText;
            LineDetect.SaveLineDataName = LineDetectName;

            ho_Image = new HImage(filePath);

            LineDetect.ho_Image = ho_Image;

            if (!LineDetect.ReadParams(LineDetect.SaveLineDataName))
            {
                MessageBox.Show("读取默认参数失败");
            }
            

            //生成ROI
            roiController.ROIList.Clear();
            roiController.roiMode = ROIRectangle2;
            ROIRectangle2.midR = (LineDetect.hv_MeasureRow1 + LineDetect.hv_MeasureRow2) / 2;
            ROIRectangle2.midC = (LineDetect.hv_MeasureColumn1 + LineDetect.hv_MeasureColumn2) / 2;
            HOperatorSet.AngleLx(LineDetect.hv_MeasureRow1, LineDetect.hv_MeasureColumn1, LineDetect.hv_MeasureRow2, LineDetect.hv_MeasureColumn2, out HTuple AngleL);
            ROIRectangle2.phi = -Convert.ToDouble(AngleL.ToString());
            HOperatorSet.DistancePp(LineDetect.hv_MeasureRow1, LineDetect.hv_MeasureColumn1, LineDetect.hv_MeasureRow2, LineDetect.hv_MeasureColumn2, out HTuple LengthL);
            ROIRectangle2.length1 = Convert.ToDouble(LengthL.ToString())/2;
            ROIRectangle2.length2 = LineDetect.hv_MeasureLength1;


            roiController.roiMode.createROI(ROIRectangle2.midC, ROIRectangle2.midR);
            LineDetect.RefreshParams();
            ReDraw();

            roiController.ROIList.Add(roiController.roiMode);
            roiController.activeROIidx = -1;

            HWndCtrl.addIconicVar(ho_Image);
            HWndCtrl.resetAll();
            HWndCtrl.repaint();

            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘
            TLPCheckParam.Controls.Clear();
            TLPCheckParam.Controls.Add(LineDetect);
            LineDetect.Show();
            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();//刷新控件

        }


        private void Button_InC_Click(object sender, EventArgs e)
        {
            LoadingParams("内侧相机：正极料线", "LineParams_InC", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_1.bmp");
        }


        private void Button_InA_Click(object sender, EventArgs e)
        {
            LoadingParams("内侧相机：负极料线", "LineParams_InA", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_2.bmp");
        }


        private void Button_InUpS_Click(object sender, EventArgs e)
        {
            LoadingParams("内侧相机：上隔膜料线", "LineParams_InUpS", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_1.bmp");
        }


        private void Button_InDownS_Click(object sender, EventArgs e)
        {
            LoadingParams("内侧相机：下隔膜料线", "LineParams_InDownS", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle1_2.bmp");
        }



        private void Button_OutC_Click(object sender, EventArgs e)
        {
            LoadingParams("外侧相机：正极料线", "LineParams_OutC", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_1.bmp");
        }


        private void Button_OutA_Click(object sender, EventArgs e)
        {
            LoadingParams("外侧相机：负极料线", "LineParams_OutA", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_2.bmp");
        }


        private void Button_OutUpS_Click(object sender, EventArgs e)
        {
            LoadingParams("外侧相机：上隔膜料线", "LineParams_OutUpS", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_1.bmp");
        }


        private void Button_OutDownS_Click(object sender, EventArgs e)
        {
            LoadingParams("外侧相机：下隔膜料线", "LineParams_OutDownS", AppDomain.CurrentDomain.BaseDirectory + "Parameters\\Modle2_2.bmp");
        }

    }
}
