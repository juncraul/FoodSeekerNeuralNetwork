﻿using Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public class Food : BasePopulation
    {
        public double FoodValue { get; set; }

        public Food(Vector2 position, string specieType, Color color)
        {
            FoodValue = 100;
            Position = position;
            Radius = 5;
            Color = color;
            SpecieType = specieType;
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));
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
                int hashPosition = obj.Position == null ? 0 : obj.Position.GetHashCode();

                //Get hash code for the Code field. 
                int hashRadius = obj.Radius.GetHashCode();

                int hashColor = obj.Color.GetHashCode();

                //Calculate the hash code for the product. 
                return hashPosition ^ hashRadius ^ hashColor;
            }
        }
    }
}
