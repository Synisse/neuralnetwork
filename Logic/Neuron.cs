using System;
using System.Collections.Generic;

namespace MachineLearningNeuralNetwork.Logic
{
    public class Neuron
    {

        private double _bias;
        private double _error;
        private double _input;
        private const double Steepness = 1;
        private double output = double.MinValue;
        private readonly List<Weight> _weights;

        public double Output
        {
            get
            {
                if (output != double.MinValue)
                {
                    return output;
                }
                return 1 / (1 + Math.Exp(-Steepness * (_input + _bias)));
            }
            set
            {
                output = value;
            }
        }

        

        private double Derivative
        {
            get 
            {
                double activation = Output;
                return activation * (1 - activation);
            }
        }

        public Neuron()
        {
           
        }

        public Neuron(Layer inputNeurons, Random rnd)
        {
            _weights = new List<Weight>();
            foreach (var inputNeuron in inputNeurons)
            {
                var weight = new Weight
                    {
                        InputNeuron = inputNeuron,
                        Value = rnd.NextDouble()*2 - 1
                    };
                _weights.Add(weight);
            }
        }

        public void Init()
        {
            _error = 0;
            _input = 0;
            foreach (var weight in _weights)
            {
                _input += weight.InputNeuron.Output * weight.Value;
            }
        }

        public void CalculateError(double delta)
        {
            if (_weights != null)
            {
                _error += delta;
                foreach (var weight in _weights)
                {
                    weight.InputNeuron.CalculateError(_error * weight.Value);
                }
            }
        }

        public void ReassignWeights()
        {
            foreach (var weight in _weights)
            {
                weight.Value += _error * Derivative * LearningRate.Instance.Rate * weight.InputNeuron.Output;
            }
            _bias += _error * Derivative * LearningRate.Instance.Rate;
        }
    }
}
