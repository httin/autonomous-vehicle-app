namespace ThesisInterface.UserControls
{
    partial class AutoSetting
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoSetting));
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.Velocity_label = new System.Windows.Forms.Label();
            this.Kgain_label = new System.Windows.Forms.Label();
            this.SelfUpdate_label = new System.Windows.Forms.Label();
            this.VelocityTb = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.KGainTb = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.AvoidEnable = new Bunifu.Framework.UI.BunifuImageButton();
            this.TopPanel = new Bunifu.Framework.UI.BunifuGradientPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.OnSelfUpdateBt = new Bunifu.Framework.UI.BunifuFlatButton();
            this.SendBt = new Bunifu.Framework.UI.BunifuFlatButton();
            this.OffSelfUpdateBt = new Bunifu.Framework.UI.BunifuFlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.AvoidEnable)).BeginInit();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this;
            this.bunifuDragControl1.Vertical = true;
            // 
            // Velocity_label
            // 
            this.Velocity_label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Velocity_label.AutoSize = true;
            this.Velocity_label.BackColor = System.Drawing.Color.White;
            this.Velocity_label.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Velocity_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.Velocity_label.Location = new System.Drawing.Point(3, 76);
            this.Velocity_label.Name = "Velocity_label";
            this.Velocity_label.Size = new System.Drawing.Size(91, 25);
            this.Velocity_label.TabIndex = 26;
            this.Velocity_label.Text = "Velocity";
            // 
            // Kgain_label
            // 
            this.Kgain_label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Kgain_label.AutoSize = true;
            this.Kgain_label.BackColor = System.Drawing.Color.White;
            this.Kgain_label.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Kgain_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.Kgain_label.Location = new System.Drawing.Point(3, 108);
            this.Kgain_label.Name = "Kgain_label";
            this.Kgain_label.Size = new System.Drawing.Size(82, 25);
            this.Kgain_label.TabIndex = 26;
            this.Kgain_label.Text = "K Gain";
            // 
            // SelfUpdate_label
            // 
            this.SelfUpdate_label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SelfUpdate_label.AutoSize = true;
            this.SelfUpdate_label.BackColor = System.Drawing.Color.White;
            this.SelfUpdate_label.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelfUpdate_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.SelfUpdate_label.Location = new System.Drawing.Point(3, 142);
            this.SelfUpdate_label.Name = "SelfUpdate_label";
            this.SelfUpdate_label.Size = new System.Drawing.Size(188, 25);
            this.SelfUpdate_label.TabIndex = 26;
            this.SelfUpdate_label.Text = "Self Update Mode";
            // 
            // VelocityTb
            // 
            this.VelocityTb.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.VelocityTb.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.VelocityTb.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.VelocityTb.BorderThickness = 2;
            this.VelocityTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.VelocityTb.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.VelocityTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.VelocityTb.isPassword = false;
            this.VelocityTb.Location = new System.Drawing.Point(103, 76);
            this.VelocityTb.Margin = new System.Windows.Forms.Padding(4);
            this.VelocityTb.Name = "VelocityTb";
            this.VelocityTb.Size = new System.Drawing.Size(98, 30);
            this.VelocityTb.TabIndex = 27;
            this.VelocityTb.Text = "0.4";
            this.VelocityTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // KGainTb
            // 
            this.KGainTb.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.KGainTb.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.KGainTb.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.KGainTb.BorderThickness = 2;
            this.KGainTb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.KGainTb.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.KGainTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.KGainTb.isPassword = false;
            this.KGainTb.Location = new System.Drawing.Point(103, 108);
            this.KGainTb.Margin = new System.Windows.Forms.Padding(4);
            this.KGainTb.Name = "KGainTb";
            this.KGainTb.Size = new System.Drawing.Size(98, 30);
            this.KGainTb.TabIndex = 27;
            this.KGainTb.Text = "0.6";
            this.KGainTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.label1.Location = new System.Drawing.Point(3, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 26;
            this.label1.Text = "Allow Avoiding";
            // 
            // AvoidEnable
            // 
            this.AvoidEnable.BackColor = System.Drawing.Color.White;
            this.AvoidEnable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AvoidEnable.Image = global::ThesisInterface.Properties.Resources.ON;
            this.AvoidEnable.ImageActive = null;
            this.AvoidEnable.Location = new System.Drawing.Point(170, 193);
            this.AvoidEnable.Name = "AvoidEnable";
            this.AvoidEnable.Size = new System.Drawing.Size(64, 54);
            this.AvoidEnable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AvoidEnable.TabIndex = 29;
            this.AvoidEnable.TabStop = false;
            this.AvoidEnable.Zoom = 10;
            // 
            // TopPanel
            // 
            this.TopPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TopPanel.BackgroundImage")));
            this.TopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.TopPanel.Controls.Add(this.label4);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.GradientBottomLeft = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.TopPanel.GradientBottomRight = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.TopPanel.GradientTopLeft = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(24)))), ((int)(((byte)(91)))));
            this.TopPanel.GradientTopRight = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(49)))), ((int)(((byte)(85)))));
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Quality = 10;
            this.TopPanel.Size = new System.Drawing.Size(344, 47);
            this.TopPanel.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(5)))), ((int)(((byte)(65)))));
            this.label4.Location = new System.Drawing.Point(15, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(316, 26);
            this.label4.TabIndex = 1;
            this.label4.Text = "SETTING FOR MODE AUTO";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OnSelfUpdateBt
            // 
            this.OnSelfUpdateBt.Activecolor = System.Drawing.Color.Green;
            this.OnSelfUpdateBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OnSelfUpdateBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.OnSelfUpdateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OnSelfUpdateBt.BorderRadius = 0;
            this.OnSelfUpdateBt.ButtonText = "ON";
            this.OnSelfUpdateBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OnSelfUpdateBt.DisabledColor = System.Drawing.Color.Gray;
            this.OnSelfUpdateBt.Iconcolor = System.Drawing.Color.Transparent;
            this.OnSelfUpdateBt.Iconimage = ((System.Drawing.Image)(resources.GetObject("OnSelfUpdateBt.Iconimage")));
            this.OnSelfUpdateBt.Iconimage_right = null;
            this.OnSelfUpdateBt.Iconimage_right_Selected = null;
            this.OnSelfUpdateBt.Iconimage_Selected = null;
            this.OnSelfUpdateBt.IconMarginLeft = 0;
            this.OnSelfUpdateBt.IconMarginRight = 0;
            this.OnSelfUpdateBt.IconRightVisible = true;
            this.OnSelfUpdateBt.IconRightZoom = 0D;
            this.OnSelfUpdateBt.IconVisible = true;
            this.OnSelfUpdateBt.IconZoom = 90D;
            this.OnSelfUpdateBt.IsTab = false;
            this.OnSelfUpdateBt.Location = new System.Drawing.Point(20, 171);
            this.OnSelfUpdateBt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OnSelfUpdateBt.Name = "OnSelfUpdateBt";
            this.OnSelfUpdateBt.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.OnSelfUpdateBt.OnHovercolor = System.Drawing.Color.Chocolate;
            this.OnSelfUpdateBt.OnHoverTextColor = System.Drawing.Color.White;
            this.OnSelfUpdateBt.selected = false;
            this.OnSelfUpdateBt.Size = new System.Drawing.Size(123, 24);
            this.OnSelfUpdateBt.TabIndex = 23;
            this.OnSelfUpdateBt.Text = "ON";
            this.OnSelfUpdateBt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OnSelfUpdateBt.Textcolor = System.Drawing.Color.White;
            this.OnSelfUpdateBt.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // SendBt
            // 
            this.SendBt.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.SendBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SendBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.SendBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SendBt.BorderRadius = 0;
            this.SendBt.ButtonText = "SEND";
            this.SendBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendBt.DisabledColor = System.Drawing.Color.Green;
            this.SendBt.Iconcolor = System.Drawing.Color.Transparent;
            this.SendBt.Iconimage = ((System.Drawing.Image)(resources.GetObject("SendBt.Iconimage")));
            this.SendBt.Iconimage_right = null;
            this.SendBt.Iconimage_right_Selected = null;
            this.SendBt.Iconimage_Selected = null;
            this.SendBt.IconMarginLeft = 0;
            this.SendBt.IconMarginRight = 0;
            this.SendBt.IconRightVisible = true;
            this.SendBt.IconRightZoom = 0D;
            this.SendBt.IconVisible = true;
            this.SendBt.IconZoom = 90D;
            this.SendBt.IsTab = false;
            this.SendBt.Location = new System.Drawing.Point(208, 76);
            this.SendBt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendBt.Name = "SendBt";
            this.SendBt.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.SendBt.OnHovercolor = System.Drawing.Color.Chocolate;
            this.SendBt.OnHoverTextColor = System.Drawing.Color.White;
            this.SendBt.selected = false;
            this.SendBt.Size = new System.Drawing.Size(126, 24);
            this.SendBt.TabIndex = 23;
            this.SendBt.Text = "SEND";
            this.SendBt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SendBt.Textcolor = System.Drawing.Color.White;
            this.SendBt.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // OffSelfUpdateBt
            // 
            this.OffSelfUpdateBt.Activecolor = System.Drawing.Color.Green;
            this.OffSelfUpdateBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OffSelfUpdateBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.OffSelfUpdateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OffSelfUpdateBt.BorderRadius = 0;
            this.OffSelfUpdateBt.ButtonText = "OFF";
            this.OffSelfUpdateBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OffSelfUpdateBt.DisabledColor = System.Drawing.Color.Gray;
            this.OffSelfUpdateBt.Iconcolor = System.Drawing.Color.Transparent;
            this.OffSelfUpdateBt.Iconimage = ((System.Drawing.Image)(resources.GetObject("OffSelfUpdateBt.Iconimage")));
            this.OffSelfUpdateBt.Iconimage_right = null;
            this.OffSelfUpdateBt.Iconimage_right_Selected = null;
            this.OffSelfUpdateBt.Iconimage_Selected = null;
            this.OffSelfUpdateBt.IconMarginLeft = 0;
            this.OffSelfUpdateBt.IconMarginRight = 0;
            this.OffSelfUpdateBt.IconRightVisible = true;
            this.OffSelfUpdateBt.IconRightZoom = 0D;
            this.OffSelfUpdateBt.IconVisible = true;
            this.OffSelfUpdateBt.IconZoom = 90D;
            this.OffSelfUpdateBt.IsTab = false;
            this.OffSelfUpdateBt.Location = new System.Drawing.Point(208, 171);
            this.OffSelfUpdateBt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OffSelfUpdateBt.Name = "OffSelfUpdateBt";
            this.OffSelfUpdateBt.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(9)))), ((int)(((byte)(42)))));
            this.OffSelfUpdateBt.OnHovercolor = System.Drawing.Color.Chocolate;
            this.OffSelfUpdateBt.OnHoverTextColor = System.Drawing.Color.White;
            this.OffSelfUpdateBt.selected = false;
            this.OffSelfUpdateBt.Size = new System.Drawing.Size(123, 24);
            this.OffSelfUpdateBt.TabIndex = 23;
            this.OffSelfUpdateBt.Text = "OFF";
            this.OffSelfUpdateBt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OffSelfUpdateBt.Textcolor = System.Drawing.Color.White;
            this.OffSelfUpdateBt.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // AutoSetting
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.AvoidEnable);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.KGainTb);
            this.Controls.Add(this.VelocityTb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelfUpdate_label);
            this.Controls.Add(this.Kgain_label);
            this.Controls.Add(this.Velocity_label);
            this.Controls.Add(this.OnSelfUpdateBt);
            this.Controls.Add(this.SendBt);
            this.Controls.Add(this.OffSelfUpdateBt);
            this.MaximumSize = new System.Drawing.Size(344, 250);
            this.MinimumSize = new System.Drawing.Size(344, 250);
            this.Name = "AutoSetting";
            this.Size = new System.Drawing.Size(344, 250);
            ((System.ComponentModel.ISupportInitialize)(this.AvoidEnable)).EndInit();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        public Bunifu.Framework.UI.BunifuMetroTextbox KGainTb;
        public Bunifu.Framework.UI.BunifuMetroTextbox VelocityTb;
        private System.Windows.Forms.Label SelfUpdate_label;
        private System.Windows.Forms.Label Kgain_label;
        private System.Windows.Forms.Label Velocity_label;
        public Bunifu.Framework.UI.BunifuFlatButton OnSelfUpdateBt;
        public Bunifu.Framework.UI.BunifuFlatButton SendBt;
        public Bunifu.Framework.UI.BunifuFlatButton OffSelfUpdateBt;
        private Bunifu.Framework.UI.BunifuGradientPanel TopPanel;
        private System.Windows.Forms.Label label4;
        public Bunifu.Framework.UI.BunifuImageButton AvoidEnable;
        private System.Windows.Forms.Label label1;
    }
}
