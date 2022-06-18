using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgeGenderDetect
{
    public partial class ViewForm : MaterialSkin.Controls.MaterialForm
    {
        StartForm frm;
        public ViewForm(StartForm form1)
        {
            InitializeComponent();
            frm = form1;
            VideoStart.ViewVideo += ViewForm_ViewVideo;
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            frm.ViewButtonEnable = true;
            VideoStart.ViewVideo -= ViewForm_ViewVideo;
        }

        private void ViewForm_ViewVideo()
        {
            imageBox.Image = PublicData.Image;
        }
    }
}
