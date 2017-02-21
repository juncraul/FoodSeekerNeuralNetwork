using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mathematics.Test
{
    [TestClass]
    public class Matrix_Test
    {
        [TestMethod]
        public void MatrixConstructor_Test()
        {
            Matrix a = new Matrix(2, 3);

            Assert.AreEqual(a.Lines, 2);
            Assert.AreEqual(a.Columns, 3);
            Assert.AreEqual(a.TheMatrix.Length, 6);
        }

        [TestMethod]
        public void MatrixMultiplication_Test()
        {
            Matrix a = new Matrix(2, 3);
            Matrix b = new Matrix(3, 2);

            a.TheMatrix[0, 0] = 1;
            a.TheMatrix[0, 1] = 2;
            a.TheMatrix[0, 2] = 3;
            a.TheMatrix[1, 0] = 4;
            a.TheMatrix[1, 1] = 5;
            a.TheMatrix[1, 2] = 6;

            b.TheMatrix[0, 0] = 7;
            b.TheMatrix[0, 1] = 8;
            b.TheMatrix[1, 0] = 9;
            b.TheMatrix[1, 1] = 10;
            b.TheMatrix[2, 0] = 11;
            b.TheMatrix[2, 1] = 12;

            Matrix c = a * b;
            Matrix d = 2 * a;
            Matrix e = a * 2;

            Assert.AreEqual(c.TheMatrix[0, 0], 58);
            Assert.AreEqual(c.TheMatrix[0, 1], 64);
            Assert.AreEqual(c.TheMatrix[1, 0], 139);
            Assert.AreEqual(c.TheMatrix[1, 1], 154);

            Assert.AreEqual(d.TheMatrix[0, 0], 2);
            Assert.AreEqual(d.TheMatrix[0, 1], 4);
            Assert.AreEqual(d.TheMatrix[0, 2], 6);
            Assert.AreEqual(d.TheMatrix[1, 0], 8);
            Assert.AreEqual(d.TheMatrix[1, 1], 10);
            Assert.AreEqual(d.TheMatrix[1, 2], 12);

            Assert.AreEqual(e.TheMatrix[0, 0], 2);
            Assert.AreEqual(e.TheMatrix[0, 1], 4);
            Assert.AreEqual(e.TheMatrix[0, 2], 6);
            Assert.AreEqual(e.TheMatrix[1, 0], 8);
            Assert.AreEqual(e.TheMatrix[1, 1], 10);
            Assert.AreEqual(e.TheMatrix[1, 2], 12);
        }

        [TestMethod]
        public void MatrixAddition_Test()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = new Matrix(2, 2);

            a.TheMatrix[0, 0] = 1;
            a.TheMatrix[0, 1] = 2;
            a.TheMatrix[1, 0] = 3;
            a.TheMatrix[1, 1] = 4;

            b.TheMatrix[0, 0] = 5;
            b.TheMatrix[0, 1] = 6;
            b.TheMatrix[1, 0] = 7;
            b.TheMatrix[1, 1] = 8;

            Matrix c = a + b;
            Matrix d = 3 + a;
            Matrix e = a + 3;

            Assert.AreEqual(c.TheMatrix[0, 0], 6);
            Assert.AreEqual(c.TheMatrix[0, 1], 8);
            Assert.AreEqual(c.TheMatrix[1, 0], 10);
            Assert.AreEqual(c.TheMatrix[1, 1], 12);

            Assert.AreEqual(d.TheMatrix[0, 0], 4);
            Assert.AreEqual(d.TheMatrix[0, 1], 5);
            Assert.AreEqual(d.TheMatrix[1, 0], 6);
            Assert.AreEqual(d.TheMatrix[1, 1], 7);

            Assert.AreEqual(e.TheMatrix[0, 0], 4);
            Assert.AreEqual(e.TheMatrix[0, 1], 5);
            Assert.AreEqual(e.TheMatrix[1, 0], 6);
            Assert.AreEqual(e.TheMatrix[1, 1], 7);
        }

        [TestMethod]
        public void MatrixSubstraction_Test()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = new Matrix(2, 2);

            a.TheMatrix[0, 0] = 1;
            a.TheMatrix[0, 1] = 2;
            a.TheMatrix[1, 0] = 3;
            a.TheMatrix[1, 1] = 4;

            b.TheMatrix[0, 0] = 5;
            b.TheMatrix[0, 1] = 6;
            b.TheMatrix[1, 0] = 7;
            b.TheMatrix[1, 1] = 8;

            Matrix c = a - b;
            Matrix d = 3 - a;
            Matrix e = a - 3;

            Assert.AreEqual(c.TheMatrix[0, 0], -4);
            Assert.AreEqual(c.TheMatrix[0, 1], -4);
            Assert.AreEqual(c.TheMatrix[1, 0], -4);
            Assert.AreEqual(c.TheMatrix[1, 1], -4);

            Assert.AreEqual(d.TheMatrix[0, 0], 2);
            Assert.AreEqual(d.TheMatrix[0, 1], 1);
            Assert.AreEqual(d.TheMatrix[1, 0], 0);
            Assert.AreEqual(d.TheMatrix[1, 1], -1);

            Assert.AreEqual(e.TheMatrix[0, 0], -2);
            Assert.AreEqual(e.TheMatrix[0, 1], -1);
            Assert.AreEqual(e.TheMatrix[1, 0], 0);
            Assert.AreEqual(e.TheMatrix[1, 1], 1);
        }

        [TestMethod]
        public void MatrixTranspose_Test()
        {
            Matrix a = new Matrix(2, 3);

            a.TheMatrix[0, 0] = 1;
            a.TheMatrix[0, 1] = 2;
            a.TheMatrix[0, 2] = 3;
            a.TheMatrix[1, 0] = 4;
            a.TheMatrix[1, 1] = 5;
            a.TheMatrix[1, 2] = 6;

            Matrix b = a.Transpose();

            Assert.AreEqual(b.TheMatrix[0, 0], 1);
            Assert.AreEqual(b.TheMatrix[0, 1], 4);
            Assert.AreEqual(b.TheMatrix[1, 0], 2);
            Assert.AreEqual(b.TheMatrix[1, 1], 5);
            Assert.AreEqual(b.TheMatrix[2, 0], 3);
            Assert.AreEqual(b.TheMatrix[2, 1], 6);
        }

        [TestMethod]
        public void GetMaxValueIndex_Test()
        {
            Matrix a = new Matrix(6, 1);

            a.TheMatrix[0, 0] = 1;
            a.TheMatrix[1, 0] = 2;
            a.TheMatrix[2, 0] = 30;
            a.TheMatrix[3, 0] = 4;
            a.TheMatrix[4, 0] = 5;
            a.TheMatrix[5, 0] = 6;

            Assert.AreEqual(a.GetMaxValueIndex(), 2);
        }

        [TestMethod]
        public void GenerateRandomValuesBetween_Test()
        {
            Matrix a = new Matrix(2, 3);

            a.TheMatrix[0, 0] = 0;
            a.TheMatrix[0, 1] = 0;
            a.TheMatrix[0, 2] = 0;
            a.TheMatrix[1, 0] = 0;
            a.TheMatrix[1, 1] = 0;
            a.TheMatrix[1, 2] = 0;
            a.GenerateRandomValuesBetween(0.2f, 1.0f);

            Assert.AreNotEqual(a.TheMatrix[0, 0], 0);
            Assert.AreNotEqual(a.TheMatrix[0, 1], 0);
            Assert.AreNotEqual(a.TheMatrix[0, 2], 0);
            Assert.AreNotEqual(a.TheMatrix[1, 0], 0);
            Assert.AreNotEqual(a.TheMatrix[1, 1], 0);
            Assert.AreNotEqual(a.TheMatrix[1, 2], 0);
        }

        [TestMethod]
        public void CheckIfTestClassWorks_Test()
        {
            Assert.AreEqual(3, 3);
        }
    }
}
