using System;

namespace Mathematics
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(a * b.X, a * b.Y);
        }

        public static Vector2 operator *(Vector2 a, double b)
        {
            return b * a;
        }

        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public Vector2 Normaize()
        {
            return this * (1 / Magnitude());
        }

        public Vector2 Rotate(double radian)
        {
            return new Vector2(X * Math.Cos(radian) - Y * Math.Sin(radian), X * Math.Sin(radian) + Math.Cos(radian));
        }

        public int GetHashCode(Vector2 obj)
        {
            int hashX = obj.X.GetHashCode();

            int hashY = obj.Y.GetHashCode();
            
            return hashX ^ hashY;
        }
    }
}
