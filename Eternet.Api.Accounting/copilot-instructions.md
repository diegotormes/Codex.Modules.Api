# Tech stack:
- xUnit
- FluentAssertions
- NSubstitute

# Code Conventions
- **Primary constructors** for DI whenever possible.
- Use `var` when the type is obvious; use `nameof` instead of magic strings when referring to members.
- **No namespace removal** in existing files; keep file-scoped namespaces in new files _after_ using clauses.
- **One blank line** between:  
  1) using blocks • 2) namespace • 3) type declaration.
- **Line length ≤ 120 chars**; break long invocations/queries for readability.
- **Indent multiline strings** the same as surrounding code.
- **Parameter & local names** in **camelCase**; **no PascalCase for parameters**.
- **No unexplained abbreviations** (`service`, not `svc`; `response`, not `resp`).
- Avoid exposing **data-access models** to the presentation layer—map to DTOs.
- Prefer **English** error messages; expose them through `public static` helpers (`ServiceNotFound(id)`).
- Do **not** wrap EF calls in try/catch just to log—`ExceptionBehavior` handles it.
- Never attach success or error messages to successful responses.
- Verify spelling before committing; no typos in identifiers, keys, or messages.
- Use clear, domain-specific English names; avoid unexplained abbreviations (`LastInvoice`, not `UltFact`).
- Never hard-code primary-key values when seeding; let EF/database generate IDs.
- Do **not** embed Spanish text in identifiers, messages, or comments—keep everything in English.
- Avoid single-letter variable names (except trivial loop indices); prefer descriptive names.
- Do **not** extract single-use string or procedure constants—inline them for clarity.
- Prefer `public static` helper methods for reusable error messages instead of `string.Format` templates.
- Prefer generic, domain-focused names without redundant qualifiers; avoid prefixes/suffixes like **Created**, **Last**, or **Raw** when the concept is clear (`SalesInvoiceId`, **not** `CreatedInvoiceId`).

# Project Structure Conventions
- Mirror the production folder hierarchy in test projects and keep namespaces in sync after file moves or renames.

# Formatting Checklist (auto-review)
- ✔️ Blank line groups (using / namespace / type).
- ✔️ Camel-case parameters.
- ✔️ Descriptive names (no cryptic abbreviations).
- ✔️ 120-character line limit.
- ✔️ No data models in response DTOs.
- ✔️ English domain errors via helpers.
- ✔️ Steps respect SRP & DRY.
- ✔️ Tests free of conditional logic and external stubs.
- ✔️ No typos, spelling mistakes, or broken grammar in code, comments, or error messages.

## Framework-Specific Guidelines

### xUnit
- Leverage parameterized tests using `[Theory]` with `[InlineData]` when possible.

### NSubstitute
- Use `ReturnsNull()` extension when applicable.

### AutoFixture
1. Share `_fixture` for all tests using auto-mocking.
2. Use `Create` or the builder pattern for complex object creation.

---

# Testing Conventions

## General
- **No stubbing stored procedures**; seed required data with EF entities.
- **No logic in tests** (e.g., `if`); split into separate test cases.
- **No mocking log calls** or fragile verifications.
- Reuse shared strings/error codes via the production helper being tested.
- Use fixture-generated objects with navigation properties rather than manual ID assignment.
- Compare error messages in tests by calling the same static helper used in production code; never duplicate raw strings.
- NEVER create, stub, or alter stored procedures inside tests; seed required data via EF entities instead.

## Naming

### Guidelines

When generating test names, follow these conventions:

1. **Format**
   Use the pattern `Method_Condition_ExpectedBehavior`.

2. **Omitting the Method Name Prefix**
   - If the class being tested only has one method (unless it is an extension method), omit the method name prefix in the test name.

3. **Omitting the Condition**
   - If the method being tested is simple and has only one possible execution path (i.e., there are no conditions or variations in behavior), omit the "condition" from the test name.

4. **Verb Tense**
	- Write in the **present simple tense**.

5. **Voice**
   - Use **active voice** (e.g., `ReturnsError` instead of `ErrorIsReturned`).

6. **Condition Description**
   - Aim to follow a descriptive pattern that clearly outlines the scenario or condition.
   - **Recommended**: `Subject + Is/Has + Condition` (e.g., `InputIsNull`, `ValueIsNegative`).
   - **However**, if your scenario needs a different construction for clarity, you may use other forms such as `SubjectDoesNotExist`, `SubjectAlreadyHasCondition`, or `ThereAre<Subject>` (e.g., `AdditionalChargeDoesNotExist`, `PhoneLineAlreadyHasDiscount`, `ThereAreVendors`).

7. **Subject Agreement**
   - Use **third-person singular** conjugation for verbs (e.g., `he/she/it` form, typically ending in `-s/-es`).

### Examples

- **Single-method class, no conditions**:
  - `GeneratesUniqueId`

- **Single-method class, multiple conditions**:
  - `InputIsInvalid_ReturnsError`
  - `ValueIsNegative_ReturnsZero`

- **Multiple-method class, no conditions**:
  - `Validate_ReturnsError`
  - `Calculate_ReturnsZero`

- **Multiple-method class, multiple conditions**:
  - `Validate_InputIsNull_ReturnsError`
  - `Calculate_ValueIsNegative_ReturnsZero`

- **Real-world scenarios**:
  - `AdditionalChargeDoesNotExist_ReturnsInvalidStateError`
  - `AdditionalChargeExists_RemovesAdditionalCharge`
  - `PhoneLineAlreadyHasDiscount_ReturnsInvalidStateError`
  - `ThereAreVendors_ReturnsThem`

## Additional Notes
Ensure test names are concise, descriptive, and adhere to these rules.
