using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 通用架构._2.子界面;

namespace 通用架构
{
    public partial class SubForm : Form
    {
        /// <summary>
        ///变量定义
        /// </summary>
        #region
        private int SubMenuItemNum;  //主菜单按钮序号
        public CheckForm_2Views CheckForm_Pos1;
        public CheckForm_4Views CheckForm_Pos2;

        #endregion


        /// <summary>
        /// 子窗口主函数
        /// </summary>
        public SubForm()
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
            SubMenu.Renderer = new MyRenderer();
            //

            //实例化：工位1检测画面
            CheckForm_Pos1 = new CheckForm_2Views();
            CheckForm_Pos1.TopLevel = false;
            CheckForm_Pos1.Dock = DockStyle.Fill;
            CheckForm_Pos1.FormBorderStyle = FormBorderStyle.None;
            CheckForm_Pos1.Size = SubFormPanel.Size;
            SubFormPanel.Controls.Add(CheckForm_Pos1);
            CheckForm_Pos1.Show();
            //

            //实例化：工位2检测画面
            CheckForm_Pos2 = new CheckForm_4Views();
            CheckForm_Pos2.TopLevel = false;
            CheckForm_Pos2.Dock = DockStyle.Fill;
            CheckForm_Pos2.FormBorderStyle = FormBorderStyle.None;
            CheckForm_Pos2.Size = SubFormPanel.Size;
            SubFormPanel.Controls.Add(CheckForm_Pos2);
            CheckForm_Pos2.Show();
            //

        }

        ///<summary>
        ///鼠标悬停菜单改为透明
        ///</summary>
        #region
        private class MyRenderer : ToolStripProfessionalRenderer { public MyRenderer() : base(new MyColors()) { } }
        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected { get { return Color.Transparent; } }
            public override Color MenuItemSelectedGradientBegin { get { return Color.Transparent; } }
            public override Color MenuItemSelectedGradientEnd { get { return Color.Transparent; } }
            public override Color MenuItemBorder { get { return Color.Transparent; } }
        }
        #endregion


        ///<summary>
        ///子菜单按钮激活，按钮显示变为红色
        ///</summary>
        private void SubMenuItemActive()
        {
            try
            {
                switch (SubMenuItemNum)
                {
                    case 1:
                        this.CheckPos1Item.Visible = false;
                        this.CheckPos1Item_Active.Visible = true;

                        this.CheckPos2Item.Visible = true;
                        this.CheckPos2Item_Active.Visible = false;

                        this.CheckPos3Item.Visible = true;
                        this.CheckPos3Item_Active.Visible = false;

                        break;
                    case 2:

                        this.CheckPos2Item.Visible = false;
                        this.CheckPos2Item_Active.Visible = true;

                        this.CheckPos1Item.Visible = true;
                        this.CheckPos1Item_Active.Visible = false;

                        this.CheckPos3Item.Visible = true;
                        this.CheckPos3Item_Active.Visible = false;
                        break;
                    case 3:

                        this.CheckPos3Item.Visible = false;
                        this.CheckPos3Item_Active.Visible = true;

                        this.CheckPos1Item.Visible = true;
                        this.CheckPos1Item_Active.Visible = false;

                        this.CheckPos2Item.Visible = true;
                        this.CheckPos2Item_Active.Visible = false;

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
        private void CheckPos1Item_Click(object sender, EventArgs e)
        {
            SubMenuItemNum = 1;
            SubMenuItemActive();
            SubMenuItemNum = 0;

            SubFormPanel.Controls.Clear();
            CheckForm_Pos1.Show();
            SubFormPanel.Controls.Add(CheckForm_Pos1);
        }

        private void CheckPos2Item_Click(object sender, EventArgs e)
        {
            SubMenuItemNum = 2;
            SubMenuItemActive();
            SubMenuItemNum = 0;

            SubFormPanel.Controls.Clear();
            CheckForm_Pos2.Show();
            SubFormPanel.Controls.Add(CheckForm_Pos2);
        }

        private void CheckPos3Item_Click(object sender, EventArgs e)
        {
            SubMenuItemNum = 3;
            SubMenuItemActive();
            SubMenuItemNum = 0;

            SubFormPanel.Controls.Clear();
        }
        #endregion


    }
}
