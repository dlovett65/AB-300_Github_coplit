param webAppName string = 'ab300-web-${uniqueString(resourceGroup().id)}'
param appServicePlanName string = 'ab300-plan'
param skuName string = 'B1'
param skuTier string = 'Basic'
param skuSize string = 'B1'
param location string = resourceGroup().location
