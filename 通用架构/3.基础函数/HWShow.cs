using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;

namespace 通用架构._3.基础函数
{
    class HWShow
    {
        /// <summary>
        ///变量定义
        /// </summary>
        #region
        public HTuple hv_StartX, hv_EndX;
        public HTuple hv_StartY, hv_EndY;
        public HTuple hv_ZoomFactor;
        public HTuple hv_XldHomMat2D;
        #endregion

        /// <summary>
        ///在Hwindow中，显示图片
        /// </summary>
        public void ShowImage(HTuple hv_Window, HObject ho_Image, int win_Width, int win_Height)
        {
            //缩放显示图像，并且计算缩放比例和窗口起始位置
            HOperatorSet.ClearWindow(hv_Window);
            HTuple hv_Width = new HTuple();
            HTuple hv_Height = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            int imgWidth = int.Parse(hv_Width.ToString());
            int imgHeight = int.Parse(hv_Height.ToString());
            double imgAspectRatio = (double)imgWidth / (double)imgHeight;

            double winAspectRatio = (double)win_Width / (double)win_Height;

            HOperatorSet.SetSystem("int_zooming", "false");//图像缩放之前最好将此参数设置为false.

            HTuple hv_Para = new HTuple("constant");
            HObject ho_ZoomImage;
            HOperatorSet.GenEmptyObj(out ho_ZoomImage);
            //图片宽高大于于窗口的宽高
            if (win_Width < imgWidth && imgAspectRatio > winAspectRatio)
            {
                //超宽图像               
                HOperatorSet.ZoomImageSize(ho_Image, out ho_ZoomImage, win_Width, win_Width / imgAspectRatio, hv_Para);
                hv_StartX = 0 - (win_Height - win_Width / imgAspectRatio) / 2;
                hv_StartY = 0;
                hv_ZoomFactor = (double)win_Width / (double)imgWidth;
            }
            if (win_Height < imgHeight && imgAspectRatio < winAspectRatio)
            {
                //超高图像                
                HOperatorSet.ZoomImageSize(ho_Image, out ho_ZoomImage, win_Height * imgAspectRatio, win_Height, hv_Para);
                hv_StartX = 0;
                hv_StartY = 0 - (win_Width - win_Height * imgAspectRatio) / 2;
                hv_ZoomFactor = (double)win_Height / (double)imgHeight;
            }
            //图片宽高小于窗口的宽高
            if (win_Width > imgWidth && win_Height > imgHeight && imgAspectRatio > winAspectRatio)
            {
                //超宽图像               
                HOperatorSet.ZoomImageSize(ho_Image, out ho_ZoomImage, win_Width, win_Width / imgAspectRatio, hv_Para);
                hv_StartX = 0 - (win_Height - win_Width / imgAspectRatio) / 2;
                hv_StartY = 0;
                hv_ZoomFactor = (double)win_Width / (double)imgWidth;
            }
            if (win_Width > imgWidth && win_Height > imgHeight && imgAspectRatio < winAspectRatio)
            {
                //超高图像                
                HOperatorSet.ZoomImageSize(ho_Image, out ho_ZoomImage, win_Height * imgAspectRatio, win_Height, hv_Para);
                hv_StartX = 0;
                hv_StartY = 0 - (win_Width - win_Height * imgAspectRatio) / 2;
                hv_ZoomFactor = (double)win_Height / (double)imgHeight;
            }


            try
            {
                hv_EndX = win_Height - 1 + hv_StartX;
                hv_EndY = win_Width - 1 + hv_StartY;
                HOperatorSet.SetPart(hv_Window, hv_StartX, hv_StartY, hv_EndX, hv_EndY);//设置居中显示
                HOperatorSet.DispObj(ho_ZoomImage, hv_Window);

                HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);
                HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！" + exc.ToString());
            }
        }


        /// <summary>
        ///在Hwindow中，显示指针坐标值和灰度值
        /// </summary>
        public string Get_Mouseposition_and_Gray(HTuple hv_Window, HObject Image, HTuple Image_Width, HTuple Image_Height, HMouseEventArgs e)
        {

            String str;
            HTuple ptX, ptY, hv_Button;
            HTuple row, col, grayval;

            HOperatorSet.GetMposition(hv_Window, out ptY, out ptX, out hv_Button);
            row = (ptY / hv_ZoomFactor).TupleInt();
            col = (ptX / hv_ZoomFactor).TupleInt();
            if (Image != null && (row > 0 && row < Image_Height) && (col > 0 && col < Image_Width))//设置3个条件项，防止程序崩溃。
            {
                HOperatorSet.GetGrayval(Image, row, col, out grayval);                 //获取当前点的灰度值
                str = String.Format("X:{0}  Y:{1}  Gray:{2}", col, row, grayval); //格式化字符串

            }
            else
            {
                str ="X:     Y:     Gray:   "; //格式化字符串
            }
            return str;
        }

    }
}
