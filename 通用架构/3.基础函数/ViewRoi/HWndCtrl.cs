using System;
using System.Collections;
using HalconDotNet;
using System.Windows.Forms;




namespace 通用架构._3.基础函数
{
    public delegate void IconicDelegate(int val);
    public delegate void FuncDelegate();



    /// <summary>
    /// 这个类是HALCON窗口HWindow的一个包装类。HWndCtrl负责可视化。
    ///你可以通过GUI组件输入或鼠标移动和缩放来改变图像。
    ///HWndCtrl类使用一个图形堆栈来管理显示的图标对象。
    ///每个对象都链接到一个graphical 环境，该上环境决定如何绘制对象。
    ///可以通过调用changeGraphicSettings()来更改环境。
    ///图形化的“模式”是由GraphicsContext类和HDevelop中提供的dev_set_*映射定义的。
	/// </summary>
	public class HWndCtrl
    {
        public delegate void ReDrawDelegate();//定义直线拟合委托                                
        public event ReDrawDelegate ReDrawEvent; //定义直线拟合事件

        public delegate void ImageProcessingDelegate(HObject ho_Image);//定义图像处理委托                                
        public event ImageProcessingDelegate ImageProcessingEvent; //定义图像处理事件


        /// <summary>鼠标事件不执行任何操作</summary>
        public const int MODE_VIEW_NONE = 10;

        /// <summary>鼠标事件执行缩放</summary>
        public const int MODE_VIEW_ZOOM = 11;

        /// <summary>鼠标事件执行移动</summary>
        public const int MODE_VIEW_MOVE = 12;

        /// <summary>鼠标事件执行放大</summary>
        public const int MODE_VIEW_ZOOMWINDOW = 13;


        public const int MODE_INCLUDE_ROI = 1;

        public const int MODE_EXCLUDE_ROI = 2;


        /// <summary>
        /// 描述图像更新的常量
        /// </summary>
        public const int EVENT_UPDATE_IMAGE = 31;
        /// <summary>
        /// 描述读取图像错误的常量
        /// </summary>
        public const int ERR_READING_IMG = 32;
        /// <summary> 
        /// 描述定义 graphical context错误的常量
        /// </summary>
        public const int ERR_DEFINING_GC = 33;

        /// <summary> 
        ///HALCON对象的最大数量，可以放在图形堆栈没有损失。
        ///每额外增加一个对象，第一个条目将从堆栈中删除。
        /// </summary>
        private const int MAXNUMOBJLIST = 1;


        private int stateView;
        public bool mousePressed = false;
        private double startX, startY;
        public System.Drawing.Rectangle rect;
        public bool resetflag = true;

        /// <summary>HALCON window</summary>
        private HWindowControl viewPort;

        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private ROIController roiManager;

        /* dispROI是一个flag，用来知道何时将ROI模型添加到绘制例程中，以及ROI对象是否响应的鼠标事件 */
        private int dispROI;


        /* 基础参数, 如 窗口尺寸和图像尺寸 */
        private int windowWidth;
        private int windowHeight;
        private int imageWidth;
        private int imageHeight;
        String str;
        private int[] CompRangeX;
        private int[] CompRangeY;

        double hv_StartX, hv_StartY, hv_EndX, hv_EndY;

        private int prevCompX, prevCompY;
        private double stepSizeX, stepSizeY;


        /* Image coordinates, which describe the image part that is displayed  
           in the HALCON window */
        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        /// <summary>Error message when an exception is thrown</summary>
        public string exceptionText = "";


        /* Delegates to send notification messages to other classes */
        /// <summary>
        /// Delegate to add information to the HALCON window after 
        /// the paint routine has finished
        /// </summary>
        public FuncDelegate addInfoDelegate;

        /// <summary>
        /// Delegate to notify about failed tasks of the HWndCtrl instance
        /// </summary>
        public IconicDelegate NotifyIconObserver;


        private HWindow ZoomWindow;
        public double zoomWndFactor;
        public double zoomfactor;
        private double zoomAddOn;
        private int zoomWndSize;


