using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using HalconDotNet;
using 通用架构._4.算子库;
using 通用架构._3.基础函数;
using System.IO;
using System.Collections.Generic;

namespace 通用架构._2.子界面
{
    public partial class InformationForm : Form
    {
        #region 变量定义 
        //查看当前电芯数据标识
        public bool RefreshChartFlag = true;
        //画面缩放
        public HWndCtrl HWndCtrl1;
        string CSV_FilePath = AppDomain.CurrentDomain.BaseDirectory + "CSV\\" + DateTime.Now.ToString("yyyy-MM-dd");
        string CSV_Directory = AppDomain.CurrentDomain.BaseDirectory + "CSV\\";
        string Path_NG_Pic = "";
        int num_of_NGpic = 0;
        HObject NG1_ho_Image = null;
        public delegate void RefreshInformationForm_Chart_Event_Delegate();//保存参数 委托                              
        public event RefreshInformationForm_Chart_Event_Delegate RefreshInformationForm_Chart_Event; //保存参数 委托事件
        #endregion


        /// <summary>
        /// 主函数
        /// </summary>
        public InformationForm()
        {
            InitializeComponent();
            //画面缩放实例化
            HWndCtrl1 = new HWndCtrl(H_WControl1);
            HWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
            HWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_MOVE);

        }


        /// <summary>
        /// 窗体加载初始化函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformationForm_Load(object sender, EventArgs e)
        {
            try
            {
                //ReadCSV_Filename(CSV_FilePath);
                //历史（按日期命名的）CSV文件             
                if (Directory.Exists(CSV_Directory))
                {
                    DirectoryInfo dir1 = new DirectoryInfo(CSV_Directory);
                    DirectoryInfo[] dii = dir1.GetDirectories();
                    for (int i = dii.Length - 1; i >= 0; i--)
                    {
                        CB_CSVFileName.Items.Add(dii[i].Name);

                    }
                    CB_CSVFileName.SelectedIndex = 0;
                }
            }

            catch (Exception)
            { }
        }


        /// <summary>
        /// 读取(按日期命名)历史生产的CSV文件名到程序listview1界面
        /// </summary>
        /// <param name="Filepath"></param>
        private void ReadCSV_Filename(String Filepath)
        {
            listView1.Items.Add("当前电芯数据");
            string OK_NG_StrFlag = "";
            //当日生产数据的CSV文件
            if (!Directory.Exists(Filepath))
            {
                Directory.CreateDirectory(Filepath);
            }
            DirectoryInfo dir = new DirectoryInfo(Filepath);
            FileInfo[] finfo = dir.GetFiles();
            int errorflag = 0;
            for (int i = finfo.Length - 1; i >= 0; i--)
            {
                string CSVFILEFLAG = finfo[i].Name.Substring(finfo[i].Name.Length - 3, 3);
                if (!CSVFILEFLAG.Equals("csv"))
                {
                    errorflag++;
                }
                else
                {
                    OK_NG_StrFlag = finfo[i].Name.Substring(finfo[i].Name.Length - 6, 2);
                    if (OK_NG_StrFlag == "NG")
                    {
                        listView1.Items.Add(finfo[i].Name);
                        listView1.Items[finfo.Length - i - errorflag].SubItems[0].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        listView1.Items.Add(finfo[i].Name);
                        listView1.Items[finfo.Length - i - errorflag].SubItems[0].ForeColor = System.Drawing.Color.Black;
                    }
                }
            }

        }


