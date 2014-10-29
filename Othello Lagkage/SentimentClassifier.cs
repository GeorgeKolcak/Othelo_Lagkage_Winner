using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class SentimentClassifier
    {
        private Dictionary<byte, Dictionary<Feature, double>> featureProbabilities;

        private Dictionary<byte, double> emptyFeatureSetProbabilities;

        public SentimentClassifier(Dictionary<byte, Dictionary<Feature, double>> featureProbabilities)
        {
            this.featureProbabilities = featureProbabilities;

            emptyFeatureSetProbabilities = featureProbabilities.Keys.ToDictionary(sentiment => sentiment, sentiment => featureProbabilities[sentiment].Values.Aggregate((a, b) => (a * b)));
        }

        public byte Classify(IEnumerable<Feature> featureVector)
        {
            Dictionary<byte, double> scores = emptyFeatureSetProbabilities.Keys.ToDictionary(sentiment => sentiment,
                sentiment => (emptyFeatureSetProbabilities[sentiment] * featureVector.Select(feature => featureProbabilities[sentiment][feature] / (1 - featureProbabilities[sentiment][feature]))
                    .Aggregate((a,b) => (a*b))));

            return scores.Keys.Aggregate<byte>((a, b) => ((scores[b] > scores[a]) ? b : a));
        }
    }
}
