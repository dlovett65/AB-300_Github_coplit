param webAppName string = 'ab300-web-${uniqueString(resourceGroup().id)}'
param appServicePlanName string = 'ab300-plan'
param location string = resourceGroup().location
param skuName string = 'B1'
param skuTier string = 'Basic'
param skuSize string = 'B1'
param siteKind string = 'app'

module appServiceModule 'infra/modules/appservice.bicep' = {
  name: 'appServiceModule'
  params: {
    webAppName: webAppName
    appServicePlanName: appServicePlanName
    location: location
    skuName: skuName
    skuTier: skuTier
    skuSize: skuSize
    siteKind: siteKind
  }
}

output webAppName string = appServiceModule.outputs.webAppName
output defaultHostName string = appServiceModule.outputs.defaultHostName
output appServicePlanId string = appServiceModule.outputs.appServicePlanId
