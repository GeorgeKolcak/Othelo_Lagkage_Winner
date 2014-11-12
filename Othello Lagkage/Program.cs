using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class Program
    {
        private static readonly Dictionary<byte, int> DefaultSentimentCount = new Dictionary<byte, int>()
        {
            { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
        };

        static void Main(string[] args)
        {
            Random random = new Random();

            SentimentClassifierLearner learner = new NaiveBayesLearner();

            //learner.AddTrainingData(ReviewParser.Parse("SentimentTrainingData.txt"));

            //learner.AddTrainingData(new Feature[] { new Feature("very"), new Feature("bad"), new Feature("recommend_NEG") }, 1);
            //learner.AddTrainingData(new Feature[] { new Feature("very"), new Feature("good"), new Feature("recommend") }, 2);
            //learner.AddTrainingData(new Feature[] { new Feature("atrocious"), new Feature("bad"), new Feature("absolutely") }, 1);
            //learner.AddTrainingData(new Feature[] { new Feature("incredibly"), new Feature("superb"), new Feature("recommend") }, 2);
            //learner.AddTrainingData(new Feature[] { new Feature("good"), new Feature("but"), new Feature("not"), new Feature("recommend_NEG") }, 2);

            List<Tuple<Feature[], byte>> validationData = new List<Tuple<Feature[], byte>>();

            Dictionary<Feature, Dictionary<byte, int>> features = new Dictionary<Feature, Dictionary<byte, int>>();
            Dictionary<byte, int> globalSentimentCount = new Dictionary<byte, int>(DefaultSentimentCount);

            foreach (Tuple<Feature[], byte> review in ReviewParser.Parse("SentimentTrainingData.txt"))
            {
                if (random.Next(10) == 0)
                {
                    validationData.Add(review);
                }

                globalSentimentCount[review.Item2]++;

                foreach (Feature feature in review.Item1)
                {
                    bool newFeature;
                    Dictionary<byte, int> sentimentCount;

                    if (newFeature = !features.TryGetValue(feature, out sentimentCount))
                    {
                        sentimentCount = new Dictionary<byte, int>(DefaultSentimentCount);
                    }

                    sentimentCount[review.Item2]++;

                    if (newFeature)
                    {
                        features.Add(feature, sentimentCount);
                    }
                }
            }

            learner.Learn(globalSentimentCount, features);

            SentimentClassifier classifier = learner.Finalise();

            //classifier.Save("CLSFR.txt");

            int success = 0;
            int all = 0;
            foreach (Tuple<Feature[], byte> review in validationData)
            {
                all++;

                if (classifier.Classify(review.Item1) == review.Item2)
                {
                    success++;
                }
            }

            Console.WriteLine((double)success / all);

            Console.ReadKey();
        }
    }
}
