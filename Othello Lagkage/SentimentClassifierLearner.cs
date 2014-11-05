using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    abstract class SentimentClassifierLearner
    {
        protected Dictionary<byte, double> sentimentProbabilities;
        protected Dictionary<byte, Dictionary<Feature, double>> featureProbabilities;

        public SentimentClassifierLearner()
        {
            sentimentProbabilities = new Dictionary<byte, double>();
            featureProbabilities = new Dictionary<byte, Dictionary<Feature, double>>();
        }

        public abstract void Learn(Dictionary<byte, int> sentimentCount, Dictionary<Feature, Dictionary<byte, int>> features);

        public SentimentClassifier Finalise()
        {
            return new SentimentClassifier(featureProbabilities, sentimentProbabilities);
        }
    }
}
