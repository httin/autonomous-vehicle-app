﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThesisInterface.UserControls
{
    public partial class ManualUC : UserControl
    {
        public ManualUC()
        {
            InitializeComponent();
            VehicleStatusBox.ReadOnly = true;
            SentBox.ReadOnly = true;
            ReceiveBox.ReadOnly = true;
        }

        private void VehicleStatusBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        private void VehicleStatusBox_TextChanged(object sender, EventArgs e)
        {
            VehicleStatusBox.SelectionStart = VehicleStatusBox.Text.Length;
            VehicleStatusBox.ScrollToCaret();
        }

        private void SentBox_TextChanged(object sender, EventArgs e)
        {
            SentBox.SelectionStart = SentBox.Text.Length;
            SentBox.ScrollToCaret();
        }

        private void ReceiveBox_TextChanged(object sender, EventArgs e)
        {
            ReceiveBox.SelectionStart = ReceiveBox.Text.Length;
            ReceiveBox.ScrollToCaret();
        }

        public void StartBtClickHandler(EventHandler handler)
        {
            this.StartBt.Click += handler;
        }

        public void StopBtClickHandler(EventHandler handler)
        {
            this.StopBt.Click += handler;
        }

        public void ImportBtClickHandler(EventHandler handler)
        {
            this.ImportBt.Click += handler;
        }
        
        public void ExportBtClickHandler(EventHandler handler)
        {
            this.ExportBt.Click += handler;
        }

        public void ChangeModeBtClickHandler(EventHandler handler)
        {
            this.modeBt.Click += handler;
        }
    }
}
