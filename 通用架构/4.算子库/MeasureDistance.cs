using System;
using HalconDotNet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 通用架构._2.子界面;

namespace 通用架构._4.算子库
{
    class MeasureDistance
    {
        /// <summary>
        /// 显示检测直线
        /// </summary>
        /// <param name="hv_ImageWindow"></param>
        /// <param name="hv_ZoomFactor"></param>
        /// <param name="LineDetect"></param>
        /// <param name="color"></param>
        public static void ShowLine(HTuple hv_ImageWindow, HTuple hv_ZoomFactor, LineDetect LineDetect, HTuple color)
        {
            HTuple hv_LineRowBegin = new HTuple();
            HTuple hv_LineColumnBegin = new HTuple();
            HTuple hv_LineRowEnd = new HTuple();
            HTuple hv_LineColumnEnd = new HTuple();
            HObject ho_MeasuredLine = new HObject();

            HTuple hv_PointRow = new HTuple();
            HTuple hv_PointColumn = new HTuple();

            hv_LineRowBegin = LineDetect.hv_LineRowBegin * hv_ZoomFactor;
            hv_LineColumnBegin = LineDetect.hv_LineColumnBegin * hv_ZoomFactor;
            hv_LineRowEnd = LineDetect.hv_LineRowEnd * hv_ZoomFactor;
            hv_LineColumnEnd = LineDetect.hv_LineColumnEnd * hv_ZoomFactor;

            try
            {
                HOperatorSet.GenRegionLine(out ho_MeasuredLine, hv_LineRowBegin, hv_LineColumnBegin, hv_LineRowEnd, hv_LineColumnEnd);
                HOperatorSet.SetLineWidth(hv_ImageWindow, 3);
                HOperatorSet.SetColor(hv_ImageWindow, color);
                HOperatorSet.DispObj(ho_MeasuredLine, hv_ImageWindow);
            }
            catch (Exception)
            { }
            HOperatorSet.SetLineWidth(hv_ImageWindow, 1);
            ho_MeasuredLine.Dispose();
        }



        public static void ShowMeasureRectangle(HTuple hv_ImageWindow, HTuple hv_ZoomFactor, LineDetect LineDetect, HTuple color)
        {
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple();
            HTuple hv_MeasureLength1 = new HTuple();

            hv_Row1 = LineDetect.hv_MeasureRow1 * hv_ZoomFactor;
            hv_Column1 = LineDetect.hv_MeasureColumn1 * hv_ZoomFactor;
            hv_Row2 = LineDetect.hv_MeasureRow2 * hv_ZoomFactor;
            hv_Column2 = LineDetect.hv_MeasureColumn2 * hv_ZoomFactor;
            hv_MeasureLength1 = LineDetect.hv_MeasureLength1 * hv_ZoomFactor;
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out HTuple hv_Distance);
            HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out HTuple hv_Angle);

