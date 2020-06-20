using System;
using HalconDotNet;
using System.Collections;



namespace 通用架构._3.基础函数
{

	public delegate void FuncROIDelegate();

    /// <summary>
    ///这个类创建并管理ROI对象。它的反应
    ///使用mouseDownAction和方法向鼠标设备输入
    ///鼠标指令。你不需要知道这个类的细节
    ///构建自己的c#项目。但是你必须考虑一些事情
    ///你想在你的应用程序中使用交互式roi:有一个
    ///在ROIController和HWndCtrl之间有很密切的联系
    ///类，这意味着您必须“注册”ROIController
    ///使用HWndCtrl，所以HWndCtrl知道它必须转发用户输入
    ///(像鼠标事件)到ROIController类。 
    ///完成了ROI对象的可视化和操作
    ///通过ROIController。
    ///这个类为匹配提供了特殊的支持
    ///从roi列表中计算一个模型区域。为
    ///这个，roi是根据它们的符号加减的。
    /// </summary>
    public class ROIController
	{

        /// <summary>
        ///设置ROI模式的常数:positive ROI sign.
        /// </summary>
        public const int MODE_ROI_POS           = 21;

        /// <summary>
        ///设置ROI模式的常数:negative ROI sign.
        /// </summary>
        public const int MODE_ROI_NEG           = 22;

        /// <summary>
        ///设置ROI模式的常数:no model region 被用作计算
        /// 所有Roi对象总和
        /// </summary>
        public const int MODE_ROI_NONE          = 23;

        /// <summary>用于描述模型区域更新的常量</summary>
        public const int EVENT_UPDATE_ROI       = 50;

		public const int EVENT_CHANGED_ROI_SIGN = 51;

        /// <summary>用于描述模型区域更新的常量</summary>
        public const int EVENT_MOVING_ROI       = 52;

		public const int EVENT_DELETED_ACTROI  	= 53;

		public const int EVENT_DELETED_ALL_ROIS = 54;

		public const int EVENT_ACTIVATED_ROI   	= 55;

		public const int EVENT_CREATED_ROI   	= 56;


        public ROI     roiMode;
		private int     stateROI;
		private double  currX, currY;


        /// <summary>ActiveROI对象的索引Index</summary>
        public int      activeROIidx;
		public int      deletedIdx;

        /// <summary>列表中包含到目前为止创建的所有ROI对象</summary>
        public ArrayList ROIList;

        /// <summary>
        /// 通过汇总ROIList中所有negative ROI和positive ROI对象得到的区域
        /// </summary>
        public HRegion ModelROI;

		private string activeCol    = "orange";
		private string activeHdlCol = "red";
		private string inactiveCol  = "blue";

		/// <summary>
		/// 调用 HWndCtrl, ROI Controller 被用于
		/// </summary>
		public HWndCtrl viewController;

        /// <summary>
        /// 委托通知在model region中所做的更改
        /// </summary>
        public IconicDelegate  NotifyRCObserver;

		/// <summary>构造函数</summary>
		public ROIController()
		{
			stateROI = MODE_ROI_NONE;
			ROIList = new ArrayList();
			activeROIidx = -1;
			ModelROI = new HRegion();
			NotifyRCObserver = new IconicDelegate(dummyI);
			deletedIdx = -1;
			currX = currY = -1;
		}

        /// <summary>将HWndCtrl注册到这个ROIController实例</summary>
        public void setViewController(HWndCtrl view)
		{
			viewController = view;
		}

        /// <summary>获取ModelROI 对象</summary>
        public HRegion getModelRegion()
		{
			return ModelROI;
		}

        /// <summary>获取到目前为止创建的roi列表</summary>
        public ArrayList getROIList()
		{
			return ROIList;
		}

		/// <summary>获取 the active ROI</summary>
		public ROI getActiveROI()
		{
			if (activeROIidx != -1)
				return ((ROI)ROIList[activeROIidx]);

			return null;
		}

		public int getActiveROIIdx()
		{
			return activeROIidx;
		}

		public void setActiveROIIdx(int active)
		{
			activeROIidx = active;
		}

		public int getDelROIIdx()
		{
			return deletedIdx;
        }

        /// <summary>
        ///为了创建一个新的ROI对象，应用程序类初始化了一个
        ///“Seed”ROI实例化，并将其传递给ROIController。
        /// ROIController现在通过操纵这个新的ROI来回应
        ///实例
        /// </summary>
        /// <param name="r">
        ///“Seed”ROI对象由application forms类转发.
        /// </param>
        public void setROIShape(ROI r)
		{
			roiMode = r;    
			roiMode.setOperatorFlag(stateROI);
		}


        /// <summary>
        /// 将ROI对象的符号设置为“mode”值(MODE_ROI_NONE, MODE_ROI_POS,MODE_ROI_NEG)
        /// </summary>
        public void setROISign(int mode)
		{
			stateROI = mode;

			if (activeROIidx != -1)
			{
				((ROI)ROIList[activeROIidx]).setOperatorFlag(stateROI);
				viewController.repaint();
				NotifyRCObserver(ROIController.EVENT_CHANGED_ROI_SIGN);
			}
		}

        /// <summary>
        /// 可拖动标记为活动状态的ROI对象
        /// 若没有ROi对象为活动状态，则无事件触发。
        /// </summary>
        public void removeActive()
		{
			if (activeROIidx != -1)
			{
				ROIList.RemoveAt(activeROIidx);
				deletedIdx = activeROIidx;
				activeROIidx = -1;
				viewController.repaint();
				NotifyRCObserver(EVENT_DELETED_ACTROI);
			}
		}


