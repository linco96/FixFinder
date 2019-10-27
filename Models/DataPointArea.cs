using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FixFinder.Models
{
    [DataContract]
    public class DataPointArea
    {
        public DataPointArea(string x, double y)
        {
            this.x = x;
            this.Y = y;
        }

        [DataMember(Name = "x")]
        public string x = null;

        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}