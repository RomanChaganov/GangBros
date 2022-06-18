using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DirectShowLib;
using Emgu.CV;

namespace AgeGenderDetect
{
    public partial class StartForm : MaterialSkin.Controls.MaterialForm
    {
        private DsDevice[] webCams = null;
        private ViewForm viewForm = null;
        private Timer timer;
        private Result currentGenderAge;
        private List<Tuple<string, string>> genderAge;
        private bool genderAgeIsFull = false;

        private List<string> stringIntervalTimer = new List<string>()
        {
            "5 секунд",
            "30 секунд",
            "1 минута",
            "2 минуты"
        };

        private Dictionary<int, int> intIntervalTimer = new Dictionary<int, int>()
        {
            [0] = 5000,
            [1] = 30000,
            [2] = 60000,
            [3] = 120000
        };

        private Dictionary<string, Dictionary<string, int>> chartGenderAge = new Dictionary<string, Dictionary<string, int>>()
        {
            ["Мужчин"] = new Dictionary<string, int>
            {
                ["0-2 года"] = 0,
                ["4-6 лет"] = 0,
                ["8-12 лет"] = 0,
                ["15-20 лет"] = 0,
                ["25-32 года"] = 0,
                ["38-43 года"] = 0,
                ["48-53 года"] = 0,
                ["60-100 лет"] = 0
            },
            ["Женщин"] = new Dictionary<string, int>
            {
                ["0-2 года"] = 0,
                ["4-6 лет"] = 0,
                ["8-12 лет"] = 0,
                ["15-20 лет"] = 0,
                ["25-32 года"] = 0,
                ["38-43 года"] = 0,
                ["48-53 года"] = 0,
                ["60-100 лет"] = 0
            }
        };

        public bool ViewButtonEnable
        {
            get { return viewButton.Enabled; }
            set
            {
                viewButton.Enabled = value;
            }
        }

        public StartForm()
        {
            InitializeComponent();

            viewButton.Click += ViewButton_Click;
            stopButton.Click += StopButton_Click;
            pauseButton.Click += PauseButton_Click;
            timerComboBox.SelectedIndexChanged += TimerComboBox_SelectedIndexChanged;

            FormClosing += StartForm_FormClosing1;
            FormClosing += VideoStart.VideoStart_FormClosing;
        }

        private void StartForm_FormClosing1(object sender, FormClosingEventArgs e)
        {
            if (timer != null)
                timer.Stop();
        }

        private void TimerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Interval = intIntervalTimer[timerComboBox.SelectedIndex];
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            VideoStart.PauseVideo();
            timer.Stop();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            VideoStart.StopVideo();
            timer.Stop();
            timer.Dispose();
            if (viewForm != null)
                viewForm.Close();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            viewForm = new ViewForm(this);
            viewForm.Show();
            viewButton.Enabled = false;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            webCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach(var camera in webCams)
            {
                cameraComboBox.Items.Add(camera.Name);
            }

            cameraComboBox.SelectedIndex = 0;

            foreach (var interval in stringIntervalTimer)
            {
                timerComboBox.Items.Add(interval);
            }

            timerComboBox.SelectedIndex = 0;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (webCams.Length == 0)
                {
                    throw new Exception("Нет доступных камер!!!");
                }
                else if (VideoStart.IsEnable)
                {
                    VideoStart.StartAfterPause();
                    startButton.Enabled = false;
                    timer.Start();
                }
                else
                {
                    timer = new Timer
                    {
                        Interval = intIntervalTimer[timerComboBox.SelectedIndex]
                    };
                    timer.Tick += Timer_Tick;
                    startButton.Enabled = false;
                    VideoStart.StartVideo(cameraComboBox.SelectedIndex, this);
                    timer.Start();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!genderAgeIsFull && VideoStart.Result.IsCompleted && PublicData.GenderAge != null)
            {
                currentGenderAge = PublicData.GenderAge.ToClone();
                genderAgeIsFull = true;
            }
            else
            if (VideoStart.Result.IsCompleted && PublicData.GenderAge != null && !currentGenderAge.Equals(PublicData.GenderAge))
            {
                currentGenderAge = PublicData.GenderAge;
                genderAge = PublicData.GenderAge.GenderAge;
                if (genderAge != null)
                {
                    foreach (var tuple in genderAge)
                    {
                        chartGenderAge[tuple.Item1][tuple.Item2]++;
                    }

                    ChartUpdate();
                }
            }
        }

        private void ChartUpdate()
        {
            foreach(var chart in chartGenderAge)
            {
                mainChart.Series[chart.Key].Points.Clear();
                foreach(var age in chart.Value)
                {
                    mainChart.Series[chart.Key].Points.AddXY(age.Key, age.Value);
                }
            }
        }
    }
}
