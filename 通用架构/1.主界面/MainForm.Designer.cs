namespace 通用架构
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainFormPanel = new System.Windows.Forms.Panel();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.LoginItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginInform = new System.Windows.Forms.ToolStripMenuItem();
            this.TextInform1 = new System.Windows.Forms.ToolStripMenuItem();
            this.TextInform2 = new System.Windows.Forms.ToolStripMenuItem();
            this.TextInform3 = new System.Windows.Forms.ToolStripMenuItem();
            this.分割线1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckInfoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckInfoItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeParaItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeParaItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraParaItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CameraParaItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpItem_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckStopItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckStartItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFormPanel
            // 
            this.MainFormPanel.AutoSize = true;
            this.MainFormPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainFormPanel.BackColor = System.Drawing.Color.White;
            this.MainFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainFormPanel.Location = new System.Drawing.Point(153, 0);
            this.MainFormPanel.Name = "MainFormPanel";
            this.MainFormPanel.Size = new System.Drawing.Size(1111, 784);
            this.MainFormPanel.TabIndex = 1;
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.SystemColors.Control;
            this.MainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainMenu.BackgroundImage")));
            this.MainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MainMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginItem,
            this.LoginInform,
            this.TextInform1,
            this.TextInform2,
            this.TextInform3,
            this.分割线1,
            this.CheckItem,
            this.CheckItem_Active,
            this.CheckInfoItem,
            this.CheckInfoItem_Active,
            this.ChangeParaItem,
            this.ChangeParaItem_Active,
            this.CameraParaItem,
            this.CameraParaItem_Active,
            this.ConnectItem,
            this.ConnectItem_Active,
            this.HelpItem,
            this.HelpItem_Active,
            this.ExitItem,
            this.CheckStopItem,
            this.CheckStartItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(153, 784);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // LoginItem
            // 
            this.LoginItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.LoginItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LoginItem.DoubleClickEnabled = true;
            this.LoginItem.Image = ((System.Drawing.Image)(resources.GetObject("LoginItem.Image")));
            this.LoginItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.LoginItem.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.LoginItem.Name = "LoginItem";
            this.LoginItem.Size = new System.Drawing.Size(140, 119);
            // 
            // LoginInform
            // 
            this.LoginInform.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoginInform.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LoginInform.Name = "LoginInform";
            this.LoginInform.Size = new System.Drawing.Size(140, 23);
            this.LoginInform.Text = "用户名：未登录";
            // 
            // TextInform1
            // 
            this.TextInform1.Name = "TextInform1";
            this.TextInform1.Size = new System.Drawing.Size(140, 21);
            this.TextInform1.Text = "当日产量：0";
            // 
            // TextInform2
            // 
            this.TextInform2.Name = "TextInform2";
            this.TextInform2.Size = new System.Drawing.Size(140, 21);
            this.TextInform2.Text = "良品数：0";
            // 
            // TextInform3
            // 
            this.TextInform3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.TextInform3.Name = "TextInform3";
            this.TextInform3.Size = new System.Drawing.Size(140, 21);
            this.TextInform3.Text = "合格率：100%";
            // 
            // 分割线1
            // 
            this.分割线1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("分割线1.BackgroundImage")));
            this.分割线1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.分割线1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.分割线1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.分割线1.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.分割线1.Name = "分割线1";
            this.分割线1.Padding = new System.Windows.Forms.Padding(0);
            this.分割线1.Size = new System.Drawing.Size(136, 4);
            // 
            // CheckItem
            // 
            this.CheckItem.Image = ((System.Drawing.Image)(resources.GetObject("CheckItem.Image")));
            this.CheckItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckItem.Name = "CheckItem";
            this.CheckItem.Padding = new System.Windows.Forms.Padding(0, 9, 87, 0);
            this.CheckItem.Size = new System.Drawing.Size(140, 70);
            this.CheckItem.Text = "检测画面";
            this.CheckItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CheckItem.Visible = false;
            this.CheckItem.Click += new System.EventHandler(this.CheckItem_Click);
            // 
            // CheckItem_Active
            // 
            this.CheckItem_Active.BackgroundImage = global::通用架构.Properties.Resources.菜单_红_;
            this.CheckItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckItem_Active.ForeColor = System.Drawing.Color.White;
            this.CheckItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("CheckItem_Active.Image")));
            this.CheckItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckItem_Active.Name = "CheckItem_Active";
            this.CheckItem_Active.Padding = new System.Windows.Forms.Padding(0, 9, 87, 0);
            this.CheckItem_Active.Size = new System.Drawing.Size(140, 70);
            this.CheckItem_Active.Text = "检测画面";
            this.CheckItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CheckItem_Active.MouseHover += new System.EventHandler(this.CheckItem_Active_MouseHover);
            // 
            // CheckInfoItem
            // 
            this.CheckInfoItem.Image = ((System.Drawing.Image)(resources.GetObject("CheckInfoItem.Image")));
            this.CheckInfoItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckInfoItem.Name = "CheckInfoItem";
            this.CheckInfoItem.Padding = new System.Windows.Forms.Padding(2, 11, 85, 0);
            this.CheckInfoItem.Size = new System.Drawing.Size(140, 70);
            this.CheckInfoItem.Text = "信息统计";
            this.CheckInfoItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckInfoItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CheckInfoItem.Click += new System.EventHandler(this.CheckInfoItem_Click);
            // 
            // CheckInfoItem_Active
            // 
            this.CheckInfoItem_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckInfoItem_Active.BackgroundImage")));
            this.CheckInfoItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckInfoItem_Active.ForeColor = System.Drawing.Color.White;
            this.CheckInfoItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("CheckInfoItem_Active.Image")));
            this.CheckInfoItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckInfoItem_Active.Name = "CheckInfoItem_Active";
            this.CheckInfoItem_Active.Padding = new System.Windows.Forms.Padding(0, 11, 85, 0);
            this.CheckInfoItem_Active.Size = new System.Drawing.Size(140, 70);
            this.CheckInfoItem_Active.Text = "信息统计";
            this.CheckInfoItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckInfoItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CheckInfoItem_Active.Visible = false;
            // 
            // ChangeParaItem
            // 
            this.ChangeParaItem.Image = ((System.Drawing.Image)(resources.GetObject("ChangeParaItem.Image")));
            this.ChangeParaItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ChangeParaItem.Name = "ChangeParaItem";
            this.ChangeParaItem.Padding = new System.Windows.Forms.Padding(2, 10, 85, 0);
            this.ChangeParaItem.Size = new System.Drawing.Size(140, 70);
            this.ChangeParaItem.Text = "参数更改";
            this.ChangeParaItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ChangeParaItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ChangeParaItem.Click += new System.EventHandler(this.ChangeParaItem_Click);
            // 
            // ChangeParaItem_Active
            // 
            this.ChangeParaItem_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ChangeParaItem_Active.BackgroundImage")));
            this.ChangeParaItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ChangeParaItem_Active.ForeColor = System.Drawing.Color.White;
            this.ChangeParaItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("ChangeParaItem_Active.Image")));
            this.ChangeParaItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ChangeParaItem_Active.Name = "ChangeParaItem_Active";
            this.ChangeParaItem_Active.Padding = new System.Windows.Forms.Padding(0, 10, 85, 0);
            this.ChangeParaItem_Active.Size = new System.Drawing.Size(140, 70);
            this.ChangeParaItem_Active.Text = "参数更改";
            this.ChangeParaItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ChangeParaItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ChangeParaItem_Active.Visible = false;
            // 
            // CameraParaItem
            // 
            this.CameraParaItem.Image = ((System.Drawing.Image)(resources.GetObject("CameraParaItem.Image")));
            this.CameraParaItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CameraParaItem.Name = "CameraParaItem";
            this.CameraParaItem.Padding = new System.Windows.Forms.Padding(0, 15, 85, 0);
            this.CameraParaItem.Size = new System.Drawing.Size(140, 70);
            this.CameraParaItem.Text = "拍照设置";
            this.CameraParaItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CameraParaItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CameraParaItem.Click += new System.EventHandler(this.CameraParaItem_Click);
            // 
            // CameraParaItem_Active
            // 
            this.CameraParaItem_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CameraParaItem_Active.BackgroundImage")));
            this.CameraParaItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CameraParaItem_Active.ForeColor = System.Drawing.Color.White;
            this.CameraParaItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("CameraParaItem_Active.Image")));
            this.CameraParaItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CameraParaItem_Active.Name = "CameraParaItem_Active";
            this.CameraParaItem_Active.Padding = new System.Windows.Forms.Padding(0, 15, 85, 0);
            this.CameraParaItem_Active.Size = new System.Drawing.Size(140, 70);
            this.CameraParaItem_Active.Text = "拍照设置";
            this.CameraParaItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CameraParaItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CameraParaItem_Active.Visible = false;
            // 
            // ConnectItem
            // 
            this.ConnectItem.Image = ((System.Drawing.Image)(resources.GetObject("ConnectItem.Image")));
            this.ConnectItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ConnectItem.Name = "ConnectItem";
            this.ConnectItem.Padding = new System.Windows.Forms.Padding(0, 9, 87, 0);
            this.ConnectItem.Size = new System.Drawing.Size(140, 70);
            this.ConnectItem.Text = "通讯设置";
            this.ConnectItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ConnectItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ConnectItem.Click += new System.EventHandler(this.ConnectItem_Click);
            // 
            // ConnectItem_Active
            // 
            this.ConnectItem_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ConnectItem_Active.BackgroundImage")));
            this.ConnectItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ConnectItem_Active.ForeColor = System.Drawing.Color.White;
            this.ConnectItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("ConnectItem_Active.Image")));
            this.ConnectItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ConnectItem_Active.Name = "ConnectItem_Active";
            this.ConnectItem_Active.Padding = new System.Windows.Forms.Padding(0, 9, 87, 0);
            this.ConnectItem_Active.Size = new System.Drawing.Size(140, 70);
            this.ConnectItem_Active.Text = "通讯设置";
            this.ConnectItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ConnectItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ConnectItem_Active.Visible = false;
            // 
            // HelpItem
            // 
            this.HelpItem.Image = ((System.Drawing.Image)(resources.GetObject("HelpItem.Image")));
            this.HelpItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.HelpItem.Name = "HelpItem";
            this.HelpItem.Padding = new System.Windows.Forms.Padding(0, 14, 106, 0);
            this.HelpItem.Size = new System.Drawing.Size(140, 70);
            this.HelpItem.Text = "帮助";
            this.HelpItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpItem.Click += new System.EventHandler(this.HelpItem_Click);
            // 
            // HelpItem_Active
            // 
            this.HelpItem_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HelpItem_Active.BackgroundImage")));
            this.HelpItem_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HelpItem_Active.ForeColor = System.Drawing.Color.White;
            this.HelpItem_Active.Image = ((System.Drawing.Image)(resources.GetObject("HelpItem_Active.Image")));
            this.HelpItem_Active.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.HelpItem_Active.Name = "HelpItem_Active";
            this.HelpItem_Active.Padding = new System.Windows.Forms.Padding(0, 14, 106, 0);
            this.HelpItem_Active.Size = new System.Drawing.Size(140, 70);
            this.HelpItem_Active.Text = "帮助";
            this.HelpItem_Active.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpItem_Active.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpItem_Active.Visible = false;
            // 
            // ExitItem
            // 
            this.ExitItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ExitItem.Image = ((System.Drawing.Image)(resources.GetObject("ExitItem.Image")));
            this.ExitItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ExitItem.Margin = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.ExitItem.Name = "ExitItem";
            this.ExitItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 14);
            this.ExitItem.Size = new System.Drawing.Size(140, 55);
            this.ExitItem.Click += new System.EventHandler(this.ExitItem_Click);
            // 
            // CheckStopItem
            // 
            this.CheckStopItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CheckStopItem.Image = ((System.Drawing.Image)(resources.GetObject("CheckStopItem.Image")));
            this.CheckStopItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckStopItem.Name = "CheckStopItem";
            this.CheckStopItem.Padding = new System.Windows.Forms.Padding(0, 0, 0, 11);
            this.CheckStopItem.Size = new System.Drawing.Size(140, 55);
            this.CheckStopItem.Visible = false;
            this.CheckStopItem.Click += new System.EventHandler(this.CheckStopItem_Click);
            // 
            // CheckStartItem
            // 
            this.CheckStartItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CheckStartItem.Image = ((System.Drawing.Image)(resources.GetObject("CheckStartItem.Image")));
            this.CheckStartItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CheckStartItem.Name = "CheckStartItem";
            this.CheckStartItem.Padding = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.CheckStartItem.Size = new System.Drawing.Size(140, 55);
            this.CheckStartItem.Click += new System.EventHandler(this.CheckStartItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 784);
            this.ControlBox = false;
            this.Controls.Add(this.MainFormPanel);
            this.Controls.Add(this.MainMenu);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem LoginItem;
        private System.Windows.Forms.ToolStripMenuItem LoginInform;
        private System.Windows.Forms.ToolStripMenuItem TextInform1;
        private System.Windows.Forms.ToolStripMenuItem TextInform2;
        private System.Windows.Forms.ToolStripMenuItem TextInform3;
        private System.Windows.Forms.ToolStripMenuItem 分割线1;
        private System.Windows.Forms.ToolStripMenuItem CheckItem;
        private System.Windows.Forms.ToolStripMenuItem CheckItem_Active;
        private System.Windows.Forms.ToolStripMenuItem CheckInfoItem;
        private System.Windows.Forms.ToolStripMenuItem CheckInfoItem_Active;
        private System.Windows.Forms.ToolStripMenuItem ChangeParaItem;
        private System.Windows.Forms.ToolStripMenuItem ChangeParaItem_Active;
        private System.Windows.Forms.ToolStripMenuItem CameraParaItem;
        private System.Windows.Forms.ToolStripMenuItem CameraParaItem_Active;
        private System.Windows.Forms.ToolStripMenuItem ConnectItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectItem_Active;
        private System.Windows.Forms.ToolStripMenuItem HelpItem;
        private System.Windows.Forms.ToolStripMenuItem HelpItem_Active;
        private System.Windows.Forms.Panel MainFormPanel;
        private System.Windows.Forms.ToolStripMenuItem ExitItem;
        private System.Windows.Forms.ToolStripMenuItem CheckStopItem;
        private System.Windows.Forms.ToolStripMenuItem CheckStartItem;
    }
}

