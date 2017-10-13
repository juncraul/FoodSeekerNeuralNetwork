using Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public class BasePopulation
    {
        public Vector2 Position { get; set; }
        public double Radius { get; set; }
        public Color Color { get; set; }
        public SpecieType Type { get; set; }
        public bool IsAlive { get; set; }

        public BasePopulation()
        {
            IsAlive = true;
        }

    }

    public enum SpecieType
    {
        Plant,
        Herbivore,
        Carnivore
    }

    public class BaseComparer : IEqualityComparer<BasePopulation>
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
