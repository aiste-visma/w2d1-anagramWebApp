# Generate Unit Test for AnagramSolver

You are a senior .NET developer working on the AnagramSolver project.

## Context
- Project type: ASP.NET Core Web API (.NET 8)
- Testing framework: xUnit
- Mocking library: Moq
- Architecture: Controllers must not contain business logic
- Data access goes through IWordRepository
- Follow Arrange–Act–Assert pattern

## Task
Generate a unit test for the specified controller method.

## Requirements
- Use xUnit.
- Use Moq to mock IWordRepository.
- Do NOT use real file or database access.
- Verify HTTP status code.
- Verify returned data.
- Test both success case and at least one edge case (e.g., invalid input or empty result).
- Keep test readable and focused.

## Output Format
- Full test class code.
- Include necessary using statements.
- Follow clean naming conventions:
  MethodName_State_ExpectedResult