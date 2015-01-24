namespace MachineLearningNeuralNetwork.Logic
{
    public class InputSet
    {

        private readonly double[] _inputs;
        private readonly double[] _outputs;

        public InputSet(double[] values, int numberOfInputs, int numberOfOutputs)
        {
            _inputs = new double[numberOfInputs];
            for (var i = 0; i < numberOfInputs; i++)
            {
                _inputs[i] = values[i];
            }
            _outputs = new double[numberOfOutputs];
            for (var i = 0; i < numberOfOutputs; i++)
            {
                _outputs[i] = values[i+numberOfInputs];
            }
        }

        public int MaxOutput
        {
            get
            {
                var item = -1;
                var max = double.MinValue;
                for (int i = 0; i < Outputs.Length; i++)
                {
                    if (Outputs[i] > max)
                    {
                        max = Outputs[i];
                        item = i;
                    }
                }
                return item;
            }
        }

        public double[] Inputs
        {
            get { return _inputs; }
        }

        public double[] Outputs
        {
            get { return _outputs; }
        }
    }
}
