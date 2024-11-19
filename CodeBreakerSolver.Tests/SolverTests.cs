using NUnit.Framework;
using System;
using CodeBreakerSolver;

namespace CodeBreakerSolver.Tests
{    
    public class SolverTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("AAB")]        
        public void When_InvalidCodePegColors_Expect_Exception(string codePegColors)
        {
            Assert.Throws<ArgumentException>(() => new Solver(codePegColors, 4, false));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void When_InvalidCodePegs_Expect_Exception(int codePegs)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Solver("RGBY", codePegs, false));
        }

        [Test]
        public void Given_IsSolved_When_GiveFeedback_Expect_Exception()
        {
            Solver s = new("RGBY", 4, false);

            s.GiveFeedback(4, 0);

            Assert.That(s.IsSolved, Is.True);
            Assert.Throws<InvalidOperationException>(() => s.GiveFeedback(1,1));
        }

        [Test]
        public void Given_IsNotSolvable_When_GiveFeedback_Expect_Exception()
        {
            Solver s = new("RGBY", 4, false);

            s.GiveFeedback(3, 1);

            Assert.That(s.IsSolveable, Is.False);
            Assert.Throws<InvalidOperationException>(() => s.GiveFeedback(1, 1));
        }

        [Test]
        public void When_IsNotSolvable_Expect_CorrectState()
        {
            Solver s = new("RGBY", 4, false);

            s.GiveFeedback(3, 1);

            Assert.That(s.IsSolved, Is.False);
            Assert.That(s.IsSolveable, Is.False);
            Assert.That(s.PossibleCodes, Is.Zero);
            Assert.That(s.NextGuess, Is.Null);
        }

        [Test]
        public void When_IsSolved_Expect_CorrectState()
        {
            Solver s = new("RGBY", 4, false);

            s.GiveFeedback(4, 0);

            Assert.That(s.IsSolved, Is.True);
            Assert.That(s.IsSolveable, Is.True);
            Assert.That(s.PossibleCodes, Is.EqualTo(1));            
        }
    }
}
