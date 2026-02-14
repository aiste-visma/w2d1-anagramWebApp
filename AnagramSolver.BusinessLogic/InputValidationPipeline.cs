using AnagramSolver.BusinessLogic.ValidationSteps;
using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class InputValidationPipeline
    {
        private EmptyCheckStep emptyCheckStep;
        private LengthCheckStep lengthCheckStep;
        private CharacterCheckStep characterCheckStep;

        public InputValidationPipeline() 
        {
            emptyCheckStep = new EmptyCheckStep();
            lengthCheckStep = new LengthCheckStep();
            characterCheckStep = new CharacterCheckStep();

        }

        public async Task Execute(string userInput)
        {
            await emptyCheckStep.Handle(userInput, () => lengthCheckStep.Handle(userInput, () => (characterCheckStep.Handle(userInput, () => Task.CompletedTask))));
        }

    }
}
