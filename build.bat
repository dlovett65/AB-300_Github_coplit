@echo off
REM Build Pipeline Script - Local Execution for Windows
REM Run this script to build, test, and create artifacts locally

setlocal enabledelayedexpansion

echo.
echo ==========================================
echo AB300 Build Pipeline - Local Execution
echo ==========================================
echo.

REM Step 1: Restore dependencies
echo [1/4] Restoring dependencies...
dotnet restore
if errorlevel 1 (
    echo ERROR: Restore failed
    exit /b 1
)
echo [OK] Dependencies restored
echo.

REM Step 2: Build solution
echo [2/4] Building solution...
dotnet build --no-restore --configuration Release
if errorlevel 1 (
    echo ERROR: Build failed
    exit /b 1
)
echo [OK] Build completed
echo.

REM Step 3: Run tests with coverage
echo [3/4] Running tests with code coverage...
dotnet test tests\AB300.Web.Tests\AB300.Web.Tests.csproj ^
  --no-build ^
  --configuration Release ^
  --verbosity normal ^
  --collect:"XPlat Code Coverage" ^
  --settings coverage.runsettings
if errorlevel 1 (
    echo ERROR: Tests failed
    exit /b 1
)
echo [OK] Tests completed
echo.

REM Step 4: Publish application
echo [4/4] Publishing web application...
dotnet publish code\AB300.Web\AB300.Web.csproj ^
  --configuration Release ^
  --output .\publish ^
  --no-build
if errorlevel 1 (
    echo ERROR: Publish failed
    exit /b 1
)
echo [OK] Application published
echo.

REM Step 5: Display results
echo ==========================================
echo Build Pipeline Completed Successfully!
echo ==========================================
echo.
echo Artifacts:
echo   - Published app: .\publish
echo   - Test results: .\tests\AB300.Web.Tests\TestResults
echo.

endlocal
