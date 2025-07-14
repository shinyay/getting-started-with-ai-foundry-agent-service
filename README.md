# Azure AI Foundry - Agent Service

## Description

## Demo

## Features

- feature:1
- feature:2

## Requirement

## Usage

## Installation/Tutorial

### Step 1: Azure CLI Login

To interact with Azure services, you need to log in using the Azure CLI. Open your terminal and run the following command:

```shell
az login
```

List your Azure subscriptions to ensure you have access:

```shell
az account list --output table
```

Set your desired subscription as the active one:

```shell
az account set --subscription "YOUR_SUBSCRIPTION_ID"
```

Show the current active subscription to confirm:

```shell
az account show --output table
```

### Step 2: Register Required Resource Providers

Before creating resources, you need to register the necessary Azure resource providers.

Run the following commands in your terminal:

```shell
# Microsoft.CognitiveServicesリソースプロバイダーの登録
az provider register --namespace Microsoft.CognitiveServices

# Microsoft.MachineLearningServicesリソースプロバイダーの登録（AI Foundryで使用）
az provider register --namespace Microsoft.MachineLearningServices

# Microsoft.Searchリソースプロバイダーの登録（検索機能用）
az provider register --namespace Microsoft.Search

# Microsoft.Storageリソースプロバイダーの登録（ストレージ用）
az provider register --namespace Microsoft.Storage

# Microsoft.DocumentDBリソースプロバイダーの登録（Cosmos DB用）
az provider register --namespace Microsoft.DocumentDB

# 登録状況の包括的確認
az provider show --namespace Microsoft.CognitiveServices --query "registrationState" --output tsv
az provider show --namespace Microsoft.MachineLearningServices --query "registrationState" --output tsv
az provider show --namespace Microsoft.Search --query "registrationState" --output tsv
az provider show --namespace Microsoft.Storage --query "registrationState" --output tsv
az provider show --namespace Microsoft.DocumentDB --query "registrationState" --output tsv
```

### Step 3: Create a Resource Group

Create a resource group to contain your Azure AI Foundry resources.

```shell
# 変数の設定
set resourceGroupName "rg-aifoundry-demo-"(date +%Y%m%d)
set location "eastus"

# リソースグループの作成
az group create --name $resourceGroupName --location $location

# 作成されたリソースグループの確認
az group show --name $resourceGroupName --output table
```

### Step 4: Create an Azure AI Foundry Account

Create an Azure AI Foundry account to start using the service.

```shell
set aiAccountName "ai-foundry-account-demo"

# Azure AI Foundryアカウントの作成（AIServices kind を使用）
az cognitiveservices account create \
    --name $aiAccountName \
    --resource-group $resourceGroupName \
    --location $location \
    --kind "AIServices" \
    --sku "S0" \
    --yes

# 作成されたアカウントの確認
az cognitiveservices account show \
    --name $aiAccountName \
    --resource-group $resourceGroupName \
    --output table
```

Show Endpoint and Access Keys:

```shell
# エンドポイントの取得
az cognitiveservices account show \
    --name $aiAccountName \
    --resource-group $resourceGroupName \
    --query "properties.endpoint" \
    --output tsv

# アクセスキーの取得
az cognitiveservices account keys list \
    --name $aiAccountName \
    --resource-group $resourceGroupName \
    --output table

# 利用可能なエンドポイント一覧の取得
az cognitiveservices account show \
    --name $aiAccountName \
    --resource-group $resourceGroupName \
    --query "properties.endpoints" \
    --output json
```




## References

## Licence

Released under the [MIT license](https://gist.githubusercontent.com/shinyay/56e54ee4c0e22db8211e05e70a63247e/raw/f3ac65a05ed8c8ea70b653875ccac0c6dbc10ba1/LICENSE)

## Author

- github: <https://github.com/shinyay>
- twitter: <https://twitter.com/yanashin18618>
- mastodon: <https://mastodon.social/@yanashin>
