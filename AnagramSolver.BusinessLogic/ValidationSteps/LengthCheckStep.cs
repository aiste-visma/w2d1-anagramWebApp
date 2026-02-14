using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.ValidationSteps
{
    public class LengthCheckStep : IInputValidationStep
    {
        public Task Handle(string userInput, Func<Task> next)
        {
            if (userInput.Length <= 1|| userInput.Length > 17)
                throw new ArgumentException("Invalid input length");

            return next();
        }
    }
}
