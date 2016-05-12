# InferenceEngine

Changes in add-evaluate-nodes-functionality
 - Adds evaluate node function to all classes inheriting from NodeOrStringInterface
 - EvaluateLogic function added to CNF class to evaluate whether a given statement is true or false
 - CreateBinaryTree and ConvertToStringList are called from EvaluateLogic in CNF
	- Still called from Program.cs since we don't yet have a model to pass in to EvaluateLogic

Changes in binary-tree-multi-bracket-fix-a
 - Fixes bugs in ConjunctiveNormalForm.CreateBinaryTree(string):
  - If multiple brackets appear in a sentence, only the final set will be addressed (nested brackets work fine)
  - Brackets sub-trees are always placed at the end of the list
 - Moved the code to create bracket sub-trees into the for loop. This fixes the first bug because before the code would only attempt to create a sub-tree for brackets once it had completely searched the sentence, so finding a new set of brackets overrided the old set before they had been addressed.
 - Fixed the code that adds the bracket sub-trees to the sentence. It now correctly replaces the brackets instead of always placing it at the end.
 - Fixed the search position not being updated once sub-sentences had been replaced. Involved changes to the ADD, OR, and bracketed parts. Replacing the string before a special character and not altering the i values would sometimes cause other special characters to be skipped.
 - Added comments to increase clarity of the CreateBinaryTree method.