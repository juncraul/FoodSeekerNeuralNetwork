using Mathematics;
using System;
using System.Drawing;

namespace Environment
{
    public struct Cell
    {
        public int Food { get; set; }
        public bool IsLand { get; set; }
    }

    public class Map
    {
        private Cell[,] _map;
        private Size _mapSize;
        private SizeF _cellSize;
        private Random _random;

        public Map(Size mapSize, SizeF cellSize, Random random)
        {
            _random = random;
            _mapSize = mapSize;
            _cellSize = cellSize;
            GenerateMap(0.01, 0.5);
        }

        private void GenerateMap(double _landChance, double _landExtension)
        {
            _map = new Cell[_mapSize.Width, _mapSize.Height];

            for (var i = 0; i < _mapSize.Width; i++)
            {
                for (var j = 0; j < _mapSize.Height; j++)
                {
                    _map[i, j] = new Cell()
                    {
                        IsLand = false
                    };

                    if (_random.NextDouble() < _landChance)
                        GenerateLand(i, j, _landExtension);
                }
            }

            void GenerateLand(int i, int j, double landExtension)
            {
                if (i < 0 || j < 0 || i >= _mapSize.Width || j >= _mapSize.Height) return;
                if (_map[i, j].IsLand) return;

                _map[i, j].IsLand = true;

                if (_random.NextDouble() < landExtension) GenerateLand(i + 1, j, landExtension);
                if (_random.NextDouble() < landExtension) GenerateLand(i, j + 1, landExtension);
                if (_random.NextDouble() < landExtension) GenerateLand(i - 1, j, landExtension);
                if (_random.NextDouble() < landExtension) GenerateLand(i, j - 1, landExtension);
            }
        }

        object _lockObject = new object();

        public void Draw(Graphics graphics, Bitmap bitmap, Vector2 offSet = null)
        {
            if (offSet == null) offSet = new Vector2(0, 0);
            var brushLand = new SolidBrush(Color.White);
            var brushNotLand = new SolidBrush(Color.Black);

            //Graphics gr = Graphics.FromImage(bitmap);
            //ThreadStart processTaskThread = delegate { DrawInThread(gr); };
            //Thread newThread = new Thread(processTaskThread);
            //newThread.Start();

            //void DrawInThread(Graphics g)
            //{
            //    lock(lockObject)
            //    {
            for (var i = 0; i < _mapSize.Width; i++)
            {
                for (var j = 0; j < _mapSize.Height; j++)
                {
                    graphics.FillRectangle(_map[i, j].IsLand ? brushLand : brushNotLand,
                                           new RectangleF(i * _cellSize.Width + (int) offSet.X, j * _cellSize.Height - (int) offSet.Y, _cellSize.Width, _cellSize.Height));
                }
            }
            //    }
            //}
        }
    }
}