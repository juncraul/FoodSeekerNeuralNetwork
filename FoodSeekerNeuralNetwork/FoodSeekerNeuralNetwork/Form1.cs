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
using Mathematics;

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
        Point mouseDownPosition;
        bool mouseIsDown;
        Vector2 currentOffSet = new Vector2(0, 0);
        Vector2 permanentOffSet = new Vector2(0, 0);

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
                Interval = 1
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
                pictureBoxWorld.Image = applicationEngine.DrawWorld(permanentOffSet + currentOffSet);
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
                DoStuff();
            }
        }

        static volatile object secondLock = new object();

        private void DoStuff()
        {
            applicationEngine.DoLogic();

            //ThreadStart processTaskThread = delegate { Draw(); };
            //Thread newThread = new Thread(processTaskThread);
            //
            //lock(secondLock)
            //{
            //
            //    newThread.Start();
            //}
            //
            //
            //void Draw()
            {
                pictureBoxWorld.Image = applicationEngine.DrawWorld(permanentOffSet + currentOffSet);
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
            if (e is MouseEventArgs mouseEventArgs)
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
            DoStuff();
        }

        private void pictureBoxWorld_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPosition = e.Location;
            currentOffSet = new Vector2(0, 0);
            mouseIsDown = true;
        }

        private void pictureBoxWorld_MouseUp(object sender, MouseEventArgs e)
        {
            permanentOffSet += currentOffSet;
            currentOffSet = new Vector2(0, 0);
            mouseIsDown = false;
        }

        private void pictureBoxWorld_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseIsDown)
                currentOffSet = new Vector2(e.X - mouseDownPosition.X, mouseDownPosition.Y - e.Y);
        }
    }
}
