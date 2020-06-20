using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using 通用架构._2.子界面;

namespace 通用架构
{
    public partial class MainForm : Form
    {
        /// <summary>
        ///变量定义
        /// </summary>
        #region
        private int MainMenuItemNum;  //主菜单按钮序号
        SubForm SubForm_Check;        //检测画面子菜单
        InformationForm InformationForm;     //信息统计画面菜单
        ChangeParaForm ChangeParaForm;        //检测画面子菜单

        #endregion


        /// <summary>
        /// 主函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Initialization();
        }


        ///<summary>
        ///程序初始化
        ///</summary>
        public void Initialization()
        {
            //鼠标悬停菜单透明
            MainMenu.Renderer = new MyRenderer();
            //

            //实例化：检测画面子菜单
            SubForm_Check = new SubForm();
            SubForm_Check.TopLevel = false;
            SubForm_Check.Dock = DockStyle.Fill;
            SubForm_Check.FormBorderStyle = FormBorderStyle.None;
            SubForm_Check.Size = MainFormPanel.Size;
            MainFormPanel.Controls.Add(SubForm_Check);
            SubForm_Check.Show();

            //实例化：检测画面子菜单
            InformationForm = new InformationForm();
            InformationForm.TopLevel = false;
            InformationForm.Dock = DockStyle.Fill;
            InformationForm.FormBorderStyle = FormBorderStyle.None;
            InformationForm.Size = MainFormPanel.Size;
            MainFormPanel.Controls.Add(InformationForm);
            InformationForm.Show();

            //实例化：参数更改画面
            ChangeParaForm = new ChangeParaForm();
            ChangeParaForm.TopLevel = false;
            ChangeParaForm.Dock = DockStyle.Fill;
            ChangeParaForm.FormBorderStyle = FormBorderStyle.None;
            ChangeParaForm.Size = MainFormPanel.Size;
            MainFormPanel.Controls.Add(ChangeParaForm);
            ChangeParaForm.Show();

            //参数读写
            ChangeParaForm.LineDetect.SaveParamsEvent += new LineDetect.SaveParamsDelegate(() => { SubForm_Check.CheckForm_Pos2.ReadParams(); });
            ChangeParaForm.InspectionStandard.SaveParamsEvent += new InspectionStandard.SaveParamsDelegate(() => { SubForm_Check.CheckForm_Pos2.ReadParams(); });

            //InformationForm刷新
            SubForm_Check.CheckForm_Pos2.RefreshInformationForm_Event += new CheckForm_4Views.RefreshInformationForm_Delegate(RefreshInformationForm);
            SubForm_Check.CheckForm_Pos2.RefreshInformationForm_Chart_Event += new CheckForm_4Views.RefreshInformationForm_Chart_Event_Delegate(RefreshInformationForm_Chart);

            InformationForm.RefreshInformationForm_Chart_Event += new InformationForm.RefreshInformationForm_Chart_Event_Delegate(RefreshInformationForm_Chart);

            //生产数据更新
            SubForm_Check.CheckForm_Pos2.ProductionData_Event += new CheckForm_4Views.ProductionData_Delegate(RefreshProductionData);

        }


        /// <summary>
        /// 更新信息统计界面
        /// </summary>
        public void RefreshInformationForm()
        {
            InformationForm.Invoke(new MethodInvoker(delegate
            {
                InformationForm.listView1.Items.Insert(1, SubForm_Check.CheckForm_Pos2.CsvName + ".csv");
                if (!SubForm_Check.CheckForm_Pos2.TotalResult)
                    InformationForm.listView1.Items[1].ForeColor = System.Drawing.Color.Red;
                else
                    InformationForm.listView1.Items[1].ForeColor = System.Drawing.Color.Black;
            }));
        }

