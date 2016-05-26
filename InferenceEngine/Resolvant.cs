using System;

/// <summary>
/// This class is used by the Resolution Algorithm. It stores the parent clauses
/// that are used to produce a new resolvent. This is needed to identify the
/// clauses that were critical to producing this resolvent. Initial clauses that
/// were not resolved have null as parents.
/// </summary>
namespace InferenceEngine
{
    public class Resolvant
    {
        public Resolvant _parentA;
        public Resolvant _parentB;
        public string _clause;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parentA">First parent used to generate this resolvent.</param>
        /// <param name="parentB">Second parent used to generate this resolvent.</param>
        /// <param name="clause">The actual clause as a string.</param>
        public Resolvant(Resolvant parentA, Resolvant parentB, string clause)
        {
            _parentA = parentA;
            _parentB = parentB;
            _clause = clause;
        }
    }
}

