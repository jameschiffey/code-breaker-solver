using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CodeBreakerSolver
{
    /// <summary>
    /// Represents a solver for a single round of Code Breaker Game.
    /// </summary>
    internal class Solver
    {
        private readonly List<string> _codes = new();
        
        /// <summary>
        /// Gets a boolean indicating whether the code is still solvable. The code may become unsolveable if feedback is entered incorrectly.
        /// </summary>
        public bool IsSolveable => _codes.Count > 0;

        /// <summary>
        /// Gets a boolean indicating whether the code has been solved.
        /// </summary>
        public bool IsSolved => _codes.Count == 1;

        /// <summary>
        /// Gets the next guess that should be entered into the game. If the code has been solved, this value will be the correct code.
        /// </summary>
        public string NextGuess => _codes.FirstOrDefault();

        /// <summary>
        /// Gets the number of possible codes remaining.
        /// </summary>
        public int PossibleCodes => _codes.Count;

        /// <summary>
        /// Constructs a new instance of <see cref="Solver"/>.
        /// </summary>
        /// <param name="codePegColors">A list of possible colours, represented as a string of chars. e.g. If possible colours are Red, Green and Blue, supply "RGB".</param>
        /// <param name="codePegs">The number of codes pegs in the code.</param>
        /// <param name="hasDuplicatesInCode">Specifies whether the code can contain duplicate colours.</param>
        public Solver(string codePegColors, int codePegs, bool hasDuplicatesInCode)
        {
            if (String.IsNullOrWhiteSpace(codePegColors))
            {
                throw new ArgumentException("You must supply code peg colours.", nameof(codePegColors));
            }

            if (codePegColors.ToCharArray().Distinct().Count() != codePegColors.Length)
            {
                throw new ArgumentException("Code peg colours are not unique", nameof(codePegColors));
            }

            if (codePegs < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(codePegs), "Number of code pegs must be greater than or equal to 1.");
            }

            GenerateCodes(codePegColors.ToCharArray(), codePegs, "", hasDuplicatesInCode, _codes);
        }

        /// <summary>
        /// Accepts feedback from the game for the previous guess.
        /// </summary>
        public void GiveFeedback(int blackPegs, int whitePegs)
        {
            if (this.IsSolved || !this.IsSolveable)
            {
                throw new InvalidOperationException("The solver has already solved the code, or the code is no longer solveable.");
            }

            string previousGuess = this.NextGuess;

            // Remove any code that can't be the answer, based on the feedback pegs
            _codes.RemoveAll(x => !IsPossibleCode(x, previousGuess, blackPegs, whitePegs));
        }

        /// <summary>
        /// Recursively generates all possible codes.
        /// </summary>
        private void GenerateCodes(char[] codePegColors, int codePegCount, string current, bool allowDuplicates, List<string> combinations)
        {
            if (current.Length == codePegCount)
            {
                combinations.Add(current);
                return;
            }

            foreach (char c in codePegColors)
            {
                if (allowDuplicates || !current.Contains(c))
                {
                    GenerateCodes(codePegColors, codePegCount, current + c, allowDuplicates, combinations);
                }
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether the combination of code and guess would result in the number of black and white feedback pegs.
        /// </summary>
        private bool IsPossibleCode(string code, string guess, int blackPegs, int whitePegs)
        {
            int expectedBlackPegs = GetColorAndPositionMatchCount(code, guess);
            int expectedWhitePegs = GetColorMatchCount(code, guess) - expectedBlackPegs;

            return blackPegs == expectedBlackPegs && whitePegs == expectedWhitePegs;
        }

        /// <summary>
        /// Gets the number of pegs in the guess that match a peg in the code by colour. A peg in the code can only be matched on once.
        /// </summary>
        private int GetColorMatchCount(string code, string guess)
        {
            List<char> codeList = new(code.ToCharArray());

            int count = 0;

            foreach (char c in guess)
            {
                if (codeList.Contains(c))
                {
                    count++;
                    codeList.Remove(c); // Remove from list so we don't match it again.
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the number of pegs in the guess that match a peg in the code by colour and position.
        /// </summary>
        private int GetColorAndPositionMatchCount(string code, string guess)
        {
            int count = 0;

            for (int i = 0; i < code.Length; i++)
            {
                if (guess[i] == code[i])
                {
                    count++;
                }
            }

            return count;
        }
    }
}
