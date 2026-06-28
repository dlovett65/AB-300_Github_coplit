#!/bin/bash

# Build Pipeline Script - Local Execution
# Run this script to build, test, and create artifacts locally

set -e

echo "=========================================="
echo "AB300 Build Pipeline - Local Execution"
echo "=========================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Step 1: Restore dependencies
echo -e "${YELLOW}Step 1: Restoring dependencies...${NC}"
dotnet restore
echo -e "${GREEN}✓ Dependencies restored${NC}\n"

# Step 2: Build solution
echo -e "${YELLOW}Step 2: Building solution...${NC}"
dotnet build --no-restore --configuration Release
echo -e "${GREEN}✓ Build completed${NC}\n"

# Step 3: Run tests with coverage
echo -e "${YELLOW}Step 3: Running tests with code coverage...${NC}"
dotnet test tests/AB300.Web.Tests/AB300.Web.Tests.csproj \
  --no-build \
  --configuration Release \
  --verbosity normal \
  --collect:"XPlat Code Coverage" \
  --settings coverage.runsettings
echo -e "${GREEN}✓ Tests completed${NC}\n"

# Step 4: Publish application
echo -e "${YELLOW}Step 4: Publishing web application...${NC}"
dotnet publish code/AB300.Web/AB300.Web.csproj \
  --configuration Release \
  --output ./publish \
  --no-build
echo -e "${GREEN}✓ Application published${NC}\n"

# Step 5: Display results
echo -e "${GREEN}=========================================="
echo "Build Pipeline Completed Successfully!"
echo "==========================================${NC}"
echo ""
echo "Artifacts:"
echo "  - Published app: ./publish"
echo "  - Test results: ./tests/AB300.Web.Tests/TestResults"
echo ""
