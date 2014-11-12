using Othello_Lagkage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competition
{
    class User
    {
        public string ID { get; private set; }

        public string[] Friends { get; private set; }

        public Feature[] Review { get; private set; }

        private byte _reviewClassification;

        public byte ReviewClassification
        {
            get
            {
                return _reviewClassification;
            }
            set
            {
                _reviewClassification = value;
            }
        }

        public string ReviewClass
        {
            get
            {
                if (_reviewClassification == 0)
                {
                    return "*";
                }

                return _reviewClassification.ToString();
            }
        }

        public int Cluster { get; set; }

        public User(string id, IEnumerable<string> friends, string title, string review)
        {
            ID = id;

            Friends = friends.ToArray();

            if (title.Trim() == "*")
            {
                Review = null;
            }
            else
            {
                Review = ReviewParser.Tokenise(review).Concat(ReviewParser.Tokenise(title, true)).ToArray();
            }
        }
    }
}
