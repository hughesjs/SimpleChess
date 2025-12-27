# SimpleChessEngine

A modern C# library suite for chess game state representation and engine functionality.

## Project Status

- **SimpleChess.State** ✅ Ready to use - Complete chess game state representation library
- **SimpleChess.Engine** 🚧 In development - Move generation and engine logic

## Features

### SimpleChess.State (Available Now)

- **FEN Parsing & Validation**: Complete Forsyth-Edwards Notation support with comprehensive validation
- **Game State Representation**: Type-safe chess positions with immutable value types
- **Board Representation**: Efficient inline array-based board with coordinate system
- **Chess Rules Support**: Castling rights, en passant targets, turn counters
- **High Performance**: Zero-allocation parsing with ref structs and span-based APIs
- **Extensive Testing**: 100+ unit tests covering all validation rules and edge cases

### SimpleChess.Engine (Planned)

- Move generation and validation
- Position evaluation
- Search algorithms
- Engine analysis

## Installation

Install the packages you need via NuGet:

```bash
# For game state representation (available now)
dotnet add package SimpleChess.State

# For engine functionality (coming soon)
dotnet add package SimpleChess.Engine
```

## Usage

### Basic Example

```csharp
using SimpleChess.State;

// Create a new game in the starting position
GameState game = GameState.NewGameState;

// Parse a FEN string
if (FenGameState.TryParse("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", out FenGameState fen))
{
    GameState position = GameState.FromFen(fen);

    // Access game state
    Piece piece = position.CurrentBoard.GetPieceAt(Rank.Four, File.E);
    Colour activePlayer = position.NextToPlay;

    // Check castling rights
    bool canWhiteCastleKingside = position.CastlingRights.WhiteKingside;

    // Convert back to FEN
    FenGameState fenOutput = GameState.ToFen(position);
    string fenString = fenOutput.ToString();
}
```

For detailed architecture and API documentation, see [CLAUDE.md](CLAUDE.md).

## Building

### Prerequisites
- .NET 10 SDK (preview)
- Git

### Build Steps
```bash
git clone https://github.com/hughesjs/SimpleChessEngine.git
cd SimpleChessEngine/src
dotnet restore SimpleChessEngine.slnx
dotnet build SimpleChessEngine.slnx -c Release
```

### Running Tests

The project uses [TUnit](https://github.com/thomhurst/TUnit) as the testing framework.

```bash
# Run SimpleChess.State tests
cd src/SimpleChess.State.Tests
dotnet run -c Release

# Run SimpleChess.Engine tests (when implemented)
cd src/SimpleChess.Engine.Tests
dotnet run -c Release
```

The SimpleChess.State library has comprehensive test coverage with 100+ tests for FEN parsing and validation.

## CI/CD

This project uses GitHub Actions for continuous integration and deployment:

- **CI Pipeline**: Runs on pull requests, builds solution, runs tests, packages pre-release to GitHub Packages
- **CD Pipeline**: Runs on master branch pushes, creates releases, publishes to NuGet.org and GitHub Packages

### Local Testing
You can test the CI/CD pipelines locally using [act](https://github.com/nektos/act):

```bash
# Test CI pipeline
./scripts/test-ci.sh

# Test CD pipeline
./scripts/test-cd.sh
```

## Project Structure

The solution is organised into two main libraries:

- **SimpleChess.State** - Chess game state representation
  - FEN parsing and validation
  - Board and piece types
  - Game state management
  - Coordinate system

- **SimpleChess.Engine** - Chess engine logic (in development)
  - Move generation
  - Position evaluation
  - Search algorithms

Each library has a corresponding test project using TUnit.

## Development Notes

This project uses AI tooling (including Claude Code) for code review, validation, simple tasks, and generation of test data. However, the majority of the code is hand-written, and the entire design and architecture is handcrafted.

## Licence

MIT Licence - see [LICENSE](LICENSE) for details.

## Author

James Hughes
