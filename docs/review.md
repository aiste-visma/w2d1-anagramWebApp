# Code Review: Word Frequency Analysis Feature

**Reviewer:** GitHub Copilot  
**Date:** March 6, 2026  
**Plan:** [docs/plan.md](plan.md)  
**Verdict:** ⚠️ Approve with required changes — one bug and one plan deviation need fixing before merging.

---

## Summary

The overall implementation is clean and well-structured. The contracts, service, and DI registration all closely follow the plan. Tests cover all 16 planned cases. However, there are two issues that need addressing (one bug, one plan deviation) and three lower-priority observations.

---

## 🔴 Issues — Must Fix

### 1. NullReferenceException when request body is JSON `null`

**File:** `AnagramSolver.WebApp/Controllers/AnalysisController.cs` [line 22](../AnagramSolver.WebApp/Controllers/AnalysisController.cs)

```csharp
// Current — crashes with NullReferenceException if body is JSON `null`
public IActionResult Frequency([FromBody] FrequencyRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Text))   // ← NullReferenceException
```

Although `[ApiController]` model binding rejects missing bodies, it **does not** reject JSON bodies that deserialize to `null` (e.g., a body of literal `null`). In that case `request` is `null` and accessing `request.Text` throws.

**Fix:**

```csharp
public IActionResult Frequency([FromBody] FrequencyRequest request)
{
    if (request is null || string.IsNullOrWhiteSpace(request.Text))
        return BadRequest("Input cannot be empty.");
```

**Related test coverage gap:** None of the controller tests cover a null `request`. Add a test to `AnalysisControllerTests`:

```csharp
[Fact]
public void Frequency_NullRequest_ReturnsBadRequest()
{
    var result = _sut.Frequency(null!);

    result.Should().BeOfType<BadRequestObjectResult>();
}
```

---

### 2. Test 4.1.10 silently deviates from the plan's specified expectation

**File:** `AnagramSolver.Tests/FrequencyAnalysisServiceTests.cs` [line 109–121](../AnagramSolver.Tests/FrequencyAnalysisServiceTests.cs)

The plan (section 4.1.10) states:

> Input: `"hello, world! it's great"`  
> Expected: `"its"` (apostrophe stripped), `"hello"` and `"world"` counted.

The `\p{L}+` tokenizer splits `it's` into `["it", "s"]` — `"it"` is a stop word and `"s"` is kept as a token. The test comment acknowledges this but the test was silently adjusted to omit checking for `"its"`. This hides a behavioural difference from the plan's documented requirement.

**Decision required:** either update the plan to clarify that `it's → it + s` is the accepted tokenization, or improve tokenization to contract apostrophe-joined tokens before splitting. The plan wording ("apostrophe stripped") suggests the latter was intended.

If the current tokenization (`"s"` as a separate token) is accepted as the correct behaviour, the plan's test description and the comment in the test must both be updated to reflect this explicitly. The test should also assert the `s` token appears in results (to document the real behaviour), not silently ignore it.

---

## 🟡 Observations — Should Fix

### 3. Dead code: `?? string.Empty` in `MaxBy` is unreachable

**File:** `AnagramSolver.BusinessLogic/FrequencyAnalysisService.cs` [line 30](../AnagramSolver.BusinessLogic/FrequencyAnalysisService.cs)

```csharp
// filtered is guaranteed non-empty at this point (early return above),
// so MaxBy never returns null for a non-empty List<string>.
var longestWord = filtered.MaxBy(w => w.Length) ?? string.Empty;
```

`MaxBy` only returns `null` when the sequence is empty, but there is an explicit `filtered.Count == 0` early-return guard just above. The `?? string.Empty` fallback is dead code.

**Fix:** Remove the null-coalescing or replace it with a non-null-forgiving assertion to make the invariant explicit:

```csharp
var longestWord = filtered.MaxBy(w => w.Length)!;
```

---

### 4. Redundant `StringComparer.OrdinalIgnoreCase` on `StopWords.All`

**File:** `AnagramSolver.BusinessLogic/StopWords.cs` [line 5](../AnagramSolver.BusinessLogic/StopWords.cs)

```csharp
public static readonly HashSet<string> All = new(StringComparer.OrdinalIgnoreCase)
```

All tokens are lowercased via `ToLowerInvariant()` in `Tokenize()` before the `Contains` lookup. The case-insensitive comparer on the `HashSet` is therefore redundant. It makes the code harder to reason about: a reader might assume raw (un-normalised) strings are being checked.

**Fix:** Keep the `HashSet` with the default `Ordinal` comparer (or explicit `StringComparer.Ordinal`), since all values stored and all values checked against it are guaranteed lowercase:

```csharp
public static readonly HashSet<string> All = new(StringComparer.Ordinal)
{
    "the", "a", ...
};
```

---

### 5. No explicit character-count guard on service input

**File:** `AnagramSolver.BusinessLogic/FrequencyAnalysisService.cs`

The `[RequestSizeLimit(1_048_576)]` attribute limits the HTTP body to 1 MB, but the service itself accepts any non-whitespace string. If the service is called directly (e.g., from CLI, from other services), a very large or adversarially crafted string (e.g., one million unique words) would consume substantial CPU and memory with no bound check.

**Recommendation:** Add a reasonable character-length guard in `Analyse` to make the service independently safe:

```csharp
private const int MaxInputLength = 500_000;

public FrequencyResult Analyse(string text)
{
    if (string.IsNullOrWhiteSpace(text))
        return new FrequencyResult();

    if (text.Length > MaxInputLength)
        throw new ArgumentOutOfRangeException(nameof(text),
            $"Input must not exceed {MaxInputLength} characters.");
    ...
}
```

---

## ✅ What is Correct

| Area | Verdict |
|------|---------|
| Contracts layer (1.1–1.4) | All models and interface match the plan exactly |
| DI registration (`Program.cs`) | `AddTransient<IFrequencyAnalysisService, FrequencyAnalysisService>()` correctly placed |
| `TotalWordCount` semantics | Correctly counts all tokens *before* stop-word filtering (plan §2.2 design note) |
| Alphabetical tie-breaking | `ThenBy(wf => wf.Word, StringComparer.OrdinalIgnoreCase)` is correct |
| Unicode token extraction | `\p{L}+` with `RegexOptions.Compiled` is the right choice |
| Stop-word list size | 94 entries — within the plan's ~50–100 range |
| Test cases 4.1.1–4.1.9, 4.1.11–4.1.12 | All assertions are correct and match the plan |
| Controller tests 4.2.1–4.2.4 | Correct, including `Times.Once` verification |
| SOLID / DRY | No violations; responsibilities cleanly separated |
| No extra NuGet packages | Confirmed — only BCL APIs used |
