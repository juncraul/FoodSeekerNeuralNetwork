using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

                Assert.IsTrue(result >= -1 && result <= 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CirclesCollisionThrowsNullReferenceException596()
        {
            bool b;
            b = Functions.CirclesCollision((Vector2)null, 0, (Vector2)null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CirclesCollisionThrowsNullReferenceException295()
        {
            bool b;
            Vector2 s0 = new Vector2(0, 0);
            b = Functions.CirclesCollision(s0, 0, (Vector2)null, 0);
        }

        [TestMethod]
        public void CirclesCollision442()
        {
            bool b;
            Vector2 s0 = new Vector2(2, 2);
            Vector2 s1 = new Vector2(4, 2);
            Vector2 s2 = new Vector2(4, 3);
            b = Functions.CirclesCollision(s0, 1, s1, 1);
            Assert.AreEqual<bool>(true, b);
            b = Functions.CirclesCollision(s1, 1, s2, 1);
            Assert.AreEqual<bool>(true, b);
            b = Functions.CirclesCollision(s0, 1, s2, 1);
            Assert.AreEqual<bool>(false, b);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CollisionPointCircleThrowsNullReferenceException804()
        {
            bool b;
            b = Functions.CollisionPointCircle((Vector2)null, (Vector2)null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CollisionPointCircleThrowsNullReferenceException466()
        {
            bool b;
            Vector2 s0 = new Vector2(0, 0);
            b = Functions.CollisionPointCircle(s0, (Vector2)null, 0);
        }

        [TestMethod]
        public void CollisionPointCircle186()
        {
            bool b;
            Vector2 p0 = new Vector2(0, 0);
            Vector2 p1 = new Vector2(2, 2);
            Vector2 p2 = new Vector2(1, 2);
            Vector2 s0 = new Vector2(2, 2);
            b = Functions.CollisionPointCircle(p0, s0, 1);
            Assert.AreEqual<bool>(false, b);
            b = Functions.CollisionPointCircle(p1, s0, 1);
            Assert.AreEqual<bool>(true, b);
            b = Functions.CollisionPointCircle(p2, s0, 1);
            Assert.AreEqual<bool>(true, b);
        }
    }
}
