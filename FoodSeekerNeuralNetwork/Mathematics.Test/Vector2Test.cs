using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mathematics.Test
{
    [TestClass]
    public class Vector2Test
    {
        [TestMethod]
        public void Vector2Substraction_Test()
        {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);

            var c = a - b;

            Assert.AreEqual(c.X, -2);
            Assert.AreEqual(c.Y, -2);
        }

        [TestMethod]
        public void Vector2Addition_Test()
        {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);

            var c = a + b;

            Assert.AreEqual(c.X, 4);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Multiplication1_Test()
        {
            var a = new Vector2(1, 2);
            float b = 3;

            var c = a * b;

            Assert.AreEqual(c.X, 3);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Multiplication2_Test()
        {
            var a = new Vector2(1, 2);
            float b = 3;

            var c = b * a;

            Assert.AreEqual(c.X, 3);
            Assert.AreEqual(c.Y, 6);
        }

        [TestMethod]
        public void Vector2Dot_Test()
        {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);

            var c = Vector2.Dot(a, b);

            Assert.AreEqual(c, 11);
        }

        [TestMethod]
        public void Vector2Magnitude_Test()
        {
            var a = new Vector2(1, 2);

            var c = a.Magnitude();

            Assert.AreEqual(c, Math.Sqrt(5));
        }

        [TestMethod]
        public void Vector2Normalize_Test()
        {
            var a = new Vector2(2, 0);

            var c = a.Normalize();

            Assert.AreEqual(c.X, 1);
            Assert.AreEqual(c.Y, 0);
        }

        [TestMethod]
        public void Vector2Rotate_Test()
        {
            var a = new Vector2(0, 1);

            var b = a.Rotate(Math.PI / 2);
            var c = a.Rotate(Math.PI);
            var d = a.Rotate(3 * Math.PI / 2);
            var e = a.Rotate(2 * Math.PI);
            var f = a.Rotate(0);

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
