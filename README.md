# Testbed-GatewayServer

Testbed-GatewayServer Talks to Different Sensors using Gateways, It also facilitate necessery information about sensors to Webserver


Third Party libs used:

- MQTTnet.Extensions.ManagedClient
- Microsoft.AspNet.SignalR.Client


AppSetting of GatewayServer:

AppSetting of GatewayServer could be found in AppSetting.json to be configured, For Example AppSettings for AWS looks alike this:

  "AWS": {
    "S3": {
      "DataStore": {
        "Bucket": "wr.io",
        "FolderPath": "iot"
      },
      "SecretsKey": "******",
      "SecretsSecret": "*************"
    }
  },
  Here we can change DataStore Bucket and Folder Path of GatewayServer, There are some AppSetting for example for MQTT also available 
