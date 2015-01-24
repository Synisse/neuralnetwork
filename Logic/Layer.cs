using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningNeuralNetwork.Logic
{
    public class Layer : List<Neuron>
    {
        public Layer(int size)
        {
            for (int i = 0; i < size; i++)
            {
                base.Add(new Neuron());
            }
        }

        public Layer(int size, Layer layer, Random random)
        {
            for (int i = 0; i < size; i++)
            {
                base.Add(new Neuron(layer, random));
            }
        }

        public double[] getAllOutputs()
        {
            double[] array = new double[base.Count];
            int i = 0;
            foreach (var neuron in this)
            {
                array[i++] = neuron.Output;
            }
            return array;
        }

        public int getMaxOutput()
        {
            double maxValue = -1;
            int maxPosition = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Output > maxValue)
                {
                    maxValue = this[i].Output;
                    maxPosition = i;
                }
            }

            return maxPosition;
        }
    }
}
