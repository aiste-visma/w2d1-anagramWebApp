using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.ValidationSteps
{
    public class CharacterCheckStep : IInputValidationStep
    {
        private string allowedCharacters = "aąbcčdeęėfghiįyjklmnoprsštuvzž";
        public Task Handle(string userInput, Func<Task> next)
        {
            if (!userInput.All(c => allowedCharacters.Contains(c)))
            {
                throw new ArgumentException("Invalid character in input.");
            }
            return next();
        }
    }
}

