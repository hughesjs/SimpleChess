# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
SimpleChessEngine is a C# library for chess game logic and engine functionality. It follows modern .NET practices with strict code quality enforcement.

## Structure
- **SimpleChess.State**: Chess state representation library (net10.0)
- **SimpleChess.State.Tests**: State library tests using TUnit (net10.0)
- **SimpleChess.Engine**: Chess engine library (net10.0) - in development
- **SimpleChess.Engine.Tests**: Engine library tests using TUnit (net10.0) - in development

## Key Technologies
- C# (latest language version)
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
# From src directory, run state tests
cd SimpleChess.State.Tests
dotnet run -c Release  # or -c Debug

# Run engine tests (when implemented)
cd SimpleChess.Engine.Tests
dotnet run -c Release  # or -c Debug

# Run tests from the compiled DLL (after building)
dotnet SimpleChess.State.Tests.dll  # from bin/Release/net10.0 or bin/Debug/net10.0

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
- Package ID: `SimpleChess.State`
- Initial Version: `0.1.0`
- Licence: MIT
- Includes XML documentation
- Symbols package (snupkg) for debugging

## Architecture

### Core Components

**FEN Parsing (SimpleChess.State namespace)**
- `FenGameState`: Parses and validates FEN (Forsyth-Edwards Notation) strings
  - Implemented as a public `ref partial struct`
  - Uses `TryParse` pattern for validation
  - Validates all six FEN components: piece placement, active colour, castling, en passant, halfmove clock, fullmove number
  - Exposes validated segments via `FenSegment<TKind>` generic type
  - **Comprehensive test coverage**: 100+ tests covering all validation rules, edge cases, and boundary conditions
- `FenSegment<TKind>`: Internal generic ref struct for type-safe FEN string segments
  - Uses marker interface pattern (`IFenSegmentKind`) to constrain types
  - Provides span-like API: indexing, slicing, enumeration, implicit conversion to `ReadOnlySpan<char>`
  - Internal marker types: `PieceLayoutKind`, `NextToPlayKind`, `CastlingStateKind`, `EnPassantStateKind`, `HalfTurnCounterKind`, `FullTurnCounterKind`

**Game State Representation**
- `GameState`: Public record struct representing complete game state
  - `Board CurrentBoard`: Piece positions
  - `Colour NextToPlay`: Active player (White/Black)
  - `CastlingRights CastlingRights`: Available castling moves
  - `Square? EnPassantTarget`: En passant target square (nullable)
  - `HalfTurnCount HalfTurnCounter`: Halfmove clock (0-150, for fifty-move rule tracking)
  - `FullTurnCount FullTurnCounter`: Fullmove number (1-8840)
  - Factory method: `FromFen(FenGameState)` constructs from validated FEN
  - Factory method: `ToFen(GameState)` converts to validated FEN
  - Static property: `NewGameState` for starting position

**Board Representation**
- `Board`: Public readonly struct representing the chess board state
  - Inline array-based piece storage (64 elements) for optimal performance
  - Indexing: `((int)rank * 8) + (int)file`
  - Internal factory method: `FromFen(FenSegment<PieceLayoutKind>)` for creating boards from FEN piece layout
  - Internal factory method: `ToFen(Board, StringBuilder)` for FEN serialisation
  - Query method: `GetPieceAt(Rank rank, File file)` and `GetPieceAt(Square square)`
  - Static property: `DefaultBoard` for starting position
- `Piece`: Public readonly record struct containing colour and piece type
  - `Colour` enum: None, Black, White
  - `PieceType` enum: None, Pawn, Rook, Bishop, Knight, Queen, King
  - Internal factory method: `FromFenCode(char)` converts FEN characters
  - Internal factory method: `ToFen(Piece)` converts to FEN character

**Castling Rights**
- `CastlingRights`: Public readonly record struct representing available castling moves
  - Boolean properties: `WhiteKingside`, `WhiteQueenside`, `BlackKingside`, `BlackQueenside`
  - Internal flags enum for compact storage
  - Factory method: `FromFen(FenSegment<CastlingStateKind>)` parses FEN castling notation (K, Q, k, q, -)
  - Handles all 16 valid FEN castling combinations

**Coordinate System**
- `File` enum (A-H): Represents files (columns) - X axis
- `Rank` enum (One-Eight): Represents ranks (rows) - Y axis
- `Square`: Public readonly struct for board coordinates
  - Properties: `File File`, `Rank Rank`
  - Internal factory method: `FromFen(FenSegment<EnPassantStateKind>)` parses en passant notation
  - Returns `Square?` (null for "-", square for valid positions like "e3" or "e6")
  - Internal factory method: `ToFen(Square?, StringBuilder)` for FEN serialisation

**Turn Counters**
- `HalfTurnCount`: Public readonly record struct for halfmove clock (0-150 range)
- `FullTurnCount`: Public readonly record struct for fullmove number (1-8840 range)
- Both use internal factory methods: `FromFen(FenSegment<...>)` with validation
- Both support implicit conversion to int and explicit conversion from int with validation

### Design Patterns
- Public types for the core API with internal factory methods for construction
- Tests access internals via `InternalsVisibleTo`
- `readonly record struct` for immutable value types (Piece, CastlingRights, HalfTurnCount, FullTurnCount, GameState)
- `readonly struct` for board representation with inline arrays (Board, Square)
- `ref struct` for zero-allocation span-based parsing (FenGameState, FenSegment<TKind>)
- Try-Parse pattern for validation without exceptions (FenGameState.TryParse)
- Internal factory methods for complex object creation (`FromFen` and `ToFen` patterns throughout)
- Marker interface pattern for generic type constraints (FenSegment<TKind>)
- Separation of state representation from game rules (state library doesn't enforce draw rules)

## Important Notes
- This is a standard library, not a Roslyn analyser
- Uses TUnit instead of xUnit (requires `OutputType=Exe` in test project)
- TUnit uses Microsoft.Testing.Platform (no `Microsoft.NET.Test.Sdk` needed)
- All source files use LF line endings (enforced via .gitattributes)
- Public API exposes core types (GameState, Board, Piece, Square, etc.) with internal factory methods
- State library focuses on representation only - game rules and move validation will be in the engine library
