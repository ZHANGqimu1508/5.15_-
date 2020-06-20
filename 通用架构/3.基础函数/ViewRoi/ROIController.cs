using System;
using HalconDotNet;
using System.Collections;



namespace ͨ�üܹ�._3.��������
{

	public delegate void FuncROIDelegate();

    /// <summary>
    ///����ഴ��������ROI�������ķ�Ӧ
    ///ʹ��mouseDownAction�ͷ���������豸����
    ///���ָ��㲻��Ҫ֪��������ϸ��
    ///�����Լ���c#��Ŀ����������뿼��һЩ����
    ///���������Ӧ�ó�����ʹ�ý���ʽroi:��һ��
    ///��ROIController��HWndCtrl֮���к����е���ϵ
    ///�࣬����ζ�������롰ע�ᡱROIController
    ///ʹ��HWndCtrl������HWndCtrl֪��������ת���û�����
    ///(������¼�)��ROIController�ࡣ 
    ///�����ROI����Ŀ��ӻ��Ͳ���
    ///ͨ��ROIController��
    ///�����Ϊƥ���ṩ�������֧��
    ///��roi�б��м���һ��ģ������Ϊ
    ///�����roi�Ǹ������ǵķ��żӼ��ġ�
    /// </summary>
    public class ROIController
	{

        /// <summary>
        ///����ROIģʽ�ĳ���:positive ROI sign.
        /// </summary>
        public const int MODE_ROI_POS           = 21;

        /// <summary>
        ///����ROIģʽ�ĳ���:negative ROI sign.
        /// </summary>
        public const int MODE_ROI_NEG           = 22;

        /// <summary>
        ///����ROIģʽ�ĳ���:no model region ����������
        /// ����Roi�����ܺ�
        /// </summary>
        public const int MODE_ROI_NONE          = 23;

        /// <summary>��������ģ��������µĳ���</summary>
        public const int EVENT_UPDATE_ROI       = 50;

		public const int EVENT_CHANGED_ROI_SIGN = 51;

        /// <summary>��������ģ��������µĳ���</summary>
        public const int EVENT_MOVING_ROI       = 52;

		public const int EVENT_DELETED_ACTROI  	= 53;

		public const int EVENT_DELETED_ALL_ROIS = 54;

		public const int EVENT_ACTIVATED_ROI   	= 55;

		public const int EVENT_CREATED_ROI   	= 56;


        public ROI     roiMode;
		private int     stateROI;
		private double  currX, currY;


        /// <summary>ActiveROI���������Index</summary>
        public int      activeROIidx;
		public int      deletedIdx;

        /// <summary>�б��а�����ĿǰΪֹ����������ROI����</summary>
        public ArrayList ROIList;

        /// <summary>
        /// ͨ������ROIList������negative ROI��positive ROI����õ�������
        /// </summary>
        public HRegion ModelROI;

		private string activeCol    = "orange";
		private string activeHdlCol = "red";
		private string inactiveCol  = "blue";

		/// <summary>
		/// ���� HWndCtrl, ROI Controller ������
		/// </summary>
		public HWndCtrl viewController;

        /// <summary>
        /// ί��֪ͨ��model region�������ĸ���
        /// </summary>
        public IconicDelegate  NotifyRCObserver;

		/// <summary>���캯��</summary>
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

        /// <summary>��HWndCtrlע�ᵽ���ROIControllerʵ��</summary>
        public void setViewController(HWndCtrl view)
		{
			viewController = view;
		}

        /// <summary>��ȡModelROI ����</summary>
        public HRegion getModelRegion()
		{
			return ModelROI;
		}

        /// <summary>��ȡ��ĿǰΪֹ������roi�б�</summary>
        public ArrayList getROIList()
		{
			return ROIList;
		}

		/// <summary>��ȡ the active ROI</summary>
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
        ///Ϊ�˴���һ���µ�ROI����Ӧ�ó������ʼ����һ��
        ///��Seed��ROIʵ�����������䴫�ݸ�ROIController��
        /// ROIController����ͨ����������µ�ROI����Ӧ
        ///ʵ��
        /// </summary>
        /// <param name="r">
        ///��Seed��ROI������application forms��ת��.
        /// </param>
        public void setROIShape(ROI r)
		{
			roiMode = r;    
			roiMode.setOperatorFlag(stateROI);
		}


        /// <summary>
        /// ��ROI����ķ�������Ϊ��mode��ֵ(MODE_ROI_NONE, MODE_ROI_POS,MODE_ROI_NEG)
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
        /// ���϶����Ϊ�״̬��ROI����
        /// ��û��ROi����Ϊ�״̬�������¼�������
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
        /// ����ROIList�����ж����ModelROI����
        /// ͨ����Ӻͼ�ȥ the positive and / negativeROI����
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
		/// ������н����� ROI����
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
        /// ��ROIList�е����ж�����Ƶ�HALCON����
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
        /// ROI�����'mouse button down'�¼��ķ�Ӧ:�ı�ROI����״������ǡ�Seed��ROI��������ӵ�ROIList�С�
        /// </summary>
        /// <param name="imgX">x coordinate of mouse event</param>
        /// <param name="imgY">y coordinate of mouse event</param>
        /// <returns></returns>
        public int mouseDownAction(double imgX, double imgY)
		{
			int idxROI= -1;
			double max = 10000, dist = 0;
			double epsilon = 35.0;    //��һ��handles�����ֱ�߾���
          

            if (roiMode == null)      //����һ���µ�ROI����
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
			else if (ROIList.Count > 0)		//���߶����е�һ��Roi���вٿ�
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
        /// ROI����ԡ� 'mouse button move'�¼��ķ�Ӧ:�ƶ�
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
