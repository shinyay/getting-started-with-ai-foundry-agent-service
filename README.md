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

### Step 5: Create Supporting Resources

#### Create a Storage Account

Create a storage account to store data and models used by the Azure AI Foundry Agent Service.

```shell
# 変数の設定（短縮名 + 日付でユニークな名前を生成）
set storageAccountName "staifoundry"(date +%Y%m%d)

# Storage Accountの作成
az storage account create \
    --name $storageAccountName \
    --resource-group $resourceGroupName \
    --location $location \
    --sku "Standard_LRS" \
    --kind "StorageV2"

# Storage Accountの詳細確認
az storage account show \
    --name $storageAccountName \
    --resource-group $resourceGroupName \
    --output table
```

#### Create Azure Cosmos DB Account

Create an Azure Cosmos DB account to store structured data for the AI Foundry Agent Service.

```shell
# 変数の設定
set cosmosAccountName "cosmos-aifoundry-demo"

# 注意: East USで容量不足の場合は、別のリージョンを試してください
# 代替リージョンの例: westus2, centralus, westeurope
# set alternativeLocation "westus2"

# Option 1: 元のリージョン（eastus）で作成を試行
az cosmosdb create \
    --name $cosmosAccountName \
    --resource-group $resourceGroupName \
    --locations regionName=$location \
    --default-consistency-level "Session" \
    --enable-automatic-failover false

# Option 2: 容量不足の場合は代替リージョンを使用
# 前回の作成が失敗した場合は、まず既存のインスタンスを削除
# az cosmosdb delete \
#     --name $cosmosAccountName \
#     --resource-group $resourceGroupName \
#     --yes

# 削除完了を待ってから再作成
# set alternativeLocation "westus2"
# az cosmosdb create \
#     --name $cosmosAccountName \
#     --resource-group $resourceGroupName \
#     --locations regionName=$alternativeLocation \
#     --default-consistency-level "Session" \
#     --enable-automatic-failover false

# Cosmos DB Accountの確認
az cosmosdb show \
    --name $cosmosAccountName \
    --resource-group $resourceGroupName \
    --output table
```

#### Create Azure AI Search Service

Create Azure AI Search Service to enable search capabilities for your AI Foundry Agent Service.

```shell
# 変数の設定
set searchServiceName "search-aifoundry-demo"

# Azure AI Search Serviceの作成
az search service create \
    --name $searchServiceName \
    --resource-group $resourceGroupName \
    --location $location \
    --sku "Basic"

# Search Serviceの確認
az search service show \
    --name $searchServiceName \
    --resource-group $resourceGroupName \
    --output table
```

### Step 6: Configure Permissions

#### Get Current User Object ID

```shell
# 現在のユーザーのオブジェクトIDを取得
set currentUser (az ad signed-in-user show --query id --output tsv)
echo "Current User ID: $currentUser"
```

#### Assign Roles

Assign the appropriate roles to the current user for the AI account.

```shell
# サブスクリプションIDの取得
set subscriptionId (az account show --query id --output tsv)

# Azure AI Administrator ロールをAIアカウントに付与
az role assignment create \
    --assignee $currentUser \
    --role "Azure AI Administrator" \
    --scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/Microsoft.CognitiveServices/accounts/$aiAccountName"

# 権限の確認
az role assignment list \
    --assignee $currentUser \
    --scope "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName" \
    --output table
```


## References

## Licence

Released under the [MIT license](https://gist.githubusercontent.com/shinyay/56e54ee4c0e22db8211e05e70a63247e/raw/f3ac65a05ed8c8ea70b653875ccac0c6dbc10ba1/LICENSE)

## Author

- github: <https://github.com/shinyay>
- twitter: <https://twitter.com/yanashin18618>
- mastodon: <https://mastodon.social/@yanashin>
