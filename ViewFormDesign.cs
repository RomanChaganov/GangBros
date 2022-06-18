using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MaterialSkin;
using Emgu.CV;
using Emgu.CV.UI;

namespace AgeGenderDetect
{
    public partial class ViewForm
    {
        private ImageBox imageBox;

        void InitializeComponent()
        {
            ClientSize = new Size(800, 500);
            MinimumSize = new Size(320, 240);
            Text = "Просмотр";
            Icon = Properties.Resources.main;

            imageBox = new ImageBox()
            {
                BackColor = Color.Black,
                SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage,
                FunctionalMode = ImageBox.FunctionalModeOption.Minimum
            };

            Controls.Add(imageBox);

            SizeChanged += ViewForm_SizeChanged;
            FormClosing += ViewForm_FormClosing;
        }

        private void ViewForm_SizeChanged(object sender, EventArgs e)
        {
            imageBox.Location = new Point(1, 63);
            imageBox.Size = new Size(ClientSize.Width, ClientSize.Height - imageBox.Location.Y);
        }
    }
}
