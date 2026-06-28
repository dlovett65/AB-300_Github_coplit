This repository contains scaffolded GitHub Actions workflows and Azure Bicep modules to implement a CI/CD pipeline for the .NET 10 Web API.

**Files added**
- `.github/workflows/build.yml`: CI build workflow for restore, build, test, publish, and uploading artifact.
- `.github/workflows/iac-deploy.yml`: Runs after `Build` via `workflow_run`; logs into Azure and deploys `main.bicep` to a resource group.
- `.github/workflows/qa-deploy.yml`: Runs after `IaC Deploy` via `workflow_run`; downloads build artifact, deploys to App Service, and runs smoke tests.
- `infra/modules/appservice.bicep`: Bicep module that creates an App Service Plan and Web App and exposes outputs.
- `main.bicep`: Entrypoint Bicep which invokes the `appservice.bicep` module.

Critical notes and configuration

- Secrets required in GitHub repository settings:
  - `AZURE_CREDENTIALS`: Service principal JSON used by `azure/login` action.
  - `AZURE_RG`: Target resource group name.
  - `AZURE_WEBAPP_NAME`: Desired web app name (or use the `main.bicep` default by omitting the parameter).

- `build.yml` key sections:
  - `actions/checkout@v4`: checks out code.
  - `actions/setup-dotnet@v3`: sets up .NET 10 SDK.
  - `dotnet restore/build/test/publish`: standard .NET steps. Tests collect `XPlat Code Coverage`.
  - `actions/upload-artifact@v4`: stores published output for downstream workflows.

- `iac-deploy.yml` key sections:
  - Trigger: `workflow_run` on `Build` completion.
  - `azure/login@v1`: authenticates using `AZURE_CREDENTIALS` secret.
  - `az deployment group create --template-file main.bicep`: deploys the Bicep templates to the resource group. Pass parameters either via `--parameters` or rely on defaults.

- `qa-deploy.yml` key sections:
  - Trigger: `workflow_run` on `IaC Deploy` completion.
  - Downloads `webapp-publish` artifact and zips it for `az webapp deployment source config-zip`.
  - Simple health-check loop waits for HTTP 200.
  - Placeholder for running integration/acceptance tests (replace with `dotnet test` against integration tests or a test runner).

Notes and next steps

- You may want to parameterize location, SKU, and other operational settings; add more modular Bicep files for storage, Key Vault, App Insights, and networking.
- For production use consider: deployment slots, Application Insights, managed identity, Key Vault references, and more robust test runners.
- If you want, I can: create an `infra/parameters` file, add Application Insights and deployment slots, or wire up slot-based blue/green deployments.
