using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace 通用架构._4.算子库
{

    class ImageProcessing
    {

        ///<summary>
        ///图像裁切
        ///</summary>
        public void ImageCrop(HObject ho_Image,HTuple hv_CenterRow, HTuple hv_CenterCol, HTuple hv_Phi, HTuple hv_Length1, HTuple hv_Length2,
                              out HObject ho_Rectangle2, out HObject ho_ImagePart)
        {
            // Local iconic variables 

            HObject ho_ImageReduced;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            ho_Rectangle2.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_CenterRow, hv_CenterCol, hv_Phi,
                hv_Length1 + 10, hv_Length2 + 10);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle2, out ho_ImageReduced);
            ho_ImagePart.Dispose();
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
            ho_ImageReduced.Dispose();

            return;
        }


        ///<summary>
        ///图像锐化（时域）
        ///</summary>
        public void ImageSharpening(HObject ho_ImagePart, HTuple hv_MaskWidth, HTuple hv_MaskHeight, HTuple hv_MaskType, HTuple hv_Radius,
                                    HTuple hv_MaskSize,HTuple hv_FilterMask, HTuple hv_MultImage_Mult, HTuple hv_AddImage_Mult, 
                                    HTuple hv_ScaleImage_Mult, HTuple hv_ScaleImage_Add, out HObject ho_ImageResult)
        {
            // Local iconic variables 

            HObject ho_ImageSmooth, ho_ImageLaplace, ho_ImageScaled;
            HObject ho_ImageConverted, ho_ImageEdgeAmp, ho_ImageMean;
            HObject ho_ImageAdd1, ho_ImageMult, ho_ImageAdd2;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageSmooth);
            HOperatorSet.GenEmptyObj(out ho_ImageLaplace);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageConverted);
            HOperatorSet.GenEmptyObj(out ho_ImageEdgeAmp);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageAdd1);
            HOperatorSet.GenEmptyObj(out ho_ImageMult);
            HOperatorSet.GenEmptyObj(out ho_ImageAdd2);
            //中值滤波
            ho_ImageSmooth.Dispose();
            HOperatorSet.MedianImage(ho_ImagePart, out ho_ImageSmooth, hv_MaskType, hv_Radius,
                "mirrored");

            //拉普拉斯锐化
            ho_ImageLaplace.Dispose();
            HOperatorSet.Laplace(ho_ImageSmooth, out ho_ImageLaplace, "signed", hv_MaskSize,
                hv_FilterMask);
            ho_ImageScaled.Dispose();
            HOperatorSet.ScaleImage(ho_ImageLaplace, out ho_ImageScaled, 7, 50);
            ho_ImageConverted.Dispose();
            HOperatorSet.ConvertImageType(ho_ImageScaled, out ho_ImageConverted, "byte");

            //prewitt锐化
            ho_ImageEdgeAmp.Dispose();
            HOperatorSet.PrewittAmp(ho_ImagePart, out ho_ImageEdgeAmp);
            ho_ImageMean.Dispose();
            HOperatorSet.MeanImage(ho_ImageEdgeAmp, out ho_ImageMean, hv_MaskWidth, hv_MaskHeight);
            ho_ImageAdd1.Dispose();
            HOperatorSet.AddImage(ho_ImageEdgeAmp, ho_ImageMean, out ho_ImageAdd1, 1, 0);

            //图像锐化处理
            ho_ImageMult.Dispose();
            HOperatorSet.MultImage(ho_ImageConverted, ho_ImageAdd1, out ho_ImageMult, hv_MultImage_Mult,
                0);
            ho_ImageMean.Dispose();
            HOperatorSet.MeanImage(ho_ImageMult, out ho_ImageMean, hv_MaskWidth, hv_MaskHeight);
            ho_ImageAdd2.Dispose();
            HOperatorSet.AddImage(ho_ImageSmooth, ho_ImageMean, out ho_ImageAdd2, hv_AddImage_Mult,
                0);

            //灰度拉伸
            ho_ImageResult.Dispose();
            HOperatorSet.ScaleImage(ho_ImageAdd2, out ho_ImageResult, hv_ScaleImage_Mult,
                hv_ScaleImage_Add);
            ho_ImageSmooth.Dispose();
            ho_ImageLaplace.Dispose();
            ho_ImageScaled.Dispose();
            ho_ImageConverted.Dispose();
            ho_ImageEdgeAmp.Dispose();
            ho_ImageMean.Dispose();
            ho_ImageAdd1.Dispose();
            ho_ImageMult.Dispose();
            ho_ImageAdd2.Dispose();

            return;
        }


        ///<summary>
        ///图像拼接
        ///</summary>
        public void ImageStitching(HObject ho_ImageResult, HObject ho_Image, HObject ho_Rectangle2, HTuple hv_Phi, HTuple hv_Length1, 
                                   HTuple hv_Length2, HTuple hv_CenterRow, HTuple hv_CenterCol, out HObject ho_ImageStich)
        {
            // Local iconic variables 

            HObject ho_Rectangle2Scaled, ho_Image1, ho_Imagepaint;

            // Local control variables 

            HTuple hv_Area = null, hv_PartRow = null, hv_PartCol = null;
            HTuple hv_ScaledRows = null, hv_ScaledColumns = null, hv_GrayvalScaled = null;
            HTuple hv_rows = null, hv_cols = null, hv_Width = null;
            HTuple hv_Height = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageStich);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2Scaled);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Imagepaint);
            HOperatorSet.AreaCenter(ho_ImageResult, out hv_Area, out hv_PartRow, out hv_PartCol);
            ho_Rectangle2Scaled.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle2Scaled, hv_PartRow, hv_PartCol, hv_Phi,
                hv_Length1 + 10, hv_Length2 + 10);
            HOperatorSet.GetRegionPoints(ho_Rectangle2Scaled, out hv_ScaledRows, out hv_ScaledColumns);
            HOperatorSet.GetGrayval(ho_ImageResult, hv_ScaledRows, hv_ScaledColumns, out hv_GrayvalScaled);
            hv_rows = (hv_ScaledRows + hv_CenterRow) - hv_PartRow;
            hv_cols = (hv_ScaledColumns + hv_CenterCol) - hv_PartCol;

            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            ho_Image1.Dispose();
            HOperatorSet.GenImageConst(out ho_Image1, "byte", hv_Width, hv_Height);
            HOperatorSet.SetGrayval(ho_Image1, hv_rows, hv_cols, hv_GrayvalScaled);

            ho_Imagepaint.Dispose();
            HOperatorSet.PaintRegion(ho_Rectangle2, ho_Image, out ho_Imagepaint, 0, "fill");
            ho_ImageStich.Dispose();
            HOperatorSet.AddImage(ho_Image1, ho_Imagepaint, out ho_ImageStich, 1, 0);
            ho_Rectangle2Scaled.Dispose();
            ho_Image1.Dispose();
            ho_Imagepaint.Dispose();

            return;
        }

    }
}
