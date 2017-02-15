using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics;

namespace NeuralNetwork
{
    public class Network
    {
        public int InputNodes { get; set; }
        public int HiddenNodes { get; set; }
        public int OutputNodes { get; set; }
        public float LearningRate { get; set; }
        public Matrix WeightInput_hidden { get; set; }
        public Matrix WeighHidden_output { get; set; }
        public Matrix Hidden_Outputs { get; set; }

        public void InitializeNetwork(int inputNodes, int hiddenNodes, int outputNodes, float learningRate)
        {
            InputNodes = inputNodes;
            HiddenNodes = hiddenNodes;
            OutputNodes = outputNodes;
            LearningRate = learningRate;
            Hidden_Outputs = new Matrix(HiddenNodes, 1);

            WeightInput_hidden = new Matrix(HiddenNodes, InputNodes);
            WeightInput_hidden.GenerateRandomValuesBetween(-Math.Pow(HiddenNodes, -0.5), Math.Pow(HiddenNodes, -0.5));
            //WeightInput_hidden.GenerateRandomValuesBetween(-0.9, 0.9);
            WeighHidden_output = new Matrix(OutputNodes, HiddenNodes);
            WeighHidden_output.GenerateRandomValuesBetween(-Math.Pow(OutputNodes, -0.5), Math.Pow(OutputNodes, -0.5));
            //Weightidden_output.GenerateRandomValuesBetween(-0.9, 0.9);
        }

        public void TrainNetwrok(Matrix inputs, Matrix target)
        {
            Matrix final_outputs = QueryNetwrok(inputs);
            Matrix output_errors = target - final_outputs;
            Matrix hidden_errors = WeighHidden_output.Transpose() * output_errors;
            Matrix Hidden_Outputs_Transpose = Hidden_Outputs.Transpose();
            Matrix inputs_Transpose = inputs.Transpose();

            for (int i = 0; i < output_errors.Lines; i++)
            {
                double change = (output_errors.TheMatrix[i, 0] * final_outputs.TheMatrix[i, 0] * (1.0 - final_outputs.TheMatrix[i, 0]));
                WeighHidden_output.AddToLine(LearningRate * (change * Hidden_Outputs_Transpose), i);
            }
            //WeighHidden_output += LearningRate * ((output_errors * final_outputs * (1.0 - final_outputs)) * Hidden_Outputs.Transpose());

            for (int i = 0; i < hidden_errors.Lines; i++)
            {
                double change = (hidden_errors.TheMatrix[i, 0] * Hidden_Outputs.TheMatrix[i, 0] * (1.0 - Hidden_Outputs.TheMatrix[i, 0]));
                WeightInput_hidden.AddToLine(LearningRate * (change * inputs_Transpose), i);
            }
            //WeightInput_hidden += LearningRate * ((hidden_errors * Hidden_Outputs * (1.0 - Hidden_Outputs)) * inputs.Transpose());
        }

        public Matrix QueryNetwrok(Matrix inputs)
        {
            Matrix Hidden_Inputs = WeightInput_hidden * inputs;

            for (int i = 0; i < HiddenNodes; i++)
            {
                Hidden_Outputs.TheMatrix[i, 0] = ActivationFunction(Hidden_Inputs.TheMatrix[i, 0]);
            }

            Matrix final_Inputs = WeighHidden_output * Hidden_Outputs;
            Matrix final_Outputs = new Matrix(OutputNodes, 1);

            for (int i = 0; i < OutputNodes; i++)
            {
                final_Outputs.TheMatrix[i, 0] = ActivationFunction(final_Inputs.TheMatrix[i, 0]);
            }

            return final_Outputs;
        }

        public double ActivationFunction(double x)
        {
            return Functions.Sigmoid(x);
        }
    }
}
