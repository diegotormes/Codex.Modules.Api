# Quick‑start for Codex

**Purpose** – Provide the agent with the minimum, explicit map it needs to navigate this .NET 9 repository safely and ship useful pull‑requests. Replace every `<placeholder>` with real details.

---

## 1 • Repository landmarks

* **Solution file**      `src/Eternet.Web.sln`
* **Main project**       `src/Eternet.Web.Infrastructure/Eternet.Web.Infrastructure.csproj`
* **All tests**          `src/tests/**/*.csproj`
* Fluent UI Blazor source + docs fluentui-blazor/
    1. The entire Fluent UI Blazor project (sub‑modules, components, docs) lives here. Always reference the components and their properties exactly as defined in this checkout – do not assume future API changes.



## 2 • Build, test

```bash
# dependencies restored in setup; no network

# compile in Release
dotnet build src/Eternet.Web.sln -c Release --no-restore

# run unit + integration tests
dotnet test tests/Eternet.Web.Infrastructure.Tests/Eternet.Web.Infrastructure.Tests.csproj --no-build --no-restore --verbosity minimal
dotnet test tests/Eternet.Web.UI.Tests/Eternet.Web.Infrastructure.Tests.csproj --no-build --no-restore --verbosity minimal

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
| `/workspace/fluentui-blazor` | vendor code              | ⚠️             |


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
| **UI feature**     | Specify which Fluent UI Blazor components you expect to use; link to docs under /workspace/fluentui-blazor/examples/Demo/Shared/**/.razor  |

## 7 • Branch & PR conventions

* **Branch names** `feature/<ticket>`, `bugfix/<ticket>`, `chore/<topic>`
* **PR title**     `[<area>] Imperative summary (≤ 60 chars)`
* Add the label **`agent-codex`** so CI uses the lightweight runner.

## 8 • Exclusions ⛔

* Avoid touching code generated on `/workspace/fluentui-blazor` use this as knowledge base only.
* Don’t commit large binaries (`.dll`, images) without approval.

---

*Update this document whenever the project structure or contribution rules change.*
