using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    public static class Functions
    {
        public static double Sigmoid(double x)
        {
            return Math.Pow(1 + Math.Pow(Math.E, -x), -1);
        }
    }
}
