using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningNeuralNetwork.Logic
{
    public class LearningRate
    {
        private static LearningRate _instance;

        private LearningRate()
        {
            Rate = 0.25;
        }

        public static LearningRate Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LearningRate();
                }
                return _instance;
            }
        }

        public double Rate { get; set; }
    }
}
