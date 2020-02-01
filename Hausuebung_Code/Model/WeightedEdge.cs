using System;
using System.Collections.Generic;
using System.Text;

namespace AGT.Model
{
    public class WeightedEdge
    {
        public Vertex From { get; }
        public Vertex To { get; }
        public double Weight { get; set; }

        public WeightedEdge(Vertex from, Vertex to, double weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }
}
