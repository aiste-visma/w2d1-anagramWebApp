# Technical Plan: Word Frequency Analysis Feature

**Feature:** `POST /api/analysis/frequency`  
**Date:** March 6, 2026  
**Based on:** [docs/research.md](research.md)

---

## Overview

Add a word frequency analysis endpoint to AnagramSolver that accepts arbitrary text and returns the top 10 most frequent words, total word count, unique word count, and longest word. Stop words are filtered; processing is case-insensitive.

---

## Task Breakdown

### 1. Contracts Layer (`AnagramSolver.Contracts`)

#### 1.1 Add `FrequencyRequest` model

**File:** `AnagramSolver.Contracts/Models/FrequencyRequest.cs` *(create)*

```csharp
public class FrequencyRequest
{
    public string Text { get; set; } = string.Empty;
}
```

#### 1.2 Add `WordFrequency` model

**File:** `AnagramSolver.Contracts/Models/WordFrequency.cs` *(create)*

```csharp
public class WordFrequency
{
    public string Word  { get; set; } = string.Empty;
    public int    Count { get; set; }
}
```

#### 1.3 Add `FrequencyResult` model

**File:** `AnagramSolver.Contracts/Models/FrequencyResult.cs` *(create)*

```csharp
public class FrequencyResult
{
    public List<WordFrequency> TopWords       { get; set; } = new();
    public int                 TotalWordCount { get; set; }
    public int                 UniqueWordCount{ get; set; }
    public string              LongestWord    { get; set; } = string.Empty;
}
```

#### 1.4 Add `IFrequencyAnalysisService` interface

**File:** `AnagramSolver.Contracts/IFrequencyAnalysisService.cs` *(create)*

```csharp
public interface IFrequencyAnalysisService
{
    /// <summary>
    /// Analyses <paramref name="text"/> and returns word frequency stats.
    /// Returns an empty result (not null) when no valid tokens are found.
    /// </summary>
    FrequencyResult Analyse(string text);
}
```

Method signature:

| Name | Return type | Parameters | Notes |
|------|-------------|------------|-------|
| `Analyse` | `FrequencyResult` | `string text` | Pure function; synchronous; never throws on valid (non-null) input |

---

### 2. Business Logic Layer (`AnagramSolver.BusinessLogic`)

#### 2.1 Add `StopWords` static class

**File:** `AnagramSolver.BusinessLogic/StopWords.cs` *(create)*

Contains a single `static readonly HashSet<string>` named `All` with ~50–100 common English stop words (lower-cased). Examples: `"the"`, `"a"`, `"an"`, `"is"`, `"are"`, `"was"`, `"were"`, `"be"`, `"been"`, `"has"`, `"have"`, `"had"`, `"do"`, `"does"`, `"did"`, `"will"`, `"would"`, `"could"`, `"should"`, `"may"`, `"might"`, `"shall"`, `"can"`, `"need"`, `"dare"`, `"ought"`, `"used"`, `"i"`, `"you"`, `"he"`, `"she"`, `"it"`, `"we"`, `"they"`, `"me"`, `"him"`, `"her"`, `"us"`, `"them"`, `"my"`, `"your"`, `"his"`, `"its"`, `"our"`, `"their"`, `"this"`, `"that"`, `"these"`, `"those"`, `"in"`, `"on"`, `"at"`, `"by"`, `"for"`, `"of"`, `"with"`, `"to"`, `"from"`, `"and"`, `"or"`, `"but"`, `"not"`, `"no"`, `"so"`, `"if"`, `"as"`, `"up"`, `"out"`.

No external NuGet packages required.

#### 2.2 Implement `FrequencyAnalysisService`

**File:** `AnagramSolver.BusinessLogic/FrequencyAnalysisService.cs` *(create)*

```csharp
public class FrequencyAnalysisService : IFrequencyAnalysisService
{
    private static readonly Regex TokenizerRegex =
        new(@"\p{L}+", RegexOptions.Compiled);

    public FrequencyResult Analyse(string text);

    // Internal helper – extract from method for testability if desired:
    private static IEnumerable<string> Tokenize(string text);
}
```

