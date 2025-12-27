# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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

### Building
```bash
# From repository root, navigate to src directory
cd src

# Restore dependencies
dotnet restore SimpleChessEngine.slnx

# Build in Release mode
dotnet build SimpleChessEngine.slnx -c Release

# Build in Debug mode
dotnet build SimpleChessEngine.slnx -c Debug
```

### Running Tests
```bash
# From src directory
cd SimpleChessEngine.Tests

# Run all tests in Release mode
dotnet run -c Release

# Run all tests in Debug mode
dotnet run -c Debug

# Run tests from the compiled DLL (after building)
dotnet SimpleChessEngine.Tests.dll  # from bin/Release/net10.0 or bin/Debug/net10.0

# TUnit supports filtering tests - consult TUnit documentation for filter syntax
```

### TUnit-Specific Notes
- Test project has `OutputType=Exe` (TUnit requirement)
- Uses Microsoft.Testing.Platform (no `Microsoft.NET.Test.Sdk` needed)
- Tests are executed via `dotnet run` or directly via the compiled executable
- **Language Version**: Test project uses C# 12 (TUnit's source generator requires this)
- **Async/Await**: All test methods must be `async Task` and assertions must be `await`ed

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

## Architecture

### Core Components

**FEN Parsing (SimpleChessEngine.State namespace)**
- `FenGameState`: Parses and validates FEN (Forsyth-Edwards Notation) strings
  - Implemented as an internal `ref partial struct`
  - Uses `TryParse` pattern for validation
  - Validates all six FEN components: piece placement, active colour, castling, en passant, halfmove clock, fullmove number
  - Exposes validated segments via `FenSegment<TKind>` generic type
  - **Comprehensive test coverage**: 100+ tests covering all validation rules, edge cases, and boundary conditions
- `FenSegment<TKind>`: Generic ref struct for type-safe FEN string segments
  - Uses marker interface pattern (`IFenSegmentKind`) to constrain types
  - Provides span-like API: indexing, slicing, enumeration, implicit conversion to `ReadOnlySpan<char>`
  - Marker types: `PieceLayoutKind`, `NextToPlayKind`, `CastlingStateKind`, `EnPassantStateKind`, `HalfTurnCounterKind`, `FullTurnCounterKind`

**Game State Representation**
- `Game`: Internal class representing complete game state
  - `Board CurrentBoard`: Piece positions
  - `Colour NextToPlay`: Active player (White/Black)
  - `CastlingRights CastlingRights`: Available castling moves
  - `Square? EnPassantTarget`: En passant target square (nullable)
  - `HalfTurnCount HalfTurnCounter`: Halfmove clock (0-150, for fifty-move rule)
  - `FullTurnCount FullTurnCounter`: Fullmove number (1-8840)
  - Factory method: `FromFen(FenGameState)` constructs from validated FEN
  - Static property: `NewGame` for starting position

**Board Representation**
- `Board`: Internal sealed class representing the chess board state
  - Array-based piece storage (`Piece[]`, 64 elements)
  - Indexing: `((int)rank * 8) + (int)file`
  - Factory method: `FromFen(FenSegment<PieceLayoutKind>)` for creating boards from FEN piece layout
  - Query method: `GetPieceAt(Rank rank, File file)`
- `Piece`: Readonly record struct containing colour and piece type
  - `Colour` enum: None, Black, White
  - `PieceType` enum: None, Pawn, Rook, Bishop, Knight, Queen, King
  - Factory method: `FromFenCode(char)` converts FEN characters

**Castling Rights**
- `CastlingRights`: Public readonly record struct representing available castling moves
  - Boolean properties: `WhiteKingside`, `WhiteQueenside`, `BlackKingside`, `BlackQueenside`
  - Internal flags enum for compact storage
  - Factory method: `FromFen(FenSegment<CastlingStateKind>)` parses FEN castling notation (K, Q, k, q, -)
  - Handles all 16 valid FEN castling combinations

**Coordinate System**
- `Rank` enum (A-H): Represents files (columns) - X axis
- `File` enum (One-Eight): Represents ranks (rows) - Y axis
- `Square`: Internal readonly struct for board coordinates
  - Properties: `Rank Rank`, `File File`
  - Factory method: `FromFen(FenSegment<EnPassantStateKind>)` parses en passant notation
  - Returns `Square?` (null for "-", square for valid positions like "e3" or "e6")

**Turn Counters**
- `HalfTurnCount`: Readonly record struct for halfmove clock (0-150 range enforces fifty-move rule)
- `FullTurnCount`: Readonly record struct for fullmove number (1-8840 range)
- Both use factory methods: `FromFen(FenSegment<...>)` with validation

### Design Patterns
- Internal implementation types with public API surface (tests access internals via `InternalsVisibleTo`)
- `readonly record struct` for immutable value types (Piece, CastlingRights, HalfTurnCount, FullTurnCount, Square)
- `ref struct` for zero-allocation span-based parsing (FenGameState, FenSegment<TKind>)
- Try-Parse pattern for validation without exceptions
- Factory methods for complex object creation (`FromFen` pattern throughout)
- Marker interface pattern for generic type constraints (FenSegment<TKind>)

## Important Notes
- This is a standard library, not a Roslyn analyser
- Uses TUnit instead of xUnit (requires `OutputType=Exe` in test project)
- TUnit uses Microsoft.Testing.Platform (no `Microsoft.NET.Test.Sdk` needed)
- All source files use LF line endings (enforced via .gitattributes)
- Most core types are internal; public API will be exposed through facade classes
