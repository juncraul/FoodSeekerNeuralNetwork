﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mathematics.Test
{
    [TestClass]
    public class Functions_Test
    {
        [TestMethod]
        public void Sigmoid_Test()
        {
            double result;
            for (double x = -10; x < 10; x++)
            {
                result = Functions.Sigmoid(x);

                Assert.IsTrue(result > -1 && result < 1);
            }
        }
    }
}
