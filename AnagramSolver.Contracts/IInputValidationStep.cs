using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IInputValidationStep
    {
        Task Handle(string userInput, Func<Task> next);
    }
}
