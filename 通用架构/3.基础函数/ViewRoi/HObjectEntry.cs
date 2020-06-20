using System;
using HalconDotNet;
using System.Collections;



namespace 通用架构._3.基础函数
{

    /// <summary>
    ///这个类是一个副类，用于将图形环境链接到HALCON对象。
    ///图形环境由一个hashtable描述，该表包含图形模式列表 (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT)
    ///及其对应的值(e.g "blue", "4", "3D-plot")。
    ///在显示对象之前，这些图形状态应用于窗口。
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