        /// <summary>
        /// 更新当前生产中的电芯数据到信息统计界面
        /// </summary>
        public void RefreshInformationForm_Chart()
        {
            if (InformationForm.RefreshChartFlag == true)
            {
                //清除生产详细数据和查看的历史NG图片显示
                InformationForm.listView2.Items.Clear();
                HOperatorSet.ClearWindow(InformationForm.H_WControl1.HalconID);
                InformationForm.Index_Pic.Text = "0/0";
                InformationForm.Invoke(new MethodInvoker(delegate
                {
                    int i = 0;
                    double[] ChartMin_In = new double[5];
                    double[] ChartMin_Out = new double[5];
                    double Min_In = new double();
                    double Min_Out = new double();
                    foreach (var series in InformationForm.Chart_Inside.Series)
                    {
                        series.Points.Clear();
                    }
                    foreach (var series in InformationForm.Chart_Outside.Series)
                    {
                        series.Points.Clear();
                    }
                    for (i = 0; i < SubForm_Check.CheckForm_Pos2.DetectionNum; i++)
                    {

                        double CD_In = SubForm_Check.CheckForm_Pos2.DetectParam1_1.List_CD[i];
                        double AD_In = SubForm_Check.CheckForm_Pos2.DetectParam1_2.List_AD[i];
                        double UpSD_In = SubForm_Check.CheckForm_Pos2.DetectParam1_1.List_UpSD[i];
                        double DownSD_In = SubForm_Check.CheckForm_Pos2.DetectParam1_2.List_DownSD[i];

                        double CD_Out = SubForm_Check.CheckForm_Pos2.DetectParam2_1.List_CD[i];
                        double AD_Out = SubForm_Check.CheckForm_Pos2.DetectParam2_2.List_AD[i];
                        double UpSD_Out = SubForm_Check.CheckForm_Pos2.DetectParam2_1.List_UpSD[i];
                        double DownSD_Out = SubForm_Check.CheckForm_Pos2.DetectParam2_2.List_DownSD[i];

                        InformationForm.Chart_Inside.Series["正极"].Points.AddXY(CD_In, i + 1);
                        InformationForm.Chart_Inside.Series["负极"].Points.AddXY(AD_In, i + 1);
                        InformationForm.Chart_Inside.Series["上隔膜"].Points.AddXY(UpSD_In, i + 1);
                        InformationForm.Chart_Inside.Series["下隔膜"].Points.AddXY(DownSD_In, i + 1);


                        InformationForm.Chart_Outside.Series["正极"].Points.AddXY(CD_Out, i + 1);
                        InformationForm.Chart_Outside.Series["负极"].Points.AddXY(AD_Out, i + 1);
                        InformationForm.Chart_Outside.Series["上隔膜"].Points.AddXY(UpSD_Out, i + 1);
                        InformationForm.Chart_Outside.Series["下隔膜"].Points.AddXY(DownSD_Out, i + 1);

                        if (i == 0)
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
                        InformationForm.Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisY.MajorTickMark.Interval = 5;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;

                        InformationForm.Chart_Inside.ChartAreas[0].AxisX.Minimum = Convert.ToInt16(Min_In) - 1;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                        InformationForm.Chart_Inside.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;


                        InformationForm.Chart_Outside.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisY.MajorTickMark.Interval = 5;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                        InformationForm.Chart_Outside.ChartAreas[0].AxisX.Minimum = Convert.ToInt16(Min_Out) - 1;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                        InformationForm.Chart_Outside.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
                    }


                }));
            }
        }


        private void RefreshProductionData()
        {
            TextInform1.Text = ("当日产量：" + SubForm_Check.CheckForm_Pos2.TotalNum.ToString());
            TextInform2.Text = ("良品数：" + SubForm_Check.CheckForm_Pos2.OKNum.ToString());
            TextInform3.Text = ("合格率：" + (SubForm_Check.CheckForm_Pos2.OKRate * 100).ToString("0.##") + "%");
        }


        ///<summary>
        ///窗体切换，防止闪烁
        ///</summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }


