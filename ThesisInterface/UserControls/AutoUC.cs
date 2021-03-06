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
    public partial class AutoUC : UserControl
    {
        public AutoUC()
        {
            InitializeComponent();
        }

        public void OnBtClickHandler(EventHandler handler)
        {
            this.Onbt.Click += handler;
        }

        public void OffBtClickHandler(EventHandler handler)
        {
            this.OffBt.Click += handler;
        }

        public void OpenBtClickHandler(EventHandler handler)
        {
            this.OpenBt.Click += handler;
        }

        public void SaveBtClickHandler(EventHandler handler)
        {
            this.SaveBt.Click += handler;
        }

        public void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        public void ReversePathBtHandler(EventHandler handler)
        {
            this.ReversePathBt.Click += handler;
        }

        public void ClearPlannedMapBtClickHandler(EventHandler handler)
        {
            this.ClearPlannedMap.Click += handler;
        }

        public void ClearActualMapBtClickHandler(EventHandler handler)
        {
            this.ClearActualMapBt.Click += handler;
        }

        public void StartBtClickHandler(EventHandler handler)
        {
            this.StartBt.Click += handler;
        }

        public void StopVehicleBtClickHandler(EventHandler handler)
        {
            this.StopVehicleBt.Click += handler;
        }

        private void ReceivedTb_TextChanged(object sender, EventArgs e)
        {
            ReceivedTb.SelectionStart = ReceivedTb.Text.Length;
            ReceivedTb.ScrollToCaret();
        }

        private void SentTb_TextChanged(object sender, EventArgs e)
        {
            SentTb.SelectionStart = SentTb.Text.Length;
            SentTb.ScrollToCaret();
        }

        private void DetailInfoTb_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        private void DetailInfoTb_KeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        public void SettingBtClickHandler(EventHandler handler)
        {
            this.SettingBt.Click += handler;
        }

        /* After choosing points from planning route, we use SPLINE to pre-processing */
        public void CreatePreProcessingBtClickHandler(EventHandler handler)
        {
            this.CreatePreProcessingBt.Click += handler;
        }
        /* Importing file map_out.txt into MCU through serial */
        public void ImportProcessedMapBtClickHandler(EventHandler handler)
        {
            this.ImportProcessedMapBt.Click += handler;
        }

        public void MapProviderClick(EventHandler e)
        {
            this.MapProvider.Click += e;
        }
    }
}
