using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mathematics.Test
{
    [TestClass]
    public class Vector2_Test
    {
        [TestMethod]
        public void Vector2Substraction_Test()
        {
            Vector2 a = new Vector2(1, 2);
            Vector2 b = new Vector2(3, 4);

            Vector2 c = a - b;

            Assert.AreEqual(c.X, -2);
            Assert.AreEqual(c.Y, -2);
        }

        [TestMethod]
        public void Vector2Addition_Test()
        {
            Vector2 a = new Vector2(1, 2);
            Vector2 b = new Vector2(3, 4);

            Vector2 c = a + b;

            Assert.AreEqual(c.X, 4);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Multiplication1_Test()
        {
            Vector2 a = new Vector2(1, 2);
            float b = 3;

            Vector2 c = a * b;

            Assert.AreEqual(c.X, 3);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Multiplication2_Test()
        {
            Vector2 a = new Vector2(1, 2);
            float b = 3;

            Vector2 c = b * a;

            Assert.AreEqual(c.X, 3);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Dot_Test()
        {
            Vector2 a = new Vector2(1, 2);
            Vector2 b = new Vector2(3, 4);

            double c = Vector2.Dot(a, b);

            Assert.AreEqual(c, 11);
        }

        [TestMethod]
        public void Vector2Magnitude_Test()
        {
            Vector2 a = new Vector2(1, 2);

            double c = a.Magnitude();

            Assert.AreEqual(c, Math.Sqrt(5));
        }

        [TestMethod]
        public void Vector2Normalize_Test()
        {
            Vector2 a = new Vector2(2, 0);

            Vector2 c = a.Normaize();

            Assert.AreEqual(c.X, 1);
            Assert.AreEqual(c.Y, 0);
        }

        [TestMethod]
        public void Vector2Rotate_Test()
        {
            Vector2 a = new Vector2(0, 1);

            Vector2 b = a.Rotate(Math.PI / 2);
            Vector2 c = a.Rotate(Math.PI);
            Vector2 d = a.Rotate(3 * Math.PI / 2);
            Vector2 e = a.Rotate(2 * Math.PI);
            Vector2 f = a.Rotate(0);

            Assert.IsTrue(Math.Abs(b.X - -1) <= 0.00001f);
            Assert.IsTrue(Math.Abs(b.Y - 0) <= 0.00001f);

            Assert.IsTrue(Math.Abs(c.X - 0) <= 0.00001f);
            Assert.IsTrue(Math.Abs(c.Y - -1) <= 0.00001f);

            Assert.IsTrue(Math.Abs(d.X - 1) <= 0.00001f);
            Assert.IsTrue(Math.Abs(d.Y - 0) <= 0.00001f);

            Assert.IsTrue(Math.Abs(e.X - 0) <= 0.00001f);
            Assert.IsTrue(Math.Abs(e.Y - 1) <= 0.00001f);

            Assert.IsTrue(Math.Abs(f.X - 0) <= 0.00001f);
            Assert.IsTrue(Math.Abs(f.Y - 1) <= 0.00001f);
        }
    }
}
