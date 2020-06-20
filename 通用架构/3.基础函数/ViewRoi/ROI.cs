using System;
using HalconDotNet;


namespace 通用架构._3.基础函数
{

    /// <summary>
    ///这个类是一个基类，包含了处理roi的虚拟方法。因此，继承类需要定义/覆盖这些方法，
    ///以便为ROIController提供关于其(ROIs)形状和位置的必要信息。
    ///该示例项目为矩形、直线、圆和圆弧提供了派生的ROI形状。
    ///要使用其他形状，您必须从基类ROI派生一个新类并实现它的方法。
    /// </summary>    
    public class ROI
	{

        // 继承ROI类的类成员
        protected int   NumHandles;
		protected int	activeHandleIdx;

        /// <summary>
        /// Flag 将ROI定义为 'positive' or 'negative'.
        /// </summary>
        protected int     OperatorFlag;

        /// <summary>定义ROI的线条样式的参数。</summary>
        public HTuple     flagLineStyle;

		/// <summary>Constant for a positive ROI flag.</summary>
		public const int  POSITIVE_FLAG	= ROIController.MODE_ROI_POS;

		/// <summary>Constant for a negative ROI flag.</summary>
		public const int  NEGATIVE_FLAG	= ROIController.MODE_ROI_NEG;

		public const int  ROI_TYPE_LINE       = 10;
		public const int  ROI_TYPE_CIRCLE     = 11;
		public const int  ROI_TYPE_CIRCLEARC  = 12;
		public const int  ROI_TYPE_RECTANCLE1 = 13;
		public const int  ROI_TYPE_RECTANGLE2 = 14;


		protected HTuple  posOperation = new HTuple();
		protected HTuple  negOperation = new HTuple(new int[] { 2, 2 });

		/// <summary>构造函数  ROI class.</summary>
		public ROI() { }

        /// <summary>在鼠标位置创建一个新的ROI实例。</summary>
        /// <param name="midX">
        /// x (=column) coordinate for ROI
        /// </param>
        /// <param name="midY">
        /// y (=row) coordinate for ROI
        /// </param>
        public virtual void createROI(double midX, double midY) { }

        /// <summary>将ROI绘制到提供的窗口中。</summary>
        /// <param name="window">HALCON window</param>
        public virtual void draw(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// 返回ROI句柄与图像点(x,y)的最近距离
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        /// <returns> 
        /// Distance of the closest ROI handle.
        /// </returns>
        public virtual double distToClosestHandle(double x, double y)
		{
			return 0.0;
		}

        /// <summary> 
        /// 将ROI对象的活动手柄绘制到提供的窗口中。
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void displayActive(HalconDotNet.HWindow window) { }

        /// <summary> 
        ///重新计算ROI的形状。
        ///在图像坐标(x,y)处，对处于活动状态的ROI对象进行重新生成。
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        public virtual void moveByHandle(double x, double y) { }

        /// <summary>获取由ROI对象描述的HALCON区域。</summary>
        public virtual HRegion getRegion()
		{
			return null;
		}

		public virtual double getDistanceFromStartPoint(double row, double col)
		{
			return 0.0;
		}
		/// <summary>
		/// 获取Roi的模型参数。
		/// </summary> 
		public virtual HTuple getModelData()
		{
			return null;
		}

		/// <summary>Number of handles defined for the ROI.</summary>
		/// <returns>控制Roi手柄的数量</returns>
		public int getNumHandles()
		{
			return NumHandles;
		}

		/// <summary>获取被激活的Roi控制手柄信息</summary>
		/// <returns>Index of the active handle (from the handle list)</returns>
		public int getActHandleIdx()
		{
			return activeHandleIdx;
		}

        /// <summary>
        ///获取ROI对象的flag，表示'positive' or 'negative'.。此符号用于创建对应roi列表中的区域模型。
        /// </summary>
        public int getOperatorFlag()
		{
			return OperatorFlag;
		}

        /// <summary>
        ///将ROI对象的符号设置为positive o或negative. 
        /// The sign is used when creating a model region for matching
        /// applications by summing up all positive and negative ROI models
        /// created so far.
        /// </summary>
        /// <param name="flag">Sign of ROI object</param>
        public void setOperatorFlag(int flag)
		{
			OperatorFlag = flag;

			switch (OperatorFlag)
			{
				case ROI.POSITIVE_FLAG:
					flagLineStyle = posOperation;
					break;
				case ROI.NEGATIVE_FLAG:
					flagLineStyle = negOperation;
					break;
				default:
					flagLineStyle = posOperation;
					break;
			}
		}
	}//end of class
}//end of namespace
