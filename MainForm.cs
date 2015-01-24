using System;
using System.ComponentModel;
using System.Threading;
using MachineLearningNeuralNetwork.GUI.Points;
using MachineLearningNeuralNetwork.Logic;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MachineLearningNeuralNetwork
{
    public partial class MainForm : Form
    {
        readonly Label _coordinateLabel = new Label();
        readonly CoordinateBox _box = new CoordinateBox();
        readonly Button _trainButton = new Button();
        readonly TextBox _trainCyclesText = new TextBox();
        readonly Label _cycleLabel = new Label();
        private List<double[]> _valuesToCalculate;
        public List<double[]> TrainValues { get; set; }
        private int _totalCycles = 0;

        private const int XDimension = 100;
        private const int YDimension = 100;
        private const int RedrawRate = 50;

        private Network _network = null;

        public MainForm()
        {
            InitializeComponent();

            Init();

            InitBgWorker();
        }

        private void InitBgWorker()
        {
            bgWorker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }

        private void bgWorker_DoWork(object sender,
            DoWorkEventArgs e)
        {
            InitGraphValueSets();

            if (_network == null)
            {
                InitNetwork();
            }
            var cycles = Convert.ToInt32(e.Argument);
            for (var i = 0; i < cycles; i++)
            {
                toolStripTotalLabel.Text = (++_totalCycles).ToString();
                toolStripCurrentLabel.Text = (i+1).ToString();
                var error =  _network.TrainNetwork();
                toolStripErrorLabel.Text = error.ToString();
                var percent = Convert.ToInt32(((double)i / Convert.ToInt32(e.Argument)) * 100);
                var backgroundWorker = sender as BackgroundWorker;
                if (backgroundWorker != null)
                    Thread.Sleep(5);
                if (i%RedrawRate == 0)
                {
                    RedrawGraph();
                }
                

                backgroundWorker.ReportProgress(percent);
            }

        }

        private void bgWorker_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            RedrawGraph();
        }

        private void bgWorker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
        }

        #region init
        public void Init()
        {
            TrainValues = new List<double[]>();
            radioButton3.Checked = true;

            _box.Location = new Point(150, 20);
            _box.SetSizeByTileCount(XDimension, YDimension);
            _box.BorderStyle = BorderStyle.FixedSingle;
            _box.GenerateTiles(XDimension, YDimension);

            _box.MouseMove += box_MouseMove;
            _box.MouseClick += box_MouseClick;

            _cycleLabel.Text = "Number of Cycles";
            _cycleLabel.Location = new Point(670, 20);

            _trainCyclesText.Location = new Point(670, 40);
            _trainCyclesText.Width = 100;

            _trainButton.Location = new Point(670, 70);
            _trainButton.Width = 100;
            _trainButton.Text = "Train";
            _trainButton.Click += trainButton_Click;

            //init learningrate singleton
            LearningRate.Instance.Rate = (double)trackBar1.Value / 100;

            _coordinateLabel.Text = "X,Y";
            _coordinateLabel.Location = new Point(150, 530);

            this.Controls.Add(_coordinateLabel);
            this.Controls.Add(_box);
            this.Controls.Add(_trainCyclesText);
            this.Controls.Add(_trainButton);
            this.Controls.Add(_cycleLabel);
        }
        #endregion

        /// <summary>
        /// Initialises the network.
        /// </summary>
        private void InitNetwork()
        {
            int[] dimensions = { 2, 10, 3 };

            _network = new Network(dimensions, TrainValues);
        }

        void trainButton_Click(object sender, System.EventArgs e)
        {
            var cycles = Convert.ToInt32(_trainCyclesText.Text);
            if (_network != null) _network.UpdateTrainvalueSet(TrainValues);

            bgWorker.RunWorkerAsync(cycles);
        }

        /// <summary>
        /// Redraws the graph with all current tilevalues.
        /// </summary>
        private void RedrawGraph()
        {
            foreach (var valueSet in _valuesToCalculate)
            {
                var value = _network.ActivateValueSet(valueSet);
                if (value == 0)
                {
                    _box.SetTile(Color.LightCoral, Convert.ToInt32(valueSet[0] * XDimension * _box.TileSize), Convert.ToInt32(valueSet[1] * YDimension * _box.TileSize));
                }
                else if (value == 1)
                {
                    _box.SetTile(Color.LightBlue, Convert.ToInt32(valueSet[0] * XDimension * _box.TileSize), Convert.ToInt32(valueSet[1] * YDimension * _box.TileSize));
                }
                else
                {
                    _box.SetTile(Color.LightGreen, Convert.ToInt32(valueSet[0] * XDimension * _box.TileSize), Convert.ToInt32(valueSet[1] * YDimension * _box.TileSize));
                }
            }
            _box.Invalidate();
        }

        /// <summary>
        /// Estimates the values that are not training values.
        /// </summary>
        private void InitGraphValueSets()
        {
            _valuesToCalculate = new List<double[]>();

            foreach (var tile in _box.GetNonTrainingSetTiles())
            {
                _valuesToCalculate.Add(new double[] { (double)tile.Bounds.X / _box.Width, (double)tile.Bounds.Y / _box.Height, 0d, 0d, 0d });
            }
        }

        void box_MouseClick(object sender, MouseEventArgs e)
        {
            _box.Invalidate();
            var normalisedX = (double)e.X / _box.Width;
            var normalisedY = (double)e.Y / _box.Height;

            //create Testvalues out of graph interaction
            //input values = x + y
            //currentColor = red or blue
            if (_box.CurrentColor == Color.Red)
            {
                TrainValues.Add(new double[] { normalisedX, normalisedY, 1d, 0d, 0d });
            }
            else if (_box.CurrentColor == Color.Blue)
            {
                TrainValues.Add(new double[] { normalisedX, normalisedY, 0d, 1d, 0d });
            }
            else
            {
                TrainValues.Add(new double[] { normalisedX, normalisedY, 0d, 0d, 1d });
            }
        }

        void box_MouseMove(object sender, MouseEventArgs e)
        {
            _coordinateLabel.Text = e.X / _box.TileSize + ", " + e.Y / _box.TileSize;
        }


        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            _box.CurrentColor = Color.Red;
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            _box.CurrentColor = Color.Blue;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            _box.CurrentColor = Color.Green;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackbarValueLabel.Text = ((double)trackBar1.Value / 100).ToString();
            LearningRate.Instance.Rate = (double)trackBar1.Value / 100;
        }

    }
}