using System;
using HalconDotNet;



namespace 通用架构._3.基础函数
{

    /// <summary>
    ///这个类演示了(simple) rectangularly shaped ROI的一种变形方式。
    ///为了创建这个矩形，我们使用了一个中心点(midR, midC)，一个方向'phi'和半边长度'length1'和'length2'，类似于HALCON操作符gen_rectangle2()。
    ///这个类ROIRectangle2继承了基类ROI，并实现了(除了其他辅助方法之外)ROI.cs中定义的所有使用方法。
    /// </summary>
    public class ROIRectangle2 : ROI
	{

        /// <summary>length1矩形垂直方向边长的一半</summary>
        public double length1=100;

        /// <summary>length2矩形水平方向边长的一半</summary>
        public double length2=100;

        /// <summary>扫描点数量</summary>
        public int PointNum=20;

        /// <summary>Row 矩形中点的行坐标</summary>
        public double midR=700;

        /// <summary>Column 矩形中点的列坐标</summary>
        public double midC=700;

        /// <summary>phi 以弧度定义的矩形的旋转角度.</summary>
        public double phi= -Math.PI / 2;

		//辅助变量
		HTuple rowsInit;
		HTuple colsInit;
		HTuple rows;
		HTuple cols;

		HHomMat2D hom2D, tmp;

		/// <summary>Constructor</summary>
		public ROIRectangle2()
		{
			NumHandles = 6; // 4 corners +  1 midpoint + 1 rotationpoint			
			activeHandleIdx = 4;
		}

        /// <summary>在鼠标位置创建一个新的ROI实例</summary>
        /// <param name="midX">
        /// x (=column) coordinate for interactive ROI
        /// </param>
        /// <param name="midY">
        /// y (=row) coordinate for interactive ROI
        /// </param>
        public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

            rowsInit = new HTuple(new double[] {-1.0, -1.0, 1.0, 
												   1.0,  0.0, 0.0 });
			colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0, 
												  -1.0, 0.0, 1.2});
			//order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
			hom2D = new HHomMat2D();
			tmp = new HHomMat2D();

			updateHandlePos();
		}

        /// <summary>将ROI绘制到提供的窗口中</summary>
        /// <param name="window">HALCON window</param>
        public override void draw(HalconDotNet.HWindow window)
        {


            window.DispLine(midR + Math.Sin(phi) * length1, 
                            midC + Math.Cos(phi) * length1,
                            midR + (Math.Sin(phi) * (length1 +30)),
                            midC + (Math.Cos(phi) * (length1 +30)));

            for (int i = 0; i < PointNum; i++)
            {
                window.DispRectangle2(midR + (-length1 + length1 / PointNum + length1 * 2 / PointNum * i) * Math.Sin(phi), 
                    midC+( - length1 + length1 / PointNum + length1 * 2 / PointNum * i)*Math.Cos(phi), -phi, length1  / PointNum, length2 );
            }
            window.SetDraw("fill");
            for (int i =0; i < NumHandles-1; i++)
				window.DispRectangle2(rows[i].D, cols[i].D, -phi, 10, 10);
            window.DispCircle((midR + Math.Sin(phi) * (length1 + 36)), (midC + Math.Cos(phi) * (length1 + 36)), 12);
            window.SetLineWidth(5);
            window.DispArrow(midR - (Math.Sin(phi - Math.PI/2) * (length2-10)),
                             midC - (Math.Cos(phi - Math.PI / 2) * (length2-10)), 
                             midR -(Math.Sin(phi- Math.PI / 2) * length2 ),
				             midC - (Math.Cos(phi- Math.PI / 2) * length2 ),2.0);
            window.SetLineWidth(1);
            window.SetDraw("margin");
        }

        /// <summary> 
        /// 返回ROI句柄最接近图像点(x,y)的距离)
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        /// <returns> 
        /// Distance of the closest ROI handle.
        /// </returns>
        public override double distToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];


			for (int i=0; i < NumHandles; i++)
				val[i] = HMisc.DistancePp(y, x, rows[i].D, cols[i].D);

			for (int i=0; i < NumHandles; i++)
			{
				if (val[i] < max)
				{
					max = val[i];
					activeHandleIdx = i;
				}
			}
			return val[activeHandleIdx];
		}

        /// <summary> 
        ///将ROI对象的活动句柄绘制到提供的窗口中
        /// </summary>
        /// <param name="window">HALCON window</param>
        public override void displayActive(HalconDotNet.HWindow window)
		{
            if (activeHandleIdx != 5)
                window.DispRectangle2(rows[activeHandleIdx].D, cols[activeHandleIdx].D, -phi, 13, 13);

            if (activeHandleIdx == 5)
            {
                window.DispLine(midR + Math.Sin(phi) * length1,
                                midC + Math.Cos(phi) * length1,
                                midR + (Math.Sin(phi) * (length1+30)),
                                midC + (Math.Cos(phi) * (length1 +30)));
                window.DispCircle((midR + Math.Sin(phi) * (length1 + 36)), (midC + Math.Cos(phi) * (length1 + 36)), 15);
            }
        }


        /// <summary>获取由ROI描述的HALCON区域</summary>
        public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRectangle2(midR, midC, -phi, length1, length2);
			return region;
		}

        /// <summary>
        ///获取描述ROI模型的参数信息
        public override HTuple getModelData()
		{
			return new HTuple(new double[] { midR, midC, phi, length1, length2 });
		}

        /// <summary> 
        ///重新计算ROI实例的形状。根据图像坐标(x,y)，对处于激活状态的ROI进行平移。
        /// </summary>
        /// <param name="newX">x mouse coordinate</param>
        /// <param name="newY">y mouse coordinate</param>
        public override void moveByHandle(double newX, double newY)
		{
			double vX, vY, x=0, y=0;

			switch (activeHandleIdx)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					tmp = hom2D.HomMat2dInvert();
					x = tmp.AffineTransPoint2d(newX, newY, out y);

					length2 = Math.Abs(y);
					length1 = Math.Abs(x);

					checkForRange(x, y);
					break;
				case 4:
					midC = newX;
					midR = newY;
					break;
				case 5:
					vY = newY - rows[4].D;
					vX = newX - cols[4].D;
					phi = Math.Atan2(vY, vX);
					break;
			}
			updateHandlePos();
		}//end of method


        /// <summary>
        ///辅助方法，通过更新homography hom2D转换初始行坐标和列坐标(rowsInit, colsInit)，重新计算矩形的轮廓点
        /// </summary>
        private void updateHandlePos()
		{
			hom2D.HomMat2dIdentity();
			hom2D = hom2D.HomMat2dTranslate(midC, midR);
			hom2D = hom2D.HomMat2dRotateLocal(phi);
			tmp = hom2D.HomMat2dScaleLocal(length1, length2);
			cols = tmp.AffineTransPoint2d(colsInit, rowsInit, out rows);
		}


        //这个辅助方法使用四个矩形角(handles0到3)的坐标(x,y)来检查半长度(length1, length2)，
        //以避免当出现针对length1=length2=0的矩形的“折叠”时，矩形ROI在其中点的“形变”。
		private void checkForRange(double x, double y)
		{
			switch (activeHandleIdx)
			{
				case 0:
					if ((x < 0) && (y < 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 1:
					if ((x > 0) && (y < 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 2:
					if ((x > 0) && (y > 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				case 3:
					if ((x < 0) && (y > 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				default:
					break;
			}
		}
	}//end of class
}//end of namespace