**`Analyse(string text)` algorithm:**

1. Guard: if `string.IsNullOrWhiteSpace(text)` → return empty `FrequencyResult` (all zeros, empty collections).
2. Tokenize: `Regex.Matches(text, @"\p{L}+")` → extract all letter-sequence matches, lower-case each.
3. Filter stop words: remove tokens present in `StopWords.All`.
4. Compute stats from the filtered token list:
   - `TotalWordCount` = count of **all** tokens (before stop-word filtering, after tokenization).
   - `UniqueWordCount` = distinct token count (after stop-word filtering).
   - `LongestWord` = longest token by `Length` (after stop-word filtering); empty string if list is empty.
   - `TopWords` = group by word with `StringComparer.OrdinalIgnoreCase`, order `OrderByDescending(count).ThenBy(word)`, take 10, project to `List<WordFrequency>`.
5. Return `FrequencyResult`.

> **Design note on `TotalWordCount`:** count all tokens *before* stop-word removal so the value represents the true total words in the input (consistent with what a reader would expect). This is a product decision recorded here.

---

### 3. Web Layer (`AnagramSolver.WebApp`)

#### 3.1 Add `AnalysisController`

**File:** `AnagramSolver.WebApp/Controllers/AnalysisController.cs` *(create)*

```csharp
[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IFrequencyAnalysisService _frequencyService;

    public AnalysisController(IFrequencyAnalysisService frequencyService);

    // POST api/analysis/frequency
    [HttpPost("frequency")]
    [RequestSizeLimit(1_048_576)] // 1 MB
    public IActionResult Frequency([FromBody] FrequencyRequest request);
}
```

**`Frequency` action logic:**

1. If `request.Text` is null or whitespace → return `BadRequest("Input cannot be empty.")`.
2. Call `_frequencyService.Analyse(request.Text)`.
3. Return `Ok(result)`.

**Request / Response:**

| Direction | Type | Example |
|-----------|------|---------|
| Request body | `FrequencyRequest` | `{ "text": "the quick brown fox jumps over the lazy dog" }` |
| `200 OK` | `FrequencyResult` | `{ "topWords": [...], "totalWordCount": 9, "uniqueWordCount": 7, "longestWord": "quick" }` |
| `400 Bad Request` | `string` | `"Input cannot be empty."` |

#### 3.2 Register service in DI

**File:** `AnagramSolver.WebApp/Program.cs` *(modify)*

Add the following line in the service registration section, alongside the existing registrations:

```csharp
builder.Services.AddTransient<IFrequencyAnalysisService, FrequencyAnalysisService>();
```

---

### 4. Tests (`AnagramSolver.Tests`)

#### 4.1 Unit tests for `FrequencyAnalysisService`

**File:** `AnagramSolver.Tests/FrequencyAnalysisServiceTests.cs` *(create)*

No mocking required — test the service directly.

| # | Test name | Input | Expected outcome |
|---|-----------|-------|-----------------|
| 4.1.1 | `Analyse_EmptyString_ReturnsEmptyResult` | `""` | All counts 0, `TopWords` empty, `LongestWord` `""` |
| 4.1.2 | `Analyse_WhitespaceOnly_ReturnsEmptyResult` | `"   "` | Same as above |
| 4.1.3 | `Analyse_PunctuationOnly_ReturnsEmptyResult` | `"!!! --- ..."` | Same as above |
| 4.1.4 | `Analyse_SingleSignificantWord_ReturnsThatWord` | `"hello"` | `TopWords` = `[{hello, 1}]`, total 1, unique 1, longest `"hello"` |
| 4.1.5 | `Analyse_StopWordsOnly_ReturnsZeroTopWords` | `"the a is"` | `TopWords` empty, unique 0, total 3 |
| 4.1.6 | `Analyse_MixedCase_CountedCaseInsensitively` | `"Fox fox FOX"` | `TopWords` = `[{fox, 3}]` |
| 4.1.7 | `Analyse_ReturnsAtMostTenTopWords` | 15 distinct words, each once | `TopWords.Count` == 10 |
| 4.1.8 | `Analyse_TiesOrderedAlphabetically` | Two words with same count | Lower alphabetical word appears first |
| 4.1.9 | `Analyse_LongestWordCorrect` | `"cat elephant"` | `LongestWord` == `"elephant"` |
| 4.1.10 | `Analyse_SpecialCharacters_OnlyLettersExtracted` | `"hello, world! it's great"` | `"its"` (apostrophe stripped), `"hello"` and `"world"` counted |
| 4.1.11 | `Analyse_UniqueWordCount_ExcludesStopWords` | `"the fox and the dog"` | unique 2 (`fox`, `dog`), total 5 |
| 4.1.12 | `Analyse_UnicodeLetters_Handled` | `"café naïve"` | tokens `"café"` and `"naïve"` extracted |

