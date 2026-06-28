# Build Pipeline

This project uses GitHub Actions for continuous integration and automated testing.

## GitHub Actions Workflow

The workflow file is located at `.github/workflows/build-and-test.yml`

### Workflow Triggers
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches

### Workflow Steps

1. **Setup .NET** - Installs .NET 10.0
2. **Restore** - Restores NuGet dependencies
3. **Build** - Builds the solution in Release configuration
4. **Test** - Runs unit tests with code coverage
5. **Coverage Upload** - Uploads coverage reports to Codecov
6. **Publish** - Publishes the web application
7. **Artifacts** - Stores build artifacts for 30 days

## Local Build

You can run the build pipeline locally before pushing:

### Windows
```powershell
.\build.bat
```

### Linux/macOS
```bash
chmod +x ./build.sh
./build.sh
```

## Manual Build Commands

If you prefer to run steps individually:

### Restore dependencies
```
dotnet restore
```

### Build solution
```
dotnet build --configuration Release
```

### Run tests with coverage
```
dotnet test tests\AB300.Web.Tests\AB300.Web.Tests.csproj `
  --configuration Release `
  --collect:"XPlat Code Coverage" `
  --settings coverage.runsettings
```

### Publish application
```
dotnet publish code\AB300.Web\AB300.Web.csproj `
  --configuration Release `
  --output .\publish
```

## Artifacts

After a successful build:
- **Published Web App**: Available in `./publish` directory
- **Test Results**: Available in `./tests/AB300.Web.Tests/TestResults` directory
- **Coverage Reports**: Uploaded to Codecov (if configured)

## CI/CD Features

- ✅ Automated build on push/PR
- ✅ Unit test execution
- ✅ Code coverage collection
- ✅ Test result reporting
- ✅ Build artifact storage
- ✅ Coverage reports (Codecov integration ready)

## Configuration

### .NET Version
Update `DOTNET_VERSION` in `.github/workflows/build-and-test.yml` if you change SDK versions.

### Test Settings
Test coverage is configured via `coverage.runsettings` in the project root.

### Artifacts Retention
Build artifacts are retained for **30 days** by default. Change `retention-days` in the workflow to adjust.

## Troubleshooting

### Build fails locally
1. Ensure .NET 10.0 is installed: `dotnet --version`
2. Clean and restore: `dotnet clean && dotnet restore`
3. Try rebuilding: `dotnet build --force`

### Tests fail
1. Check test output for specific failures
2. Run tests locally with verbose output: `dotnet test -v detailed`
3. Review recent code changes

### Coverage reports not uploading
- Ensure Codecov token is configured as a GitHub secret (if needed)
- Check workflow logs for specific errors

## Next Steps

1. **Push to GitHub**: Add the workflow files and scripts to your repository
2. **Enable GitHub Actions**: Ensure Actions are enabled in repository settings
3. **Monitor Runs**: View workflow runs in the "Actions" tab of your GitHub repository
4. **Configure Codecov** (optional): 
   - Visit codecov.io
   - Connect your GitHub repository
   - Coverage reports will automatically upload
