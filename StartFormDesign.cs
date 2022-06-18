using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using MaterialSkin;

namespace AgeGenderDetect
{
    public partial class StartForm
    {
        private MaterialSkinManager materialSkinManager;
        private Label label1;
        private Label label2;
        private MaterialSkin.Controls.MaterialFlatButton startButton;
        private MaterialSkin.Controls.MaterialFlatButton pauseButton;
        private MaterialSkin.Controls.MaterialFlatButton stopButton;
        private MaterialSkin.Controls.MaterialFlatButton viewButton;
        private Chart mainChart;
        private FolderBrowserDialog folderBrowserDialog;
        private Label cameraLabel;
        private ComboBox cameraComboBox;
        private Label timerLabel;
        private ComboBox timerComboBox;

        private string fileName = null;

        void InitializeComponent()
        {
            Text = "СПВиП";
            ClientSize = new Size(1000, 500);
            Icon = Properties.Resources.main;
            MinimumSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue900, Primary.Blue900, Primary.Blue900,
                Accent.LightBlue200, TextShade.WHITE);

            label1 = new Label()
            {
                Text = "Статистика по",
                Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
            };
            label2 = new Label()
            {
                Text = "полу и возрасту",
                Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)))
            };

            startButton = new MaterialSkin.Controls.MaterialFlatButton()
            {
                Text = "Начать",
            };
            startButton.Click += StartButton_Click;
            pauseButton = new MaterialSkin.Controls.MaterialFlatButton()
            {
                Text = "Стоп"
            };
            stopButton = new MaterialSkin.Controls.MaterialFlatButton()
            {
                Text = "Прекратить"
            };
            viewButton = new MaterialSkin.Controls.MaterialFlatButton()
            {
                Text = "Просмотр"
            };

            var maleSeries = new Series()
            {
                ChartArea = "ChartArea1",
                Color = Color.Blue,
                Legend = "Legend1",
                Name = "Мужчин",
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            var femaleSeries = new Series()
            {
                ChartArea = "ChartArea1",
                Color = Color.Red,
                Legend = "Legend1",
                Name = "Женщин",
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            var titel = new Title()
            {
                Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
                Name = "Title1",
                Text = "График по данным" 
            };
            mainChart = new Chart();
            mainChart.ChartAreas.Add("ChartArea1");
            mainChart.Legends.Add("Legend1");
            mainChart.Series.Add(maleSeries);
            mainChart.Series.Add(femaleSeries);
            mainChart.Titles.Add(titel);
            foreach(var age in PublicData.ageList)
            {
                mainChart.Series["Мужчин"].Points.AddXY(age, 0);
            }

            folderBrowserDialog = new FolderBrowserDialog();

            cameraLabel = new Label()
            {
                Text = "Выбор камеры:"
            };

            cameraComboBox = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            timerLabel = new Label()
            {
                Text = "Интервал сбора данных:"
            };

            timerComboBox = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(startButton);
            Controls.Add(pauseButton);
            Controls.Add(stopButton);
            Controls.Add(viewButton);
            Controls.Add(cameraLabel);
            Controls.Add(cameraComboBox);
            Controls.Add(timerLabel);
            Controls.Add(timerComboBox);
            Controls.Add(mainChart);

            SizeChanged += StartForm_SizeChanged;
            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
            FormClosing += StartForm_FormClosing;
            Load += StartForm_Load;
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = folderBrowserDialog.SelectedPath + "\\" +
                    $"/{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}" +
                    $"_{DateTime.Now.Second}_Chart.jpg";
                mainChart.SaveImage(fileName, ChartImageFormat.Jpeg);
            }
        }

        private void StartForm_SizeChanged(object sender, EventArgs e)
        {
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                label1.Location = new Point(ClientSize.Width * 10 / 100, 85);
                label1.Size = new Size(205, ClientSize.Height / 16);

                label2.Location = new Point(label1.Location.X - ClientSize.Width / 100, label1.Bottom);
                label2.Size = new Size(225, ClientSize.Height / 16);

                startButton.Location = new Point(ClientSize.Width / 20, label2.Bottom + ClientSize.Height / 33);

                pauseButton.Location = new Point(startButton.Right + ClientSize.Width / 8, label2.Bottom + ClientSize.Height / 33);

                stopButton.Location = new Point(startButton.Location.X, startButton.Bottom + ClientSize.Height / 10);

                viewButton.Location = new Point(pauseButton.Location.X, stopButton.Location.Y);

                cameraLabel.Location = new Point(startButton.Location.X, stopButton.Bottom + ClientSize.Height / 10);
                cameraLabel.Size = new Size(127, 17);

                cameraComboBox.Location = new Point(cameraLabel.Location.X, cameraLabel.Bottom + ClientSize.Height / 50);
                cameraComboBox.Size = new Size(viewButton.Right - cameraLabel.Location.X, 10);

                timerLabel.Location = new Point(startButton.Location.X, cameraComboBox.Bottom + ClientSize.Height / 20);
                timerLabel.Size = new Size(135, 17);

                timerComboBox.Location = new Point(startButton.Location.X, timerLabel.Bottom + ClientSize.Height / 50);
                timerComboBox.Size = cameraComboBox.Size;

                mainChart.Location = new Point(viewButton.Right + ClientSize.Width / 33, label1.Location.Y);
                mainChart.Size = new Size(ClientSize.Width - mainChart.Location.X - ClientSize.Width / 50,
                    ClientSize.Height - label1.Location.Y - ClientSize.Height / 25);
            }
        }
    }
}
