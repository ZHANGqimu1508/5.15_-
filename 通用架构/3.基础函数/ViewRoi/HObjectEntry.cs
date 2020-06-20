using System;
using HalconDotNet;
using System.Collections;



namespace ͨ�üܹ�._3.��������
{

    /// <summary>
    ///�������һ�����࣬���ڽ�ͼ�λ������ӵ�HALCON����
    ///ͼ�λ�����һ��hashtable�������ñ����ͼ��ģʽ�б� (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT)
    ///�����Ӧ��ֵ(e.g "blue", "4", "3D-plot")��
    ///����ʾ����֮ǰ����Щͼ��״̬Ӧ���ڴ��ڡ�
    /// </summary>
    public class HObjectEntry
	{
		/// <summary>Hashlist defining the graphical context for HObj</summary>
		public Hashtable	gContext;

		/// <summary>HALCON object</summary>
		public HObject		HObj;



		/// <summary>Constructor</summary>
		/// <param name="obj">
		/// HALCON object that is linked to the graphical context gc. 
		/// </param>
		/// <param name="gc"> 
		/// Hashlist of graphical states that are applied before the object
		/// is displayed. 
		/// </param>
		public HObjectEntry(HObject obj, Hashtable gc)
		{
			gContext = gc;
			HObj = obj;
		}

		/// <summary>
		/// Clears the entries of the class members Hobj and gContext
		/// </summary>
		public void clear()
		{
			gContext.Clear();
			HObj.Dispose();
		}

	}//end of class
}//end of namespace
