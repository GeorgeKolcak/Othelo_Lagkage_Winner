﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class SentimentClassifier
    {
        private Dictionary<byte, Dictionary<Feature, double>> featureProbabilities;
        private Dictionary<byte, double> sentimentProbabilities;

        private Dictionary<byte, double> emptyFeatureSetProbabilities;

        public Dictionary<byte, Dictionary<Feature, double>> FeatureProbabilities
        {
            get
            {
                return featureProbabilities; //TEMP
            }
        }

        public SentimentClassifier(Dictionary<byte, Dictionary<Feature, double>> featureProbabilities, Dictionary<byte, double> sentimentProbabilities)
        {
            this.featureProbabilities = featureProbabilities;
            this.sentimentProbabilities = sentimentProbabilities;

            emptyFeatureSetProbabilities = featureProbabilities.Keys.ToDictionary(sentiment => sentiment,
                sentiment => (sentimentProbabilities[sentiment] * featureProbabilities[sentiment].Values.Aggregate((a, b) => (a * b))));
        }

        public byte Classify(IEnumerable<Feature> featureVector)
        {
            Dictionary<byte, double> scores = emptyFeatureSetProbabilities.Keys.ToDictionary(sentiment => sentiment,
                sentiment => (emptyFeatureSetProbabilities[sentiment] * featureVector.Select(feature => featureProbabilities[sentiment][feature] / (1 - featureProbabilities[sentiment][feature]))
                    .Aggregate((a,b) => (a*b))));

            return scores.Keys.Aggregate<byte>((a, b) => ((scores[b] > scores[a]) ? b : a));
        }

        public void Save(string fileName)
        {
            Feature[] allFeatures = featureProbabilities.Keys.SelectMany(sentiment => featureProbabilities[sentiment].Keys).Distinct().ToArray();

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (byte sentiment in sentimentProbabilities.Keys)
                {
                    sw.WriteLine("{0} {1}", sentiment, sentimentProbabilities[sentiment]);
                }

                foreach (Feature feature in allFeatures)
                {
                    sw.Write("{0}", feature);
                    
                    foreach(byte sentiment in featureProbabilities.Keys)
                    {
                        sw.Write(" {0};{1}", sentiment, featureProbabilities[sentiment][feature]);
                    }

                    sw.WriteLine();
                }
            }
        }

        public static SentimentClassifier Load(string fileName)
        {
            Dictionary<byte, double> sentimentProbabilities = new Dictionary<byte, double>();
            Dictionary<byte, Dictionary<Feature, double>> featureProbabilities = new Dictionary<byte, Dictionary<Feature, double>>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                for (int i = 0; i < 5; i++)
                {
                    string[] segments = sr.ReadLine().Split(' ');

                    sentimentProbabilities.Add(Byte.Parse(segments[0]), Double.Parse(segments[1], CultureInfo.InvariantCulture));
                    featureProbabilities.Add(Byte.Parse(segments[0]), new Dictionary<Feature,double>());
                }

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] segments = line.Split(' ');

                    Feature feature = new Feature(segments[0]);

                    for (int i = 1; i < segments.Length; i++)
                    {
                        string[] subsegments = segments[i].Split(';');

                        featureProbabilities[Byte.Parse(subsegments[0])].Add(feature, Double.Parse(subsegments[1], CultureInfo.InvariantCulture));
                    }
                }
            }

            return new SentimentClassifier(featureProbabilities, sentimentProbabilities);
        }
    }
}
