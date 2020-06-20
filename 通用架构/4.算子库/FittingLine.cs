using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace 通用架构._4.算子库
{
    class FittingLine
    {

        ///<summary>
        ///创建直线检测模板（无仿射变换）
        ///</summary>
        public void CreateLineModel(HTuple hv_MeasureRow1, HTuple hv_MeasureColumn1, HTuple hv_MeasureRow2, HTuple hv_MeasureColumn2,
                                    HTuple hv_MeasureLength1, HTuple hv_MeasureLength2, HTuple hv_MeasureSigma, HTuple hv_MeasureThreshold, out HTuple hv_MetrologyHandle, out HTuple hv_Index)
        {
            HTuple mhv_MetrologyHandle = new HTuple();
            hv_MetrologyHandle = new HTuple();
            hv_Index = new HTuple();
            hv_MetrologyHandle = new HTuple();

            try
            {
                HOperatorSet.ClearMetrologyModel(mhv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out mhv_MetrologyHandle);
                //HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                HOperatorSet.AddMetrologyObjectLineMeasure(mhv_MetrologyHandle, hv_MeasureRow1, hv_MeasureColumn1, hv_MeasureRow2, hv_MeasureColumn2,
                             hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                             new HTuple(), new HTuple(), out hv_Index);
                hv_MetrologyHandle = mhv_MetrologyHandle;
                return;
            }
            catch (Exception)
            {

                return;
            }
        }


        ///<summary>
        ///创建直线检测模板（仿射变换）
        ///</summary>
        public void CreateLineModel(HTuple hv_HomMat2D, HTuple hv_MeasureRow1, HTuple hv_MeasureColumn1, HTuple hv_MeasureRow2, HTuple hv_MeasureColumn2,
                                    HTuple hv_MeasureLength1, HTuple hv_MeasureLength2, HTuple hv_MeasureSigma, HTuple hv_MeasureThreshold, out HTuple hv_MetrologyHandle, out HTuple hv_Index)
        {
           
            HTuple mhv_Row1;
            HTuple mhv_Column1;
            HTuple mhv_Row2;
            HTuple mhv_Column2;

            HTuple mhv_MetrologyHandle = new HTuple();
            hv_Index = new HTuple();
            hv_MetrologyHandle = new HTuple();

            try
            {
                HOperatorSet.ClearMetrologyModel(mhv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);

                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_MeasureRow1, hv_MeasureColumn1, out mhv_Row1, out mhv_Column1);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_MeasureRow2, hv_MeasureColumn2, out mhv_Row2, out mhv_Column2);

                HOperatorSet.DistancePp(mhv_Row1, mhv_Column1, mhv_Row2, mhv_Column2, out HTuple DistancePp);
                hv_MeasureLength2 = DistancePp / 2;
                HOperatorSet.AddMetrologyObjectLineMeasure(mhv_MetrologyHandle, mhv_Row1, mhv_Column1, mhv_Row2, mhv_Column2,
                             hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                             new HTuple(), new HTuple(), out hv_Index);

                hv_MetrologyHandle = mhv_MetrologyHandle;
                return;
            }
            catch (Exception)
            {
                return;
            }
        }


        ///<summary>
        ///直线拟合（无仿射变换）
        ///</summary>
        public void FindLine(HObject ho_Image,HTuple hv_MetrologyHandle, HTuple hv_Index, HTuple hv_MeasureNumInstances, HTuple hv_MeasureDistThreshold, HTuple hv_MeasureTransition, HTuple hv_MeasureSelect,
                             HTuple hv_MeasurePointsNum, HTuple hv_MeasureMinScore, HTuple hv_MaxNumIterations, HTuple hv_MeasureInterpolation, HTuple hv_MeasureIORegions, out HTuple hv_CrossRow, 
                             out HTuple hv_CrossColumn,out HTuple hv_LineRowBegin, out HTuple hv_LineColumnBegin, out HTuple hv_LineRowEnd, out HTuple hv_LineColumnEnd)
        {
            HObject ho_Contours = new HObject();
            hv_LineRowBegin = new HTuple();
            hv_LineColumnBegin = new HTuple();
            hv_LineRowEnd = new HTuple();
            hv_LineColumnEnd = new HTuple();
            hv_CrossRow = new HTuple();
            hv_CrossColumn = new HTuple();

            try
            {
                //1.直线拟合数量：默认值： 1 值的列表： 1，2，3，4
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_instances", hv_MeasureNumInstances);
                //2.距离剔除：默认值： 3.5  值的列表： 0，1.0，2.0， 3.5，5.0
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "distance_threshold", hv_MeasureDistThreshold);
                //3.明暗方向：值列表： 'all'，'negative'， 'positive'，'uniform'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition", hv_MeasureTransition);
                //4.边缘选取：'all'，'first'，'last'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select", hv_MeasureSelect);
                //5.Roi数量：'num_measures'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures", hv_MeasurePointsNum);
                //6.最小得分：'min_score'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score", hv_MeasureMinScore);
                //7.迭代次数: 默认值：-1  值的列表： 10，100，1000
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "max_num_iterations", hv_MaxNumIterations);
                //8.插值类型：nearest_neighbor'灰度值是从最近像素的灰度值获得；'bilinear'，使用双线性插值 ；'bicubic'，则使用双三次插值。
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_interpolation", hv_MeasureInterpolation);
                //9.指定对测量结果的验证：默认值： “ false”  值列表： 'true'，'false'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "instances_outside_measure_regions", hv_MeasureIORegions);

                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle, "all", "all", out hv_CrossRow, out hv_CrossColumn);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_end", out hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_end", out hv_LineColumnEnd);
                
                //清除测量模型
                ho_Contours.Dispose();
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                
            }
            catch (Exception)
            {
             
            }
        }


        ///<summary>
        ///直线拟合（仿射变换）
        ///</summary>
        public void FindLine(HTuple hv_HomMat2DPix, HObject ho_Image, HTuple hv_MetrologyHandle, HTuple hv_Index, HTuple hv_MeasureNumInstances, HTuple hv_MeasureDistThreshold, HTuple hv_MeasureTransition,
                             HTuple hv_MeasureSelect, HTuple hv_MeasurePointsNum,HTuple hv_MeasureMinScore, HTuple hv_MaxNumIterations, HTuple hv_MeasureInterpolation, HTuple hv_MeasureIORegions, 
                             out HTuple hv_CrossRow, out HTuple hv_CrossColumn,out HTuple hv_LineRowBegin_Real, out HTuple hv_LineColumnBegin_Real, out HTuple hv_LineRowEnd_Real, out HTuple hv_LineColumnEnd_Real)
        {
            HObject ho_Contours = new HObject();
            HTuple hv_LineRowBegin = new HTuple();
            HTuple hv_LineColumnBegin = new HTuple();
            HTuple hv_LineRowEnd = new HTuple();
            HTuple hv_LineColumnEnd = new HTuple();
            hv_CrossRow = new HTuple();
            hv_CrossColumn = new HTuple();
            hv_LineRowBegin_Real = new HTuple();
            hv_LineColumnBegin_Real = new HTuple();
            hv_LineRowEnd_Real = new HTuple();
            hv_LineColumnEnd_Real = new HTuple();

            try
            {
                //1.直线拟合数量：默认值： 1 值的列表： 1，2，3，4
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_instances", hv_MeasureNumInstances);
                //2.距离剔除：默认值： 3.5  值的列表： 0，1.0，2.0， 3.5，5.0
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "distance_threshold", hv_MeasureDistThreshold);
                //3.明暗方向：值列表： 'all'，'negative'， 'positive'，'uniform'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition", hv_MeasureTransition);
                //4.边缘选取：'all'，'first'，'last'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select", hv_MeasureSelect);
                //5.Roi数量：'num_measures'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures", hv_MeasurePointsNum);
                //6.最小得分：'min_score'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score", hv_MeasureMinScore);
                //7.迭代次数: 默认值：-1  值的列表： 10，100，1000
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "max_num_iterations", hv_MaxNumIterations);
                //8.插值类型：nearest_neighbor'灰度值是从最近像素的灰度值获得；'bilinear'，使用双线性插值 ；'bicubic'，则使用双三次插值。
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_interpolation", hv_MeasureInterpolation);
                //9.指定对测量结果的验证：默认值： “ false”  值列表： 'true'，'false'
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "instances_outside_measure_regions", hv_MeasureIORegions);

                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle, "all", "all", out hv_CrossRow, out hv_CrossColumn);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_end", out hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_end", out hv_LineColumnEnd);

                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowBegin, hv_LineColumnBegin, out hv_LineRowBegin_Real, out hv_LineColumnBegin_Real);
                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowEnd, hv_LineColumnEnd, out hv_LineRowEnd_Real, out hv_LineColumnEnd_Real);

                //清除测量模型
                ho_Contours.Dispose();
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                return;
            }
            catch (Exception)
            {

                return;
            }
        }

    }
}
