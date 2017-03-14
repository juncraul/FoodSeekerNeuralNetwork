using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSeekerNeuralNetwork
{
    public class Graph
    {
        public float MaxX { get; set; }
        public float MaxY { get; set; }


        private List<List<double>> _points;
        private List<Color> _color;
        private int _numberofLines;
        private float _xOffset = 10;
        private float _yOffset = 10;
        private float _distanceBetweenLines = 10;

        public Graph(float maxX, float maxY, int numberofLines, List<Color> color)
        {
            MaxX = maxX;
            MaxY = maxY;
            _numberofLines = numberofLines;
            _points = new List<List<double>>();
            _color = color;
            for (int i = 0; i < _numberofLines; i ++)
            {
                _points.Add(new List<double>());
            }
        }

        public void AddPoint(int line, double value)
        {
            if (line >= _numberofLines || line < 0)
                throw new Exception("line needs to be between 0 and " + _numberofLines);

            //_points[line].Add(new PointF(_points[line].Count * 10, _bitmap.Height - (float)value));
            _points[line].Add(value);
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black);
            graphics.DrawLine(pen, new PointF(_xOffset, bitmap.Height - _yOffset), new PointF(_xOffset, _yOffset));
            graphics.DrawLine(pen, new PointF(_xOffset, bitmap.Height - _yOffset), new PointF(bitmap.Width - _xOffset, bitmap.Height - _yOffset));

            for(int i = 0; i < _points.Count; i ++)
            {
                if (_points[i].Count <= 1)
                    continue;
                pen.Color = _color[i];
                graphics.DrawLines(pen, GetLines(_points[i], bitmap));
            }
        }

        private PointF[] GetLines(List<double> lines, Bitmap bitmap)
        {
            Resize(bitmap);
            PointF[] points = new PointF[lines.Count];
            for(int i = 0; i < lines.Count; i ++)
            {
                points[i] = new PointF(_xOffset + i * _distanceBetweenLines * (bitmap.Width - _xOffset * 2) / MaxX, 
                    bitmap.Height - _yOffset - (float)(lines[i] * (bitmap.Height - _yOffset * 2) / MaxY));
            }
            return points;
        }

        private void Resize(Bitmap bitmap)
        {
            int numberOfLines = _points.Max(a => a.Count);
            double maxValue = _points.Max(a => a.Max(b => b));
            MaxX = (numberOfLines - 1) * _distanceBetweenLines;
            MaxY = (int)maxValue;
        }
    }
}
