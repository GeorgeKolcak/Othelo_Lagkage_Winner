using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class ReviewParser
    {
        public static IEnumerable<Tuple<Feature[], byte>> Parse(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                byte score = 0;
                string summary;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("review/score:"))
                    {
                        score = (byte)Double.Parse(line.Substring(14), CultureInfo.InvariantCulture);
                    }

                    if (line.StartsWith("review/summary:"))
                    {
                        summary = line.Substring(16);
                    }

                    if (line.StartsWith("review/text:"))
                    {
                        yield return new Tuple<Feature[], byte>(Tokenise(line.Substring(13)).ToArray(), score);
                    }
                }
            }
        }

        public static IEnumerable<Feature> Tokenise(string text)
        {


            yield break;
        }
    }
}
