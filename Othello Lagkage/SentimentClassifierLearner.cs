using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    public abstract class SentimentClassifierLearner
    {
        protected Dictionary<byte, Dictionary<Feature, double>> featureProbabilities;

        protected List<Tuple<Feature[], byte>> trainingData;
        protected List<Tuple<Feature[], byte>> validationData;

        protected HashSet<byte> sentimentClasses;
        protected HashSet<Feature> allFeatures;

        protected int ClassCount
        {
            get
            {
                return sentimentClasses.Count;
            }
        }

        protected int FeatureCount
        {
            get
            {
                return allFeatures.Count;
            }
        } 

        public SentimentClassifierLearner()
        {
            featureProbabilities = new Dictionary<byte, Dictionary<Feature, double>>();

            trainingData = new List<Tuple<Feature[], byte>>();
            validationData = new List<Tuple<Feature[], byte>>();

            allFeatures = new HashSet<Feature>();
        }

        public void AddTrainingData(Feature[] featureSet, byte sentiment)
        {
            sentimentClasses.Add(sentiment);

            foreach(Feature feature in featureSet)
            {
                allFeatures.Add(feature);
            }

            trainingData.Add(new Tuple<Feature[], byte>(featureSet, sentiment));
        }

        public void AddValidationData(Feature[] featureSet, byte sentiment)
        {
            sentimentClasses.Add(sentiment);

            foreach (Feature feature in featureSet)
            {
                allFeatures.Add(feature);
            }

            validationData.Add(new Tuple<Feature[], byte>(featureSet, sentiment));
        }

        public void AddTrainingData(IEnumerable<Tuple<Feature[], byte>> data)
        {
            foreach(Tuple<Feature[], byte> tuple in data)
            {
                AddTrainingData(tuple.Item1, tuple.Item2);
            }
        }

        public void AddValidationData(IEnumerable<Tuple<Feature[], byte>> data)
        {
            foreach (Tuple<Feature[], byte> tuple in data)
            {
                AddValidationData(tuple.Item1, tuple.Item2);
            }
        }

        public abstract void Learn();

        public SentimentClassifier Finalise()
        {
            return new SentimentClassifier(featureProbabilities);
        }
    }
}
