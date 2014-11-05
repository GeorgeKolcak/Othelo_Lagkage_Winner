using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class NaiveBayesLearner : SentimentClassifierLearner
    {
        public NaiveBayesLearner()
            : base() { }

        public override void Learn(Dictionary<byte, int> sentimentCount, Dictionary<Feature, Dictionary<byte, int>> features)
        {
            int reviewCount = sentimentCount.Values.Sum();

            foreach (byte sentiment in sentimentCount.Keys)
            {
                sentimentProbabilities.Add(sentiment, ((sentimentCount[sentiment] + 1.0) / (reviewCount + sentimentCount.Count)));

                featureProbabilities.Add(sentiment, new Dictionary<Feature, double>());
            }

            Console.WriteLine("Feature Count: {0}", features.Count);

            int i = 0;
            foreach (Feature feature in features.Keys)
            {
                i++;
                Console.Write("\rFeature: {0}", i);

                Dictionary<byte, int> featureSentimentCount = features[feature];
                int featureCount = featureSentimentCount.Values.Sum();

                foreach (byte sentiment in sentimentCount.Keys)
                {
                    featureProbabilities[sentiment].Add(feature, ((featureSentimentCount[sentiment] + 1.0) / (featureCount + features.Count)));
                }
            }

            Console.WriteLine();

            //sentimentProbabilities = sentimentClasses.ToDictionary(sentiment => sentiment,
            //    sentiment => ((trainingData.Where(tuple => (tuple.Item2 == sentiment)).Count() + 1.0) / (ClassCount + trainingData.Count)));

            //featureProbabilities = sentimentClasses.ToDictionary(sentiment => sentiment,
            //    sentiment => allFeatures.ToDictionary(feature => feature, feature => ((trainingData.Where(tuple => ((tuple.Item2 == sentiment) && tuple.Item1.Contains(feature))).Count() + 1.0) /
            //        (trainingData.Where(tuple => (tuple.Item2 == sentiment)).Count() + FeatureCount))));
        }
    }
}
