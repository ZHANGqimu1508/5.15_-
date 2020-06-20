using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 通用架构._2.子界面
{
    public partial class CheckForm_2Views : Form
    {
        public CheckForm_2Views()
        {
            InitializeComponent();

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
        ///窗体控件重绘，防止闪烁
        ///</summary>
        #region
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SendMessage(this.TLPanel_Bottom.Handle, WM_SETREDRAW, 0, IntPtr.Zero);//禁止重绘
            if (this.checkBox1.Checked == true)
            {
                this.label1.Visible = false;
                this.TLPanel_Frame1.SetColumnSpan(this.TLPanel_BGM1, 10);
                this.TLPanel_Frame2.SetColumnSpan(this.TLPanel_BGM2, 10);
            }
            else if (this.checkBox1.Checked == false)
            {
                this.TLPanel_Frame1.SetColumnSpan(this.TLPanel_BGM1, 8);
                this.TLPanel_Frame2.SetColumnSpan(this.TLPanel_BGM2, 8);
                this.label1.Visible = true;
            }
            SendMessage(TLPanel_Bottom.Handle, WM_SETREDRAW, 1, IntPtr.Zero);//取消禁止
            TLPanel_Bottom.Refresh();
        }
    }
}
