using Mathematics;
using System;
using System.Drawing;

namespace NeuralNetwork
{
    public class Network
    {
        public int InputNodes { get; set; }
        public int HiddenNodes { get; set; }
        public int OutputNodes { get; set; }
        public float LearningRate { get; set; }

        private Layer InputLayer { get; set; }
        private Layer HiddenLayer { get; set; }
        private Layer OutputLayer { get; set; }

        private int _neuronRadiusDraw;
        private int _distanceBetweenLayersDraw;
        private int _distanceBetweenNeuronsDraw;
        private int _startXDraw;
        private int _startYDraw;

        public void InitializeNetwork(int inputNodes, int hiddenNodes, int outputNodes, float learningRate, Random rand)
        {
            InputNodes = inputNodes;
            HiddenNodes = hiddenNodes;
            OutputNodes = outputNodes;
            LearningRate = learningRate;
            InputLayer = new Layer();
            HiddenLayer = new Layer();
            OutputLayer = new Layer();

            InputLayer.Weights = new Matrix(HiddenNodes, InputNodes);
            InputLayer.Weights.GenerateRandomValuesBetween(-Math.Pow(HiddenNodes, -0.5), Math.Pow(HiddenNodes, -0.5), rand);
            HiddenLayer.Weights = new Matrix(OutputNodes, HiddenNodes);
            HiddenLayer.Weights.GenerateRandomValuesBetween(-Math.Pow(OutputNodes, -0.5), Math.Pow(OutputNodes, -0.5), rand);
            OutputLayer.Output = new Matrix(OutputNodes, 1);

            _neuronRadiusDraw = 10;
            _distanceBetweenLayersDraw = 100;
            _distanceBetweenNeuronsDraw = 10;
            _startXDraw = 10;
            _startYDraw = 10;
        }

        public Matrix TrainNetwrok(Matrix inputs, Matrix target)
        {
            OutputLayer.Output = QueryNetwrok(inputs);
            OutputLayer.Errors = target - OutputLayer.Output;
            HiddenLayer.Errors = HiddenLayer.Weights.Transpose() * OutputLayer.Errors;
            Matrix HiddenLayer_Output_Transpose = InputLayer.Output.Transpose();
            Matrix inputs_Transpose = inputs.Transpose();

            for (int i = 0; i < OutputLayer.Errors.Lines; i++)
            {
                double change = (OutputLayer.Errors.TheMatrix[i, 0] * OutputLayer.Output.TheMatrix[i, 0] * (1.0 - OutputLayer.Output.TheMatrix[i, 0]));
                HiddenLayer.Weights.AddToLine(LearningRate * (change * HiddenLayer_Output_Transpose), i);
            }

            for (int i = 0; i < HiddenLayer.Errors.Lines; i++)
            {
                double change = (HiddenLayer.Errors.TheMatrix[i, 0] * InputLayer.Output.TheMatrix[i, 0] * (1.0 - InputLayer.Output.TheMatrix[i, 0]));
                InputLayer.Weights.AddToLine(LearningRate * (change * inputs_Transpose), i);
            }

            return OutputLayer.Output;
        }

        public Matrix QueryNetwrok(Matrix inputs)
        {
            InputLayer.Output = InputLayer.Weights * inputs;

            for (int i = 0; i < HiddenNodes; i++)
            {
                InputLayer.Output.TheMatrix[i, 0] = ActivationFunction(InputLayer.Output.TheMatrix[i, 0]);
            }

            HiddenLayer.Output = HiddenLayer.Weights * InputLayer.Output;
            OutputLayer.Output = new Matrix(OutputNodes, 1);

            for (int i = 0; i < OutputNodes; i++)
            {
                OutputLayer.Output.TheMatrix[i, 0] = ActivationFunction(HiddenLayer.Output.TheMatrix[i, 0]);
            }

            return OutputLayer.Output;
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color.Yellow);
            Pen pen = new Pen(Color.Black);

