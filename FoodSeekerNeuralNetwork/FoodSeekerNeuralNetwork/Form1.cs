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

namespace FoodSeekerNeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Timer timerForDrawing;
        Timer timerForLogic;
        ApplicationEngine applicationEngine;

        private void Form1_Load(object sender, EventArgs e)
        {
            applicationEngine = ApplicationEngine.GetInstance(new Size(pictureBox1.Width, pictureBox1.Height));
            timerForDrawing = new Timer();
            timerForDrawing.Interval = 200;
            timerForDrawing.Tick += TimerForDrawing_Tick;
            timerForLogic = new Timer();
            timerForLogic.Interval = 50;
            timerForLogic.Tick += TimerForLogic_Tick;


            
            timerForDrawing.Start();
            timerForLogic.Start();
        }

        private void TimerForLogic_Tick(object sender, EventArgs e)
        {
            applicationEngine.DoLogic();
        }

        private void TimerForDrawing_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = applicationEngine.Draw();
        }
    }
}
