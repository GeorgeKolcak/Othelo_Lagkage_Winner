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
    enum FeatureType
    {
        Unknown,
        Account,
        Emoticon,
        Hashtag,
        HTMLTag,
        Punctuation,
        Number,
        PhoneNumber,
        Word
    }

    class ReviewParser
    {
        private const string EmoticonRegex = @"([<>]?[:;=8][\-o\*\']?[\)\]\(\[dDpP/\:\}\{@\|\\])|([\)\]\(\[dDpP/\:\}\{@\|\\][\-o\*\']?[:;=8][<>]?)";
        private const string PhoneNumberRegex = @"\+?(\d[\d-. \\/]+)?(\([\d-. \\/]+\))?[\d-. \\/]+\d";
        private const string HtmlTagRegex = @"(<[^>]+>)";
        private const string AccountRegex = @"([\w_]+@[\w_]+)";
        private const string HashtagRegex = @"(\#+[\w_]+[\w\'_\-]*[\w_]+)";
        private const string WordRegex = @"([a-z]+[a-z'\-_]*[a-z']*)+";
        private const string NumberRegex = @"[+\-]?\d+[,/.:-]?\d*[+\-]?";
        private const string PunctuationRegex = @"[;:.!?]+";
        private const string OtherRegex = @"(\S)";
        private const string NegationRegex = @"never|no|nothing|nowhere|noone|none|not|haven't|havent|hasn't|hasnt|hadn't|hadnt|can't|cant|couldn't|couldnt|shouldn't|shouldnt|won't|wont|wouldn't|wouldnt|don't|dont|doesn't|doesnt|didn't|didnt|isn't|isnt|aren't|arent|ain't|aint";

        public static IEnumerable<Tuple<Feature[], byte>> Parse(string file)
        {
            int lineNumber = 0;

            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                byte score = 0;
                string summary = String.Empty;

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
                        yield return new Tuple<Feature[], byte>(Tokenise(line.Substring(13)).Concat(Tokenise(summary, true)).Distinct().ToArray(), score);
                    }
                }
            }

            Console.WriteLine();
        }

        private static Feature BuildFeature(string value, bool title, bool negation = false)
        {
            return new Feature(String.Format("{0}{1}{2}", (title ? "TIT_" : String.Empty), value, (negation ? "_NEG" : String.Empty)));
        }

        public static IEnumerable<Feature> Tokenise(string text, bool title = false)
        {
            text = Regex.Replace(text, HtmlTagRegex, String.Empty);

            string priorityPatterns = String.Format("({0})|({1})|({2})|({3})|({4})", EmoticonRegex, PhoneNumberRegex, NumberRegex, HashtagRegex, AccountRegex);

            foreach (Match match in Regex.Matches(text, priorityPatterns))
            {
                yield return BuildFeature(match.Value, title);
            }

            text = Regex.Replace(text, priorityPatterns, String.Empty);

            foreach (Match match in Regex.Matches(text, PunctuationRegex))
            {
                yield return BuildFeature(match.Value, title);
            }

            string[] sentences = Regex.Split(text, PunctuationRegex);

            foreach (string sentence in sentences)
            {
                bool negation = false;

                foreach (Match match in Regex.Matches(sentence, WordRegex, RegexOptions.IgnoreCase))
                {
                    if (!negation && Regex.IsMatch(match.Value, NegationRegex))
                    {
                        negation = true;
                    }

                    yield return BuildFeature(match.Value, title, negation);
                }

                string remains = Regex.Replace(sentence, WordRegex, String.Empty, RegexOptions.IgnoreCase);

                foreach (Match match in Regex.Matches(remains, OtherRegex))
                {
                    yield return BuildFeature(match.Value, title);
                }
            }

            //Feature lastMatch = null;
            //string currentString = String.Empty;
            //bool negation =false;

            //for (int i = 0; i < text.Length; i++ )
            //{
            //    currentString += text[i];

            //    bool foundMatch = false;
            //    foreach (FeatureType type in RegexPatterns.Keys)
            //    {
            //        if (Regex.IsMatch(currentString, RegexPatterns[type], RegexOptions.IgnoreCase))
            //        {
            //            lastMatch = BuildFeature(currentString, type, negation, title);
            //            foundMatch = true;
            //            break;
            //        }
            //    }

            //    if (!foundMatch && lastMatch != null)
            //    {
            //        yield return lastMatch;

            //        if (negation && (lastMatch.Type == FeatureType.Punctuation) && !Regex.IsMatch(lastMatch.ID, ",+"))
            //        {
            //            negation = false;
            //        }

            //        if (!negation && (lastMatch.Type == FeatureType.Word) && NegationWords.Contains(lastMatch.ID.ToLowerInvariant()))
            //        {
            //            negation = true;
            //        }

            //        lastMatch = null;
            //        currentString = String.Empty;

            //        if (!Char.IsWhiteSpace(text[i]))
            //        {
            //            i--;
            //        }
            //    }

            //    if (String.IsNullOrWhiteSpace(currentString))
            //    {
            //        currentString = String.Empty;
            //    }
            //}

            //if (lastMatch != null)
            //{
            //    yield return lastMatch;
            //}

            //foreach (Match match in Regex.Matches(text, String.Join("|", RegexPatterns)))
            //{
            //    yield return new Feature(match.Value);
            //}
        }
    }
}