#### 4.2 Unit tests for `AnalysisController`

**File:** `AnagramSolver.Tests/AnalysisControllerTests.cs` *(create)*

Mock `IFrequencyAnalysisService` with Moq; assert HTTP response types and payload shapes.

| # | Test name | Setup | Expected outcome |
|---|-----------|-------|-----------------|
| 4.2.1 | `Frequency_EmptyText_ReturnsBadRequest` | `request.Text = ""` | `BadRequestObjectResult` with message |
| 4.2.2 | `Frequency_WhitespaceText_ReturnsBadRequest` | `request.Text = "  "` | `BadRequestObjectResult` |
| 4.2.3 | `Frequency_ValidText_ReturnsOkWithResult` | service mock returns a populated `FrequencyResult` | `OkObjectResult` containing the result |
| 4.2.4 | `Frequency_ValidText_CallsServiceExactlyOnce` | any non-empty text | `_frequencyService.Analyse(...)` called once with same text |

---

## File Structure Summary

### Files to create

| Project | Path | Action |
|---------|------|--------|
| `AnagramSolver.Contracts` | `Models/FrequencyRequest.cs` | Create |
| `AnagramSolver.Contracts` | `Models/WordFrequency.cs` | Create |
| `AnagramSolver.Contracts` | `Models/FrequencyResult.cs` | Create |
| `AnagramSolver.Contracts` | `IFrequencyAnalysisService.cs` | Create |
| `AnagramSolver.BusinessLogic` | `StopWords.cs` | Create |
| `AnagramSolver.BusinessLogic` | `FrequencyAnalysisService.cs` | Create |
| `AnagramSolver.WebApp` | `Controllers/AnalysisController.cs` | Create |
| `AnagramSolver.Tests` | `FrequencyAnalysisServiceTests.cs` | Create |
| `AnagramSolver.Tests` | `AnalysisControllerTests.cs` | Create |

### Files to modify

| Project | Path | Change |
|---------|------|--------|
| `AnagramSolver.WebApp` | `Program.cs` | Register `IFrequencyAnalysisService` → `FrequencyAnalysisService` as `AddTransient` |

---

## Dependency Notes

- **No new NuGet packages** are required. `System.Text.RegularExpressions` is part of .NET 8 BCL.
- `AnagramSolver.BusinessLogic` already references `AnagramSolver.Contracts`; no new project references needed.
- `AnagramSolver.Tests` already references `AnagramSolver.BusinessLogic` and `AnagramSolver.WebApp`; no new project references needed.

---

## Implementation Order

```
1.1 → 1.2 → 1.3 → 1.4   (Contracts – no dependencies)
       ↓
2.1 → 2.2                 (BusinessLogic – depends on Contracts)
       ↓
3.1 → 3.2                 (WebApp – depends on BusinessLogic + Contracts)
       ↓
4.1 → 4.2                 (Tests – depends on all above)
```

Tasks within the same numbered group (e.g. 1.1–1.4) can be implemented in parallel since they have no inter-dependencies.
