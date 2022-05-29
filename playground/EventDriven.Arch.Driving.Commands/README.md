Para configurar o projeto para rodar precisa criar um **local.settings.json**

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "MongoSettings:ConnectionString": "[mongodb://.....]",
    "MongoSettings:DatabaseName":"[databaseName]",
    "ServiceBusSettings:ConnectionString": "[Endpoint=sb://....]",
    "ServiceBusSettings:Exchange": "[exchange]",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

Como funciona? A chamada das functions de Comando eventualmente levam a uma emissão de evento. 