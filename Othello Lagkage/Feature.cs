using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Lagkage
{
    public class Feature
    {
        public string ID { get; private set; }

        public Feature(string id)
        {
            ID = id.Replace(" ", "").Replace("\t", "");
        }

        public override string ToString()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            Feature other;
            if ((other = obj as Feature) == null)
            {
                return false;
            }

            return (ID == other.ID);
        }

        public override int GetHashCode()
        {
            return (137 + (13 * ID.GetHashCode()));
        }
    }
}
