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

        Graphics graphics;
        Bitmap bitmap;
        Random random;
        List<Agent> agents;
        Timer timerForDrawing;
        Timer timerForLogic;

        private void Form1_Load(object sender, EventArgs e)
        {
            random = new Random();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            agents = new List<Agent>();
            timerForDrawing = new Timer();
            timerForDrawing.Interval = 200;
            timerForDrawing.Tick += TimerForDrawing_Tick;
            timerForLogic = new Timer();
            timerForLogic.Interval = 50;
            timerForLogic.Tick += TimerForLogic_Tick;


            for (int i = 0; i < 10; i ++)
            {
                agents.Add(new Agent(new Point(random.Next() % pictureBox1.Width, random.Next() % pictureBox1.Height)));
            }
            timerForDrawing.Start();
        }

        private void TimerForLogic_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TimerForDrawing_Tick(object sender, EventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.Wheat);
            graphics.FillRectangle(brush, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            foreach(Agent a in agents)
            {
                a.Draw(graphics);
            }

            pictureBox1.Image = bitmap;
        }
    }
}