        /// <summary>
        /// 计算ROIList中所有对象的ModelROI区域
        /// 通过添加和减去 the positive and / negativeROI对象。
        /// </summary>
        public bool defineModelROI()
		{
			HRegion tmpAdd, tmpDiff, tmp;
			double row, col;

			if (stateROI == MODE_ROI_NONE)
				return true;

			tmpAdd = new HRegion();
			tmpDiff = new HRegion();
      tmpAdd.GenEmptyRegion();
      tmpDiff.GenEmptyRegion();

			for (int i=0; i < ROIList.Count; i++)
			{
				switch (((ROI)ROIList[i]).getOperatorFlag())
				{
					case ROI.POSITIVE_FLAG:
						tmp = ((ROI)ROIList[i]).getRegion();
						tmpAdd = tmp.Union2(tmpAdd);
						break;
					case ROI.NEGATIVE_FLAG:
						tmp = ((ROI)ROIList[i]).getRegion();
						tmpDiff = tmp.Union2(tmpDiff);
						break;
					default:
						break;
				}//end of switch
			}//end of for

			ModelROI = null;

			if (tmpAdd.AreaCenter(out row, out col) > 0)
			{
				tmp = tmpAdd.Difference(tmpDiff);
				if (tmp.AreaCenter(out row, out col) > 0)
					ModelROI = tmp;
			}

			//in case the set of positiv and negative ROIs dissolve 
			if (ModelROI == null || ROIList.Count == 0)
				return false;

			return true;
		}


		/// <summary>
		/// 清除所有建立的 ROI对象
		/// </summary>
		public void reset()
		{
			ROIList.Clear();
			activeROIidx = -1;
			ModelROI = null;
			roiMode = null;
			NotifyRCObserver(EVENT_DELETED_ALL_ROIS);
		}


		/// <summary>
		/// Deletes this ROI instance if a 'seed' ROI object has been passed
		/// to the ROIController by the application class.
		/// 
		/// </summary>
		public void resetROI()
		{
			activeROIidx = -1;
			roiMode = null;
		}

		/// <summary>Defines the colors for the ROI objects</summary>
		/// <param name="aColor">Color for the active ROI object</param>
		/// <param name="inaColor">Color for the inactive ROI objects</param>
		/// <param name="aHdlColor">
		/// Color for the active handle of the active ROI object
		/// </param>
		public void setDrawColor(string aColor,
								   string aHdlColor,
								   string inaColor)
		{
			if (aColor != "")
				activeCol = aColor;
			if (aHdlColor != "")
				activeHdlCol = aHdlColor;
			if (inaColor != "")
				inactiveCol = inaColor;
		}


        /// <summary>
        /// 将ROIList中的所有对象绘制到HALCON窗口
        /// </summary>
        /// <param name="window">HALCON window</param>
        public void paintData(HalconDotNet.HWindow window)
		{
			window.SetDraw("margin");
			window.SetLineWidth(1);

			if (ROIList.Count > 0)
			{
				window.SetColor(inactiveCol);
				window.SetDraw("margin");

				for (int i=0; i < ROIList.Count; i++)
				{
					window.SetLineStyle(((ROI)ROIList[i]).flagLineStyle);
					((ROI)ROIList[i]).draw(window);
				}

				if (activeROIidx != -1)
				{
					window.SetColor(activeCol);
					window.SetLineStyle(((ROI)ROIList[activeROIidx]).flagLineStyle);
					((ROI)ROIList[activeROIidx]).draw(window);

					window.SetColor(activeHdlCol);
					((ROI)ROIList[activeROIidx]).displayActive(window);
				}
			}
		}

        /// <summary>
        /// ROI对象对'mouse button down'事件的反应:改变ROI的形状，如果是“Seed”ROI，则将其添加到ROIList中。
        /// </summary>
        /// <param name="imgX">x coordinate of mouse event</param>
        /// <param name="imgY">y coordinate of mouse event</param>
        /// <returns></returns>
        public int mouseDownAction(double imgX, double imgY)
		{
			int idxROI= -1;
			double max = 10000, dist = 0;
			double epsilon = 35.0;    //到一个handles的最大直线距离
          

            if (roiMode == null)      //创建一个新的ROI对象
            {
                try
                {
                    roiMode.createROI(imgX, imgY);
                    ROIList.Add(roiMode);
                    roiMode = null;
                    activeROIidx = ROIList.Count - 1;
                    viewController.repaint();

                    NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
                }
                catch (Exception)
                { }

			}
			else if (ROIList.Count > 0)		//或者对现有的一个Roi进行操控
			{
				activeROIidx = -1;

				for (int i =0; i < ROIList.Count; i++)
				{
					dist = ((ROI)ROIList[i]).distToClosestHandle(imgX, imgY);
					if ((dist < max) && (dist < epsilon))
					{
						max = dist;
						idxROI = i;
					}
				}

				if (idxROI >= 0)
				{
					activeROIidx = idxROI;
					NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);
				}

				viewController.repaint();
			}
			return activeROIidx;
		}

        /// <summary>
        /// ROI对象对“ 'mouse button move'事件的反应:移动
        /// the active ROI.
        /// </summary>
        /// <param name="newX">x coordinate of mouse event</param>
        /// <param name="newY">y coordinate of mouse event</param>
        public void mouseMoveAction(double newX, double newY)
		{
			if ((newX == currX) && (newY == currY))
				return;

			((ROI)ROIList[activeROIidx]).moveByHandle(newX, newY);
			viewController.repaint();
			currX = newX;
			currY = newY;
			NotifyRCObserver(ROIController.EVENT_MOVING_ROI);
		}


		/***********************************************************/
		public void dummyI(int v)
		{
		}

	}
}