        /// <summary>
        /// 日期选择，更新listview1中的CSV文件名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_CSVFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //添加当日生产详细数据
            string CSV_File_IndexPath = AppDomain.CurrentDomain.BaseDirectory + "CSV\\" + CB_CSVFileName.Text;
            ReadCSV_Filename(CSV_File_IndexPath);
        }


        /// <summary>
        /// 单击打开生产详细数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            string filename = listView1.FocusedItem.SubItems[0].Text;//获取鼠标当前行
            double[] ChartMin_In = new double[5];
            double[] ChartMin_Out = new double[5];
            double Min_In = new double();
            double Min_Out = new double();
            //查看当前电芯数据
            if (filename == "当前电芯数据")
            {
                //触发回调函数更新当前电芯数据
                RefreshChartFlag = true;
                if (RefreshInformationForm_Chart_Event != null)
                    RefreshInformationForm_Chart_Event();
            }
            //查看历史生产电芯数据
            else
            {
                RefreshChartFlag = false;
                string Path_csv = CSV_Directory + "\\" + CB_CSVFileName.Text + "\\" + filename;
                Path_NG_Pic = AppDomain.CurrentDomain.BaseDirectory + "Screen\\" + CB_CSVFileName.Text + "\\" + filename.Substring(0, 8);
                foreach (var series in Chart_Inside.Series)
                {
                    series.Points.Clear();
                }
                foreach (var series in Chart_Outside.Series)
                {
                    series.Points.Clear();
                }
                if (System.IO.File.Exists(Path_csv))
                {
                    listView2.Items.Clear();
                    List<String[]> ls = CsvWrite.ReadCSV(Path_csv);


                    for (int i = 1; i < ls.Count; i++)
                    {
                        if (ls[i][0] != "")
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = ls[i][0];
                            lvi.SubItems.Add(ls[i][5]);
                            lvi.SubItems.Add(ls[i][6]);
                            lvi.SubItems.Add(ls[i][7]);
                            lvi.SubItems.Add(ls[i][8]);

                            lvi.SubItems.Add(ls[i][13]);
                            lvi.SubItems.Add(ls[i][14]);
                            lvi.SubItems.Add(ls[i][15]);
                            lvi.SubItems.Add(ls[i][16]);

                            lvi.SubItems.Add(ls[i][17]);
                            //lvi.SubItems.Add(path_csv);
                            listView2.BeginUpdate();
                            listView2.Items.Add(lvi);
                            //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                            listView2.EndUpdate();

                            double CD_In = Convert.ToDouble(ls[i][1]);
                            double AD_In = Convert.ToDouble(ls[i][2]);
                            double UpSD_In = Convert.ToDouble(ls[i][3]);
                            double DownSD_In = Convert.ToDouble(ls[i][4]);

                            double CD_Out = Convert.ToDouble(ls[i][9]);
                            double AD_Out = Convert.ToDouble(ls[i][10]);
                            double UpSD_Out = Convert.ToDouble(ls[i][11]);
                            double DownSD_Out = Convert.ToDouble(ls[i][12]);


                            Chart_Inside.Series["正极"].Points.AddXY(CD_In, i);
                            Chart_Inside.Series["负极"].Points.AddXY(AD_In, i);
                            Chart_Inside.Series["上隔膜"].Points.AddXY(UpSD_In, i);
                            Chart_Inside.Series["下隔膜"].Points.AddXY(DownSD_In, i);


                            Chart_Outside.Series["正极"].Points.AddXY(CD_Out, i);
                            Chart_Outside.Series["负极"].Points.AddXY(AD_Out, i);
                            Chart_Outside.Series["上隔膜"].Points.AddXY(UpSD_Out, i);
                            Chart_Outside.Series["下隔膜"].Points.AddXY(DownSD_Out, i);

                            if (i == 1)
                            {
                                ChartMin_In = new double[] { CD_In, AD_In, UpSD_In, DownSD_In };
                                ChartMin_Out = new double[] { CD_Out, AD_Out, UpSD_Out, DownSD_Out };
                                Min_In = ChartMin_In.Min();
                                Min_Out = ChartMin_Out.Min();
                            }
                            else
                            {
                                ChartMin_In = new double[] { CD_In, AD_In, UpSD_In, DownSD_In, Min_In };
                                ChartMin_Out = new double[] { CD_Out, AD_Out, UpSD_Out, DownSD_Out, Min_Out };
                                Min_In = ChartMin_In.Min();
                                Min_Out = ChartMin_Out.Min();
                            }

                            Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
                            Chart_Inside.ChartAreas[0].AxisY.MajorTickMark.Interval = 5;
                            Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                            Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;

                            Chart_Inside.ChartAreas[0].AxisX.Minimum = Convert.ToInt16(Min_In) - 1;
                            Chart_Inside.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
                            Chart_Inside.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                            Chart_Inside.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                            Chart_Inside.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;


                            Chart_Outside.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
                            Chart_Outside.ChartAreas[0].AxisY.MajorTickMark.Interval = 5;
                            Chart_Outside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                            Chart_Outside.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                            Chart_Outside.ChartAreas[0].AxisX.Minimum = Convert.ToInt16(Min_Out) - 1;
                            Chart_Outside.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
                            Chart_Outside.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                            Chart_Outside.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                            Chart_Outside.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
                        }
                        else
                            i = ls.Count;
                    }
                }
                else
                {
                }
                //显示对应的NG图片
                if (Directory.Exists(Path_NG_Pic))
                {
                    var files = Directory.GetFiles(Path_NG_Pic, "*.png");
                    if (files.Length != 0)
                    {
                        NG1_ho_Image = new HImage(files[0]);

                        //画面缩放
                        HWndCtrl1.addIconicVar(NG1_ho_Image);
                        HWndCtrl1.resetAll();
                        HWndCtrl1.repaint();
                        Index_Pic.Text = "1/" + files.Length.ToString();
                        Btn_nextpic.Enabled = true;
                    }
                    else
                    {
                        HOperatorSet.ClearWindow(H_WControl1.HalconID);
                        Index_Pic.Text = "0/0";
                    }
                }
                else
                {
                    HOperatorSet.ClearWindow(H_WControl1.HalconID);
                    Index_Pic.Text = "0/0";
                }
            }
        }


        /// <summary>
        /// 图像坐标和灰度值显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HWControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (NG1_ho_Image != null)
                try
                {
                    GrayPosition1.Text = HWndCtrl1.Get_Mouseposition_and_Gray(H_WControl1.HalconID, NG1_ho_Image, e);
                }
                catch (Exception)
                {

                }
        }


        #region buttun按钮单击事件
        private void Btn_NextPic_Click(object sender, EventArgs e)
        {
            num_of_NGpic++;
            var files = Directory.GetFiles(Path_NG_Pic, "*.png");
            if (Directory.Exists(Path_NG_Pic))
            {
                int length = files.Length;

                if (num_of_NGpic >= length)
                {
                    num_of_NGpic = 0;
                }

                //HOperatorSet.ReadImage(out NG1_ho_Image, files[num_of_NGpic]);
                NG1_ho_Image = new HImage(files[num_of_NGpic]);
                //画面缩放
                HWndCtrl1.addIconicVar(NG1_ho_Image);
                HWndCtrl1.repaint();
                Index_Pic.Text = (num_of_NGpic + 1).ToString() + "/" + length.ToString();
            }

        }


        private void Btn_PreviousPic_Click(object sender, EventArgs e)
        {
            num_of_NGpic--;

            var files = Directory.GetFiles(Path_NG_Pic, "*.png");
            if (Directory.Exists(Path_NG_Pic))
            {
                int length = files.Length;

                if (num_of_NGpic < 0)
                {
                    num_of_NGpic = length - 1;
                }

                //HOperatorSet.ReadImage(out NG1_ho_Image, files[num_of_NGpic]);
                NG1_ho_Image = new HImage(files[num_of_NGpic]);
                //画面缩放
                HWndCtrl1.addIconicVar(NG1_ho_Image);
                HWndCtrl1.repaint();
                Index_Pic.Text = (num_of_NGpic + 1).ToString() + "/" + length.ToString();
            }
        }


        private void ResetImage_Click(object sender, EventArgs e)
        {
            //重置显示画面
            HWndCtrl1.resetAll();
            HWndCtrl1.repaint();
        }
        #endregion

    }
}