        ///<summary>
        ///鼠标悬停菜单改为透明
        ///</summary>
        #region
        private class MyRenderer : ToolStripProfessionalRenderer { public MyRenderer() : base(new MyColors()) { } }
        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected { get { return Color.LightCoral; } }
            public override Color MenuItemSelectedGradientBegin { get { return Color.LightCoral; } }
            public override Color MenuItemSelectedGradientEnd { get { return Color.LightCoral; } }
            public override Color MenuItemBorder { get { return Color.LightCoral; } }
        }
        #endregion


        ///<summary>
        ///主菜单按钮激活，按钮显示变为红色
        ///</summary>
        private void MainMenuItemActive()
        {
            try
            {
                this.CheckItem.Visible = true;
                this.CheckItem_Active.Visible = false;

                this.CheckInfoItem.Visible = true;
                this.CheckInfoItem_Active.Visible = false;

                this.ChangeParaItem.Visible = true;
                this.ChangeParaItem_Active.Visible = false;

                this.CameraParaItem.Visible = true;
                this.CameraParaItem_Active.Visible = false;

                this.ConnectItem.Visible = true;
                this.ConnectItem_Active.Visible = false;

                this.HelpItem.Visible = true;
                this.HelpItem_Active.Visible = false;

                switch (MainMenuItemNum)
                {
                    case 1:
                        this.CheckItem.Visible = false;
                        this.CheckItem_Active.Visible = true;
                        break;
                    case 2:
                        this.CheckInfoItem.Visible = false;
                        this.CheckInfoItem_Active.Visible = true;
                        break;
                    case 3:
                        this.ChangeParaItem.Visible = false;
                        this.ChangeParaItem_Active.Visible = true;
                        break;
                    case 4:
                        this.CameraParaItem.Visible = false;
                        this.CameraParaItem_Active.Visible = true;
                        break;
                    case 5:
                        this.ConnectItem.Visible = false;
                        this.ConnectItem_Active.Visible = true;
                        break;
                    case 6:
                        this.HelpItem.Visible = false;
                        this.HelpItem_Active.Visible = true;
                        break;

                }
            }
            catch
            { }
        }


        ///<summary>
        ///主菜单功能按钮，子界面调用
        ///</summary>
        #region
        private void CheckItem_Click(object sender, EventArgs e)
        {
            //按钮激活：检测画面按钮
            MainMenuItemNum = 1;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：检测画面子窗口
            MainFormPanel.Controls.Clear();
            SubForm_Check.Show();
            MainFormPanel.Controls.Add(SubForm_Check);

        }

        private void CheckInfoItem_Click(object sender, EventArgs e)
        {
            //按钮激活：信息统计按钮
            MainMenuItemNum = 2;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：信息统计子窗口
            MainFormPanel.Controls.Clear();
            InformationForm.Show();
            MainFormPanel.Controls.Add(InformationForm);


        }

        private void ChangeParaItem_Click(object sender, EventArgs e)
        {
            //按钮激活：参数更改按钮
            MainMenuItemNum = 3;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：参数更改子窗口
            MainFormPanel.Controls.Clear();
            ChangeParaForm.Show();
            MainFormPanel.Controls.Add(ChangeParaForm);
        }

        private void CameraParaItem_Click(object sender, EventArgs e)
        {
            //按钮激活：拍照设置按钮
            MainMenuItemNum = 4;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：拍照设置窗口
            MainFormPanel.Controls.Clear();
        }

        private void ConnectItem_Click(object sender, EventArgs e)
        {
            //按钮激活：通讯设置按钮
            MainMenuItemNum = 5;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：通讯设置窗口
            MainFormPanel.Controls.Clear();
        }

        private void HelpItem_Click(object sender, EventArgs e)
        {
            //按钮激活：帮助按钮
            MainMenuItemNum = 6;
            MainMenuItemActive();
            MainMenuItemNum = 0;

            //子窗口调用：帮助窗口
            MainFormPanel.Controls.Clear();
        }
        private void CheckStartItem_Click(object sender, EventArgs e)
        {
            //按钮激活：开始检测按钮
            CheckStartItem.Visible = false;
            CheckStopItem.Visible = true;
        }
        private void CheckStopItem_Click(object sender, EventArgs e)
        {
            //按钮激活：停止检测按钮
            CheckStopItem.Visible = false;
            CheckStartItem.Visible = true;
        }
        private void ExitItem_Click(object sender, EventArgs e)
        {
            SubForm_Check.CheckForm_Pos2.DetectParam1_1.CameraBasler.CloseCam();
            SubForm_Check.CheckForm_Pos2.DetectParam1_2.CameraBasler.CloseCam();
            SubForm_Check.CheckForm_Pos2.DetectParam2_1.CameraBasler.CloseCam();
            SubForm_Check.CheckForm_Pos2.DetectParam2_2.CameraBasler.CloseCam();

            //退出主程序
            Environment.Exit(0);
        }


        #endregion

        private void CheckItem_Active_MouseHover(object sender, EventArgs e)
        {
            //this.CheckItem_Active.Image = global::通用架构.Properties.Resources.显示;
            //this.CheckItem_Active.ForeColor = System.Drawing.Color.Black; 
        }
    }
}
