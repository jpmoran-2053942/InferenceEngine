using System;

namespace InferenceEngine
{
    public class Resolvant
    {
        public Resolvant _parentA;
        public Resolvant _parentB;
        public string _clause;

        public Resolvant(Resolvant parentA, Resolvant parentB, string clause)
        {
            _parentA = parentA;
            _parentB = parentB;
            _clause = clause;
        }
    }
}

