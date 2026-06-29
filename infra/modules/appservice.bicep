param webAppName string
param appServicePlanName string
param location string = 'eastus' //resourceGroup().location
param skuName string = 'B1'
param skuTier string = 'Basic'
param skuSize string = 'B1'
param siteKind string = 'app'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-10-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
    tier: skuTier
    size: skuSize
  }
  properties: {
    reserved: false
  }
}

resource webApp 'Microsoft.Web/sites@2023-10-01' = {
  name: webAppName
  location: location
  kind: siteKind
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}

output webAppName string = webApp.name
output defaultHostName string = webApp.properties.defaultHostName
output appServicePlanId string = appServicePlan.id
