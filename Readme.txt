COS30019 Assignment 2 Report

Student Details:
 - James Moran, 2053942, COS30019_A02_T014
 - Madeleine Cochrane, 9993908, COS30019_A02_T014

Features:
 - Determines whether a query can be inferred from a knowledge base using:
  - Truth Table method
   - Accepts any propositional logic sentence as knowledge base and query
  - Forward Chaining
   - Must use horn form and query must be a single symbol
  - Backward Chaining
   - Must use horn form and query must be a single symbol
  - Resolution Algorithm
   - Accepts any propositional logic sentence as knowledge base and query

Test cases:
All algorithms have been tested with the following inputs:

 - The provided test case with the assignment
TELL
p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;
ASK
d

 - Testing various syntax conversions
TELL
p2\/a<=>b=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;
ASK
d

 - Testing interactions between nested implication 
TELL
b>c>d;
ASK
C

 - Testing complex serires of conjunction and disjunction
TELL
(p1&p2+p3)+p4&3+1&(a+b&(c+d));
ASK
C

 - Testing interaction between redundant brackets and negation
TELL
(-(A&B+C));
ASK
C

 - Testing non-trivial proofs with implication
TELL
(P>Q)>Q; (P>P)>R; (R>S)>-(S>Q);
ASK
P

 - Testing chaining with multiple paths
TELL
b>a; c>a; d&e>b; d; e;
ASK
a

 - Testing chaining with multiple paths
TELL
b>a; c>a; d&e>b; c;
ASK
a

 - Testing chaining with multiple paths
TELL
b>a; c>a; f&g>a; d&e>a; d; e;
ASK
a

 - Testing chaining with multiple paths
TELL
b>a; c>a; f&g>a; d&e>a; d; e;
ASK
a

 - Testing chaining with looping
TELL
b>a; a>b;
ASK
a

 - Testing chaining with redundant paths
TELL
B=>A; C>B; B>C; D>C; D
ASK
A

They all function as expected. An interesting observation is that the resolution algorithm sometimes returns all queries as true. This is an issue with the resolution algorithm in general, not a quirk of the implementation. Similarly, Resolution Algorithm can take a long time to determine if a query cannot be inferred. While a more efficient implementation may have been possible, this is also considered a limitation of the algorithm.

Acknowledgements:
All code is completely original. Pseudocode inspired by the following sources:
 - Artificial Intelligence: A Modern Approach (3rd Edition) by Stuart Russell and Peter Norvig
   - Provided pseudocode for the Truth Table, Forward Chaining, and Resolution Algorithm methods
   - Provided relevant background theory for the problems
 - http://cs.jhu.edu/~jason/tutorials/convert-to-CNF by Prof. Jason Eisner
   - Provided pseudocode for converting to conjunctive normal form
 - MIT Lecture on Resolution Theorem, found at http://ocw.mit.edu/courses/electrical-engineering-and-computer-science/6-825-techniques-in-artificial-intelligence-sma-5504-fall-2002/lecture-notes/Lecture7FinalPart1.pdf
   - Provided theory on the Resolution Algorithm, specifically its limitations in certain cases

Notes:
Our program is able to parse all forms of propositional logic. However, the forward and backwards chaining must have the knowledge base in horn form and the query as a single symbol.

Input to the program is as follows:
 InferenceEngine.exe <method> <filename>

The input for each of the methods are as follows:
 - Truth Table = "TT"
 - Forward Chaining = "FC"
 - Backward Chaining = "BC"
 - Resolution Algorithm = "R"

Summary report:
We managed this assignment by collaborating on the planning of the program, then each person being resposible for writing each class. The work breakdown was:
James:
 - FileReader, ClauseParsing, ConvertToCNF, ForwardChainProver, HornClauseReader, ResolutionProver, TranslateSyntax, Resolvants
Maddy:
 - ANDNode, ConjunctiveNormalForm, HoldsString, HornClauseStruct, LeafNode, NOTNode, Node, NodeOrStringInterface, ORNode, Program, BackwardsChainProver

As we collaborated on the overall design, feedback was constant during the planning phase. During development, we used GitHub to keep track of progress and review each other's work. This resulted in several bug fixes between us.

Contribution:
James: 50%
Maddy: 50%

GitHub link: https://github.com/jpmoran-2053942/InferenceEngine