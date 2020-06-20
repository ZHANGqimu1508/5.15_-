using System;
using HalconDotNet;


namespace ͨ�üܹ�._3.��������
{

    /// <summary>
    ///�������һ�����࣬�����˴���roi�����ⷽ������ˣ��̳�����Ҫ����/������Щ������
    ///�Ա�ΪROIController�ṩ������(ROIs)��״��λ�õı�Ҫ��Ϣ��
    ///��ʾ����ĿΪ���Ρ�ֱ�ߡ�Բ��Բ���ṩ��������ROI��״��
    ///Ҫʹ��������״��������ӻ���ROI����һ�����ಢʵ�����ķ�����
    /// </summary>    
    public class ROI
	{

        // �̳�ROI������Ա
        protected int   NumHandles;
		protected int	activeHandleIdx;

        /// <summary>
        /// Flag ��ROI����Ϊ 'positive' or 'negative'.
        /// </summary>
        protected int     OperatorFlag;

        /// <summary>����ROI��������ʽ�Ĳ�����</summary>
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

		/// <summary>���캯��  ROI class.</summary>
		public ROI() { }

        /// <summary>�����λ�ô���һ���µ�ROIʵ����</summary>
        /// <param name="midX">
        /// x (=column) coordinate for ROI
        /// </param>
        /// <param name="midY">
        /// y (=row) coordinate for ROI
        /// </param>
        public virtual void createROI(double midX, double midY) { }

        /// <summary>��ROI���Ƶ��ṩ�Ĵ����С�</summary>
        /// <param name="window">HALCON window</param>
        public virtual void draw(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// ����ROI�����ͼ���(x,y)���������
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
        /// ��ROI����Ļ�ֱ����Ƶ��ṩ�Ĵ����С�
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void displayActive(HalconDotNet.HWindow window) { }

        /// <summary> 
        ///���¼���ROI����״��
        ///��ͼ������(x,y)�����Դ��ڻ״̬��ROI��������������ɡ�
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        public virtual void moveByHandle(double x, double y) { }

        /// <summary>��ȡ��ROI����������HALCON����</summary>
        public virtual HRegion getRegion()
		{
			return null;
		}

		public virtual double getDistanceFromStartPoint(double row, double col)
		{
			return 0.0;
		}
		/// <summary>
		/// ��ȡRoi��ģ�Ͳ�����
		/// </summary> 
		public virtual HTuple getModelData()
		{
			return null;
		}

		/// <summary>Number of handles defined for the ROI.</summary>
		/// <returns>����Roi�ֱ�������</returns>
		public int getNumHandles()
		{
			return NumHandles;
		}

		/// <summary>��ȡ�������Roi�����ֱ���Ϣ</summary>
		/// <returns>Index of the active handle (from the handle list)</returns>
		public int getActHandleIdx()
		{
			return activeHandleIdx;
		}

        /// <summary>
        ///��ȡROI�����flag����ʾ'positive' or 'negative'.���˷������ڴ�����Ӧroi�б��е�����ģ�͡�
        /// </summary>
        public int getOperatorFlag()
		{
			return OperatorFlag;
		}

        /// <summary>
        ///��ROI����ķ�������Ϊpositive o��negative. 
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
