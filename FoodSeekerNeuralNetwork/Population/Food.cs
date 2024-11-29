using Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public class Food : BasePopulation
    {
        public double FoodValue { get; set; }

        public Food(Vector2 position, SpecieType specieType, Color color, int foodValue)
        {
            FoodValue = foodValue;
            Position = position;
            Radius = 5;
            Color = color;
            Type = specieType;
        }

        public void Draw(Graphics graphics, Bitmap bitmap, Vector2 offSet = null)
        {
            if (offSet == null) offSet = new Vector2(0, 0);
            var brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, 
                new Rectangle((int)(Position.X - Radius + offSet.X), bitmap.Height - (int)(Position.Y + Radius + offSet.Y), (int)Radius * 2, (int)Radius * 2));
        }

        public class FoodComparer : IEqualityComparer<BasePopulation>
        {

            public bool Equals(BasePopulation x, BasePopulation y)
            {
                //Check whether the objects are the same object. 
                return Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(BasePopulation obj)
            {
                //Get hash code for the Name field if it is not null. 
                var hashPosition = obj.Position == null ? 0 : obj.Position.GetHashCode();

                //Get hash code for the Code field. 
                var hashRadius = obj.Radius.GetHashCode();

                var hashColor = obj.Color.GetHashCode();

                //Calculate the hash code for the product. 
                return hashPosition ^ hashRadius ^ hashColor;
            }
        }
    }
}
