# SimpleChessEngine

A simple chess engine library for C#.

## Features

(To be implemented)

## Installation

Install via NuGet:

```bash
dotnet add package SimpleChessEngine
```

## Usage

(To be documented)

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
```bash
cd src/SimpleChessEngine.Tests
dotnet run -c Release
```

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

## Licence

MIT Licence - see [LICENSE](LICENSE) for details.

## Author

James Hughes
