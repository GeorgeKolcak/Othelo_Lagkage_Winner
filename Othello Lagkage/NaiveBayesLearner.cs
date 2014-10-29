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

        public override void Learn(Tuple<Feature[], byte>[] trainingData)
        {
            IGrouping<byte, Tuple<Feature[], byte>>[] reviewsBySentiment = trainingData.GroupBy(tuple => tuple.Item2).ToArray();

            Feature[] allFeatures = trainingData.SelectMany(tuple => tuple.Item1).Distinct().ToArray();

            Console.WriteLine("Class Count: {0}", reviewsBySentiment.Length);
            Console.WriteLine("Feature Count: {0}", allFeatures.Length);

            foreach(IGrouping<byte, Tuple<Feature[], byte>> sentiment in reviewsBySentiment)
            {
                Console.Write("\r" + sentiment.Key + " ");

                sentimentProbabilities.Add(sentiment.Key, ((sentiment.Count() + 1.0) / (trainingData.Count() + reviewsBySentiment.Length)));

                featureProbabilities.Add(sentiment.Key, new Dictionary<Feature, double>());

                int i = 0;
                foreach (Feature feature in allFeatures)
                {
                    i++;
                    Console.Write(i);

                    int featureCountPerSentiment = sentiment.Where(tuple => tuple.Item1.Contains(feature)).Count();

                    featureProbabilities[sentiment.Key].Add(feature, ((featureCountPerSentiment + 1.0) / (sentiment.Count() + allFeatures.Length)));
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
