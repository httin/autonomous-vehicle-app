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
    public partial class AutoSetting : UserControl
    {
        public AutoSetting()
        {
            InitializeComponent();
        }

        public void SendButtonClickHandler(EventHandler e)
        {
            this.SendBt.Click += e;
        }

        public void OnSelfUpdateBtClickHandler(EventHandler e)
        {
            this.OnSelfUpdateBt.Click += e;
        }

        public void OffSelfUpdateBtClickHandler(EventHandler e)
        {
            this.OffSelfUpdateBt.Click += e;
        }

        public void AvoidEnableClickHandler(EventHandler e)
        {
            this.AvoidEnable.Click += e;
        }
    }
}
