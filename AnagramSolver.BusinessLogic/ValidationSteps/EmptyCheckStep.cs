using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.ValidationSteps
{
    public class EmptyCheckStep : IInputValidationStep
    {
        public Task Handle(string userInput, Func<Task> next)
        {
            if(string.IsNullOrWhiteSpace(userInput))
                throw new ArgumentException("Input cannot be empty");

            return next();
        }
    }
}
