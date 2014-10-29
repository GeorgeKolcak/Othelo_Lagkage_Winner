using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    class ReviewParser
    {
        private const string EmoticonRegex = @"([<>]?[:;=8][\-o\*\']?[\)\]\(\[dDpP/\:\}\{@\|\\])|([\)\]\(\[dDpP/\:\}\{@\|\\][\-o\*\']?[:;=8][<>]?)";
        private const string PhoneNumberRegex = @"((\+?[01][\-\s.])?([\(]?\d{3}[\-\s.\)]*)?\d{3}[\-\s.]*\d{4})";
        private const string HtmlTagRegex = @"(<[^>]+>)";
        private const string AccountRegex = @"([\w_]+@[\w_]+)";
        private const string HashtagRegex = @"(\#+[\w_]+[\w\'_\-]*[\w_]+)";
        private const string WordRegex = @"([a-z][a-z'\-_]+[a-z'])";
        private const string NumberRegex = @"([+\-]?\d+[,/.:-]\d+[+\-]?)";
        private const string EverythingElseNotWhitespaceRegex = @"(\S)";

        private static readonly string[] RegexPatterns = new string[]
        {
            EmoticonRegex,
            PhoneNumberRegex,
            HtmlTagRegex,
            AccountRegex,
            HashtagRegex,
            WordRegex,
            NumberRegex,
            EverythingElseNotWhitespaceRegex
        };

        public static IEnumerable<Tuple<Feature[], byte>> Parse(string file)
        {
            int lineNumber = 0;

            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                byte score = 0;
                string summary;

                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    Console.Write("\r" + lineNumber);

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
                        yield return new Tuple<Feature[], byte>(Tokenise(line.Substring(13)).Distinct().ToArray(), score);
                    }
                }
            }

            Console.WriteLine();
        }

        public static IEnumerable<Feature> Tokenise(string text)
        {
            foreach (Match match in Regex.Matches(text, String.Join("|", RegexPatterns)))
            {
                yield return new Feature(match.Value);
            }
        }
    }
}
