using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningNeuralNetwork.Logic
{
    public class Network : List<Layer>
    {
        private int[] _dimensions;
        private List<InputSet> _inputSets;

        private Layer Inputs
        {
            get { return base[0]; }
        }

        private Layer Outputs
        {
            get { return base[base.Count - 1]; }
        }

        public Network(int[] dimensions, List<double[]> trainingValues)
        {
            _dimensions = dimensions;
            Initialise();
            InitTrainValues(trainingValues);
        }


        public void UpdateTrainvalueSet(List<double[]> trainingValues)
        {
            InitTrainValues(trainingValues);
        }

        private void Initialise()
        {
            //reset instance
            base.Clear();

            //init inputlayer
            base.Add(new Layer(_dimensions[0]));

            //init hidden and outputlayer
            for (int i = 1; i < _dimensions.Length; i++)
            {
                base.Add(new Layer(_dimensions[i], base[i-1],new Random()));
            }

        }

        private void InitTrainValues(List<double[]> trainingValues)
        {
            _inputSets = new List<InputSet>();
            foreach (var trainValue in trainingValues)
            {
                _inputSets.Add(new InputSet(trainValue,Inputs.Count,Outputs.Count));
            }
        }

        public double TrainNetwork()
        {
            double error = 0;

            foreach (var inputSet in _inputSets)
            {
                ActivateInputSet(inputSet);

                for (int i = 0; i < Outputs.Count; i++)
                {
                    double delta = inputSet.Outputs[i] - Outputs[i].Output;
                    Outputs[i].CalculateError(delta);
                    error += Math.Pow(delta, 2);
                }
                AdjustWeights();
            }
            return error;
        }

        private void ActivateInputSet(InputSet inputSet)
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                Inputs[i].Output = inputSet.Inputs[i];
            }

            for (int i = 1; i < base.Count; i++)
            {
                foreach (var neuron in base[i])
                {
                    neuron.Init();
                }
            }

            

        }

        public int ActivateValueSet(double[] values)
        {
            InputSet inputSet = new InputSet(values, Inputs.Count,Outputs.Count);
            if (values.Length == (Inputs.Count + Outputs.Count))
            {
                for (int i = 0; i < Inputs.Count; i++)
                {
                    Inputs[i].Output = inputSet.Inputs[i];
                }

                for (int i = 1; i < base.Count; i++)
                {
                    foreach (var neuron in base[i])
                    {
                        neuron.Init();
                    }
                }
            }

            return base[base.Count - 1].getMaxOutput();
        }

        private void AdjustWeights()
        {
            //backpropagate weights
            for (int i = base.Count-1; i > 0 ; i--)
            {
                foreach (var neuron in base[i])
                {
                    neuron.ReassignWeights();
                }
            }

        }

    }
}
