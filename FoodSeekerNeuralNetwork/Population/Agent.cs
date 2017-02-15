using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Population
{
    public class Agent
    {
        public double Health { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }

        public Agent(Point position)
        {
            Position = position;
            Size = new Size(20, 20);
            Color = Color.Red;
        }

        public void Draw(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, new Rectangle(Position, Size));
        }
    }
}
