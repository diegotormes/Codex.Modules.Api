# Quick‑start for Codex

**Purpose** – Provide the agent with the minimum, explicit map it needs to navigate this .NET 9 repository safely and ship useful pull‑requests. Replace every `<placeholder>` with real details.

---

## 1 • Repository landmarks

* **Solution file**      `tests/Eternet.Accounting.Api.Tests.sln`
* **Main project**       `src/Eternet.Accounting.Api/Eternet.Accounting.Api.csproj`
* **All tests**          `tests/**/*.csproj`



## 2 • Build, test

```bash
# dependencies restored in setup; no network

# compile in Release
dotnet build tests/Eternet.Accounting.Api.Tests.sln -c Release --no-restore --verbosity minimal

# run unit + integration tests
dotnet test tests/Eternet.Accounting.Api.Tests.sln --no-build --no-restore --verbosity minimal

```

The pull‑request **must pass** all commands above in CI.

## 3 • Development environment (local & agent)

1. Install SDK 9 (Linux/macOS):

   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 9.0 --install-dir "$HOME/.dotnet"
   echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> ~/.profile
   echo 'export PATH="$PATH:$HOME/.dotnet"'  >> ~/.profile
   ```

2. Source the profile or open a new shell.

3. Warm the NuGet cache with `dotnet restore && dotnet build`.

## 4 • Where can the agent edit?

| Path / project               | Description              | Safe to modify |
| ---------------------------- | ------------------------ | ---------------|
| `src/<App>.csproj`           | Production code          | ✅             |
| `src/tests/<App>.Tests`      | Unit / integration tests | ✅             |


If a task requires changes outside the allowed paths, **ask for confirmation** in a follow‑up message.

## 5 • Style guide excerpts

*For the complete coding conventions used by the team, see ****`copilot-instructions.md`**** in the repo root.*

* Follow the `.editorconfig` in the repo root.
* Prefer expression‑bodied members when the expression ≤ 100 chars.
* Use `using` aliases for long namespaces.
* Modifier order: `public | protected | internal | private`, `static`, `readonly`, `required`.

## 6 • Typical tasks & hints

| Task type          | What to include in the prompt                                   |
| ------------------ | --------------------------------------------------------------- |
| **Add tests**      | Name the methods needing coverage; place tests in `src/tests/`. |
| **Refactor**       | Point to the exact file/project;                                |
| **Bug fix**        | Paste full stack trace + repro steps.                           |

## 7 • Branch & PR conventions

* **Branch names** `feature/<ticket>`, `bugfix/<ticket>`, `chore/<topic>`
* **PR title**     `[<area>] Imperative summary (≤ 60 chars)`
* Add the label **`agent-codex`** so CI uses the lightweight runner.

## 8 • Exclusions ⛔

* Don’t commit large binaries (`.dll`, images) without approval.

---

*Update this document whenever the project structure or contribution rules change.*
