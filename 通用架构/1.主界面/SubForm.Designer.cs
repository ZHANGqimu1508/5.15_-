namespace 通用架构
{
    partial class SubForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubForm));
            this.SubMenu = new System.Windows.Forms.MenuStrip();
            this.CheckPos1Item = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPos1Item_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPos2Item = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPos2Item_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPos3Item = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckPos3Item_Active = new System.Windows.Forms.ToolStripMenuItem();
            this.SubFormPanel = new System.Windows.Forms.Panel();
            this.SubMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SubMenu
            // 
            this.SubMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SubMenu.BackgroundImage")));
            this.SubMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SubMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckPos1Item,
            this.CheckPos1Item_Active,
            this.CheckPos2Item,
            this.CheckPos2Item_Active,
            this.CheckPos3Item,
            this.CheckPos3Item_Active});
            this.SubMenu.Location = new System.Drawing.Point(0, 0);
            this.SubMenu.Name = "SubMenu";
            this.SubMenu.Size = new System.Drawing.Size(1095, 60);
            this.SubMenu.TabIndex = 0;
            this.SubMenu.Text = "menuStrip1";
            // 
            // CheckPos1Item
            // 
            this.CheckPos1Item.Font = new System.Drawing.Font("Adobe 黑体 Std R", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CheckPos1Item.ForeColor = System.Drawing.Color.DimGray;
            this.CheckPos1Item.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos1Item.Margin = new System.Windows.Forms.Padding(30, 5, 30, 7);
            this.CheckPos1Item.Name = "CheckPos1Item";
            this.CheckPos1Item.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.CheckPos1Item.Size = new System.Drawing.Size(131, 44);
            this.CheckPos1Item.Text = "工位1检测";
            this.CheckPos1Item.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos1Item.Visible = false;
            this.CheckPos1Item.Click += new System.EventHandler(this.CheckPos1Item_Click);
            // 
            // CheckPos1Item_Active
            // 
            this.CheckPos1Item_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckPos1Item_Active.BackgroundImage")));
            this.CheckPos1Item_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckPos1Item_Active.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckPos1Item_Active.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CheckPos1Item_Active.Margin = new System.Windows.Forms.Padding(30, 6, 10, 0);
            this.CheckPos1Item_Active.Name = "CheckPos1Item_Active";
            this.CheckPos1Item_Active.Padding = new System.Windows.Forms.Padding(30, 0, 0, 18);
            this.CheckPos1Item_Active.Size = new System.Drawing.Size(143, 50);
            this.CheckPos1Item_Active.Text = "工位1检测";
            // 
            // CheckPos2Item
            // 
            this.CheckPos2Item.Font = new System.Drawing.Font("Adobe 黑体 Std R", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CheckPos2Item.ForeColor = System.Drawing.Color.DimGray;
            this.CheckPos2Item.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos2Item.Margin = new System.Windows.Forms.Padding(30, 5, 30, 7);
            this.CheckPos2Item.Name = "CheckPos2Item";
            this.CheckPos2Item.Padding = new System.Windows.Forms.Padding(0);
            this.CheckPos2Item.Size = new System.Drawing.Size(101, 44);
            this.CheckPos2Item.Text = "工位2检测";
            this.CheckPos2Item.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos2Item.Click += new System.EventHandler(this.CheckPos2Item_Click);
            // 
            // CheckPos2Item_Active
            // 
            this.CheckPos2Item_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckPos2Item_Active.BackgroundImage")));
            this.CheckPos2Item_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckPos2Item_Active.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckPos2Item_Active.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CheckPos2Item_Active.Margin = new System.Windows.Forms.Padding(0, 6, 10, 0);
            this.CheckPos2Item_Active.Name = "CheckPos2Item_Active";
            this.CheckPos2Item_Active.Padding = new System.Windows.Forms.Padding(30, 0, 0, 18);
            this.CheckPos2Item_Active.Size = new System.Drawing.Size(143, 50);
            this.CheckPos2Item_Active.Text = "工位2检测";
            this.CheckPos2Item_Active.Visible = false;
            // 
            // CheckPos3Item
            // 
            this.CheckPos3Item.Font = new System.Drawing.Font("Adobe 黑体 Std R", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CheckPos3Item.ForeColor = System.Drawing.Color.DimGray;
            this.CheckPos3Item.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos3Item.Margin = new System.Windows.Forms.Padding(30, 5, 30, 7);
            this.CheckPos3Item.Name = "CheckPos3Item";
            this.CheckPos3Item.Padding = new System.Windows.Forms.Padding(0);
            this.CheckPos3Item.Size = new System.Drawing.Size(101, 44);
            this.CheckPos3Item.Text = "工位3检测";
            this.CheckPos3Item.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckPos3Item.Click += new System.EventHandler(this.CheckPos3Item_Click);
            // 
            // CheckPos3Item_Active
            // 
            this.CheckPos3Item_Active.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckPos3Item_Active.BackgroundImage")));
            this.CheckPos3Item_Active.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckPos3Item_Active.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckPos3Item_Active.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CheckPos3Item_Active.Margin = new System.Windows.Forms.Padding(0, 6, 10, 0);
            this.CheckPos3Item_Active.Name = "CheckPos3Item_Active";
            this.CheckPos3Item_Active.Padding = new System.Windows.Forms.Padding(30, 0, 0, 18);
            this.CheckPos3Item_Active.Size = new System.Drawing.Size(143, 50);
            this.CheckPos3Item_Active.Text = "工位3检测";
            this.CheckPos3Item_Active.Visible = false;
            // 
            // SubFormPanel
            // 
            this.SubFormPanel.AutoSize = true;
            this.SubFormPanel.BackColor = System.Drawing.Color.White;
            this.SubFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubFormPanel.Location = new System.Drawing.Point(0, 60);
            this.SubFormPanel.Name = "SubFormPanel";
            this.SubFormPanel.Size = new System.Drawing.Size(1095, 708);
            this.SubFormPanel.TabIndex = 1;
            // 
            // SubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1095, 768);
            this.ControlBox = false;
            this.Controls.Add(this.SubFormPanel);
            this.Controls.Add(this.SubMenu);
            this.DoubleBuffered = true;
            this.Name = "SubForm";
            this.SubMenu.ResumeLayout(false);
            this.SubMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip SubMenu;
        private System.Windows.Forms.ToolStripMenuItem CheckPos1Item_Active;
        private System.Windows.Forms.ToolStripMenuItem CheckPos1Item;
        private System.Windows.Forms.ToolStripMenuItem CheckPos2Item;
        private System.Windows.Forms.ToolStripMenuItem CheckPos2Item_Active;
        private System.Windows.Forms.ToolStripMenuItem CheckPos3Item;
        private System.Windows.Forms.ToolStripMenuItem CheckPos3Item_Active;
        private System.Windows.Forms.Panel SubFormPanel;
    }
}