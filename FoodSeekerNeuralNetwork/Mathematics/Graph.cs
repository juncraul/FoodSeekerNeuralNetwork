using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathematics
{
    public class Graph
    {
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        private List<List<double>> _points;
        private List<Color> _color;
        private int _numberofLines;
        private float _xOffset = 20;
        private float _yOffset = 10;

        public Graph(float maxX, float maxY, int numberofLines, List<Color> color)
        {
            MaxX = maxX;
            MaxY = maxY;
            _numberofLines = numberofLines;
            _points = new List<List<double>>();
            _color = color;
            for (var i = 0; i < _numberofLines; i++)
            {
                _points.Add(new List<double>());
            }
        }

        public void AddPoint(int line, double value)
        {
            if (line >= _numberofLines || line < 0)
                throw new Exception("line needs to be between 0 and " + _numberofLines);

            _points[line].Add(value);
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            var brush = new SolidBrush(Color.White);
            var pen = new Pen(Color.Black);
            graphics.DrawLine(pen, new PointF(_xOffset, bitmap.Height - _yOffset), new PointF(_xOffset, _yOffset));
            graphics.DrawLine(pen, new PointF(_xOffset, bitmap.Height - _yOffset), new PointF(bitmap.Width - _xOffset, bitmap.Height - _yOffset));

            for (var i = 0; i < _points.Count; i++)
            {
                if (_points[i].Count <= 1)
                    continue;

                pen.Color = _color[i];
                graphics.DrawLines(pen, GetLines(_points[i], bitmap));
            }

            brush.Color = Color.Black;
            var ysegments = 5;
            var yUnit = (int) (MaxY / ysegments - MaxY / ysegments % 5);
            for (var i = 0; i <= ysegments; i++)
            {
                var text = (i * yUnit).ToString();
                graphics.DrawString(text, new Font("Consolas", 8), brush, 0, (int) (bitmap.Height - _yOffset - (i * ((bitmap.Height - 2 * _yOffset) / ysegments))));
            }

            var xsegments = 10;
            var xUnit = (int) (MaxX / xsegments - MaxX / xsegments % 5);
            for (var i = 0; i <= xsegments; i++)
            {
                var text = (i * xUnit).ToString();
                graphics.DrawString(text, new Font("Consolas", 8), brush, (int) (_xOffset + (i * ((bitmap.Width - 2 * _xOffset) / xsegments))), bitmap.Height - 10);
            }
        }

        private PointF[] GetLines(List<double> lines, Bitmap bitmap)
        {
            Resize(bitmap);
            var points = new PointF[lines.Count];
            for (var i = 0; i < lines.Count; i++)
            {
                points[i] = new PointF(_xOffset + i * (bitmap.Width - _xOffset * 2) / MaxX,
                                       bitmap.Height - _yOffset - (float) (lines[i] * (bitmap.Height - _yOffset * 2) / MaxY));
            }

            return points;
        }

        private void Resize(Bitmap bitmap)
        {
            var numberOfLines = _points.Max(a => a.Count);
            var maxValue = _points.Max(a => a.Max(b => b));
            MaxX = (numberOfLines - 1) - (numberOfLines - 1) % 50 + 50;
            MaxY = (int) (maxValue - maxValue % 50 + 50);
        }
    }
}