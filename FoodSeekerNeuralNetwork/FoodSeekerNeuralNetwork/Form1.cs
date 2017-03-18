using Population;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FoodSeekerNeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        System.Windows.Forms.Timer timerForDrawing;
        System.Windows.Forms.Timer timerForLogic;
        ApplicationEngine applicationEngine;

        private void Form1_Load(object sender, EventArgs e)
        {
            applicationEngine = ApplicationEngine.GetInstance(pictureBoxWorld.Size,
                                                              pictureBoxBrainView.Size,
                                                              pictureBoxGraph.Size);
            timerForDrawing = new System.Windows.Forms.Timer()
            {
                Interval = 20
            };
            timerForDrawing.Tick += TimerForDrawing_Tick;
            timerForLogic = new System.Windows.Forms.Timer()
            {
                Interval = 10
            };
            timerForLogic.Tick += TimerForLogic_Tick;

            //UpdateTimer = new System.Threading.Timer(UpdateCallback, null, 1, Timeout.Infinite);

            //timerForDrawing.Start();
            timerForLogic.Start();
        }

        // Initialize timer as a one-shot
        System.Threading.Timer UpdateTimer;

        private static volatile Object thisLock = new Object();

        void UpdateCallback(object state)
        {
            lock (thisLock)
            {
                applicationEngine.DoLogic();
                pictureBoxWorld.Image = applicationEngine.DrawWorld();
                pictureBoxBrainView.Image = applicationEngine.DrawBrain();
                pictureBoxGraph.Image = applicationEngine.DrawGraph();
                // re-enable the timer
                UpdateTimer.Change(10, Timeout.Infinite);
            }
        }

        private void TimerForLogic_Tick(object sender, EventArgs e)
        {
            lock (thisLock)
            {
                applicationEngine.DoLogic();
                pictureBoxWorld.Image = applicationEngine.DrawWorld();
                pictureBoxBrainView.Image = applicationEngine.DrawBrain();
                pictureBoxGraph.Image = applicationEngine.DrawGraph();
            }
        }

        private void TimerForDrawing_Tick(object sender, EventArgs e)
        {
            //pictureBoxWorld.Image = applicationEngine.DrawWorld();
            //pictureBoxBrainView.Image = applicationEngine.DrawBrain();
            //pictureBoxGraph.Image = applicationEngine.DrawGraph();
        }

        private void pictureBoxWorld_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                applicationEngine.SelectAgent(new Point(mouseEventArgs.X, pictureBoxWorld.Height - mouseEventArgs.Y));
            }
            
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            timerForLogic.Interval = 1 + trackBarSpeed.Value * 2;
            int value = trackBarSpeed.Value;
            labelSpeed.Text = "Tick every " + timerForLogic.Interval + " milliseconds";
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timerForLogic.Stop();
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            timerForLogic.Start();
        }

        private void buttonNextFrame_Click(object sender, EventArgs e)
        {
            applicationEngine.DoLogic();
        }
    }
}
