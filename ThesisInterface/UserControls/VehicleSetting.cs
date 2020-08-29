using System;
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
    public partial class VehicleSetting : UserControl
    {
        public VehicleSetting()
        {
            InitializeComponent();
        }
        
        public void OpenSPBtClickHandler(EventHandler handler)
        {
            this.OpenSPBt.Click += handler;
        }

        public void SendBtClickHandler(EventHandler handler)
        {
            this.SendBt.Click += handler;
        }

        public void SaveBtClickHandler(EventHandler handler)
        {
            this.SaveBt.Click += handler;
        }

        public void UpdateSPBtClickHandler(EventHandler handler)
        {
            this.UpdateSP.Click += handler;
        }

        private void ReceiveMessTextBox_TextChanged(object sender, EventArgs e)
        {
            ReceiveMessTextBox.SelectionStart = ReceiveMessTextBox.Text.Length;
            ReceiveMessTextBox.ScrollToCaret();
        }

        private void SentMessTextBox_TextChanged(object sender, EventArgs e)
        {
            SentMessTextBox.SelectionStart = SentMessTextBox.Text.Length;
            SentMessTextBox.ScrollToCaret();
        }
    }
}
