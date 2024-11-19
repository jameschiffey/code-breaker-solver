using System;
using System.Text;

namespace CodeBreakerSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            RenderInstructions();

            // Read in the parameters of the game
            string possibleColors;
            string codePegsString;
            int codePegs;
            string allowDuplicatesString;
            bool allowDuplicates;

            do
            {
                WriteLine();
                WriteLine("Enter the possible colours:");
                possibleColors = Console.ReadLine().ToUpper();
            } 
            while (String.IsNullOrEmpty(possibleColors));

            do
            {
                WriteLine();
                WriteLine("Enter the number of pegs:");
                codePegsString = Console.ReadLine();
            }
            while (!Int32.TryParse(codePegsString, out codePegs));

            do
            {
                WriteLine();
                WriteLine("Are duplicate colours allowed? (Y/N)");
                allowDuplicatesString = Console.ReadLine().ToUpper();
            }
            while (allowDuplicatesString != "Y" && allowDuplicatesString != "N");

            allowDuplicates = (allowDuplicatesString == "Y");

            // Construct a solver
            Solver solver = new(possibleColors,codePegs, allowDuplicates);            

            // Loop until code is solved
            do
            {
                WriteLine();
                WriteLine($"There are {solver.PossibleCodes} possible codes.");                

                WriteLine($"Guess: {solver.NextGuess}");

                // Get feedback from user
                string blackPegsString;
                int blackPegs;
                string whitePegsString;
                int whitePegs;

                do
                {
                    WriteLine();
                    WriteLine("Enter the number of black feedback pegs:");
                    blackPegsString = Console.ReadLine();
                }
                while (!Int32.TryParse(blackPegsString, out blackPegs));

                do
                {
                    WriteLine();
                    WriteLine("Enter the number of white feedback pegs:");
                    whitePegsString = Console.ReadLine();
                }
                while (!Int32.TryParse(whitePegsString, out whitePegs));

                solver.GiveFeedback(blackPegs, whitePegs);
            } 
            while (!solver.IsSolved && solver.IsSolveable);

            // Check that the user didn't make a mistake entering feedback
            if (solver.IsSolveable)
            {
                WriteLine();
                WriteLine($"The code is {solver.NextGuess}.");
            }
            else
            {
                WriteLine();
                WriteLine($"The code is not solveable. Did you make a mistake entering feedback?");
            }

            WriteLine();
            WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        /// <summary>
        /// Renders the instructions to the console.
        /// </summary>
        private static void RenderInstructions()
        {
            WriteLine("Welcome to Code Breaker Solver.", ConsoleColor.White);
            WriteLine();

            WriteLine("How to use:", ConsoleColor.White);
            WriteLine("• Enter the possible colours as a sequence of unique characters. e.g. If the possible colours are Red, Green, Blue and Yellow, enter: RGBY");
            WriteLine("• Enter the number of pegs in the code.");
            WriteLine("• Indicate whether duplicate colours are allowed within the code.");
            WriteLine("• The solver will suggest a guess. Enter the guess into the game, and then enter the number of black and white \"feedback\" pegs into the solver.");
            WriteLine("• Keep doing this until you win!");
            WriteLine();

            WriteLine("Press Ctrl + C at any time to quit.");
        }

        /// <summary>
        /// Writes text, followed by a line terminator, to the console using the provided foreground colour.
        /// </summary>
        private static void WriteLine(object value = null, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = foregroundColor;

            Console.WriteLine(value);

            // Restore default colour
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
