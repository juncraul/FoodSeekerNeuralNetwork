using Mathematics;

namespace NeuralNetwork
{
    public class Layer
    {
        public Matrix Output { get; set; }
        public Matrix Weights { get; set; }
        public Matrix Errors { get; set; }
    }
}
