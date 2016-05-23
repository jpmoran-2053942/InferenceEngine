using System;
using System.Collections.Generic;

namespace InferenceEngine
{
    public struct HornClause
    {
        public HornClause(string[] allPremise, string allConclusion)
        {
            premise = new List<string>();
            premise.AddRange(allPremise);
            conclusion = allConclusion;
        }

        public HornClause(string allPremise)
        {
            premise = new List<string>();
            premise.Add(allPremise);
            conclusion = null;
        }

        public List<string> premise;
        public string conclusion;
    }
}