            for (int i = 0; i < InputLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < InputLayer.Weights.Lines; j++)
                {
                    pen.Width = (int)(Math.Abs(InputLayer.Weights.TheMatrix[j, i]) * 5);
                    pen.Color = InputLayer.Weights.TheMatrix[j, i] < 0 ? Color.Red : Color.Blue;
                    graphics.DrawLine(pen, _startXDraw + _neuronRadiusDraw, (_startYDraw + i * (_neuronRadiusDraw * 2 + 10)) + _neuronRadiusDraw,
                                           _startXDraw + _distanceBetweenLayersDraw + _neuronRadiusDraw, (_startYDraw + j * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)) + _neuronRadiusDraw);
                }
            }

            for (int i = 0; i < HiddenLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < HiddenLayer.Weights.Lines; j++)
                {
                    pen.Width = (int)(Math.Abs(HiddenLayer.Weights.TheMatrix[j, i]) * 5);
                    pen.Color = HiddenLayer.Weights.TheMatrix[j, i] < 0 ? Color.Red : Color.Blue;
                    graphics.DrawLine(pen, _startXDraw + _distanceBetweenLayersDraw + _neuronRadiusDraw, (_startYDraw + i * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)) + _neuronRadiusDraw,
                                           _startXDraw + _distanceBetweenLayersDraw * 2 + _neuronRadiusDraw, (_startYDraw + j * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)) + _neuronRadiusDraw);
                }
            }

            for (int i = 0; i < InputNodes; i++)
            {
                graphics.FillEllipse(brush, new Rectangle(_startXDraw, (_startYDraw + i * (_neuronRadiusDraw * 2 + 10)), (_neuronRadiusDraw * 2), (_neuronRadiusDraw * 2)));
            }

            for (int i = 0; i < HiddenNodes; i++)
            {
                graphics.FillEllipse(brush, new Rectangle(_startXDraw + _distanceBetweenLayersDraw, (_startYDraw + i * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)), (_neuronRadiusDraw * 2), (_neuronRadiusDraw * 2)));
            }

            for (int i = 0; i < OutputNodes; i++)
            {
                brush.Color = Color.Yellow;
                graphics.FillEllipse(brush, new Rectangle(_startXDraw + _distanceBetweenLayersDraw * 2, (_startYDraw + i * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)), (_neuronRadiusDraw * 2), (_neuronRadiusDraw * 2)));

                string text = string.Format("{0:N3}", OutputLayer.Output.TheMatrix[i, 0]);
                brush.Color = Color.Black;
                graphics.DrawString(text, new Font("Consolas", 8), brush, _neuronRadiusDraw * 2 + _startXDraw + _distanceBetweenLayersDraw * 2, (_startYDraw + i * (_neuronRadiusDraw * 2 + _distanceBetweenNeuronsDraw)));
            }
        }

        public double ActivationFunction(double x)
        {
            return Functions.Sigmoid(x);
        }

        public string ConvertNetworkToBitString()
        {
            string bits = string.Empty;

            for (int i = 0; i < InputLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < InputLayer.Weights.Lines; j++)
                {
                    int value = (int)(((InputLayer.Weights.TheMatrix[j, i] + 1) / 2) * 1000);
                    int scaledDownTo256Bit = (int)(value * 255.0 / 999.0);
                    bits += Functions.ToBin(scaledDownTo256Bit, 8);
                }
            }

            for (int i = 0; i < HiddenLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < HiddenLayer.Weights.Lines; j++)
                {
                    int value = (int)(((HiddenLayer.Weights.TheMatrix[j, i] + 1) / 2) * 1000);
                    int scaledDownTo256Bit = (int)(value * 255.0 / 999.0);
                    bits += Functions.ToBin(scaledDownTo256Bit, 8);
                }
            }
            return bits;
        }

        public void ConvertBitStringToNetwork(string bits)
        {
            int index = 0;
            for (int i = 0; i < InputLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < InputLayer.Weights.Lines; j++)
                {
                    int scaledDownTo256Bit = Convert.ToInt32(bits.Substring(index, 8), 2);
                    double value = (scaledDownTo256Bit * 999 / 255.0 / 1000.0 * 2 - 1);
                    InputLayer.Weights.TheMatrix[j, i] = value;
                    index += 8;
                }
            }

            for (int i = 0; i < HiddenLayer.Weights.Columns; i++)
            {
                for (int j = 0; j < HiddenLayer.Weights.Lines; j++)
                {
                    int scaledDownTo256Bit = Convert.ToInt32(bits.Substring(index, 8), 2);
                    double value = (scaledDownTo256Bit * 999 / 255.0 / 1000.0 * 2 - 1);
                    HiddenLayer.Weights.TheMatrix[j, i] = value;
                    index += 8;
                }
            }
        }
    }
}
