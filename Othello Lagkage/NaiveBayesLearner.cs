using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class NaiveBayesLearner : SentimentClassifierLearner
    {
        private Dictionary<byte, double> sentimentProbabilities;

        public NaiveBayesLearner()
            : base()
        {
            sentimentProbabilities = new Dictionary<byte, double>();
        }

        public override void Learn()
        {
            sentimentProbabilities = sentimentClasses.ToDictionary(sentiment => sentiment,
                sentiment => ((trainingData.Where(tuple => (tuple.Item2 == sentiment)).Count() + 1.0) / (ClassCount + trainingData.Count)));

            featureProbabilities = sentimentClasses.ToDictionary(sentiment => sentiment,
                sentiment => allFeatures.ToDictionary(feature => feature, feature => ((trainingData.Where(tuple => ((tuple.Item2 == sentiment) && tuple.Item1.Contains(feature))).Count() + 1.0) /
                    (trainingData.Where(tuple => (tuple.Item2 == sentiment)).Count() + FeatureCount))));
        }
    }
}
