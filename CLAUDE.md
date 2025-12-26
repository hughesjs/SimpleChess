# SimpleChessEngine - Claude Context

## Project Overview
SimpleChessEngine is a C# 10 library for chess game logic and engine functionality. It follows modern .NET practices with strict code quality enforcement.

## Structure
- **SimpleChessEngine**: Main library project (net10.0)
- **SimpleChessEngine.Tests**: Unit test project using TUnit (net10.0)

## Key Technologies
- C# 10.0 (explicit language version)
- .NET 10 (preview)
- TUnit testing framework
- Modern .slnx solution format
- GitHub Actions CI/CD with semantic versioning

## Code Quality Standards
- **No `var`**: Explicit types required (enforced at error level)
- **File-scoped namespaces**: Required (warning level)
- **Nullable reference types**: Enabled and strictly enforced
- **EditorConfig**: Comprehensive rules for formatting and style
- **InternalsVisibleTo**: Tests have access to internal members

## Building and Testing
```bash
# Restore and build
cd src
dotnet restore SimpleChessEngine.slnx
dotnet build SimpleChessEngine.slnx -c Release

# Run tests
cd SimpleChessEngine.Tests
dotnet run -c Release
```

## CI/CD Workflow
- **Pull Requests**: Build, test, package pre-release to GitHub Packages
- **Master Branch**: Full release with semantic versioning, publish to NuGet.org and GitHub Packages
- **Versioning**: Automated semantic versioning based on commit messages
  - `feat:`, `feature:`, `minor:` → Minor version bump
  - `fix:`, `bugfix:`, `patch:` → Patch version bump
  - `major:`, `breaking:` → Major version bump

## Testing with act
Local CI/CD testing available via scripts:
- `./scripts/test-ci.sh` - Test PR build pipeline
- `./scripts/test-cd.sh` - Test release pipeline

## Package Configuration
- Package ID: `SimpleChessEngine`
- Initial Version: `0.1.0`
- Licence: MIT
- Includes XML documentation
- Symbols package (snupkg) for debugging

## Important Notes
- This is a standard library, not a Roslyn analyser
- Uses TUnit instead of xUnit (requires `OutputType=Exe` in test project)
- TUnit uses Microsoft.Testing.Platform (no `Microsoft.NET.Test.Sdk` needed)
- All source files use LF line endings (enforced via .gitattributes)

## Placeholder Files
To allow commits before implementation, the following placeholder files exist:

**src/SimpleChessEngine/ChessBoard.cs**:
```csharp
namespace SimpleChessEngine;

/// <summary>
/// Placeholder class for chess board representation.
/// </summary>
public sealed class ChessBoard
{
    // Implementation to be added
}
```

**src/SimpleChessEngine.Tests/ChessBoardTests.cs**:
```csharp
namespace SimpleChessEngine.Tests;

/// <summary>
/// Placeholder tests for ChessBoard.
/// </summary>
public sealed class ChessBoardTests
{
    // Tests to be added
}
```

## Repository Setup Complete
The repository structure is complete with:
- Git configuration (.gitignore, .gitattributes, LICENSE)
- Code quality configuration (.editorconfig, global.json, dotnet-tools.json)
- Project structure (solution, library, and test projects)
- CI/CD pipelines (GitHub Actions workflows)
- Test scripts for local pipeline testing
- Placeholder code to enable building

The repository is ready for chess engine implementation.
