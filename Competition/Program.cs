using Accord.Math;
using Accord.Math.Decompositions;
using Othello_Lagkage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competition
{
    class Program
    {
        private static readonly char[] Separators = new char[] { ' ', '\t' };

        static void Main(string[] args)
        {
            List<User> users = new List<User>();

            using (StreamReader sr = new StreamReader("friendships.reviews.txt"))
            {
                string line;

                string user = String.Empty;
                string[] friends = new string[] { };
                string title = String.Empty;
                string review = String.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        users.Add(new User(user, friends, title, review));
                        continue;
                    }

                    if (line.StartsWith("user:"))
                    {
                        user = line.Substring(6);
                    }
                    else if (line.StartsWith("friends:"))
                    {
                        friends = line.Substring(9).Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else if (line.StartsWith("summary:"))
                    {
                        title = line.Substring(9);
                    }
                    else if (line.StartsWith("review:"))
                    {
                        review = line.Substring(8);
                    }
                }
            }

            SentimentClassifier classifier = SentimentClassifier.Load("CLSFR.txt");

            foreach (User user in users)
            {
                user.ReviewClassification = classifier.Classify(user.Review);
            }

            double[,] laplacian = new double[100, 100];

            for (int i = 0; i < 100; i++)
            {
                int count = 0;

                for (int j = (i + 1); j < 100; j++)
                {
                    if (users[i].Friends.Contains(users[j].ID))
                    {
                        count++;
                        laplacian[i, j] = (-1);
                        laplacian[j, i] = (-1);
                    }
                }

                laplacian[i, i] = count;
            }

            EigenvalueDecomposition decomposition = new EigenvalueDecomposition(laplacian);

            double[][] clusteringData = new double[100][];

            for (int i = 0; i < 100; i++)
            {
                clusteringData[i] = new double[10];

                for (int j = (i + 1); j < 10; j++)
                {
                    clusteringData[i][j] = decomposition.Eigenvectors[i,j];
                }
            }

            int[] clusters = ClusteringKMeans.KMeansDemo.Cluster(clusteringData, 10);

            for (int i = 0; i < 100; i++)
            {
                users[i].Cluster = clusters[i];
            }

            bool[] willBuy = new bool[users.Count];

            for (int i = 0; i < 100; i++)
            {
                int weight = 0;
                double score = 0;

                for (int j = 0; j < 100; j++)
                {
                    if ((users[j].Review != null) && users[i].Friends.Contains(users[j].ID))
                    {
                        int influence = 1;

                        if (users[i].Cluster != users[j].Cluster)
                        {
                            influence *= 10;
                        }

                        if (users[j].ID == "kyle")
                        {
                            influence *= 10;
                        }

                        weight += influence;
                        score += (influence * users[j].ReviewClassification);
                    }
                }

                score /= weight;

                willBuy[i] = (score > 3);
            }

            using (StreamWriter sw = new StreamWriter("results.txt"))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    sw.WriteLine("{0}\t{1}\t{2}", users[i].ID, users[i].ReviewClass, ((users[i].Review == null) ? (willBuy[i] ? "yes" : "no") : "*"));
                }
            }
        }
    }
}