            HOperatorSet.GenRectangle2(out HObject ho_Rectangle, (hv_Row1 + hv_Row2) / 2, (hv_Column1 + hv_Column2) / 2, hv_Angle, hv_Distance / 2, hv_MeasureLength1);
            HOperatorSet.SetColor(hv_ImageWindow, color);
            HOperatorSet.SetDraw(hv_ImageWindow, "margin");
            HOperatorSet.SetLineWidth(hv_ImageWindow, 2);
            HOperatorSet.DispObj(ho_Rectangle, hv_ImageWindow);
            HOperatorSet.SetLineWidth(hv_ImageWindow, 1);
            ho_Rectangle.Dispose();
        }


        /// <summary>
        /// 在Hwindow中写文字
        /// </summary>
        /// <param name="hv_ImageWindow"></param>
        /// <param name="WriteString"></param>
        /// <param name="FontSize"></param>
        /// <param name="FontColor"></param>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        public static void WriteStringInHwindown(HTuple hv_ImageWindow, HTuple WriteString, HTuple FontSize, HTuple FontColor, HTuple Row, HTuple Column)
        {
            set_display_font(hv_ImageWindow, FontSize, "mono", "true", "false");
            HOperatorSet.SetColor(hv_ImageWindow, FontColor);
            HOperatorSet.SetTposition(hv_ImageWindow, Row, Column);
            HOperatorSet.WriteString(hv_ImageWindow, WriteString);
        }


        /// <summary>
        /// 线线夹角
        /// </summary>
        /// <param name="hv_Line1Row1"></param>
        /// <param name="hv_Line1Column1"></param>
        /// <param name="hv_Line1Row2"></param>
        /// <param name="hv_Line1Column2"></param>
        /// <param name="hv_Line2Row1"></param>
        /// <param name="hv_Line2Column1"></param>
        /// <param name="hv_Line2Row2"></param>
        /// <param name="hv_Line2Column2"></param>
        /// <returns></returns>
        public static double L2LAngle(HTuple hv_Line1Row1, HTuple hv_Line1Column1, HTuple hv_Line1Row2, HTuple hv_Line1Column2,
                 HTuple hv_Line2Row1, HTuple hv_Line2Column1, HTuple hv_Line2Row2, HTuple hv_Line2Column2)
        {
            try
            {
                double Angle_Result;
                HOperatorSet.AngleLl(hv_Line1Row1, hv_Line1Column1, hv_Line1Row2, hv_Line1Column2, hv_Line2Row1, hv_Line2Column1, hv_Line2Row2, hv_Line2Column2, out HTuple angle);
                Angle_Result = angle;
                Angle_Result = Math.Abs(Angle_Result) / Math.PI * 180;

                if (Angle_Result > 90)
                    Angle_Result = 180 - Angle_Result;
                return Angle_Result;
            }
            catch (Exception)
            { return 100; }
        }


        /// <summary>
        /// 线线距离
        /// </summary>
        /// <param name="hv_Line1Row1"></param>
        /// <param name="hv_Line1Column1"></param>
        /// <param name="hv_Line1Row2"></param>
        /// <param name="hv_Line1Column2"></param>
        /// <param name="hv_Line2Row1"></param>
        /// <param name="hv_Line2Column1"></param>
        /// <param name="hv_Line2Row2"></param>
        /// <param name="hv_Line2Column2"></param>
        /// <returns></returns>
        public static double L2LDistance(HTuple hv_Line1Row1, HTuple hv_Line1Column1, HTuple hv_Line1Row2, HTuple hv_Line1Column2,
                 HTuple hv_Line2Row1, HTuple hv_Line2Column1, HTuple hv_Line2Row2, HTuple hv_Line2Column2)
        {
            try
            {
                HTuple hv_DistanceMin1;
                HTuple hv_DistanceMax1;
                HTuple hv_DistanceMin2;
                HTuple hv_DistanceMax2;

                HOperatorSet.DistanceSl(hv_Line1Row1, hv_Line1Column1, hv_Line1Row2, hv_Line1Column2,
                            hv_Line2Row1, hv_Line2Column1, hv_Line2Row2, hv_Line2Column2,
                            out hv_DistanceMin1, out hv_DistanceMax1);
                HOperatorSet.DistanceSl(hv_Line2Row1, hv_Line2Column1, hv_Line2Row2, hv_Line2Column2,
                           hv_Line1Row1, hv_Line1Column1, hv_Line1Row2, hv_Line1Column2,
                           out hv_DistanceMin2, out hv_DistanceMax2);

                return ((hv_DistanceMin1 + hv_DistanceMax1 + hv_DistanceMin2 + hv_DistanceMax2) / 4);
            }
            catch (Exception)
            {
                return 100;
            }
        }


        /// <summary>
        /// 十字准心线
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="hv_ImageWindow"></param>
        /// <param name="hv_Length"></param>
        /// <param name="hv_DisX"></param>
        /// <param name="hv_DisY"></param>
        /// <param name="hv_Color"></param>
        public static void ShowCoordinate(HObject ho_Image, HTuple hv_ImageWindow, HTuple hv_Length, HTuple hv_DisX, HTuple hv_DisY, HTuple hv_Color)
        {
            HTuple hv_Width = null;
            HTuple hv_Height = null;
            HTuple hv_OriginX = null;
            HTuple hv_OriginY = null;
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

            HOperatorSet.TupleRound(hv_Height / 2, out hv_OriginX);
            HOperatorSet.TupleRound(hv_Width / 2, out hv_OriginY);

            HOperatorSet.GenRegionLine(out HObject regionLines_Row, hv_OriginX, 0, hv_OriginX, hv_Width);
            HOperatorSet.GenRegionLine(out HObject regionLines_Column, 0, hv_OriginY, hv_Height, hv_OriginY);

            HTuple hv_AxesX = new HTuple();
            HTuple hv_AxesY = new HTuple();

            //行方向右左
            for (int i = 1; i * hv_DisY + hv_OriginY < hv_Width; i++)
            {
                HTuple hv_AxesX_R = new HTuple();
                HTuple hv_AxesY_R = new HTuple();

                hv_AxesX_R = hv_OriginX;
                hv_AxesY_R = i * hv_DisY + hv_OriginY;

                HOperatorSet.TupleInsert(hv_AxesX, hv_AxesX.Length, hv_AxesX_R, out hv_AxesX);
                HOperatorSet.TupleInsert(hv_AxesY, hv_AxesY.Length, hv_AxesY_R, out hv_AxesY);
            }

            for (int i = 1; hv_OriginY - i * hv_DisY > 0; i++)
            {
                HTuple hv_AxesX_L = new HTuple();
                HTuple hv_AxesY_L = new HTuple();

                hv_AxesX_L = hv_OriginX;
                hv_AxesY_L = hv_OriginY - i * hv_DisY;

                HOperatorSet.TupleInsert(hv_AxesX, hv_AxesX.Length, hv_AxesX_L, out hv_AxesX);
                HOperatorSet.TupleInsert(hv_AxesY, hv_AxesY.Length, hv_AxesY_L, out hv_AxesY);
            }
            //列方向下上
            for (int i = 1; i * hv_DisX + hv_OriginX < hv_Height; i++)
            {
                HTuple hv_AxesX_D = new HTuple();
                HTuple hv_AxesY_D = new HTuple();

                hv_AxesX_D = i * hv_DisX + hv_OriginX;
                hv_AxesY_D = hv_OriginY;

                HOperatorSet.TupleInsert(hv_AxesX, hv_AxesX.Length, hv_AxesX_D, out hv_AxesX);
                HOperatorSet.TupleInsert(hv_AxesY, hv_AxesY.Length, hv_AxesY_D, out hv_AxesY);
            }
            for (int i = 1; hv_OriginX - i * hv_DisX > 0; i++)
            {
                HTuple hv_AxesX_U = new HTuple();
                HTuple hv_AxesY_U = new HTuple();

                hv_AxesX_U = hv_OriginX - i * hv_DisX;
                hv_AxesY_U = hv_OriginY;

                HOperatorSet.TupleInsert(hv_AxesX, hv_AxesX.Length, hv_AxesX_U, out hv_AxesX);
                HOperatorSet.TupleInsert(hv_AxesY, hv_AxesY.Length, hv_AxesY_U, out hv_AxesY);
            }

            HObject ho_MeasuredCross = null;
            HOperatorSet.GenCrossContourXld(out ho_MeasuredCross, hv_AxesX, hv_AxesY, hv_Length, 0);//先生成一个大的十字

            HOperatorSet.SetColor(hv_ImageWindow, hv_Color);
            HOperatorSet.DispObj(regionLines_Row, hv_ImageWindow);
            HOperatorSet.DispObj(regionLines_Column, hv_ImageWindow);
            HOperatorSet.DispObj(ho_MeasuredCross, hv_ImageWindow);
            regionLines_Row.Dispose();
            regionLines_Column.Dispose();
            ho_MeasuredCross.Dispose();

        }


        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_Size"></param>
        /// <param name="hv_Font"></param>
        /// <param name="hv_Bold"></param>
        /// <param name="hv_Slant"></param>
        public static void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Restore previous behaviour
                hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
            }
            else
            {
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP.TupleInt();
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Courier";
                hv_Fonts[1] = "Courier 10 Pitch";
                hv_Fonts[2] = "Courier New";
                hv_Fonts[3] = "CourierNew";
                hv_Fonts[4] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Consolas";
                hv_Fonts[1] = "Menlo";
                hv_Fonts[2] = "Courier";
                hv_Fonts[3] = "Courier 10 Pitch";
                hv_Fonts[4] = "FreeMono";
                hv_Fonts[5] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Luxi Sans";
                hv_Fonts[1] = "DejaVu Sans";
                hv_Fonts[2] = "FreeSans";
                hv_Fonts[3] = "Arial";
                hv_Fonts[4] = "Liberation Sans";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Times New Roman";
                hv_Fonts[1] = "Luxi Serif";
                hv_Fonts[2] = "DejaVu Serif";
                hv_Fonts[3] = "FreeSerif";
                hv_Fonts[4] = "Utopia";
                hv_Fonts[5] = "Liberation Serif";
            }
            else
            {
                hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
            }
            hv_Style = "";
            if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Bold";
            }
            else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Bold";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Italic";
            }
            else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Slant";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
            {
                hv_Style = "Normal";
            }
            HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
            hv_Font_COPY_INP_TMP = "";
            for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
            {
                hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                        break;
                    }
                }
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                throw new HalconException("Wrong value of control parameter Font");
            }
            hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
            HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }


        /// <summary>
        /// Halcon窗口显示文字
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_String"></param>
        /// <param name="hv_CoordSystem"></param>
        /// <param name="hv_Row"></param>
        /// <param name="hv_Column"></param>
        /// <param name="hv_Color"></param>
        /// <param name="hv_Box"></param>
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
              HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            // Local control variables 

            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_UseShadow = null;
            HTuple hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow (same as if no second value is given)
            //       otherwise -> use given string as color string for the shadow color
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //Transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //Display text box depending on text size
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                    hv_Box_COPY_INP_TMP = new HTuple();
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                {
                    //Use default ShadowColor set above
                }
                else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                    "false"))) != 0)
                {
                    hv_UseShadow = 0;
                }
                else
                {
                    hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            1));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                }
            }
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
            {
                //Valid color?
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                //Calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //Display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                //Set shadow color
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if ((int)(hv_UseShadow) != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1, hv_C2 + 1);
                }
                //Set box color
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }


        /// <summary>
        /// 轮廓点拟合直线
        /// </summary>
        /// <param name="ho_Line"></param>
        /// <param name="hv_Rows"></param>
        /// <param name="hv_Cols"></param>
        /// <param name="hv_ActiveNum"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        public static void pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
                 HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2,
                 out HTuple hv_Column2)
        {



            // Local iconic variables 

            HObject ho_Contour = null;

            // Local control variables 

            HTuple hv_Length = null, hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Length1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            try
            {
                //初始化
                hv_Row1 = 0;
                hv_Column1 = 0;
                hv_Row2 = 0;
                hv_Column2 = 0;
                //产生一个空的直线对象，用于保存拟合后的直线
                ho_Line.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Line);
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(1)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                    //拟合直线。使用的算法是'tukey'，其他算法请参考fit_line_contour_xld的描述部分。
                    HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_Row1,
                        out hv_Column1, out hv_Row2, out hv_Column2, out hv_Nr, out hv_Nc, out hv_Dist);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_Dist, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();

                        return;
                    }
                    //根据拟合结果，产生直线xld
                    ho_Line.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2),
                        hv_Column1.TupleConcat(hv_Column2));
                }

                ho_Contour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Contour.Dispose();

                throw HDevExpDefaultException;
            }
        }


        /// <summary>
        /// 箭头绘制
        /// </summary>
        /// <param name="ho_Arrow"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_HeadLength"></param>
        /// <param name="hv_HeadWidth"></param>
        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
                HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);

            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);

            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            return;
        }
    }
}