        /// <summary> 
        /// List of HALCON objects to be drawn into the HALCON window. 
        /// The list shouldn't contain more than MAXNUMOBJLIST objects, 
        /// otherwise the first entry is removed from the list.
        /// </summary>
        public HObject ho_Image = new HObject();
        public HObject ho_Image_Changed = new HObject();
        /// <summary>
        /// Instance that describes the graphical context for the
        /// HALCON window. According on the graphical settings
        /// attached to each HALCON object, this graphical context list 
        /// is updated constantly.
        /// </summary>
        private GraphicsContext mGC;


        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        public HWndCtrl(HWindowControl view)
        {
            viewPort = view;
            stateView = MODE_VIEW_NONE;
            windowWidth = viewPort.Size.Width;
            windowHeight = viewPort.Size.Height;

            zoomWndFactor = (double)imageWidth / viewPort.Width;
            zoomAddOn = Math.Pow(0.9, 5);
            zoomWndSize = 150;

            /*default*/
            CompRangeX = new int[] { 0, 100 };
            CompRangeY = new int[] { 0, 100 };

            prevCompX = prevCompY = 0;

            dispROI = MODE_INCLUDE_ROI;//1;

            viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
            viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);
            viewPort.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.mouseWheeled);


            addInfoDelegate = new FuncDelegate(dummyV);
            NotifyIconObserver = new IconicDelegate(dummy);

