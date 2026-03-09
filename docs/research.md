# Research: Word Frequency Analysis Feature

**Feature Request:** `POST /api/analysis/frequency`  
Accepts text, returns top 10 most frequent words, total word count, unique word count, and longest word. Ignores stop words, case-insensitive, handles empty input and special characters.

---

## 1. Existing Codebase Overview

### Solution Structure

| Project | Role |
|---|---|
| `AnagramSolver.Contracts` | Interfaces and shared models |
| `AnagramSolver.BusinessLogic` | Core logic, validation pipeline, decorators |
| `AnagramSolver.WebApp` | ASP.NET Core Web API + MVC controllers |
| `AnagramSolver.Tests` | xUnit test project (Moq, FluentAssertions) |
| `AnagramSolver.EF.CodeFirst` | EF Core (SQL Server) data layer |
| `AnagramSolver.Dapper` | Dapper-based repository |

**Target framework:** .NET 8.0  
**No NLP/text-analysis libraries** are currently present.

---

## 2. Existing API Endpoint Patterns

All API controllers use the same conventions:

```csharp
[ApiController]
[Route("api/[controller]")]
public class SomeController : ControllerBase { ... }
```

- **Swagger** is configured via `Swashbuckle.AspNetCore` (enabled in Development).
- **Cancellation tokens** are obtained from `HttpContext.RequestAborted` and forwarded through async calls.
- **Request bodies** use `[FromBody]` with plain types (e.g., `string`) or model classes.
- **Response shape** is always `Ok(payload)` or `BadRequest(message)` — no envelope wrapper.

### Existing API controllers

| Controller | Route | Methods |
|---|---|---|
| `WordsApiController` | `api/words` | `GET`, `GET /{id}`, `POST`, `DELETE /{word}` |
| `AnagramsController` | `api/anagrams` | `GET /{word}` |
| `AiController` | `api/ai` | `POST /chat`, `GET /chat/{sessionId}/history` |

The new endpoint route `api/analysis/frequency` follows the same pattern: a new `AnalysisController` with `[Route("api/[controller]")]`.

---

## 3. Text Processing Patterns Already in Use

### 3.1 Normalization
- **Lower-casing** is pervasive: `input.ToLower()`, `word.ToLower()`, `Helpers.lower()`.
- **Whitespace stripping** is done in `HomeController.Index` via `.Replace(" ", "").ToLower()`.

### 3.2 Character filtering
`CharacterCheckStep` maintains an explicit allowlist of Lithuanian characters:
```csharp
private string allowedCharacters = "aąbcčdeęėfghiįyjklmnoprsštuvzž";
```
The frequency feature operates on **arbitrary English/Unicode text**, so this allowlist is intentionally too narrow and **must not be reused**. Instead, a Regex-based tokenizer (`\b[a-zA-Z]+\b` or `\p{L}+`) is appropriate.

### 3.3 Validation pipeline (Chain-of-Responsibility)
`InputValidationPipeline` chains `IInputValidationStep` handlers:
- `EmptyCheckStep` → `LengthCheckStep` → `CharacterCheckStep`

Each step calls `next()` on success, or throws `ArgumentException` on failure.

For the frequency endpoint, **separate validation** is needed: the pipeline constraints (length 2–17 chars, Lithuanian chars only) are incompatible with free-form text analysis. Custom validation should be implemented directly in the service or controller.

### 3.4 Word frequency mechanics (LINQ already used)
`LetterBag` uses `.GroupBy(c => c).ToDictionary(...)` for character frequency — the exact same LINQ approach applies to word frequency:
```csharp
words.GroupBy(w => w, StringComparer.OrdinalIgnoreCase)
     .ToDictionary(g => g.Key, g => g.Count())
```
`LINQwithDictionary.cs` and `EFcoreQuereis.cs` also demonstrate existing LINQ fluency in the team.

---

## 4. Dependency Injection Setup

Services are registered in `Program.cs` using standard ASP.NET Core DI:
- `AddTransient`, `AddScoped`, `AddSingleton` — all three lifetimes are in use.
- `IOptions<AppSettings>` via `builder.Services.Configure<AppSettings>(...)`.
- The new service (`IFrequencyAnalysisService` + `FrequencyAnalysisService`) should be registered as **`AddSingleton`** or **`AddTransient`** (no state, no DB dependency needed for the core logic).

