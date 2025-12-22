# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Critical Rules

**These rules override all other instructions:**

1. **NEVER commit directly to main** - Always create a feature branch and submit a pull request
2. **Conventional commits** - Format: `type(scope): description`
3. **GitHub Issues for TODOs** - Use `gh` CLI to manage issues, no local TODO files. Use conventional commit format for issue titles
4. **Pull Request titles** - Use conventional commit format (same as commits)
5. **Branch naming** - Use format: `type/scope/short-description` (e.g., `feat/ui/settings-dialog`)
6. **Working an issue** - Always create a new branch from an updated main branch
7. **Check branch status before pushing** - Verify the remote tracking branch still exists. If a PR was merged/deleted, create a new branch from main instead
8. **WPF for all UI** - All UI must be implemented using WPF (XAML/C#). No web-based technologies (HTML, JavaScript, WebView)

---

### GitHub CLI Commands

```bash
gh issue list                    # List open issues
gh issue view <number>           # View details
gh issue create --title "type(scope): description" --body "..."
gh issue close <number>
```

### Conventional Commit Types

| Type | Description |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `refactor` | Code change that neither fixes a bug nor adds a feature |
| `test` | Adding or updating tests |
| `chore` | Maintenance tasks |

---

## Project Overview

VS-BreakpointNotifier is a Visual Studio 2022 extension (VSIX) that displays a message box notification when a breakpoint is hit during debugging. This helps developers who are multi-tasking while waiting for breakpoints to trigger.

## Build Commands

```bash
# Restore NuGet packages
nuget restore src/CodingWithCalvin.BreakpointNotifier.sln

# Build Release (x64)
msbuild src/CodingWithCalvin.BreakpointNotifier/CodingWithCalvin.BreakpointNotifier.csproj /p:configuration=Release /p:platform=x64 /p:DeployExtension=False

# Build Debug (x64)
msbuild src/CodingWithCalvin.BreakpointNotifier/CodingWithCalvin.BreakpointNotifier.csproj /p:configuration=Debug /p:platform=x64 /p:DeployExtension=False
```

Output: `bin\x64\{Configuration}\CodingWithCalvin.BreakpointNotifier.vsix`

## Development Setup

- Requires Visual Studio 2022 with C# development workload
- Install "Extensibility Essentials 2022" extension for VS development
- Open `CodingWithCalvin.BreakpointNotifier.sln` in Visual Studio
- Test by running in experimental VS instance (F5 from VS)

## Architecture

The extension has a minimal architecture with two core files:

- **BreakpointNotifierPackage.cs** - Main VS Package class extending `AsyncPackage`. Initializes on solution load and sets up the debugger event handler.

- **DebuggerEvents.cs** - Implements `IVsDebuggerEvents` interface. Listens for debugger mode changes and shows a MessageBox when `DBGMODE.DBGMODE_Break` is triggered.

## Technology Stack

- C# / .NET Framework 4.8
- Visual Studio SDK (v17.0+)
- VSIX v3 package format
- x64 architecture only

## CI/CD

GitHub Actions workflows in `.github/workflows/`:

- **release_build_and_deploy.yml** - Triggered on push to main or PR. Versions, builds, and uploads VSIX artifact.
- **publish.yml** - Manual trigger to publish to VS Marketplace and create GitHub release.

Versioning is automated via `CodingWithCalvin/GHA-VSVsixVersioner` action.