            // graphical stack 
            mGC = new GraphicsContext();
            mGC.gcNotification = new GCDelegate(exceptionGC);
        }


        /// <summary>
        /// Registers an instance of an ROIController with this window 
        /// controller (and vice versa).
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useROIController(ROIController rC)
        {
            roiManager = rC;
            rC.setViewController(this);
        }


        /// <summary>
        /// Read dimensions of the image to adjust own window settings
        /// </summary>
        /// <param name="image">HALCON image</param>
        private void setImagePart(HImage image)
        {
            string s;
            int w, h;

            image.GetImagePointer1(out s, out w, out h);
            setImagePart(0, 0, h, w);
        }


        /// <summary>
        /// Adjust window settings by the values supplied for the left 
        /// upper corner and the right lower corner
        /// </summary>
        /// <param name="r1">y coordinate of left upper corner</param>
        /// <param name="c1">x coordinate of left upper corner</param>
        /// <param name="r2">y coordinate of right lower corner</param>
        /// <param name="c2">x coordinate of right lower corner</param>
        private void setImagePart(int r1, int c1, int r2, int c2)
        {
            if (resetflag)
            {
                ImgRow1 = r1;
                ImgCol1 = c1;
                ImgRow2 = imageHeight = r2;
                ImgCol2 = imageWidth = c2;
                rect.X = (int)ImgCol1;
                rect.Y = (int)ImgRow1;
                rect.Height = (int)imageHeight;
                rect.Width = (int)imageWidth;
                resetflag = false;
            }

        }


        /// <summary>
        /// Sets the view mode for mouse events in the HALCON window
        /// (zoom, move, magnify or none).
        /// </summary>
        /// <param name="mode">One of the MODE_VIEW_* constants</param>
        public void setViewState(int mode)
        {
            stateView = mode;
            if (roiManager != null)
                roiManager.resetROI();
        }

        /********************************************************************/
        private void dummy(int val)
        {
        }

        private void dummyV()
        {
        }

        /*******************************************************************/
        private void exceptionGC(string message)
        {
            exceptionText = message;
            NotifyIconObserver(ERR_DEFINING_GC);
        }

        /// <summary>
        /// Paint or don't paint the ROIs into the HALCON window by 
        /// defining the parameter to be equal to 1 or not equal to 1.
        /// </summary>
        public void setDispLevel(int mode)
        {
            dispROI = mode;
        }
        /****************************************************************************/
        private void zoomImage(double x, double y, double scale)
        {
            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            if (viewPort.ImagePart.Width < 15 || viewPort.ImagePart.Height < 15)
            {
                if (scale <= 1)
                    return;
            }

            percentC = (x - ImgCol1) / viewPort.ImagePart.Width;
            percentR = (y - ImgRow1) / viewPort.ImagePart.Height;

            lengthC = viewPort.ImagePart.Width * scale;
            lengthR = viewPort.ImagePart.Height * scale;

            ImgCol1 = x - lengthC * percentC;
            ImgRow1 = y - lengthR * percentR;

            lenC = (int)Math.Round(lengthC);
            lenR = (int)Math.Round(lengthR);


            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (lenC > 0) ? lenC : 1;
            rect.Height = (lenR > 0) ? lenR : 1;

            zoomfactor = imageHeight / rect.Height;

            repaint();

        }

        /*******************************************************************/
        private void moveImage(double motionX, double motionY)
        {
            ImgRow1 += -motionY;
            ImgRow2 += -motionY;

            ImgCol1 += -motionX;
            ImgCol2 += -motionX;


            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            repaint();
        }


        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void resetAll()
        {
            resetflag = true;
            setImagePart((int)hv_StartX, (int)hv_StartY, (int)hv_EndX, (int)hv_EndY);

        }


        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseWheeled(object sender, HalconDotNet.HMouseEventArgs e)
        {

            double scale;
            if (e.Delta < 0)
                scale = 0.9;
            else if (e.Delta > 0)
                scale = 1 / 0.9;
            else
                scale = 1;

            zoomImage(e.X, e.Y, scale);
        }

        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {

            mousePressed = true;
            int activeROIidx = -1;
            double scale;

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
            {
                activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
            }

            if (activeROIidx == -1)
            {
                switch (stateView)
                {
                    case MODE_VIEW_MOVE:
                        startX = e.X;
                        startY = e.Y;
                        break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_ZOOMWINDOW:
                        activateZoomWindow((int)e.X, (int)e.Y);
                        break;
                    default:
                        break;
                }

            }
            //end of if
        }

        /*******************************************************************/
        private void activateZoomWindow(int X, int Y)
        {
            double posX, posY;
            int zoomZone;

            if (ZoomWindow != null)
                ZoomWindow.Dispose();

            HOperatorSet.SetSystem("border_width", 10);
            ZoomWindow = new HWindow();

            posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
            posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

            zoomZone = (int)((zoomWndSize / 2) * zoomWndFactor * zoomAddOn);
            ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
                                   zoomWndSize, zoomWndSize,
                                   viewPort.HalconID, "visible", "");
            ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
            repaint(ZoomWindow);
            ZoomWindow.SetColor("black");
        }

        /*******************************************************************/
        private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            mousePressed = false;

            if (roiManager != null
                && (roiManager.activeROIidx != -1)
                && (dispROI == MODE_INCLUDE_ROI))
            {
                roiManager.NotifyRCObserver(ROIController.EVENT_UPDATE_ROI);
                roiManager.activeROIidx = -1;
                repaint(viewPort.HalconWindow);
            }
            else if (stateView == MODE_VIEW_ZOOMWINDOW)
            {
                ZoomWindow.Dispose();
            }
        }

        /*******************************************************************/
        private void mouseMoved(object sender, HalconDotNet.HMouseEventArgs e)
        {
            double motionX, motionY;
            double posX, posY;
            double zoomZone;

            if (!mousePressed)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (roiManager != null && (roiManager.activeROIidx != -1) && (dispROI == MODE_INCLUDE_ROI))
                {
                    roiManager.mouseMoveAction(e.X, e.Y);
                }
                else if (stateView == MODE_VIEW_MOVE)
                {
                    motionX = ((e.X - startX));
                    motionY = ((e.Y - startY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = e.X - motionX;
                        startY = e.Y - motionY;
                    }
                }
                else if (stateView == MODE_VIEW_ZOOMWINDOW)
                {
                    HSystem.SetSystem("flush_graphic", "false");
                    ZoomWindow.ClearWindow();


                    posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
                    posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
                    zoomZone = (zoomWndSize / 2) * zoomWndFactor * zoomAddOn;

                    ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
                                                (int)posX - (zoomWndSize / 2),
                                                zoomWndSize, zoomWndSize);
                    ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
                                       (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
                    repaint(ZoomWindow);

                    HSystem.SetSystem("flush_graphic", "true");
                    ZoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
                }
            }
        }

        /// <summary>
        /// Triggers a repaint of the HALCON window
        /// </summary>
        public void repaint()
        {
            try
            {
                viewPort.ImagePart = rect;

                if (ImageProcessingEvent != null)
                    ImageProcessingEvent(ho_Image);

                repaint(viewPort.HalconWindow);
            }
            catch (Exception)
            { }

        }

        /// <summary>
        /// Repaints the HALCON window 'window'
        /// </summary>
        public void repaint(HalconDotNet.HWindow window)

        {
            HSystem.SetSystem("flush_graphic", "false");
            window.ClearWindow();


            window.DispObj(ho_Image_Changed);

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                roiManager.paintData(window);

            HSystem.SetSystem("flush_graphic", "true");

            window.SetColor("black");
            window.DispLine(-100.0, -100.0, -101.0, -101.0);

            if (ReDrawEvent != null)
                ReDrawEvent();
            mGC.stateOfSettings.Clear();
        }

        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/

        /// <summary>
        ///将一个图形对象添加到图形堆栈中，类似于为HDevelop图形堆栈定义它的方式。
        /// </summary>
        /// <param name="obj">Iconic object</param>
        public void addIconicVar(HObject obj)
        {
            HObjectEntry entry;

            if (obj == null)
                return;

            if (obj is HImage)
            {
                double win_Width, win_Height;
                string s;
                win_Width = viewPort.WindowSize.Width;
                win_Height = viewPort.WindowSize.Height;
                ((HImage)obj).GetImagePointer1(out s, out imageWidth, out imageHeight);

                double imgAspectRatio = imageWidth / imageHeight;
                double winAspectRatio = win_Width / win_Height;

                try
                {
                    hv_StartX = hv_StartY = hv_EndX = hv_EndY = 0;

                    if (imgAspectRatio > winAspectRatio)
                    {
                        //超宽图像
                        hv_StartY = 0;
                        zoomWndFactor = imageWidth / win_Width;
                        hv_StartX = -(win_Height - imageHeight / zoomWndFactor) / 2 * zoomWndFactor;
                    }
                    if (imgAspectRatio <= winAspectRatio)
                    {
                        //超高图像                
                        hv_StartX = 0;
                        zoomWndFactor = imageHeight / win_Height;
                        hv_StartY = -(win_Width - imageWidth / zoomWndFactor) / 2 * zoomWndFactor;

                    }
                    hv_EndX = imageHeight - hv_StartX * 2;
                    hv_EndY = imageWidth - hv_StartY * 2;
                    setImagePart((int)hv_StartX, (int)hv_StartY, (int)hv_EndX, (int)hv_EndY);

                }
                catch (Exception)
                {
                }

            }//if

            entry = new HObjectEntry(obj, mGC.copyContextList());

            ho_Image_Changed = ho_Image = entry.HObj;

        }
        public string Get_Mouseposition_and_Gray(HTuple hv_Window, HObject Image, HMouseEventArgs e)
        {
            HTuple ptX, ptY, hv_Button;
            HTuple row, col, grayval;
            HTuple Image_Width, Image_Height;
            HOperatorSet.GetImageSize(Image, out Image_Width, out Image_Height);
            HOperatorSet.GetMposition(hv_Window, out ptY, out ptX, out hv_Button);
            row = (ptY).TupleInt();
            col = (ptX).TupleInt();
            if (Image != null && (row >= 0 && row <= Image_Height) && (col >= 0 && col <= Image_Width))//设置3个条件项，防止程序崩溃。
            {
                HOperatorSet.GetGrayval(Image, row, col, out grayval);                 //获取当前点的灰度值
                str = String.Format("X:{0}  Y:{1}  Gray:{2}", col, row, grayval); //格式化字符串

            }
            else
            {
                str = "X: —  Y: —  Gray: — "; //格式化字符串
            }
            return str;
        }





    }//end of class
}//end of namespace
