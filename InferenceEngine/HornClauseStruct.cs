using System;
using System.Collections.Generic;

/// <summary>
/// Stores horn clauses in a form convenient for forward and backward chaining.
/// This inovolves storing the premises as a list of strings and the conclusion
/// as an explicit variable. If a clause is a fact, the conclusion is null.
/// </summary>
namespace InferenceEngine
{
    public struct HornClause
    {
        public List<string> premise;
        public string conclusion;
        
        /// <summary>
        /// Constructor for horn clauses with conclusions.
        /// </summary>
        /// <param name="allPremise">The array of premises.</param>
        /// <param name="allConclusion">The conclusion.</param>
        public HornClause(string[] allPremise, string allConclusion)
        {
            premise = new List<string>();
            premise.AddRange(allPremise);
            conclusion = allConclusion;
        }

        /// <summary>
        /// Constructor for truth statements. The conclusion is null.
        /// </summary>
        /// <param name="allPremise">The array of premises.</param>
        public HornClause(string allPremise)
        {
            premise = new List<string>();
            premise.Add(allPremise);
            conclusion = null;
        }

    }
}

