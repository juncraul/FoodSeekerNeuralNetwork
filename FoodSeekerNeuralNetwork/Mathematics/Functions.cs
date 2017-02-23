using System;

namespace Mathematics
{
    public static class Functions
    {
        public static double Sigmoid(double x)
        {
            return Math.Pow(1 + Math.Pow(Math.E, -x), -1);
        }

        //function to check if the ray hits the Circle
        public static bool RayIntersectsCricle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleOrigin, double circleRadius, out double distance)
        {
            distance = -1;
            Vector2 l = circleOrigin - rayOrigin;
            double tca = Vector2.Dot(l, rayDirection.Normaize());
            if (tca < 0)
                return false;
            double s = Math.Sqrt(Vector2.Dot(l, l) - tca * tca);
            if (s > circleRadius) return false;
            double thc = Math.Sqrt(Math.Pow(circleRadius, 2) - Math.Pow(s, 2));
            distance = tca - thc < tca + thc ? tca - thc : tca + thc;
            Vector2 intersectionPosition = rayOrigin + rayDirection * distance; //maybe used later
            Vector2 normal = (intersectionPosition - circleOrigin).Normaize();      //maybe used later
            return s < circleRadius;
        }

        public static bool CirclesCollision(Vector2 circle0Origin, double circle0Radius, Vector2 circle1Origin, double circle1Radius)
        {
            return DistanceBetweenTwoPoints(circle0Origin, circle1Origin) < circle0Radius + circle1Radius;
        }

        public static double DistanceBetweenTwoPoints(Vector2 point0, Vector2 point1)
        {
            return Math.Sqrt(Math.Pow(point0.X - point1.X, 2) + Math.Pow(point0.Y - point1.Y, 2));
        }

        public static bool CollisionPointCircle(Vector2 point, Vector2 circleOrigin, double circleRadius)
        {
            return circleRadius > DistanceBetweenTwoPoints(point, circleOrigin);
        }
    }
}
