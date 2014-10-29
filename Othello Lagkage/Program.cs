using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class Program
    {
        static void Main(string[] args)
        {
            SentimentClassifierLearner learner = new NaiveBayesLearner();

            //learner.AddTrainingData(ReviewParser.Parse("SentimentTrainingData.txt"));

            //learner.AddTrainingData(new Feature[] { new Feature("very"), new Feature("bad"), new Feature("recommend_NEG") }, 1);
            //learner.AddTrainingData(new Feature[] { new Feature("very"), new Feature("good"), new Feature("recommend") }, 2);
            //learner.AddTrainingData(new Feature[] { new Feature("atrocious"), new Feature("bad"), new Feature("absolutely") }, 1);
            //learner.AddTrainingData(new Feature[] { new Feature("incredibly"), new Feature("superb"), new Feature("recommend") }, 2);
            //learner.AddTrainingData(new Feature[] { new Feature("good"), new Feature("but"), new Feature("not"), new Feature("recommend_NEG") }, 2);

            learner.Learn(ReviewParser.Parse("SentimentTrainingData.txt").ToArray());

            SentimentClassifier classifier = learner.Finalise();
            
            foreach (byte sentiment in classifier.FeatureProbabilities.Keys)
            {
                foreach (Feature feature in classifier.FeatureProbabilities[sentiment].Keys)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", sentiment, feature, classifier.FeatureProbabilities[sentiment][feature]);
                }
            }

            Console.ReadKey();
        }
    }
}