---

## 5. Proposed Component Map

```
POST /api/analysis/frequency
        │
        ▼
AnalysisController          (AnagramSolver.WebApp/Controllers)
        │  [FromBody] FrequencyRequest { Text: string }
        │
        ▼
IFrequencyAnalysisService   (AnagramSolver.Contracts)
        │
        ▼
FrequencyAnalysisService    (AnagramSolver.BusinessLogic)
        │  returns FrequencyResult
        ▼
FrequencyResult             (AnagramSolver.Contracts/Models)
  ├── TopWords: List<WordFrequency>  (top 10)
  ├── TotalWordCount: int
  ├── UniqueWordCount: int
  └── LongestWord: string
```

---

## 6. Risks and Edge Cases

| Risk / Edge Case | Description | Mitigation |
|---|---|---|
| **Empty / whitespace input** | `string.IsNullOrWhiteSpace(text)` | Return `BadRequest("Input cannot be empty.")` before any processing |
| **Special characters / punctuation** | Hyphens, apostrophes, digits, symbols in text | Tokenize with `Regex.Matches(input, @"\p{L}+")` to extract only letter sequences |
| **Case sensitivity** | "The" vs "the" must count as one word | Lower-case all tokens before grouping |
| **Stop words** | Common words ("the", "a", "is", etc.) skew results | Maintain a static `HashSet<string>` stop word list; filter tokens before counting |
| **Very large input** | Multi-MB payloads could cause memory issues | Consider adding a `[RequestSizeLimit]` attribute or validating `text.Length` with a configured max |
| **All tokens are stop words** | Would yield zero results | Return an empty `TopWords` list with counts of 0; do not throw |
| **Unicode / multi-script text** | Non-Latin scripts | `\p{L}+` regex handles Unicode letters; test explicitly |
| **Ties in frequency** | Multiple words with the same count for rank 10 | Use `OrderByDescending(...).ThenBy(w => w)` for deterministic ordering |
| **Null body** | Request body missing entirely | ASP.NET model binding returns 400 automatically if `[FromBody]` is required |
| **Single-character words** | "I", "a" — legitimate words but low value | Can optionally filter `word.Length > 1`; this must be a product decision |

---

## 7. Stop Words Strategy

No stop-word library is currently used in the project. Options:

| Option | Pros | Cons |
|---|---|---|
| **Static `HashSet<string>` in code** | Zero dependencies, fast O(1) lookup | Requires manual maintenance; English-only by default |
| **Configurable list in `appsettings.json`** | Easy to extend | Slightly more boilerplate |
| **NLP library (e.g., `NTextCat`, `NLTK` via Python)** | Sophisticated | Overkill for this scope; adds heavy dependency |

**Recommendation:** a static `HashSet<string>` of ~50–100 common English stop words, consistent with the project's "no external NLP packages" approach.

---

## 8. Testing Considerations

The test project uses **xUnit + Moq + FluentAssertions** (no integration test framework). The new service is pure logic with no I/O, making it highly testable:

- Unit test `FrequencyAnalysisService` directly (no mocking needed).
- Test cases: empty input, punctuation-only input, single word, stop-word-only input, ties, very long word, mixed case.
- `AnalysisController` can be tested by mocking `IFrequencyAnalysisService` with Moq, following the pattern seen in `HomeControllerTests.cs` (currently commented out).

---

## 9. Summary of Recommended Implementation Approach

1. **Add models** to `AnagramSolver.Contracts/Models`: `FrequencyRequest`, `FrequencyResult`, `WordFrequency`.
2. **Add interface** `IFrequencyAnalysisService` to `AnagramSolver.Contracts`.
3. **Implement** `FrequencyAnalysisService` in `AnagramSolver.BusinessLogic` using LINQ + Regex tokenization + stop-word `HashSet`.
4. **Add controller** `AnalysisController` in `AnagramSolver.WebApp/Controllers` following the existing `[ApiController]` / `ControllerBase` pattern.
5. **Register** the service in `Program.cs` as `AddTransient<IFrequencyAnalysisService, FrequencyAnalysisService>()`.
6. **No new NuGet packages** required.

---

*Research completed: March 6, 2026*
